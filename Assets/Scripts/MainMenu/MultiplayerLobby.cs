using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerLobby : MonoBehaviour {

	private GUIStyle serverListStyle = new GUIStyle();
	public GUIStyle titleStyle;

	private Vector2 scrollPosition = Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}

	void openServer()
	{
		ReadyUpToGame ready = GetComponent<ReadyUpToGame>();
		ready.enabled = true;
		enabled = false;
		GameProperties.isSinglePlayer = false;
	}

	void OnEnable() {

		scrollPosition = Vector2.zero;
		ServerManager.Instance.RefreshHostList();
		serverListStyle.normal.background = GraphicsUtil.MakeTexture(100, 1, new Color(0.0f, 0.0f, 0.0f, 0.1f));
		
	}

	void OnGUI() {

		GUI.Label(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 25, MainMenu.WIDTH, 20), "Mulitplayer Servers", titleStyle);

		GUI.Box(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 100, MainMenu.WIDTH, Screen.height - 250), "", serverListStyle);
		
		GUILayout.BeginArea(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 100, MainMenu.WIDTH, Screen.height - 125));

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, new GUILayoutOption[]{GUILayout.Width(400), GUILayout.Height(Screen.height - 250)});


		ServerManager manager = ServerManager.Instance;
		HostData[] hostList = manager.hostList;
		if (hostList != null)
		{
			for (int i = 0; i < hostList.Length; i++)
			{
				HostData game = hostList[i];
				if (hostList[i].comment.ToLower() == "closed" || game.playerLimit == game.connectedPlayers) {
					continue;
				}

				string text = "[" + game.connectedPlayers + "/" + game.playerLimit + "]   " + game.gameName;
				if (GUILayout.Button(text, new GUILayoutOption[]{GUILayout.MaxWidth(MainMenu.WIDTH), GUILayout.Height(30)})) 
				{
					GameProperties.serverName = hostList[i].gameName;
					NetworkConnectionError error = manager.JoinServer(hostList[i]);
					Debug.Log(error.ToString());
					if (error != NetworkConnectionError.NoError) {
						ServerManager.Instance.RefreshHostList();
					}
				}
			}
		}
		
		GUILayout.EndScrollView();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[]{GUILayout.MaxWidth(MainMenu.WIDTH)});
		if (GUILayout.Button("Refresh", new GUILayoutOption[]{GUILayout.Width(MainMenu.WIDTH / 2 - 50), GUILayout.Height(50)})) {
			scrollPosition = Vector2.zero;
			ServerManager.Instance.RefreshHostList();
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Create Server", new GUILayoutOption[]{GUILayout.Width(MainMenu.WIDTH / 2 - 50), GUILayout.Height(50)})) {
			GetComponent<ServerCreate>().enabled = true;
			enabled = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Back", new GUILayoutOption[]{GUILayout.Width(MainMenu.BACK_WIDTH), GUILayout.Height(MainMenu.BUTTON_HEIGHT)})) {
			GetComponent<MainMenu>().enabled = true;
			this.enabled = false;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void OnConnectedToServer() {
		openServer();
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
		ServerManager.Instance.RefreshHostList();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
