using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

/**
 * Handles the connection/disconnection of users
 **/
public class GameHost : Singleton<GameHost> {

	protected GameHost() {}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);

		ServerComms.Instance.playerDisconnected(player);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		if (PlayerInfo.Instance.players.Count == GameProperties.maxPlayers) {
			Network.CloseConnection(player, true);
			return;
		}

		int nextId = PlayerInfo.Instance.getNextPlayerID();
		ServerComms.Instance.networkView.RPC("setPlayerID", player, nextId);
		ServerComms.Instance.playerConnected(player, nextId);
		ServerComms.Instance.networkView.RPC("setMaxPlayers", player, GameProperties.maxPlayers);
	}

	public void startServer(int maxPlayers, string serverName) {
		DontDestroyOnLoad(this);
		GameProperties.isSinglePlayer = false;
		GameProperties.maxPlayers = maxPlayers;
		GameProperties.serverName = serverName;

		Network.maxConnections = maxPlayers;
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(Constants.GAME_NAME, serverName);
	}

	void OnServerInitialized() {
		int myId = PlayerInfo.Instance.getNextPlayerID();
		Messenger<int>.Broadcast(Events.PLAYER_ID_SET, myId);
		Network.Instantiate(Resources.Load<GameObject>("prefabs/ServerComms"), Vector3.zero, Quaternion.identity, 0);
		ServerComms.Instance.playerConnected(Network.player, myId);
	}

}
