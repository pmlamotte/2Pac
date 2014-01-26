using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class BoardObjectCollider : MonoBehaviour {

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
		if ( !( GameProperties.isSinglePlayer && Network.isServer ) ) return;
		PacmanData[] players = Players;
		GhostMover[] ghosts = Ghosts;
		foreach ( GhostMover ghost in ghosts )
		{
			foreach ( PacmanData player in players )
			{
				if ( BoardLocation.SqrDistance( ghost.Data.boardLocation, player.Data.boardLocation ) < Constants.BoardCellRadius * Constants.BoardCellRadius )
				{
					player.hitByGhost( ghost.gameObject );
				}
			}

		}
	}
}
