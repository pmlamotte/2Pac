using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Level : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;
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
	
	PacmanData SpawnPlayer()
	{
		PacmanData player = null;
		if ( GameProperties.isSinglePlayer )
		{
			player = (PacmanData)((GameObject) Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity)).GetComponent<PacmanData>();
			player.gameObject.GetComponent<PlayerIcon>().enabled = false;
		}
		else
		{
			player = ((GameObject)Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<PacmanData>();
		}
		player.maxSpeed = 15;
		return player;
		
	}

	private GhostMover SpawnGhost()
	{	
		GhostMover ghost = null;
		if( GameProperties.isSinglePlayer )
		{
			ghost = ((GameObject) Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity)).GetComponent<GhostMover>();
		}
		else
		{
			ghost = ((GameObject)Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<GhostMover>();
		}
		ghost.Data.maxSpeed = 14;
		return ghost;
	}

	
	public void InitializeLevel()
	{
		Time.timeScale = 0;
		Data.CreateBoard();


		if ( Network.isServer )
		{
			// spawn players
			foreach (PlayerInfo.Player player in PlayerInfo.Instance.players) {
				PacmanData playerObject = SpawnPlayer();
				playerObject.setPlayerNum( player.id );
				playerObject.boardLocation = new BoardLocation( Data.PlayerSpawns[player.id], new IntVector2(0,0) );
				playerObject.lastBoardLocation = playerObject.boardLocation.Clone();
			}
		}
		else if ( GameProperties.isSinglePlayer )
		{
			// spawn player
			PacmanData playerObject = SpawnPlayer();
			playerObject.setPlayerNum( 0 );
			playerObject.boardLocation = new BoardLocation( Data.PlayerSpawns[0], new IntVector2( 0, 0 ) );
			playerObject.lastBoardLocation = playerObject.boardLocation.Clone();
		}

		if ( GameProperties.isSinglePlayer || Network.isServer )
		{
			foreach ( int ghostNum in Data.GhostSpawns.Keys )
			{
				GhostMover ghost = SpawnGhost();
				if ( GameProperties.isSinglePlayer )
				{
					ghost.BroadcastMessage( "SetGhostNumber", ghostNum );
				}
				else
				{
					ghost.networkView.RPC( "SetGhostNumber", RPCMode.All, ghostNum );
				}

				ghost.setGhostNumber( ghostNum );
				ghost.Data.boardLocation = new BoardLocation( Data.GhostSpawns[ghostNum], new IntVector2( 0, 0 ) );
				ghost.Data.lastBoardLocation = ghost.Data.boardLocation.Clone();
			}
		}

		GetComponent<BoardRenderer>().CreateBoard();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
