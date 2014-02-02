using UnityEngine;
using System.Collections;
using System;

public class GhostRenderer : MonoBehaviour {

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

	[RPC] public void AtePowerPellet( int playerNum )
	{
		if(  playerNum == GameProperties.myPlayer.id )
		{
			GetComponent<SpriteRenderer>().sprite = (Sprite) Resources.Load( "Media/Images/ghost_frightened", typeof(Sprite) );
			GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load( "Animations/GhostFrightened" );
		}
	}


	[RPC] public void SetGhostNumber( int num )
	{
		GetComponent<SpriteRenderer>().sprite = (Sprite) Resources.Load( "Media/Images/spritesheet_" + num, typeof(Sprite) );
		GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load( "Animations/Ghost" + num, typeof( RuntimeAnimatorController ) );
	}
	
	[RPC] public void powerPelletEat( int playerNum )
	{
		Data.PlayersCanEat.Add( playerNum );
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Data.transform.position = Board.convertToRenderPos( this.Data.boardLocation );
	}
}
