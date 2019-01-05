using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SucoSnake.Core
{
	public enum QueueMode
	{
		Sequential,
		Parallel,
	}

	public class Queue : IMove
	{

		private Queue<IMove> _queue = new Queue<IMove>();
		private List<IMove> _list = new List<IMove>();
		private QueueMode _mode;

		public Queue (QueueMode mode)
		{
			_mode = mode;
		}

		public void AddMove(IMove move)
		{
			if (_mode == QueueMode.Sequential)
			{
				_queue.Enqueue(move);
			}
			else
			{
				_list.Add(move);
			}
		}

		public MoveStepResult Step()
		{
			if (_mode == QueueMode.Sequential)
			{
				if (!_queue.Any())
				{
					return MoveStepResult.Skipped;
				}
				var move = _queue.Peek();
				while (move != null)
				{
					var result = move.Step();
					if (result != MoveStepResult.Done)
					{
						_queue.Dequeue();
						move = _queue.Any() ? _queue.Peek() : null;
					}
					else
					{
						return result;
					}
				}
				return MoveStepResult.ImmediatelyDone;
			}
			else if(_mode == QueueMode.Parallel)
			{
				if( !_list.Any() )
				{
					return MoveStepResult.Skipped;
				}

				foreach (IMove move in _list.ToArray())
				{
					var result = move.Step();
					if (result != MoveStepResult.Done)
					{
						_list.Remove(move);
					}
				}
				return _list.Any()  ? MoveStepResult.Done : MoveStepResult.ImmediatelyDone;
			}
			
			return MoveStepResult.Skipped;
		}
	}
}
