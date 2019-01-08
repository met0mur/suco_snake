using SucoSnake.Core;

namespace Assets.Sources.Core
{
	public static class SpotGameExtentions
	{
		#region Public Members
		public static Cell GetCell( this Spot spot )
		{
			if( spot.Nest != null )
			{
				var content = ( NodeContent ) spot.Nest;
				return content.Parent;
			}

			throw new SpotException( "GetCell: have not cell" );
		}

		public static bool HaveCell(this Spot spot)
		{
			return spot.Nest != null && spot.Nest is NodeContent;
		}
		#endregion
	}
}
