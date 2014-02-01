using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class BoardObjectCollider : MonoBehaviour {
	
	public BoardAccessor _Board;
	public BoardAccessor Accessor
	{
		get
		{
			if ( _Board == null )
			{
				_Board = GetComponent<BoardAccessor>();
			}
			return _Board;
		}
		private set {}
	}


	GameObject[] Players
	{
		get 
		{
			return GameObject.FindGameObjectsWithTag("Pacman");
		}
		set 
		{
			throw new NotImplementedException();
		}
	}

	GameObject[] Ghosts
	{
		get 
		{
			return GameObject.FindGameObjectsWithTag( "Ghost" );
		}
		set 
		{
			throw new NotImplementedException();
		}
	}

	// Use this for initialization
	void Start () {
	}

	[RPC] public void NotifyGameOver()
	{
		GameObject.Find("StartUp").GetComponent<LevelLost>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;
		GameObject[] players = Players;
		GameObject[] ghosts = Ghosts;
		foreach ( GameObject ghostObject in ghosts )
		{
			GhostMover ghost = ghostObject.GetComponent<GhostMover>();
			foreach ( GameObject playerObject in players )
			{
				PacmanData player = playerObject.GetComponent<PacmanData>();
				int collDistance = 3 * Constants.BoardCellDiameter / 4;
				int distance = BoardLocation.OrthogonalDistance( ghost.Data.boardLocation, player.Data.boardLocation );
				if ( distance < collDistance )
				{
					if ( GameProperties.isSinglePlayer )
					{
						player.gameObject.GetComponent<PacmanMover>().hitByGhost();
					}
					else
					{
						player.networkView.RPC( "hitByGhost", RPCMode.All );
					}
				}
			}
		}

		foreach ( GameObject playerObject in players )
		{
			PacmanData player = playerObject.GetComponent<PacmanData>();
			foreach ( BoardObject pellet in Accessor.EatPelletsInRadius( player.Data.boardLocation, Constants.BoardCellRadius / 2 * 4 / 5 /*todo*/ ) )
			{


				if ( GameProperties.isSinglePlayer )
				{
					player.gameObject.SendMessage( "AtePellet" );
					Destroy(pellet.gameObject);
				}
				else 
				{
					player.gameObject.networkView.RPC( "AtePellet", RPCMode.All );
					Network.Destroy( pellet.gameObject );
				}
			}
		}

		
		if ( GameData.Instance.PlayerLives <= 0 )
		{
			// game over
			if ( GameProperties.isSinglePlayer )
			{
				NotifyGameOver();
			}
			else
			{
				this.networkView.RPC( "NotifyGameOver", RPCMode.All );
			}
		}
	}
}
