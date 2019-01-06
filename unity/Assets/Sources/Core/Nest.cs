using System;
using System.Collections.Generic;
using System.Linq;

namespace SucoSnake.Core
{
	public class NestException : Exception
	{
		#region Constructors
		public NestException( string message ) : base( message )
		{
		}
		#endregion
	}

	public class NullRefNestException : NestException
	{
		#region Constructors
		public NullRefNestException(string message) : base(message)
		{
		}
		#endregion
	}

	public class ContainNestException : NestException
	{
		#region Constructors
		public ContainNestException(string message) : base(message)
		{
		}
		#endregion
	}


	public delegate void NestEggEvent( NestEggEventArgs args );

	public enum NestEggEventType
	{
		Added,
		Removed,
		Moved,
		Custom
	}

	public class NestEggEventArgs
	{
		#region Public Fields
		public NestEggEventType Type;
		public Nest From;
		public Nest To;
		public Egg Self;
		public bool Bubble;
		public bool Stopped;
		#endregion
	}

	public class Nest : Egg
	{
		#region Private Fields
		private List< Egg > _eggs = new List< Egg >();
		#endregion

		#region Public Members
		public void Add( Egg egg )
		{
			if( egg == null )
			{
				throw new NullRefNestException( string.Format( "Nest '{0}' got empty egg", GetName() ) );
			}

			if( _eggs.Contains( egg ) )
			{
				throw new ContainNestException( string.Format( "Nest '{0}' already contains egg '{1}'", GetName(), egg.GetFullName() ) );
			}

			_eggs.Add( egg );
			egg.Knock += EggOnKnock;
			egg.SetNest( null );
			egg.SetNest( this );
		}

		public void Remove( Egg egg )
		{
			if( egg == null )
			{
				throw new NullRefNestException( string.Format( "Nest {0} got empty egg", GetName() ) );
			}

			if( !_eggs.Contains( egg ) )
			{
				throw new ContainNestException( string.Format( "Nest {0} does not contains egg {1}", GetName(), egg.GetFullName() ) );
			}

			_eggs.Remove( egg );
			egg.SetNest( null );
			egg.Knock -= EggOnKnock;
		}

		public void MoveTo( Egg egg, Nest nest )
		{
			if( egg == null )
			{
				throw new NullRefNestException( string.Format( "Nest {0} got empty egg", GetName() ) );
			}

			if( nest == null )
			{
				throw new NullRefNestException( string.Format( "Nest {0} got empty nest", GetName() ) );
			}

			if( !_eggs.Contains( egg ) )
			{
				throw new ContainNestException( string.Format( "Nest {0} does not contains egg {1}", GetName(), egg.GetFullName() ) );
			}

			_eggs.Remove( egg );
			egg.SetNest( nest );
			egg.Knock -= EggOnKnock;
		}

		public void Remove< T >() where T : Egg
		{
			var egg = GetNested< T >();

			if( egg != null )
			{
				Remove( egg );
			}
		}

		public T GetNested< T >() where T : Egg
		{
			var list = _eggs.OfType< T >().ToList();
			return list.Any() ? list.First() : null;
		}
		#endregion

		#region Private Members
		private void EggOnKnock( NestEggEventArgs args )
		{
			if( args.Bubble )
			{
				InvokeKnock( args );
			}
		}
		#endregion
	}

	public class Egg
	{
		#region Public Fields
		public string Name;
		#endregion

		#region Properties
		public Nest Nest { get; private set; }
		#endregion

		#region Events
		public event NestEggEvent Knock;
		#endregion

		#region Public Members
		public void SetNest( Nest nest )
		{
			NestEggEventType type;
			var lastNest = Nest;
			Nest = nest;
			if( lastNest != null && nest != null )
			{
				type = NestEggEventType.Moved;
			}
			else if( lastNest == null && nest != null )
			{
				type = NestEggEventType.Added;
			}
			else if( lastNest != null && nest == null )
			{
				type = NestEggEventType.Removed;
			}
			else
			{
				return;
			}

			InvokeKnock( new NestEggEventArgs { Self = this, Type = type, From = lastNest, To = nest, Bubble = true } );
		}

		public string GetName()
		{
			return Name ?? GetType().Name;
		}

		public string GetFullName()
		{
			var name = GetName();
			if( Nest == null )
			{
				return "lonely-" + name;
			}

			return Nest.GetFullName() + "." + name;
		}
		#endregion

		#region Protected Members
		protected void InvokeKnock( NestEggEventArgs args )
		{
			if( Knock != null )
			{
				Knock.Invoke( args );
			}
		}
		#endregion
	}
}
