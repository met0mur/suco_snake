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
				var move = _queue.Peek();
				while (move != null)
				{
					var result = move.Step();
					if (result != MoveStepResult.Done)
					{
						_queue.Dequeue();
						move = _queue.Peek();
					}
					else
					{
						return result;
					}
				}
				return MoveStepResult.ImmediatelyDone;
			}
			else
			{
				foreach (IMove move in _list.ToArray())
				{
					var result = move.Step();
					if (result != MoveStepResult.Done)
					{
						_list.Remove(move);
					}
				}
				return _list.Any() ? MoveStepResult.ImmediatelyDone : MoveStepResult.Done;
			}
			
		}
	}
}
