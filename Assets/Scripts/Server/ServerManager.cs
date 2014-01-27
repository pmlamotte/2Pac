using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ServerManager : Singleton<ServerManager> {

	public HostData[] hostList;

	protected ServerManager() {}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
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
	
	public NetworkConnectionError JoinServer(HostData hostData)
	{
		return Network.Connect(hostData);
	}
	void OnServerInitialized()
	{
		// todo
		//SpawnPlayer(playerNum);
	}
	


	public void RefreshHostList()
	{
		MasterServer.RequestHostList(Constants.GAME_NAME);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
