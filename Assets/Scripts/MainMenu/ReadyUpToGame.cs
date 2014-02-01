using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ReadyUpToGame : MonoBehaviour {

	public GUIStyle titleStyle;
	public int WIDTH = 250;
	public int BUTTON_HEIGHT = 30;
	public int NAME_WIDTH = 100;
	public int READY_WIDTH = 100;

	public void OnGUI()
	{
		PlayerInfo playerInfo = PlayerInfo.Instance;

		GUI.Label(new Rect(Screen.width / 2 - WIDTH / 2, 25, WIDTH, 20), GameProperties.serverName, titleStyle);

		GUILayout.BeginArea(new Rect(Screen.width / 2 - WIDTH / 2, 100, WIDTH, Screen.height - 100 - 150));
		for (int i = 0; i < GameProperties.maxPlayers; i++) {

			PlayerInfo.Player player = playerInfo.getPlayerByID(i);

			GUILayout.BeginHorizontal(new GUILayoutOption[]{GUILayout.Width(WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
			if (player != null) {
				if (player.id == GameProperties.myPlayer.id) {
					GUILayout.Box("You", new GUILayoutOption[]{GUILayout.Width(NAME_WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
					GUILayout.FlexibleSpace();
					if (GUILayout.Button(player.ready ? "Ready" : "Not ready", new GUILayoutOption[]{ GUILayout.Width( READY_WIDTH ), GUILayout.Height(BUTTON_HEIGHT)})) {
						playerInfo.toggleCurrentPlayerReady();
					}
				} else {
					GUILayout.Box("Connected", new GUILayoutOption[]{GUILayout.Width(NAME_WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
					GUILayout.FlexibleSpace();
					GUILayout.Box(player.ready ? "Ready" : "Not ready", new GUILayoutOption[]{GUILayout.Width(READY_WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
				}
			} else {
				GUILayout.Box("Not Connected", new GUILayoutOption[]{GUILayout.Width(NAME_WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
				GUILayout.FlexibleSpace();
				GUILayout.Box("Not ready", new GUILayoutOption[]{GUILayout.Width(READY_WIDTH), GUILayout.Height(BUTTON_HEIGHT)});
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Label("", new GUILayoutOption[]{GUILayout.Height(20)});

		if (Network.isServer && playerInfo.allPlayersReady()) {
			if (GUILayout.Button("Start Game", new GUILayoutOption[] {GUILayout.Height(BUTTON_HEIGHT)})) {
				Network.maxConnections = -1;
				MasterServer.RegisterHost(Constants.GAME_NAME, GameProperties.serverName, "Closed");
				ServerComms.Instance.LoadLevel("Networked", 1);
			}
		} else {
			GUILayout.Label("", new GUILayoutOption[]{GUILayout.Height(BUTTON_HEIGHT)});
		}

		GUILayout.Label("", new GUILayoutOption[]{GUILayout.Height(30)});

		if (GUILayout.Button("Back", new GUILayoutOption[]{GUILayout.Height(MainMenu.BUTTON_HEIGHT)})) {
			Network.Disconnect();
			ServerManager.Instance.disconnectedFromServer = false;
			GetComponent<MainMenu>().enabled = true;
			this.enabled = false;
		}
		GUILayout.EndArea();
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
