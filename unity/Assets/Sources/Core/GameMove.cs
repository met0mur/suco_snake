using System;
using System.Linq;
using SucoSnake.Core;

namespace Assets.Sources.Core
{
	public class MoveException : Exception
	{
		#region Constructors
		public MoveException(string message = null) : base(message)
		{
		}
		#endregion
	}

	public class SpotMove : Move
	{
		#region Private Fields
		private Cell _from;
		private Cell _to;
		#endregion

		#region Constructors
		public SpotMove( Cell from, Cell to ) : base( MoveMode.OneStep )
		{
			if (from == to)
			{
				throw new MoveException("Same cells setted");
			}

			_from = from;
			_to = to;
		}
		#endregion

		#region Protected Members
		protected override void OnFinished()
		{
			base.OnFinished();

			var path = new SpotRunner( _from.Spot ).ToArray();

			//positions shift
			var lastCell = _to;
			foreach( var spot in path )
			{
				var targetCell = lastCell;
				lastCell = spot.GetCell();
				targetCell.SetSpot( spot );
			}

			//links shift
			for( var i = path.Length - 2; i >= 1; i-- )
			{
				path[ i ].ShiftLinksTo( path[ i + 1 ], path[ i - 1 ] );
			}
		}
		#endregion
	}
}
