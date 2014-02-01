
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class NetworkManager : MonoBehaviour {


	private string gameName = "";
	private int playerNum = 0;
	private int playerCount = 1;


	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	[RPC] void SpawnPlayer(int num)
	{
		playerNum = num;
		BoardLocation pos = new BoardLocation( new IntVector2(0,0), new IntVector2( 0, 0 ) );
		int x = 1;
		int y = 1;
		switch (num) 
		{
			case 0: x = 1; y = 1; break;
			case 1: x = 17; y = 1; break;
			case 2: x = 1; y = 8; break;
			case 3: x = 17; y = 8; break;
		}

		pos = new BoardLocation( new IntVector2( x, y ), pos.offset );

		PacmanData player = (PacmanData)((GameObject) Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<PacmanData>();
		
		PacmanData animate = player.GetComponent<PacmanData>();
		animate.Data.maxSpeed = 8;
		animate.Data.boardLocation = pos;

		animate.setPlayerNum(playerNum);

		SpawnGhost();
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			//hostList = MasterServer.PollHostList();
		}
	}
	
	
	void OnPlayerConnected(NetworkPlayer player) {
		networkView.RPC("SpawnPlayer", player, playerCount);
		playerCount++;
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	} 

	private void SpawnGhost()
	{	
		GhostMover ghost = ((GameObject)Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<GhostMover>();
		ghost.setGhostNumber( 0 );

		GhostMover animate = ghost.GetComponent<GhostMover>();
		animate.Data.maxSpeed = 4;

	}

	// Use this for initialization
	void Start () {
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		
		SpawnPlayer(0);
		SpawnGhost();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
