using Assets.Sources.Core;

namespace SucoSnake.Core
{
	public class NodeContent : Nest, INodeContent
	{
		#region Properties
		public Cell Parent { get; private set; }
		#endregion

		#region Public Members
		public void SetParentNode( ILinkedNode parent )
		{
			Parent = ( Cell ) parent;
		}
		#endregion
	}

	public class CellEventArgs : NestEggEventArgs
	{
	}

	public class CellsField : INodeContentFactory
	{
		#region Public Fields
		public Nest Root = new Nest { Name = "root" };
		#endregion

		#region Private Fields
		private Cell[ , ] _list;
		#endregion

		#region Public Members
		public INodeContent CreateContent( ILinkedNode parent )
		{
			var content = new NodeContent();
			Root.Add( content );
			content.SetParentNode( parent );
			return content;
		}

		public Cell CreateCell()
		{
			var cell = new Cell();
			cell.Init( this );
			return cell;
		}

		public Cell GetCellAt( int x, int y )
		{
			return _list[ x, y ];
		}

		public void Fill( int xSize, int ySize )
		{
			var list = new Cell[ xSize, ySize ];
			for( var x = 0; x < xSize; x++ )
			{
				for( var y = 0; y < ySize; y++ )
				{
					var cell = CreateCell();
					cell.X = x;
					cell.Y = y;
					cell.GetContent().Name = cell.GetContent().GetType().Name + "-" + x + ":" + y;
					list[ x, y ] = cell;
					if( y > 0 )
					{
						cell.SetNextNode( SquareNodeSides.Top, list[ x, y - 1 ] );
						list[ x, y - 1 ].SetNextNode( SquareNodeSides.Bottom, cell );
					}

					if( x > 0 )
					{
						cell.SetNextNode( SquareNodeSides.Left, list[ x - 1, y ] );
						list[ x - 1, y ].SetNextNode( SquareNodeSides.Right, cell );
					}
				}
			}

			_list = list;
		}
		#endregion
	}


	public class Cell : SquareLinkedNode< NodeContent >
	{
		#region Public Fields
		public int X;
		public int Y;
		#endregion

		#region Properties
		public Spot Spot { get; private set; }
		#endregion

		#region Public Members
		public void SetSpot( Spot spot )
		{
			if (Spot != null && Spot == spot)
			{
				throw new SpotSetInCellException("Cant set same spot: " + Spot.GetFullName());
			}

			if ( Spot != null )
			{
				throw new SpotSetInCellException("Spot already setted: " + Spot.GetFullName() + ". New spot:" + spot.GetFullName());
			}

			var content = GetContent();
			if( spot != null )
			{
				if( spot.HaveCell() )
				{
					var lastCell = spot.GetCell();
					lastCell.Spot = null;
					spot.Nest.MoveTo( spot, content );
				}
				else
				{
					content.Add( spot );
				}
			}

			Spot = spot;
		}
		#endregion
	}
}
