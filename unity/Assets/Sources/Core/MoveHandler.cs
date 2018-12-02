using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SucoSnake.Core
{
	public interface IMoveHandler
	{
		void HandleMove(Move move);
	}

	/*public interface IMoveHandler<TMove> where TMove : Move
	{
		void HandleMove(TMove move);
	}*/

	public interface IMoveHandlersAggregator
	{
		void AddHandler(IMoveHandler handler, bool invoke = false);
	}

	public class MoveHandlersAggregator : IMoveHandler, IMoveHandlersAggregator
	{

		public void AddHandler(IMoveHandler handler, bool invoke = false)
		{
			
		}

		public void HandleMove(Move move)
		{
			
		}
	}
}
