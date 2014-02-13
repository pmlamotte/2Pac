using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MainMenu : MonoBehaviour {

	public const int WIDTH = 400;
	public const int BUTTON_HEIGHT = 50;
	public const int BACK_WIDTH = 100;
	public const int POPUP_HEIGHT = 150;
	public GUIStyle style;

	// Use this for initialization
	void Start () {
	}

	void OnEnable() {
		GameProperties.myPlayer = null;
		PlayerInfo.Instance.clear();
		GameData.Instance.Clear();
	}

	private int centeredXPos() {
		return Screen.width / 2 - WIDTH / 2;
	}

	private int centeredXPos(int width) {
		return Screen.width / 2 - width / 2;
	}

	private int centeredYPos(int height) {
		return Screen.height / 2 - height / 2;
	}

	void OnGUI() {



		bool window = false;
		if (ServerManager.Instance.disconnectedFromServer) {
			window = true;
			GUI.Window(0, new Rect(centeredXPos(), centeredYPos(POPUP_HEIGHT), MainMenu.WIDTH, POPUP_HEIGHT), DisconnectedPopup, new GUIContent("Disconnected"));
		}

		GUI.Label(new Rect(centeredXPos(), 100, WIDTH, 75), new GUIContent("2PAC"), style);

		if (!window) {
			if (GUI.Button(new Rect(centeredXPos(), 225, WIDTH, BUTTON_HEIGHT), "Play Game")) {
				GetComponent<SinglePlayer>().enabled = true;
				this.enabled = false;
			}

			if (GUI.Button(new Rect(centeredXPos(), 325, WIDTH, BUTTON_HEIGHT), "Online Multiplayer")) {
				GetComponent<MultiplayerLobby>().enabled = true;
				this.enabled = false;
			}
		}

	}

	void DisconnectedPopup(int id) {
		// this should be updated to use guilayout so it's not absolute positioning
		GUI.Label(new Rect(70,45,MainMenu.WIDTH,POPUP_HEIGHT), "You have been disconnected from the server");
		if (GUI.Button(new Rect(MainMenu.WIDTH / 2 - 75 / 2, POPUP_HEIGHT - 40 - 20, 75, 40), "Continue")) {
			// eww, wonder if we could get some event to fire off when the scene loads so this could be handled better
			ServerManager.Instance.disconnectedFromServer = false;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
