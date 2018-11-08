using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SucoSnake.Core
{
	public abstract class Controller
	{
		public void Init()
		{
			InternalInit();
		}

		public void Update()
		{
			InternalUpdate();
		}

		protected virtual void InternalInit()
		{

		}

		protected virtual void InternalUpdate()
		{

		}
	}

	public class ControllerAggregator : Controller
	{
		public void Add( Controller controller )
		{

		}

		protected override void InternalInit()
		{
			base.InternalInit();

		}

	}
}
