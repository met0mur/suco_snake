using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SucoSnake.Core;
using UnityEngine;

namespace Assets.Tests
{
	public class SpotTest
	{
		#region Public Members
		[ Test ]
		public void Initial()
		{
			var spot1 = new Spot();
			var spot2 = new Spot();
			var spot3 = new Spot();
			var spot4 = new Spot();
			var list = new Stack< Spot >();
			list.Push( spot4 );
			list.Push( spot3 );
			list.Push( spot2 );
			list.Push( spot1 );

			spot1.CreateLinkWith( spot2 );
			spot2.CreateLinkWith( spot3 );
			spot3.CreateLinkWith( spot4 );

			var runner = new SpotRunner( spot1 );
			foreach( var spot in runner )
			{
				Assert.AreEqual( list.Pop(), spot );
			}
		}

		[ Test ]
		public void InitByConfTest()
		{
			var field = InitByConf( new int[ 5, 5 ]
			{
				{ 1, 1, 0, 0, 1 }, 
				{ 0, 1, 0, 1, 1 }, 
				{ 1, 1, 1, 1, 0 }, 
				{ 0, 1, 0, 1, 0 }, 
				{ 0, 1, 1, 0, 0 }
			}, 5, 5 );

			Assert.IsNotNull( field[ 0, 0 ] );
			Assert.IsNotNull( field[ 1, 0 ] );
			Assert.IsNull( field[ 2, 0 ] );
			Assert.IsNull( field[ 3, 0 ] );
			Assert.IsNotNull( field[ 4, 0 ] );

			Assert.IsNotNull( field[ 0, 0 ].GetLinkWith( field[ 1, 0 ] ) );
			Assert.IsNotNull( field[ 1, 2 ].GetLinkWith( field[ 0, 2 ] ) );
			Assert.IsNotNull( field[ 1, 2 ].GetLinkWith( field[ 2, 2 ] ) );
			Assert.IsNotNull( field[ 1, 2 ].GetLinkWith( field[ 1, 1 ] ) );
			Assert.IsNotNull( field[ 1, 2 ].GetLinkWith( field[ 1, 3 ] ) );
			Assert.IsNull(field[1, 2].GetLinkWith(field[0, 0]));
		}

		public Spot[ , ] InitByConf( int[ , ] fieldConf, int w, int h )
		{
			Spot[ , ] field = new Spot[ h, w ];

			for( int x = 0; x < h; x++ )
			{
				for( int y = 0; y < w; y++ )
				{
					var value = fieldConf[ y, x ];
					if( value > 0 )
					{
						var spot = new Spot();
						field[ x, y ] = spot;

						if( x > 0 )
						{
							var left = field[ x - 1, y ];
							if( left != null )
							{
								spot.CreateLinkWith( left );
							}
						}

						if( y > 0 )
						{
							var top = field[ x, y - 1 ];
							if( top != null )
							{
								spot.CreateLinkWith( top );
							}
						}
					}
				}
			}

			return field;
		}

		[Test]
		public void PathRun()
		{
			var field = InitByConf(new int[5, 5]
			{
				{ 1, 1, 0, 0, 1 },
				{ 0, 1, 0, 1, 1 },
				{ 0, 1, 1, 1, 0 },
				{ 0, 0, 0, 1, 0 },
				{ 1, 1, 1, 1, 0 }
			}, 5, 5);
			
			var runner = new SpotRunner( field[0,0] );
			Assert.AreEqual( runner.Last() , field[ 4, 0 ] );
		}

		[Test]
		public void SwitchSimple()
		{
			var field = InitByConf(new int[5, 5]
			{
				{ 1, 1, 0, 0, 1 },
				{ 0, 1, 0, 1, 1 },
				{ 0, 1, 1, 1, 0 },
				{ 0, 0, 0, 1, 0 },
				{ 1, 1, 1, 1, 0 }
			}, 5, 5);

			Assert.IsNotNull( field[ 3, 2 ].GetLinkWith( field[ 2, 2 ] ) );
			Assert.IsNotNull( field[ 3, 2 ].GetLinkWith( field[ 3, 1 ] ) );
			Assert.IsNotNull( field[ 3, 2 ].GetLinkWith( field[ 3, 3 ] ) );

			field[ 3, 2 ].ShiftLinksTo( field[ 3, 3 ], field[ 2, 2 ] );

			Assert.IsNotNull( field[ 3, 2 ].GetLinkWith( field[ 2, 2 ] ) );
			Assert.IsNull( field[ 3, 2 ].GetLinkWith( field[ 3, 1 ] ) );
			Assert.IsNotNull( field[ 3, 2 ].GetLinkWith( field[ 3, 3 ] ) );

			Assert.IsNotNull( field[ 3, 3 ].GetLinkWith( field[ 3, 1 ] ) );
		}

		[Test]
		public void PathSwap()
		{
			var field = InitByConf(new int[7, 3]
			{
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 1, 1 },
				{ 1, 0, 0 },
				{ 1, 1, 1 },
				{ 1, 0, 0 },
				{ 1, 0, 0 }
			}, 7, 3);

			var runner = new SpotRunner( field[ 0, 0 ] );
			Assert.AreEqual( runner.Last(), field[ 0, 6 ] );
			Assert.IsNotNull( field[ 0, 0 ].GetLinkWith( field[ 0, 1 ] ) );
			Assert.IsNotNull( field[ 0, 2 ].GetLinkWith( field[ 1, 2 ] ) );
			Assert.IsNotNull( field[ 0, 4 ].GetLinkWith( field[ 1, 4 ] ) );

			Assert.AreEqual( 1, field[ 0, 0 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 1 ].Links.Count );
			Assert.AreEqual( 3, field[ 0, 2 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 3 ].Links.Count );
			Assert.AreEqual( 3, field[ 0, 4 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 5 ].Links.Count );

			var list = runner.ToArray();

			//TODO: replace this foreach by MOVE extend
			for( var i = 1; i < list.Length - 1; i++ )
			{
				list[ i ].ShiftLinksTo( list[ i - 1 ], list[ i + 1 ] );
			}

			Assert.AreEqual( 1, field[ 0, 0 ].Links.Count );
			Assert.AreEqual( 3, field[ 0, 1 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 2 ].Links.Count );
			Assert.AreEqual( 3, field[ 0, 3 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 4 ].Links.Count );
			Assert.AreEqual( 2, field[ 0, 5 ].Links.Count );

			Assert.IsNotNull( field[ 0, 1 ].GetLinkWith( field[ 1, 2 ] ) );
			Assert.IsNotNull( field[ 0, 3 ].GetLinkWith( field[ 1, 4 ] ) );

		}

		[Test]
		public void PathSwap2()
		{
			var field = InitByConf(new int[7, 3]
			{
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 1, 0 },
				{ 1, 1, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 }
			}, 7, 3);

			Assert.IsNotNull( field[ 0, 3 ].GetLinkWith( field[ 1, 3 ] ) );
			Assert.IsNotNull( field[ 0, 4 ].GetLinkWith( field[ 1, 4 ] ) );

			var runner = new SpotRunner( field[ 0, 0 ] );
			var list = runner.ToArray();
			for( var i = 1; i < list.Length - 1; i++ )
			{
				list[ i ].ShiftLinksTo( list[ i - 1 ], list[ i + 1 ] );
			}

			Assert.IsNotNull( field[ 0, 2 ].GetLinkWith( field[ 1, 3 ] ) );
			Assert.IsNotNull( field[ 0, 3 ].GetLinkWith( field[ 1, 4 ] ) );
		}

		[Test]
		public void PathSwap3()
		{
			var field = InitByConf(new int[7, 3]
			{
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 },
				{ 1, 0, 0 }
			}, 7, 3);

			var runner = new SpotRunner( field[ 0, 0 ] );
			var list = runner.ToArray();

			list[ 3 ].ShiftLinksTo( list[ 2 ] );

			Assert.IsNotNull( list[ 2 ].GetLinkWith( list[ 3 ] ) );
			Assert.IsNotNull( list[ 2 ].GetLinkWith( list[ 4 ] ) );
			Assert.IsNull( list[ 3 ].GetLinkWith( list[ 4 ] ) );
		}

		//TODO: insert case

		#endregion
	}
}
