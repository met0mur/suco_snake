using System;

namespace SucoSnake.Core
{
	public class CellContent : Nest, ICellContent
	{
		#region Public Members
		public void SetParentCell( ICell parent )
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class CellEventArgs : NestEggEventArgs
	{
	}

	public class CellContentFactory : ICellContentFactory
	{
		#region Public Fields
		public Nest Root;
		#endregion

		#region Public Members
		public ICellContent CreateContent( ICell parent )
		{
			var content = new CellContent();
			Root.Add( content );
			return content;
		}
		#endregion
	}


	public class Cell : SquareCell< CellContent >
	{

	}
}
