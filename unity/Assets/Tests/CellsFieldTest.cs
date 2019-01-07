using NUnit.Framework;
using SucoSnake.Core;

public class CellsFieldTest
{
	#region Public Members
	[ Test ]
	public void FieldDirections()
	{
		var field = new CellsField();
		field.Fill( 10, 10 );
		var a = field.GetCellAt( 5, 5 );
		var b = field.GetCellAt( 5, 6 );
		Assert.AreEqual( b.Y, ( a.GetBottomNode() as Cell ).Y );
		b = field.GetCellAt( 5, 4 );
		Assert.AreEqual( b.Y, ( a.GetTopNode() as Cell ).Y );
		b = field.GetCellAt( 4, 5 );
		Assert.AreEqual( b.X, ( a.GetLeftNode() as Cell ).X );
		b = field.GetCellAt( 6, 5 );
		Assert.AreEqual( b.X, ( a.GetRightNode() as Cell ).X );
	}

	[ Test ]
	public void FieldRunner()
	{
		var field = new CellsField();
		field.Fill( 10, 10 );
		var a = field.GetCellAt( 5, 5 );
		Cell b = null;
		foreach( var c in a.RunSide( SquareNodeSides.Right ) )
		{
			b = ( Cell ) c;
		}

		Assert.AreEqual( field.GetCellAt( 9, 5 ), b );
	}
	#endregion
}
