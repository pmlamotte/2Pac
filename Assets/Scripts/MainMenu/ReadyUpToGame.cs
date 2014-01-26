using UnityEngine;
using System.Collections;

public class ReadyUpToGame : MonoBehaviour {


	public void OnGUI()
	{
		PlayerInfo playerInfo = PlayerInfo.Instance;

		for (int i = 0; i < GameProperties.maxPlayers; i++) {

			PlayerInfo.Player player = playerInfo.getPlayerByID(i);

			if (player != null) {
				if (player.id == GameProperties.myPlayer.id) {
					GUI.Box (new Rect (250,200 + (i * 75),100,30), "You");
					if (GUI.Button(new Rect(375, 200 + (i * 75), 100,30), player.ready ? "Ready" : "Not ready")) {
						playerInfo.toggleCurrentPlayerReady();
					}
				} else {
					GUI.Box (new Rect (250,200 + (i * 75),100,30), "Connected");
					GUI.Box(new Rect(375, 200 + (i * 75), 100, 30), player.ready ? "Ready" : "Not ready");
				}
			} else {
				GUI.Box (new Rect (250,200 + (i * 75),100,30), "Not Connected");
				GUI.Box(new Rect(375, 200 + (i * 75), 100, 30), "Not ready");
			}
		}

		if (Network.isServer && playerInfo.allPlayersReady()) {
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Game")) {
				ServerComms.Instance.networkView.RPC("LoadLevel", RPCMode.AllBuffered, "Networked", 1);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
