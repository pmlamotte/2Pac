using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

/**
 * Handles the connection/disconnection of users
 **/
public class GameHost : MonoBehaviour {

	public int maxPlayers = 0;
	public List<PlayerInfo> players;

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

		for (int i = 0; i < players.Count; i++) {
			if (players[i].networkPlayer.guid == player.guid) {
				players.RemoveAt(i);
			}
		}
	}

	void OnPlayerConnected(NetworkPlayer player) {
		PlayerInfo info = new PlayerInfo(getNextPlayerID(), player);
		players.Add(info);
	}

	public void startServer(int maxPlayers, string serverName) {
		DontDestroyOnLoad(this);
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(Constants.GAME_NAME, serverName);
		GameProperties.isSinglePlayer = false;
	}

	private int getNextPlayerID() {

		SortedList<int, int> ids = new SortedList<int,int>();
		for (int i = 0; i < maxPlayers; i++) {
			ids.Add(i, i);
		}

		foreach (PlayerInfo info in players) {
			ids.Remove(info.id);
		}
		return ids[0];
	}

	public class PlayerInfo {
		public int id = -1;
		public NetworkPlayer networkPlayer;

		public PlayerInfo(int id, NetworkPlayer networkPlayer) {
			this.id = id;
			this.networkPlayer = networkPlayer;
		}
	}
}
