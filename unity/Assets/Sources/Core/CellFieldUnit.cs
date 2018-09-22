using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SucoSnake.Core {
	public class CellFieldUnit {

		public CellFieldUnit(short width, short height) {
			this.width = width;
			this.height = height;
		}
	
		private short width;
		private short height;
	
		public void Initialize()
		{

		}

		public CellUnit GetCell(short x, short y)
		{
			return null;
		}

	}
}
