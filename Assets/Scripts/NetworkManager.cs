﻿
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class NetworkManager : MonoBehaviour {


	private string gameName = "";
	private int playerNum = 0;
	private int playerCount = 1;
	
	private void StartServer()
	{
		gameName = Random.value + "";
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		//MasterServer.RegisterHost(typeName, gameName);

	}

	void OnServerInitialized()
	{
		SpawnPlayer(playerNum);
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
				StartServer();
			}
		}
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived);
			//hostList = MasterServer.PollHostList();
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
		Debug.Log("player num: " + num);
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

		PacmanAnimate player = (PacmanAnimate)((GameObject) Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent( "PacmanAnimate" );
		
		PacmanAnimate animate = (PacmanAnimate) player.GetComponent("PacmanAnimate");
		animate.spawnPosition = pos;
		animate.maxSpeed = 8;
		animate.boardLocation = pos;

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
		animate.maxSpeed = 4;

	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
