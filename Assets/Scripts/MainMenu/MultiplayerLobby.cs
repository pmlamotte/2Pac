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
		serverListStyle.normal.background = MakeTex(100, 1, new Color(0.0f, 0.0f, 0.0f, 0.1f));
		
	}

	private Texture2D MakeTex(int width, int height, Color col) {
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++) {
			pix[i] = col;
		}
		
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	void OnGUI() {

		GUI.Label(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 25, MainMenu.WIDTH, 20), "Mulitplayer Servers", titleStyle);

		GUI.Box(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 100, MainMenu.WIDTH, Screen.height - 250), "", serverListStyle);
		
		GUILayout.BeginArea(new Rect(Screen.width / 2 - MainMenu.WIDTH / 2, 100, MainMenu.WIDTH, Screen.height - 175));

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
