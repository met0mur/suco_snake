using System;
using System.Collections;
using System.Collections.Generic;

namespace SucoSnake.Core
{
	public interface ILinkedNode
	{
		#region Public Members
		INodeContent Init( INodeContentFactory factory );
		INodeContent GetContent();
		#endregion
	}

	public interface INodeContentFactory
	{
		#region Public Members
		INodeContent CreateContent( ILinkedNode parent );
		#endregion
	}

	public interface INodeContent
	{
		#region Public Members
		void SetParentNode( ILinkedNode parent );
		#endregion
	}

	public enum SquareNodeSides
	{
		Left,
		Right,
		Top,
		Bottom
	}

	public class NodeSideRunner< TSidesEnum, TNodeContent > : IEnumerator< LinkedNode< TSidesEnum, TNodeContent > >, IEnumerable< LinkedNode< TSidesEnum, TNodeContent > >
		where TSidesEnum : struct, IConvertible where TNodeContent : INodeContent
	{
		#region Private Fields
		private readonly TSidesEnum _side;
		private readonly LinkedNode< TSidesEnum, TNodeContent > _initialLinkedNode;
		private LinkedNode< TSidesEnum, TNodeContent > _currentLinkedNode;
		#endregion

		#region Properties
		public LinkedNode< TSidesEnum, TNodeContent > Current { get { return _currentLinkedNode; } }

		object IEnumerator.Current { get { return _currentLinkedNode; } }
		#endregion

		#region Constructors
		public NodeSideRunner( TSidesEnum side, LinkedNode< TSidesEnum, TNodeContent > initialLinkedNode )
		{
			_side = side;
			_initialLinkedNode = initialLinkedNode;
			_currentLinkedNode = initialLinkedNode;
		}
		#endregion

		#region Public Members
		public void Dispose()
		{
			_currentLinkedNode = null;
		}

		public bool MoveNext()
		{
			if( _currentLinkedNode == null )
			{
				_currentLinkedNode = _initialLinkedNode;
				return true;
			}

			if( _currentLinkedNode.HasNextNode( _side ) )
			{
				_currentLinkedNode = _currentLinkedNode.GetNextNode( _side );
				return true;
			}

			return false;
		}

		public void Reset()
		{
			_currentLinkedNode = null;
		}

		public IEnumerator< LinkedNode< TSidesEnum, TNodeContent > > GetEnumerator()
		{
			return this;
		}
		#endregion

		#region Interface Implementations
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}

	public class LinkedNode< TSidesEnum, TNodeContent > : ILinkedNode where TSidesEnum : struct, IConvertible where TNodeContent : INodeContent
	{
		#region Protected Fields
		protected Dictionary< TSidesEnum, LinkedNode< TSidesEnum, TNodeContent > > _nodes = new Dictionary< TSidesEnum, LinkedNode< TSidesEnum, TNodeContent > >();
		#endregion

		#region Private Fields
		private TNodeContent _content;
		#endregion

		#region Public Members
		public bool HasNextNode( TSidesEnum side )
		{
			return _nodes.ContainsKey( side );
		}

		public LinkedNode< TSidesEnum, TNodeContent > GetNextNode( TSidesEnum side )
		{
			return _nodes[ side ];
		}

		public virtual void SetNextNode( TSidesEnum side, LinkedNode< TSidesEnum, TNodeContent > linkedNode )
		{
			_nodes.Add( side, linkedNode );
		}

		public IEnumerable< LinkedNode< TSidesEnum, TNodeContent > > RunSide( TSidesEnum side )
		{
			return new NodeSideRunner< TSidesEnum, TNodeContent >( side, this );
		}

		public INodeContent Init( INodeContentFactory factory )
		{
			_content = ( TNodeContent ) factory.CreateContent( this );
			return _content;
		}

		public TNodeContent GetContent()
		{
			return _content;
		}
		#endregion

		#region Interface Implementations
		INodeContent ILinkedNode.GetContent()
		{
			return _content;
		}
		#endregion
	}

	public class SquareLinkedNode< TNodeContent > : LinkedNode< SquareNodeSides, TNodeContent > where TNodeContent : INodeContent
	{
		#region Public Members
		public SquareLinkedNode< TNodeContent > GetLeftNode()
		{
			return ( SquareLinkedNode< TNodeContent > ) GetNextNode( SquareNodeSides.Left );
		}

		public SquareLinkedNode< TNodeContent > GetRightNode()
		{
			return ( SquareLinkedNode< TNodeContent > ) GetNextNode( SquareNodeSides.Right );
		}

		public SquareLinkedNode< TNodeContent > GetTopNode()
		{
			return ( SquareLinkedNode< TNodeContent > ) GetNextNode( SquareNodeSides.Top );
		}

		public SquareLinkedNode< TNodeContent > GetBottomNode()
		{
			return ( SquareLinkedNode< TNodeContent > ) GetNextNode( SquareNodeSides.Bottom );
		}
		#endregion
	}
}
