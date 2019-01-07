using System.Security.Cryptography;

namespace SucoSnake.Core
{
	public class NodeContent : Nest, INodeContent
	{
		#region Properties
		public ILinkedNode Parent { get; private set; }
		#endregion

		#region Public Members
		public void SetParentNode( ILinkedNode parent )
		{
			Parent = parent;
		}
		#endregion
	}

	public class CellEventArgs : NestEggEventArgs
	{
	}

	public class CellsField : INodeContentFactory
	{
		#region Public Fields
		public Nest Root = new Nest();
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

		public Spot Spot { get; private set; }

		public void SetSpot(Spot spot)
		{
			Spot = spot;
			GetContent().Add( spot );
		}
		#endregion
	}
}
