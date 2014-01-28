using UnityEngine;
using System.Collections;
using AssemblyCSharp;

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
				Network.maxConnections = -1;
				MasterServer.RegisterHost(Constants.GAME_NAME, GameProperties.serverName, "Closed");
				ServerComms.Instance.LoadLevel("Networked", 1);
			}
		}

		if (GUI.Button(new Rect(Screen.width / 2 - MainMenu.BACK_WIDTH / 2, 375, MainMenu.BACK_WIDTH, MainMenu.BUTTON_HEIGHT), "Back")) {
			Network.Disconnect();
			GetComponent<MainMenu>().enabled = true;
			this.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
