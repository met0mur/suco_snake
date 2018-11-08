using System;
using System.Collections;
using System.Collections.Generic;

namespace SucoSnake.Core
{
	public interface ICell
	{
		ICellContent Init(ICellContentFactory factory);
		ICellContent GetContent();
	}

	public interface ICellContentFactory
	{
		#region Public Members
		ICellContent CreateContent(ICell parent);
		#endregion
	}

	public interface ICellContent
	{
		#region Public Members
		void SetParentCell( ICell parent );
		#endregion
	}

	public enum SquareCellSides
	{
		Left,
		Right,
		Top,
		Bottom
	}

	public class CellSideRunner<TSidesEnum, TCellContent> : IEnumerator<Cell<TSidesEnum, TCellContent>> where TSidesEnum : struct, IConvertible where TCellContent : ICellContent
	{
		public CellSideRunner( IConvertible side )
		{
			throw new NotImplementedException();
		}

		public Cell<TSidesEnum, TCellContent> Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public bool MoveNext()
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}
	}

	public class Cell< TSidesEnum, TCellContent > : ICell where TSidesEnum : struct, IConvertible where TCellContent : ICellContent
	{
		
		#region Public Members
		public bool HasNextCell( TSidesEnum side )
		{
			return false;
		}

		public Cell< TSidesEnum, TCellContent > GetNextCell( TSidesEnum side )
		{
			return null;
		}

		public void SetNextCell( TSidesEnum side, Cell<TSidesEnum, TCellContent> cell )
		{
			
		}

		public IEnumerator< Cell< TSidesEnum, TCellContent > > RunSide( TSidesEnum side )
		{
			return new CellSideRunner< TSidesEnum, TCellContent >(side);
		}
		#endregion

		public ICellContent Init( ICellContentFactory factory )
		{
			throw new NotImplementedException();
		}

		public ICellContent GetContent()
		{
			throw new NotImplementedException();
		}

		public TCellContent GetContent1()
		{
			throw new NotImplementedException();
		}
		
	}

	public class SquareCell< TCellContent > : Cell< SquareCellSides, TCellContent > where TCellContent : ICellContent
	{
		#region Public Members
		public SquareCell< TCellContent > GetLeftCell()
		{
			return ( SquareCell< TCellContent > ) GetNextCell( SquareCellSides.Left );
		}

		public SquareCell< TCellContent > GetRightCell()
		{
			return ( SquareCell< TCellContent > ) GetNextCell( SquareCellSides.Right );
		}

		public SquareCell<TCellContent> GetTopCell()
		{
			return (SquareCell<TCellContent>)GetNextCell(SquareCellSides.Top);
		}

		public SquareCell<TCellContent> GetBottomCell()
		{
			return (SquareCell<TCellContent>)GetNextCell(SquareCellSides.Bottom);
		}
		#endregion
	}
}
