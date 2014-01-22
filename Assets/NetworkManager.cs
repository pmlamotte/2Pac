using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "2Pac";
	private string gameName = "";
	private int playerNum = 0;
	private int playerCount = 1;
	
	private void StartServer()
	{
		gameName = Random.value + "";
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);

	}

	void OnServerInitialized()
	{
		SpawnPlayer(playerNum);
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
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
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	[RPC] void SpawnPlayer(int num)
	{
		playerNum = num;
		Vector3 pos = new Vector3(0,0,0);
		switch (Network.connections.Length) {
		case 0: pos.x = 1.5f; pos.y = 1.5f; break;
		case 1: pos.x = 17.5f; pos.y = 1.5f; break;
		case 2: pos.x = 1.5f; pos.y = 8.5f; break;
		case 3: pos.x = 17.5f; pos.y = 8.5f; break;
		}
		
		GameObject player = (GameObject) Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
		
		PacmanAnimate animate = (PacmanAnimate) player.GetComponent("PacmanAnimate");
		animate.spawnPosition = pos;
		animate.maxSpeed = 0.0049f;
		animate.direction.x = 0;
		player.transform.position = pos;
		OnStart onStart =(OnStart)GameObject.Find( "StartUp" ).GetComponent( "OnStart" );
		onStart.players.Add( player );

		animate.setPlayerNum(playerNum);

		SpawnGhost();
	}

	private void SpawnGhost()
	{	
		GameObject ghost = (GameObject) Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
		
		OnStart.board.insertGhost( ghost );
		GhostMovement animate = (GhostMovement) ghost.GetComponent("GhostMovement");
		animate.maxSpeed = 0.03f;

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
