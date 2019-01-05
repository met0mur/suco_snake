using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SucoSnake.Core
{
	//	Sequential Scheme Of Two Step Move
	//
	//   |--------------|--------------|<-------------                                         
	//   |              |              |                                         
	//   |   Started    |  InProgress  |  FInished                             
	//   |_____________________________|<_____________
	//	 |				|			   |
	//	 |				|			   |
	//   First block of work are starded
	//					|			   |
	//					First are ended second are started
	//					               |
	//								   Second block of work finished
	//								   Starting Next
	//								   |--------------|--------------|<-------------                                         
	//								   |              |              |                                         
	//								   |   Started    |  InProgress  |  FInished                             
	//								   |_____________________________|<_____________
	//								   |			  |			     |
	//								   |			  |			     |
	//								   First block of work are starded
	//								   				  |			     |
	//								   				  First are ended second are started
	//								   				                 |
	//								   							     Second block of work are done
	//
	//   Move protected methods scheme
	//   ____________________________________________________________                                         
	//   |								                            |                           
	//   |							Action			                |                         
	//   |__________________________________________________________|                                        
	//   |              |              |                            |                           
	//   |			    |			   |		  Finished          |                         
	//   |				|			   |____________________________|
	//   |  Started     |  InProgress  |              |             |                           
	//   |			    |			   |  Cancelled   |  Completed  |                         
	//   |______________|______________|______________|_____________|
	//
	//
	//
	//

	public enum MoveStepResult
	{
		Done,
		ImmediatelyDone,
		Skipped
	}

	public enum MoveMode
	{
		Immediately,
		OneStep,
		NStep,
		Infinity
	}

	public enum MoveState
	{
		Initial,
		InQueue,
		Started,
		InProgress,
		Completed,
		Canceled
	}

	public interface IMove
	{
		MoveStepResult Step();
	}

	public class Move : IMove, IMoveHandlersAggregator
	{
		private readonly MoveHandlersAggregator _aggregtor = new MoveHandlersAggregator();
		private bool _cancelRequested;
		private bool _finishRequested;

		public MoveMode Mode { get; private set; }

		public uint StepsInitial { get; private set; }
		public uint StepsLeft { get; private set; }

		public MoveState State { get; private set; }

		public Move() {}

		public Move(MoveMode mode)
		{
			Init(mode);
		}

		public Move(uint numSteps)
		{
			Init(numSteps);
		}
		
		public void AddHandler(IMoveHandler handler, bool invoke = false)
		{
			_aggregtor.AddHandler(handler, invoke);
		}

		public void Init(MoveMode mode)
		{
			StepsLeft = 1;
			StepsInitial = 1;
			Mode = mode;
			State = MoveState.Initial;
		}

		public void Init(uint numSteps)
		{
			if (numSteps <= 1)
			{
				StepsLeft = 1;
				StepsInitial = 1;
				Mode = MoveMode.OneStep;
			}
			else
			{
				StepsInitial = numSteps;
				StepsLeft = numSteps;
				Mode = MoveMode.NStep;
			}
			State = MoveState.Initial;
		}
		
		public void MarkInQueue()
		{
			State = MoveState.InQueue;
			OnInQueue();
			_aggregtor.HandleMove(this);
		}

		public void Cancel()
		{
			_cancelRequested = true;
		}

		public void Finish()
		{
			_finishRequested = true;
		}

		public MoveStepResult Step()
		{
			MoveStepResult result = MoveStepResult.Done;
			MoveState state = State;

			if (!Validate() || _cancelRequested)
			{
				state = MoveState.Canceled;
				result = MoveStepResult.Skipped;
			}
			else if (Mode == MoveMode.Immediately)
			{
				state = MoveState.Completed;
				result = MoveStepResult.ImmediatelyDone;
			}
			else if (Mode == MoveMode.NStep || Mode == MoveMode.OneStep)
			{
				if (StepsLeft == 0)
				{
					state = MoveState.Completed;
					result = MoveStepResult.ImmediatelyDone;
				}
				else
				{
					if (State == MoveState.Started)
					{
						state = MoveState.InProgress;
					}
					else if (State != MoveState.InProgress)
					{
						state = MoveState.Started;
					}
					StepsLeft--;
				}
			}
			else if (Mode == MoveMode.Infinity)
			{
				if (_finishRequested)
				{
					state = MoveState.Completed;
					result = MoveStepResult.ImmediatelyDone;
				} else
				{
					if (State == MoveState.Started)
					{
						state = MoveState.InProgress;
					}
					else if (State != MoveState.InProgress)
					{
						state = MoveState.Started;
					}
				}
			}

			OnAction();

			if (state == MoveState.Started)
			{
				OnStart();
			}
			else if (state == MoveState.InProgress)
			{
				OnProgress();
			}
			else if (state == MoveState.Canceled)
			{
				OnFinished();
				OnCanceled();
			}
			else if (state == MoveState.Completed)
			{
				OnFinished();
				OnCompleted();
			}

			State = state;
			_aggregtor.HandleMove(this);

			return result;
		}

		protected virtual bool Validate()
		{
			return true;
		}

		protected virtual void OnAction()
		{

		}

		protected virtual void OnInQueue()
		{

		}

		protected virtual void OnStart()
		{

		}

		protected virtual void OnProgress()
		{

		}

		protected virtual void OnCompleted()
		{

		}
		
		protected virtual void OnFinished()
		{

		}

		protected virtual void OnCanceled()
		{

		}

	}
}
