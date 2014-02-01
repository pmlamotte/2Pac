using UnityEngine;
using System.Collections;

public class ServerComms : Singleton<ServerComms> {



	// load the mainmenu
	[RPC] public void ExitGame()
	{
		Application.LoadLevel( "MainMenu" );
	}



	[RPC] public void setPlayerID(int id) {
		Messenger<int>.Broadcast(Events.PLAYER_ID_SET, id);
	}

	[RPC] void setPlayerReadyChanged(int id, bool ready) {

		Messenger<int, bool>.Broadcast(Events.PLAYER_READY_CHANGED, id, ready);
	}

	[RPC] public void playerConnected(NetworkPlayer player, int id) {

		if (Network.isServer) {
			networkView.RPC("playerConnected", RPCMode.OthersBuffered, player, id);
		}

		if (GameProperties.myPlayer == null || GameProperties.myPlayer.id != id) {
			Messenger<NetworkPlayer, int>.Broadcast(Events.PLAYER_CONNECTED, player, id);
		}
	}

	[RPC] public void playerDisconnected(NetworkPlayer player) {

		if (Network.isServer) {
			networkView.RPC("playerDisconnected", RPCMode.OthersBuffered, player);
		}

		Messenger<NetworkPlayer>.Broadcast(Events.PLAYER_DISCONNECTED, player);
	}

	[RPC] public void setMaxPlayers(int maxPlayers) {
		GameProperties.maxPlayers = maxPlayers;
	}

	// todo just loads networked
	[RPC] public void LoadLevel (string level, int levelPrefix) {

		PlayerInfo.Instance.setPlayerStatusToLoading();
		if (Network.isServer) {
			ServerComms.Instance.networkView.RPC("LoadLevel", RPCMode.Others, "Networked", levelPrefix);
		}

		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);	
		
		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;
		
		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.
		Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(level);

		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);
	}

	/**
	 * Sent to the server with the player id 
	 **/
	[RPC] void finishedLoadingLevel(int id) {

		Debug.Log("received loaded message from id: " + id);
		PlayerInfo.Instance.setPlayerLoaded(id);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
