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


	PacmanData[] Players
	{
		get 
		{
			return GameObject.FindObjectsOfType<PacmanData>();
		}
		set 
		{
			throw new NotImplementedException();
		}
	}

	GhostMover[] Ghosts
	{
		get 
		{
			return GameObject.FindObjectsOfType<GhostMover>();
		}
		set 
		{
			throw new NotImplementedException();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;
		PacmanData[] players = Players;
		GhostMover[] ghosts = Ghosts;
		foreach ( GhostMover ghost in ghosts )
		{
			foreach ( PacmanData player in players )
			{
				int collDistance = 3 * Constants.BoardCellDiameter / 4;
				if ( BoardLocation.SqrDistance( ghost.Data.boardLocation, player.Data.boardLocation ) < collDistance * collDistance )
				{
					if ( GameProperties.isSinglePlayer || player.networkView.isMine )
					{
						player.gameObject.GetComponent<PacmanMover>().hitByGhost();
					}
					else
					{
						player.networkView.RPC( "hitByGhost", player.networkView.owner );
					}
				}
			}
		}

		foreach ( PacmanData player in players )
		{
			foreach ( BoardObject pellet in Accessor.EatPelletsInRadius( player.Data.boardLocation, Constants.BoardCellRadius / 2 * 4 / 5 /*todo*/ ) )
			{
				player.gameObject.SendMessage( "AtePellet" );
				if ( GameProperties.isSinglePlayer )
				{
					Destroy(pellet.gameObject);
				}
				else 
				{
					Network.Destroy( pellet.gameObject );
				}
			}
		}
	}
}
