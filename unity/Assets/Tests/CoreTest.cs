using NUnit.Framework;
using SucoSnake.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFieldTest {

	[Test]
	public void Start () {
		var field = new CellFieldUnit(10, 10);
		field.Initialize();
		var cell = field.GetCell(5, 5);
		Assert.AreEqual(cell.X, 5);
		Assert.AreEqual(cell.Y, 5);
		Assert.NotNull(field.GetCell(0, 0));
		Assert.NotNull(field.GetCell(9, 9));
	}
}
