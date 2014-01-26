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
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameProperties.isSinglePlayer && !networkView.isMine) return;
		int maxSpeed = this.Data.maxSpeed * (int)( Time.deltaTime * 1000 );

		IntVector2 toMove = new IntVector2(0,0);
		foreach ( PacmanData player in players )
		{
			// todo, will move to the last player in list
			IntVector2 direction = Board.moveTowards( Data.boardLocation, player.Data.boardLocation, maxSpeed );
			toMove = direction;
		}
		Data.boardLocation = Board.tryMove( Data.boardLocation, toMove );
		

	}
}
