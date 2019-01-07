using System;
using System.Collections;
using System.Collections.Generic;

namespace SucoSnake.Core
{
	public class SpotException : Exception
	{
		#region Constructors
		public SpotException( string message = null ) : base( message )
		{
		}
		#endregion
	}

	public class SpotLinkAlreadyCreatedException : SpotException
	{
	}

	public class SpotLinkWithItselfException : SpotException
	{
	}

	public class SpotLinkAlreadyDeletedException : SpotException
	{
	}

	public class SpotLinkCorruptedException : SpotException
	{
	}

	public class SpotLinkSwapException : SpotException
	{
	}

	public class SpotLinkInsertException : SpotException
	{
	}

	public class SpotLink
	{
		#region Public Fields
		public Spot SpotA;
		public Spot SpotB;
		#endregion

		#region Public Members
		public Spot GetNext( Spot spot )
		{
			if( spot == null )
			{
				throw new ArgumentNullException();
			}

			if( SpotA == null || SpotB == null )
			{
				throw new SpotLinkCorruptedException();
			}

			if( !SpotA.Links.Contains( this ) || !SpotB.Links.Contains( this ) )
			{
				throw new SpotLinkAlreadyDeletedException();
			}

			if( spot == SpotA )
			{
				return SpotB;
			}

			if( spot == SpotB )
			{
				return SpotA;
			}

			return null;
		}

		public void Delete()
		{
			if( SpotA == null || SpotB == null )
			{
				throw new SpotLinkCorruptedException();
			}

			if( !SpotA.Links.Contains( this ) || !SpotB.Links.Contains( this ) )
			{
				throw new SpotLinkAlreadyDeletedException();
			}

			SpotA.Links.Remove( this );
			SpotB.Links.Remove( this );
		}
		#endregion
	}

	public class Spot : Nest
	{
		#region Public Fields
		public Boo.Lang.List< SpotLink > Links = new Boo.Lang.List< SpotLink >();
		#endregion

		#region Public Members
		public void CreateAndInsertLinkWith( Spot spot, Spot between )
		{
			if( between == null )
			{
				throw new ArgumentNullException();
			}

			if( between == this )
			{
				throw new SpotLinkWithItselfException();
			}

			var targetLink = GetLinkWith( between );
			if( targetLink == null )
			{
				throw new SpotLinkInsertException();
			}

			CreateLinkWith( spot );
			targetLink.Delete();
			between.CreateLinkWith( between );
		}

		public void CreateLinkWith( Spot spot )
		{
			if( spot == null )
			{
				throw new ArgumentNullException();
			}

			if( spot == this )
			{
				throw new SpotLinkWithItselfException();
			}

			foreach( var spotLink in Links )
			{
				if( spotLink.GetNext( this ) == spot )
				{
					throw new SpotLinkAlreadyCreatedException();
				}
			}

			var link = new SpotLink { SpotA = this, SpotB = spot };
			Links.Add( link );
			spot.Links.Add( link );
		}

		public SpotLink GetLinkWith( Spot spot )
		{
			if( spot == null )
			{
				throw new ArgumentNullException();
			}

			if( spot == this )
			{
				throw new SpotLinkWithItselfException();
			}

			foreach( var spotLink in Links )
			{
				if( spotLink.GetNext( this ) == spot )
				{
					return spotLink;
				}
			}

			return null;
		}

		public void ShiftLinksTo( Spot target, Spot exclude = null )
		{
			if( target == null )
			{
				throw new ArgumentNullException();
			}

			var targetLink = GetLinkWith( target );
			if( targetLink == null )
			{
				throw new SpotLinkSwapException();
			}

			var swapped = new List< Spot >();
			foreach( var link in Links.ToArray() )
			{
				var next = link.GetNext( this );
				if( next != exclude && next != target )
				{
					swapped.Add( link.GetNext( this ) );
					link.Delete();
				}
			}

			foreach( var spot in swapped )
			{
				target.CreateLinkWith( spot );
			}
		}
		#endregion
	}

	public class SpotRunner : IEnumerable< Spot >, IEnumerator< Spot >
	{
		#region Private Fields
		private readonly Spot _initialSpot;
		private Spot _lastSpot;
		#endregion

		#region Properties
		public Spot Current { get; private set; }

		object IEnumerator.Current { get { return Current; } }
		#endregion

		#region Constructors
		public SpotRunner( Spot spot )
		{
			_initialSpot = spot;
		}
		#endregion

		#region Public Members
		public IEnumerator< Spot > GetEnumerator()
		{
			return this;
		}

		public bool MoveNext()
		{
			if( Current == null )
			{
				Current = _initialSpot;
				return true;
			}

			foreach( var link in Current.Links )
			{
				var next = link.GetNext( Current );
				if( next != _lastSpot )
				{
					_lastSpot = Current;
					Current = next;
					return true;
				}
			}

			return false;
		}

		public void Reset()
		{
			Current = null;
			_lastSpot = null;
		}

		public void Dispose()
		{
			Reset();
		}
		#endregion

		#region Interface Implementations
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
