using UnityEngine;
using System.Collections;
using System;

public class GhostRenderer : MonoBehaviour {

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

	[RPC] public void SetGhostNumber( int num )
	{
		GetComponent<SpriteRenderer>().sprite = (Sprite) Resources.Load( "Media/spritesheet_" + num, typeof(Sprite) );
		GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController) Resources.Load( "Animations/Ghost" + num, typeof( RuntimeAnimatorController ) );
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Data.transform.position = Board.convertToRenderPos( this.Data.boardLocation );
	}
}
