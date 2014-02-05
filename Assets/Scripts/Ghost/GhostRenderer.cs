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
	
	Sprite normalSprite;
	RuntimeAnimatorController normalAnimatorController;

	Sprite frightenedSprite;
	RuntimeAnimatorController frightenedAnimatorController;


	void OnEnable()
	{
		Messenger<int>.AddListener( Events.PACMAN_ATE_POWER_PELLET, OnAtePowerPellet );
		Messenger<int>.AddListener( Events.POWER_PELLET_FINISHED, OnPowerPelletFinished );
	}
	void OnDisable()
	{
		Messenger<int>.RemoveListener( Events.PACMAN_ATE_POWER_PELLET, OnAtePowerPellet );
		Messenger<int>.RemoveListener( Events.POWER_PELLET_FINISHED, OnPowerPelletFinished );
	}


	public void OnPowerPelletFinished(int playerNum )
	{
		if ( Data.PlayersCanEat.Count == 0 )
		{
			// return to normal animation
			GetComponent<SpriteRenderer>().sprite = normalSprite;
			GetComponent<Animator>().runtimeAnimatorController = normalAnimatorController;
		}
	}

    public void OnAtePowerPellet( int playerNum )
	{
		if( GameProperties.isSinglePlayer || playerNum == GameProperties.myPlayer.id )
		{
			frightenedSprite= (Sprite) Resources.Load( "Media/Images/ghost_frightened", typeof(Sprite) );
			GetComponent<SpriteRenderer>().sprite = frightenedSprite;
			frightenedAnimatorController = (RuntimeAnimatorController) Resources.Load( "Animations/GhostFrightened" );
			GetComponent<Animator>().runtimeAnimatorController = frightenedAnimatorController;
		}
	}


	[RPC] public void SetGhostNumber( int num )
	{
		normalSprite = (Sprite) Resources.Load( "Media/Images/spritesheet_" + num, typeof(Sprite) );
		GetComponent<SpriteRenderer>().sprite = normalSprite;
		normalAnimatorController = (RuntimeAnimatorController) Resources.Load( "Animations/Ghost" + num, typeof( RuntimeAnimatorController ) );
		GetComponent<Animator>().runtimeAnimatorController = normalAnimatorController;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Data.transform.position = Board.convertToRenderPos( this.Data.boardLocation );
	}
}
