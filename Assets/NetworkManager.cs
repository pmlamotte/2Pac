using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "2Pac";
	private const string gameName = "2Pac Test Room";
	
	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);

	}

	void OnServerInitialized()
	{
		SpawnPlayer();
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


	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		SpawnPlayer();
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	} 
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	
	private void SpawnPlayer()
	{
		Vector3 pos = new Vector3(0,0,0);
		switch (Network.connections.Length) {
		case 0: pos.x = 1.5f; pos.y = 1.5f; break;
		case 1: pos.x = 17.5f; pos.y = 1.5f; break;
		case 2: pos.x = 1.5f; pos.y = 8.5f; break;
		case 3: pos.x = 17.5f; pos.y = 8.5f; break;
		}
		
		GameObject player = (GameObject) Network.Instantiate(playerPrefab, pos, Quaternion.identity, 0);
		
		
		PacmanAnimate animate = (PacmanAnimate) player.GetComponent("PacmanAnimate");
		animate.maxSpeed = 0.0049f;
		animate.direction.x = 0;
		player.transform.position = pos;
		OnStart onStart =(OnStart)GameObject.Find( "StartUp" ).GetComponent( "OnStart" );
		onStart.players.Add( player );
		SpawnGhost();
	}

	private void SpawnGhost()
	{
		Vector3 pos = new Vector3(0,0,0);
		switch (Network.connections.Length) {
		case 0: pos.x = 1.5f; pos.y = 1.5f; break;
		case 1: pos.x = 17.5f; pos.y = 1.5f; break;
		case 2: pos.x = 1.5f; pos.y = 8.5f; break;
		default: pos.x = 17.5f; pos.y = 8.5f; break;
		} // todo spawn location
		
		GameObject player = (GameObject) Network.Instantiate(ghostPrefab, pos, Quaternion.identity, 0);
		
		
		GhostMovement animate = (GhostMovement) player.GetComponent("GhostMovement");
		animate.maxSpeed = 0.03f;
		player.transform.position = pos;

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
