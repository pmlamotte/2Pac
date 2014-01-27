using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using System;

public class GhostMover : MonoBehaviour {
	private PacmanData[] players;

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


	// Use this for initialization
	void Start () {
		players = GameObject.FindObjectsOfType<PacmanData>();
		// position ghost
		Data.boardLocation = new BoardLocation( new IntVector2( 9,3 ), new IntVector2(0,0));
		Data.lastBoardLocation = Data.boardLocation.Clone();
		Data.direction = new IntVector2( 0, 0 );
	}

	private Boolean canTurn( int maxSpeed )
	{
		IntVector2 normalizedDirection = this.Data.direction.Normalized();

		IntVector2 newOffset = Data.boardLocation.offset + normalizedDirection;
		// ensure that turns are only allowed as "preturns" and not "postturns"
		if ( Math.Abs( newOffset.x ) + Math.Abs( newOffset.y ) > 0 && Math.Abs( newOffset.x ) + Math.Abs( newOffset.y ) > Math.Abs( Data.boardLocation.offset.x ) + Math.Abs( Data.boardLocation.offset.y ) )
		{
			return false;
		}

		IntVector2 reverseDirection = normalizedDirection * -1;
		foreach ( IntVector2 dir in Constants.directions )
		{
			// same direction doesn't count as a turn
			// reverse direction is not allowed
			if ( dir.Equals( normalizedDirection ) || dir.Equals( reverseDirection ) ) continue;

			IntVector2 dirToTry = dir * maxSpeed;

			// try moving that way
			if ( BoardLocation.SqrDistance( Board.tryMove( this.Data.boardLocation, dirToTry ), Data.boardLocation ) > 0 )
			{
				return true;
			}
		}

		return false;
	}


	// Update is called once per frame
	void Update () {
		if (!GameProperties.isSinglePlayer && !networkView.isMine) return;
		int maxSpeed = (int)( Time.deltaTime * 1000 * this.Data.maxSpeed );

				
		if ( canTurn( maxSpeed ) )
		{
			IntVector2 toMove = new IntVector2(0,0);
			int minDistance = int.MaxValue;
			foreach ( PacmanData player in players )
			{
				// "targets" closest player
				int distance;
				IntVector2 direction = Board.moveTowards( Data, player.Data.boardLocation, maxSpeed, out distance );
				if ( distance < minDistance )
				{
					toMove = direction;
					minDistance = distance;
				}

			}
			Data.boardLocation = Board.tryMove( Data.boardLocation, toMove );
			Data.direction = toMove;
		}
		else
		{
			Data.direction.Normalize();
			Data.direction = Data.direction * maxSpeed;
			Data.boardLocation = Board.tryMove( Data.boardLocation, Data.direction );
			// set direction to 0 if couldn't move
			if ( BoardLocation.SqrDistance( Data.boardLocation, Data.lastBoardLocation ) == 0 )
			{
				Data.direction = new IntVector2( 0, 0 );
			}

		}
	}
}
