using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ServerManager : MonoBehaviour {

	public HostData[] hostList;
	
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
	
	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	void OnServerInitialized()
	{
		// todo
		//SpawnPlayer(playerNum);
	}
	


	private void RefreshHostList()
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
