using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ServerCreate : MonoBehaviour {

	public string serverName = "";
	const int WIDTH = 400;
	const int HEIGHT = 25;
	public ComboBox comboBox = new ComboBox();
	private GUIContent[] maxPlayerOpts;
	private GUIStyle listStyle = new GUIStyle();
	public GUIStyle titleStyle;

	// Use this for initialization
	void Awake () {
		maxPlayerOpts = new GUIContent[3];
		maxPlayerOpts[0] = new GUIContent("2");
		maxPlayerOpts[1] = new GUIContent("3");
		maxPlayerOpts[2] = new GUIContent("4");

		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background =
		listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left =
		listStyle.padding.right =
		listStyle.padding.top =
		listStyle.padding.bottom = 4;
	}

	void openServer()
	{
		ReadyUpToGame ready = GetComponent<ReadyUpToGame>();
		ready.enabled = true;
		enabled = false;
		GameProperties.isSinglePlayer = false;
	}

	void OnGUI() {
		GUI.Label(new Rect(Screen.width / 2 - WIDTH / 2, 25, WIDTH, HEIGHT), "Create Server", titleStyle);

		GUI.Label(new Rect(Screen.width / 2 - WIDTH / 2, 125, WIDTH, HEIGHT), "Server Name");
		serverName = GUI.TextField(new Rect(Screen.width / 2 - WIDTH / 2, 150, WIDTH, HEIGHT), serverName);

		GUI.Label(new Rect(Screen.width / 2 - WIDTH / 2, 200, WIDTH, HEIGHT), "Max Players");

		if (GUI.Button(new Rect(Screen.width / 2 - 250 / 2, 325, 250, 50), "Start Server")) {
			GameHost.Instance.startServer(comboBox.GetSelectedItemIndex() + 2, (serverName == null) ? "test: " + Constants.random.Next() : serverName);
			openServer();
		}

		int selectedItemIndex = comboBox.GetSelectedItemIndex();
		comboBox.List(new Rect(Screen.width / 2 - WIDTH / 2, 225, 75, HEIGHT), maxPlayerOpts[selectedItemIndex], maxPlayerOpts, listStyle);
	}

	// Update is called once per frame
	void Update () {

	}
}
