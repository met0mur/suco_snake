namespace SucoSnake.Core
{
	public class CellField : Nest
	{
		#region Private Fields
		private short width;
		private short height;
		#endregion

		#region Constructors
		public CellField( short width, short height )
		{
			this.width = width;
			this.height = height;
		}
		#endregion

		#region Public Members
		public void Initialize()
		{
		}

		public Cell GetCell( short x, short y )
		{
			return null;
		}
		#endregion
	}
}
