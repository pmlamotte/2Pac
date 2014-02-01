using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using System;

public class GhostMover : MonoBehaviour {
	public BoardObject _Data;
	public BoardObject Data
	{
		get
		{
			if ( _Data == null )
			{
				_Data = GetComponent<BoardObject>();
			}
			return _Data;
		}
		private set { throw new NotImplementedException(); }
	}

	public BoardAccessor _Board;
	public BoardAccessor Board
	{
		get
		{
			if ( _Board == null )
			{
				_Board = GameObject.FindObjectOfType<BoardAccessor>();
			}
			return _Board;
		}
		private set {}
	}

	private GhostAI AI {get; set;}
	public int ghostNumber {get; set;}

	private Boolean HasTurned = false;


	// Use this for initialization
	void Start () {
		// position ghost
	}

	public void setGhostNumber( int num )
	{
		this.ghostNumber = num;
		GhostAIFactory ghostFactory = new GhostAIFactory( GameObject.FindObjectsOfType<PacmanData>(), Board );
		AI = ghostFactory.getGhostByNumber( num, Data );

		IntVector2 spawn = Board.GetGhostSpawn( num );
		this.Data.boardLocation = new BoardLocation( spawn, new IntVector2(0, 0 ) );
		this.Data.lastBoardLocation = this.Data.boardLocation.Clone();


	}

	public List<IntVector2> GetLegalTurns( int maxSpeed )
	{
		List<IntVector2> result = new List<IntVector2>();

		IntVector2 normalizedDirection = this.Data.direction.Normalized();
		
		IntVector2 newOffset = Data.boardLocation.offset + normalizedDirection;
		// ensure that turns are only allowed as "preturns" and not "postturns"

		IntVector2 reverseDirection = normalizedDirection * -1;
		foreach ( IntVector2 dir in Constants.directions )
		{
			// reverse direction is not allowed
			if ( dir.Equals( reverseDirection ) ) continue;
			
			IntVector2 dirToTry = dir * maxSpeed;

			// make sure there is an opening in that direction
			if ( !Board.isOpen( dir + this.Data.boardLocation.location ) ) continue;
			
			// try moving that way
			if ( BoardLocation.SqrDistance( Board.tryMove( this.Data.boardLocation, dirToTry ), Data.boardLocation ) > 0 )
			{
				result.Add( dir.Clone() );
			}
		}
		// if there is only one option, make sure it isn't the same direction already being traveled.
		// in that case, there's no point in determining to choose that direction.
		if ( result.Count == 1 && result[0].Equals( this.Data.direction.Normalized() ) )
		{
			result.Clear();
		}
		
		return result;
	}

	private Boolean canTurn( int maxSpeed )
	{
		// check greater than 1 ( going straight is always an option
		return GetLegalTurns( maxSpeed ).Count > 0;
	}


	// Update is called once per frame
	void Update () {
		if (!GameProperties.isSinglePlayer && !networkView.isMine) return;
		if ( AI == null ) return;
		int maxSpeed = (int)( Time.deltaTime * 1000 * this.Data.maxSpeed );
				
		if ( !Data.boardLocation.location.Equals( Data.lastBoardLocation.location ) )
		{
			HasTurned = false;
		}

		if ( canTurn( maxSpeed ) && !HasTurned )
		{
			HasTurned = true; // must turn
			if ( Data.boardLocation.location.Equals( new IntVector2( 6, 11 ) ) )
			{
				int x = 0;
				x++;
			}
			IntVector2 toMove = AI.ComputeDirection( GetLegalTurns( maxSpeed ), maxSpeed );
			toMove = toMove * maxSpeed;
			Data.boardLocation = Board.tryMove( Data.boardLocation, toMove );
			Data.direction = toMove;

			if ( Data.direction.x == 0 && Data.direction.y == 0 )
			{
				int x = 0;
				x++;
			}
		}
		else
		{
			Data.direction.Normalize();

			IntVector2 directionToMove = Data.direction * maxSpeed;
			Data.boardLocation = Board.tryMove( Data.boardLocation, directionToMove );
		}
		
	}
}
