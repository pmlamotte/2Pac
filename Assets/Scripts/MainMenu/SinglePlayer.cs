using UnityEngine;
using System.Collections;

public class SinglePlayer : MonoBehaviour {

	public GUIStyle titleStyle;

	// Use this for initialization
	void Start () {

	}

	private int centeredXPos() {
		return Screen.width / 2 - MainMenu.WIDTH / 2;
	}

	void OnGUI() {

		GUI.Label(new Rect(centeredXPos(), 25, MainMenu.WIDTH, 75), new GUIContent("Player Select"), titleStyle);
		
		if (GUI.Button(new Rect(centeredXPos(), 150, MainMenu.WIDTH, MainMenu.BUTTON_HEIGHT), "1 Player")) {
			GameProperties.isSinglePlayer = true;
			GameProperties.maxPlayers = 1;
			Application.LoadLevel("Networked");
		}
		
		if (GUI.Button(new Rect(centeredXPos(), 250, MainMenu.WIDTH, MainMenu.BUTTON_HEIGHT), "2 Player")) {
			//TODO
		}

		if (GUI.Button(new Rect(Screen.width / 2 - MainMenu.BACK_WIDTH / 2, 375, MainMenu.BACK_WIDTH, MainMenu.BUTTON_HEIGHT), "Back")) {
			GetComponent<MainMenu>().enabled = true;
			this.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
