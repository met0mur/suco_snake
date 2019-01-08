using NUnit.Framework;

namespace SucoSnake.Core
{
	class TestEgg : Egg
	{
		#region Public Members
		public void InvokeTest()
		{
			InvokeKnock( new NestEggEventArgs { Self = this, Type = NestEggEventType.Custom, Bubble = true } );
		}
		#endregion
	}

	public class EggNestTest
	{
		#region Public Members
		[ Test ]
		public void Initial()
		{
			new Nest();
			new Egg();
		}

		[ Test ]
		public void EggEvent()
		{
			var egg = new TestEgg();
			var flag = false;
			egg.Knock += args => flag = true;
			egg.InvokeTest();
			Assert.True( flag );
		}

		[ Test ]
		public void EggAddEvent()
		{
			var nest = new Nest();
			var egg = new TestEgg();
			var flag = false;
			egg.Knock += args => flag = args.Type == NestEggEventType.Added;
			nest.Add( egg );
			Assert.True( flag );
		}


		[ Test ]
		public void EggRemoveEvent()
		{
			var nest = new Nest();
			var egg = new TestEgg();
			var flag = false;
			egg.Knock += args => flag = args.Type == NestEggEventType.Removed;
			nest.Add( egg );
			nest.Remove( egg );
			Assert.True( flag );
		}

		[ Test ]
		public void EggMoveEvent()
		{
			var nest = new Nest();
			var nest2 = new Nest();
			var egg = new TestEgg();
			var flag = false;
			egg.Knock += args => flag = args.Type == NestEggEventType.Moved && args.From == nest && args.To == nest2;
			nest.Add( egg );
			nest.MoveTo( egg, nest2 );
			Assert.True( flag );
			Assert.IsNotNull( nest2.GetNested<TestEgg>() );
		}

		[ Test ]
		public void EggBubleEvent()
		{
			var nest = new Nest();
			var egg = new TestEgg();
			var flag = false;
			nest.Knock += args => flag = args.Type == NestEggEventType.Custom && args.Self == egg;
			nest.Add( egg );
			egg.InvokeTest();
			Assert.True( flag );
		}

		[ Test ]
		public void EggDoubleBubleEvent()
		{
			var nest = new Nest();
			var nest2 = new Nest();
			nest2.Add( nest );
			var egg = new TestEgg();
			var flag = false;
			nest2.Knock += args => flag = args.Type == NestEggEventType.Custom && args.Self == egg;
			nest.Add( egg );
			egg.InvokeTest();
			Assert.True( flag );
		}

		[ Test ]
		public void EmptyNestGetter()
		{
			var nest = new Nest();
			nest.GetNested< Egg >();
			nest.Remove< Egg >();
		}

		[ Test ]
		public void NestNullAddError()
		{
			var nest = new Nest();
			Assert.Throws< NullRefNestException >( () => nest.Add( null ) );
		}

		[ Test ]
		public void NestDoubleAddError()
		{
			var egg = new TestEgg();
			var nest = new Nest();
			nest.Add( egg );
			Assert.Throws< ContainNestException >( () => nest.Add( egg ) );
		}

		[ Test ]
		public void EmptyNestRemoveError()
		{
			var nest = new Nest();
			Assert.Throws< ContainNestException >( () => nest.Remove( new Egg() ) );
		}

		[ Test ]
		public void EmptyNestRemoveError2()
		{
			var nest = new Nest();
			Assert.Throws< NullRefNestException >( () => nest.Remove( null ) );
		}
		#endregion
	}
}
