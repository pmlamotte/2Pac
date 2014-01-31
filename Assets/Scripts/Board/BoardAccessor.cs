using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System;

public class BoardAccessor : MonoBehaviour {

	public BoardData Data;
	
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
	void Awake () {
		Data = GetComponent<BoardData>();
		Debug.Log( Data );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<BoardObject> EatPelletsInRadius(BoardLocation pos, int radius )
	{
		List<BoardObject> result = new List<BoardObject>();
		List<BoardObject> inSqare;
		if ( Data.Pellets.TryGetValue( pos.location, out inSqare ) )
		{
			foreach ( BoardObject g in inSqare )
			{
				int distance = BoardLocation.SqrDistance(g.boardLocation, pos );
				if ( distance <= radius * radius )
				{
					result.Add( g );
				}
			}
		}

		foreach (BoardObject g in result )
		{
			inSqare.Remove( g );
		}

		return result;
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
		float x = .5f + b.location.x + ((float)b.offset.x) / Constants.BoardCellDiameter;
		float y = .5f + b.location.y + ((float)b.offset.y) / Constants.BoardCellDiameter;
		return new Vector3( x, y, 0 );
	}
	
	public IntVector2 GetGhostSpawn( int num )
	{
		if ( Data.GhostSpawns.ContainsKey( num ) )
		{
			return Data.GhostSpawns[num];
		}
		else
		{
			return new IntVector2( 0, 0 );
		}
	}

	public IntVector2 GetPlayerSpawn( int num )
	{
		if ( Data.PlayerSpawns.ContainsKey( num ) )
		{
			return Data.PlayerSpawns[num];
		}
		else
		{
			return new IntVector2( 0, 0 );
		}
	}
	
	
	public IntVector2 moveTowards( BoardObject boardObjectData, BoardLocation target, int maxSpeed, out int distance )
	{
		return moveTowards( boardObjectData, target, maxSpeed, out distance, false );
	}

	public IntVector2 moveTowards( BoardObject boardObjectData, BoardLocation target, int maxSpeed, out int distance, bool canReverse )
	{
		BoardLocation pos = boardObjectData.boardLocation.Clone();
		target = target.Clone();
		
		// get grid positions
		IntVector2 thisPos = pos.location;
		IntVector2 targetPos = target.location;
		
		HashSet<IntVector2> visited = new HashSet<IntVector2>();
		Dictionary<IntVector2, IntVector2> previous = new Dictionary<IntVector2, IntVector2>();
		
		LinkedList<IntVector2> queue = new LinkedList<IntVector2>();
		queue.AddLast( thisPos );

		IntVector2 reverseDir = boardObjectData.direction.Normalized();
		reverseDir = reverseDir * -1;

		IntVector2 closestReachable = null;
		int closestDistance = int.MaxValue;
		
		while ( queue.Count > 0 )
		{
			IntVector2 p = queue.First.Value;
			queue.RemoveFirst();

			if ( IntVector2.OrthogonalDistance( p, target.location ) < closestDistance ) 
			{
				closestReachable = p.Clone();
				closestDistance = IntVector2.OrthogonalDistance( p, target.location );
			}

			if ( p.Equals( targetPos ) ) break;
			foreach ( IntVector2 dir in Constants.directions )
			{
				if ( !canReverse && p.Equals( pos.location ) && dir.Equals( reverseDir ) )
				{
					// not allowed
					continue;
				}
				IntVector2 posToVisit = dir + p;
				if ( !isOpen( posToVisit ) ) continue;
				if ( visited.Contains( posToVisit ) ) continue;
				visited.Add( posToVisit );
				previous[posToVisit] = p;
				queue.AddLast( posToVisit );
			}
		}
		
		LinkedList<IntVector2> path = new LinkedList<IntVector2>();
		IntVector2 curr = closestReachable;
		try
		{
		while ( !curr.Equals( thisPos ) && previous.ContainsKey( curr ) )
		{
			path.AddFirst( curr );
			curr = previous[curr];
		}
		}
		catch ( KeyNotFoundException e )
		{
			int x = 0;  
			x++;
		}

		distance = path.Count;
		
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
		//otherwise, we are at our target. Any move will suffice
		distance = int.MaxValue;
		
		return new IntVector2(0, 0);
		
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
