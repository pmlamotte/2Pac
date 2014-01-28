using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	private BoardData _Data;
	public BoardData Data
	{
		get 
		{
			if ( _Data == null )
			{
				_Data = GetComponent<BoardData>();
			}
			return _Data;
		}
		private set { _Data = null; }
	}

	public void InitializeLevel()
	{
		Data.CreateBoard();

		PacmanMover[] players = GameObject.FindObjectsOfType<PacmanMover>();
		GhostMover[] ghosts = GameObject.FindObjectsOfType<GhostMover>();
		
		foreach ( PacmanMover player in players )
		{
			
		}

		int i = 0; 
		foreach ( GhostMover ghost in ghosts )
		{
			ghost.setGhostNumber( i );
			i++;
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
