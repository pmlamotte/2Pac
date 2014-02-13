using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AssemblyCSharp;
using System;

public class GhostMover : MonoBehaviour {

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

	
	public GhostData _Data;
	public GhostData Data
	{
		get
		{
			if ( _Data == null )
			{
				_Data = GetComponent<GhostData>();
			}
			return _Data;
		}
		private set { throw new NotImplementedException(); }
	}


	private GhostAI AI {get; set;}


	// Use this for initialization
	void Start () {
	}

	[RPC] public void AtePowerPellet( int playerNum )
	{
		Messenger<int>.Broadcast( Events.PACMAN_ATE_POWER_PELLET, playerNum );

		Data.direction = Data.direction * -1;
		Data.HasTurned = false;

		lock ( Data.PlayersCanEat )
		{
			Data.PlayersCanEat.AddLast( playerNum );
		}

		StartCoroutine( PowerPelletTimeOut( playerNum ) );

	}

	private IEnumerator PowerPelletTimeOut( int playerNum )
	{
		yield return new WaitForSeconds( Constants.PowerPelletTime );
		lock( Data.PlayersCanEat )
		{
			if ( Data.PlayersCanEat.Count > 0 )
			{
				Data.PlayersCanEat.RemoveFirst();
			}
		}

		Messenger<int>.Broadcast( Events.POWER_PELLET_FINISHED, playerNum );
	}



	[RPC] public void hitByPacman()
	{
		Time.timeScale = 0;
		StartCoroutine( EatGhostPause() );

		lock ( Data.PlayersCanEat )
		{
			Data.PlayersCanEat.Clear();
		}
		Messenger<int>.Broadcast( Events.POWER_PELLET_FINISHED, -1 );

		if ( GameProperties.isSinglePlayer || Network.isServer )
		{
			this.Board.resetGhost( this );
		}
	}

	private IEnumerator EatGhostPause()
	{
		yield return StartCoroutine( CoroutineUtil.WaitForRealSeconds( 1.0f ) );
		Time.timeScale = 1.0f;
	}

	public void setGhostNumber( int num )
	{
		Data.ghostNumber = num;
		GhostAIFactory ghostFactory = new GhostAIFactory( GameObject.FindObjectsOfType<PacmanData>(), Board );
		AI = ghostFactory.getGhostByNumber( num, Data );

		IntVector2 spawn = Board.GetGhostSpawn( num );
		Data.boardLocation = new BoardLocation( spawn, new IntVector2(0, 0 ) );
		Data.lastBoardLocation = Data.boardLocation.Clone();


	}

	public List<IntVector2> GetLegalTurns( int maxSpeed )
	{
		List<IntVector2> result = new List<IntVector2>();

		IntVector2 normalizedDirection = Data.direction.Normalized();
		
		IntVector2 reverseDirection = normalizedDirection * -1;
		foreach ( IntVector2 dir in Constants.directions )
		{
			// reverse direction is not allowed
			if ( dir.Equals( reverseDirection ) ) continue;
			
			IntVector2 dirToTry = dir * maxSpeed;

			// make sure there is an opening in that direction
			if ( !Board.isOpen( dir + Data.boardLocation.location ) ) continue;
			
			// try moving that way
			if ( BoardLocation.SqrDistance( Board.tryMove( Data.boardLocation, dirToTry ), Data.boardLocation ) > 0 )
			{
				result.Add( dir.Clone() );
			}
		}
		// if there is only one option, make sure it isn't the same direction already being traveled.
		// in that case, there's no point in determining to choose that direction.
		if ( result.Count == 1 && result[0].Equals( Data.direction.Normalized() ) )
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
		int maxSpeed = (int)( Time.deltaTime * 1000 * Data.maxSpeed );
				
		if ( Data.PlayersCanEat.Count > 0 )
		{
			maxSpeed /= 2;
		}


		if ( !Data.boardLocation.location.Equals( Data.lastBoardLocation.location ) )
		{
			Data.HasTurned = false;
		}

		if ( canTurn( maxSpeed ) && !Data.HasTurned )
		{
			Data.HasTurned = true; // must turn
			if ( Data.boardLocation.location.Equals( new IntVector2( 6, 11 ) ) )
			{
				int x = 0;
				x++;
			}
			IntVector2 toMove = AI.ProcessTurn( GetLegalTurns( maxSpeed ), maxSpeed );
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
