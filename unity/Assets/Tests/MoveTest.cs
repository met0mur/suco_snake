using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SucoSnake.Core
{
	public enum MoveTestInternamMethodName
	{
		OnAction,
		OnCanceled,
		OnCompleted,
		OnFinished,
		OnProgress,
		OnStart
	}

	public class TestMove : Move
	{
		#region Events
		public event Action< MoveTestInternamMethodName > InternalCalls;
		#endregion

		#region Protected Members
		protected override void OnAction()
		{
			base.OnAction();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnAction );
			}
		}

		protected override void OnCanceled()
		{
			base.OnCanceled();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnCanceled );
			}
		}

		protected override void OnCompleted()
		{
			base.OnCompleted();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnCompleted );
			}
		}

		protected override void OnFinished()
		{
			base.OnFinished();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnFinished );
			}
		}

		protected override void OnProgress()
		{
			base.OnProgress();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnProgress );
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			if( InternalCalls != null )
			{
				InternalCalls.Invoke( MoveTestInternamMethodName.OnStart );
			}
		}

		protected override bool Validate()
		{
			return base.Validate();
		}
		#endregion
	}

	public class MoveTest
	{
		#region Public Members
		[ Test ]
		public void States()
		{
			var move = new TestMove();
			Assert.AreEqual( MoveState.Initial, move.State );

			move = new TestMove();
			move.Init( MoveMode.Immediately );
			Assert.AreEqual( MoveState.Initial, move.State );
			move.MarkInQueue();
			Assert.AreEqual( MoveState.InQueue, move.State );

			var result = move.Step();
			Assert.AreEqual( MoveState.Completed, move.State );
			Assert.AreEqual( MoveStepResult.ImmediatelyDone, result );

			//

			move = new TestMove();
			move.Init( MoveMode.OneStep );
			Assert.AreEqual( MoveState.Initial, move.State );
			move.MarkInQueue();
			Assert.AreEqual( MoveState.InQueue, move.State );

			result = move.Step();
			Assert.AreEqual( MoveState.Started, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( MoveState.Completed, move.State );
			Assert.AreEqual( MoveStepResult.ImmediatelyDone, result );

			//

			move = new TestMove();
			move.Init( 4 );
			Assert.AreEqual( MoveState.Initial, move.State );
			move.MarkInQueue();
			Assert.AreEqual( MoveState.InQueue, move.State );

			result = move.Step();
			Assert.AreEqual( 3, move.StepsLeft );
			Assert.AreEqual( MoveState.Started, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( 2, move.StepsLeft );
			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( 1, move.StepsLeft );
			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( 0, move.StepsLeft );
			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( 0, move.StepsLeft );
			Assert.AreEqual( MoveState.Completed, move.State );
			Assert.AreEqual( MoveStepResult.ImmediatelyDone, result );

			//

			move = new TestMove();
			move.Init( MoveMode.Infinity );
			Assert.AreEqual( MoveState.Initial, move.State );
			move.MarkInQueue();
			Assert.AreEqual( MoveState.InQueue, move.State );

			result = move.Step();
			Assert.AreEqual( MoveState.Started, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			result = move.Step();
			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			for( var i = 0; i < 100; i++ )
			{
				result = move.Step();
			}

			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			move.Finish();

			result = move.Step();
			Assert.AreEqual( MoveState.Completed, move.State );
			Assert.AreEqual( MoveStepResult.ImmediatelyDone, result );


			//

			move = new TestMove();
			move.Init( MoveMode.Infinity );

			result = move.Step();
			Assert.AreEqual( MoveState.Started, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			for( var i = 0; i < 100; i++ )
			{
				result = move.Step();
			}

			Assert.AreEqual( MoveState.InProgress, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			move.Cancel();

			result = move.Step();
			Assert.AreEqual( MoveState.Canceled, move.State );
			Assert.AreEqual( MoveStepResult.Skipped, result );

			//

			move = new TestMove();
			move.Init( MoveMode.Infinity );

			result = move.Step();
			Assert.AreEqual( MoveState.Started, move.State );
			Assert.AreEqual( MoveStepResult.Done, result );

			move.Cancel();

			result = move.Step();
			Assert.AreEqual( MoveState.Canceled, move.State );
			Assert.AreEqual( MoveStepResult.Skipped, result );

			//

			move = new TestMove();
			move.Init( MoveMode.Infinity );

			move.Cancel();

			result = move.Step();
			Assert.AreEqual( MoveState.Canceled, move.State );
			Assert.AreEqual( MoveStepResult.Skipped, result );

			//

			move = new TestMove();
			move.Init( MoveMode.OneStep );

			move.Cancel();

			result = move.Step();
			Assert.AreEqual( MoveState.Canceled, move.State );
			Assert.AreEqual( MoveStepResult.Skipped, result );
		}

		[ Test ]
		public void Internal()
		{
			var move = new TestMove();
			move.Init( MoveMode.Immediately );
			var list = new Stack< MoveTestInternamMethodName >();
			move.InternalCalls += type => list.Push( type );
			move.Step();

			Assert.AreEqual( MoveTestInternamMethodName.OnCompleted, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnFinished, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );

			move = new TestMove();
			move.Init( MoveMode.Infinity );
			list = new Stack< MoveTestInternamMethodName >();
			move.InternalCalls += type => list.Push( type );
			move.Step();

			Assert.AreEqual( MoveTestInternamMethodName.OnStart, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );
			move.Step();
			Assert.AreEqual( MoveTestInternamMethodName.OnProgress, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );
			move.Step();
			Assert.AreEqual( MoveTestInternamMethodName.OnProgress, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );
			move.Finish();
			move.Step();
			Assert.AreEqual( MoveTestInternamMethodName.OnCompleted, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnFinished, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );


			move = new TestMove();
			move.Init( MoveMode.Infinity );
			list = new Stack< MoveTestInternamMethodName >();
			move.InternalCalls += type => list.Push( type );
			move.Step();

			Assert.AreEqual( MoveTestInternamMethodName.OnStart, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );
			move.Cancel();
			move.Step();
			Assert.AreEqual( MoveTestInternamMethodName.OnCanceled, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnFinished, list.Pop() );
			Assert.AreEqual( MoveTestInternamMethodName.OnAction, list.Pop() );
		}


		[ Test ]
		public void Queue()
		{
			var queue = new Queue( QueueMode.Sequential );
			Assert.AreEqual( MoveStepResult.Skipped, queue.Step());

			queue.AddMove( new Move( MoveMode.Immediately ) );
			Assert.AreEqual( MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual( MoveStepResult.Skipped, queue.Step());

			queue.AddMove( new Move( MoveMode.Immediately ) );
			queue.AddMove( new Move( MoveMode.Immediately ) );
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue.AddMove(new Move(MoveMode.OneStep));
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue.AddMove(new Move(MoveMode.OneStep));
			queue.AddMove(new Move(MoveMode.OneStep));
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			var subqueue = new Queue( QueueMode.Sequential );
			queue.AddMove( subqueue );
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			subqueue = new Queue( QueueMode.Sequential );
			subqueue.AddMove(new Move(MoveMode.OneStep));
			queue.AddMove( subqueue );
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue = new Queue( QueueMode.Parallel );
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue.AddMove(new Move(MoveMode.Immediately));
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue.AddMove(new Move(MoveMode.OneStep));
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

			queue.AddMove(new Move(MoveMode.OneStep));
			queue.AddMove(new Move(MoveMode.OneStep));
			Assert.AreEqual(MoveStepResult.Done, queue.Step());
			Assert.AreEqual(MoveStepResult.ImmediatelyDone, queue.Step());
			Assert.AreEqual(MoveStepResult.Skipped, queue.Step());

		}
		#endregion
	}
}
