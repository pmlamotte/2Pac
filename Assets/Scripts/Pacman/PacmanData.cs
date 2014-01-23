using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class PacmanData : MonoBehaviour {
	
	public float score = 0;
	public int playerNum = 0;
	public BoardLocation spawnPosition = new BoardLocation( new IntVector2( 1, 1), new IntVector2(0,0) );

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

	public PacmanData( )
	{
		Data.boardLocation = new BoardLocation( new IntVector2(0,0), new IntVector2(0,0) );
		Data.lastBoardLocation = Data.boardLocation.Clone();
	}

	
	public void hitByGhost( GameObject ghost )
	{
		this.Data.boardLocation = spawnPosition.Clone();
		SendMessage("PacmanHit");
	}

	[RPC] public void setPlayerNum(int num) {
		playerNum = num;
		if (networkView.isMine) {
			networkView.RPC("setPlayerNum", RPCMode.OthersBuffered, num);
		}
	}
	
	void Update () {
	}
}
