using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System;

public class BoardAccessor : MonoBehaviour {

	private BoardData _Data;
	public BoardData Data
	{
		get 
		{
			if ( _Data == null )
			{
				_Data = GameObject.FindObjectOfType<BoardData>();
			}
			return _Data;
		}
		private set { _Data = null; }
	}
	
	public int Height 
	{
		get { return Data.Height; }
		private set { throw new NotImplementedException(); }
	}
	public int Width
	{
		get { return Data.Width; }
		private set {throw new NotImplementedException(); }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isOpen( IntVector2 v )
	{
		return isOpen( v.x, v.y );
	}
	
	public bool isOpen( int x, int y )
	{
		if ( x < 0 ) return false;
		if ( y < 0 ) return false;
		if ( x >= Data.board.GetLength(1) ) return false;
		if ( y >= Data.board.GetLength(0) ) return false;
		return !Data.board[y,x];
	}
	
	
	public Vector3 convertToRenderPos( BoardLocation b )
	{
		float x = .5f + b.location.x + ((float)b.offset.x) / Constants.BoardCellRadius;
		float y = .5f + b.location.y + ((float)b.offset.y) / Constants.BoardCellRadius;
		return new Vector3( x, y, 0 );
	}
	
	
	public void insertGhost( GameObject g )
	{
		GhostMover gMove = (GhostMover)g.GetComponent<GhostMover>();
		gMove.Data.boardLocation = new BoardLocation( Data.ghostSpawn, new IntVector2( 0,0 ) );
	}
	
	
	public IntVector2 moveTowards( BoardLocation pos, BoardLocation target, int maxSpeed )
	{
		pos = pos.Clone();
		target = target.Clone();
		
		// get grid positions
		IntVector2 thisPos = pos.location;
		IntVector2 targetPos = target.location;
		
		HashSet<IntVector2> visited = new HashSet<IntVector2>();
		Dictionary<IntVector2, IntVector2> previous = new Dictionary<IntVector2, IntVector2>();
		
		LinkedList<IntVector2> queue = new LinkedList<IntVector2>();
		queue.AddLast( thisPos );
		
		while ( queue.Count > 0 )
		{
			IntVector2 p = queue.First.Value;
			queue.RemoveFirst();
			if ( p.Equals( targetPos ) ) break;
			foreach ( IntVector2 dir in Constants.directions )
			{
				IntVector2 posToVisit = dir + p;
				if ( !isOpen( posToVisit ) ) continue;
				if ( visited.Contains( posToVisit ) ) continue;
				visited.Add( posToVisit );
				previous[posToVisit] = p;
				queue.AddLast( posToVisit );
			}
		}
		
		LinkedList<IntVector2> path = new LinkedList<IntVector2>();
		IntVector2 curr = targetPos;
		while ( !curr.Equals( thisPos ) )
		{
			path.AddFirst( curr );
			curr = previous[curr];
		}
		
		// there is a path and its length is not 0
		if ( path.Count > 0 )
		{
			// head in the direction of first
			IntVector2 direction = new IntVector2( path.First.Value.x, path.First.Value.y ) - new IntVector2( thisPos.x, thisPos.y );
			direction.Normalize();
			direction *= maxSpeed;
			
			// direction will be orthogonal
			BoardLocation afterMove = tryMove( pos, direction );
			if ( BoardLocation.SqrDistance( afterMove, pos ) > maxSpeed * maxSpeed / 4 )
			{
				// move there
				return direction;
			}
		}
		//otherwise move towards center of current square
		IntVector2 cellPos = pos.offset;
		cellPos.Normalize();
		cellPos *= -maxSpeed;
		return cellPos;
		
	}
	
	public BoardLocation tryMove( BoardLocation pos, IntVector2 vel )
	{
		// the tile coordinates
		int x = pos.location.x;
		int y = pos.location.y;
		
		// the position within the tile, center is (0,0), +y is up
		int cellPosX = pos.offset.x;
		int cellPosY = pos.offset.y;
		
		cellPosX += vel.x;
		cellPosY += vel.y;
		
		// array representing whether there is a path to the right, up, left, and down respectively
		bool[] roads = new bool[4]{ isOpen(x + 1,y), 
			isOpen(x, y + 1), 
			isOpen(x - 1,y), 
			isOpen(x, y - 1)};
		
		// force position to an axis when there is not a road in the direction the position is off
		if ( cellPosX > 0 && !roads[0] ) cellPosX = 0;
		if ( cellPosX < 0 && !roads[2] ) cellPosX = 0;
		if ( cellPosY > 0 && !roads[1] ) cellPosY = 0;
		if ( cellPosY < 0 && !roads[3] ) cellPosY = 0;
		
		// force to the closer axis
		if ( Math.Abs( cellPosX ) > Math.Abs( cellPosY ) ) cellPosY = 0;
		else cellPosX = 0;
		
		// translate back to world coords and return
		return new BoardLocation( new IntVector2( x, y ), new IntVector2( cellPosX, cellPosY ));
		
	}
}
