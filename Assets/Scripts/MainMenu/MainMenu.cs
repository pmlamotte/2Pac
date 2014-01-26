using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private const int WIDTH = 400;
	public const int BUTTON_HEIGHT = 50;
	public GUIStyle style;

	// Use this for initialization
	void Start () {
		PlayerInfo playerInfo = PlayerInfo.Instance;
	}

	private int centeredXPos() {
		return Screen.width / 2 - WIDTH / 2;
	}

	void OnGUI() {

		GUI.Label(new Rect(centeredXPos(), 100, WIDTH, 75), new GUIContent("2PAC"), style);

		if (GUI.Button(new Rect(centeredXPos(), 225, WIDTH, BUTTON_HEIGHT), "Single Player")) {
			GetComponent<SinglePlayer>().enabled = true;
			this.enabled = false;
		}

		if (GUI.Button(new Rect(centeredXPos(), 325, WIDTH, BUTTON_HEIGHT), "Online Multiplayer")) {
			GetComponent<MultiplayerLobby>().enabled = true;
			this.enabled = false;
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
