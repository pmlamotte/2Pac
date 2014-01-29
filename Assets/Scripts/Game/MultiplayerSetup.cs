using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerSetup : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;
	public GUIStyle waitingStyle;

	// Use this for initialization
	void Start () {
		ServerComms.Instance.networkView.RPC("finishedLoadingLevel", RPCMode.All, GameProperties.myPlayer.id);
		Time.timeScale = 0;
	}

	void setupGame() {
		GameObject.FindObjectOfType<Level>().InitializeLevel();
		
		GetComponent<LevelWon>().enabled = true;
		
	}

	void OnEnable() {
		waitingStyle.fontSize = 50;
		waitingStyle.alignment = TextAnchor.UpperCenter;
		waitingStyle.normal.textColor = new Color(1.0f, 1.0f, 1.0f);
	}

	void OnGUI() {
		if (!PlayerInfo.Instance.allPlayersLoaded()) {
			int numPlayersLoading = PlayerInfo.Instance.numPlayersLoading();
			GUI.Label(new Rect(Screen.width / 2 - 200 / 2, Screen.height / 2 - 100 / 2, 200, 100),
			          new GUIContent("Waiting for " + numPlayersLoading + " players..."),
			          waitingStyle);
		}
	}

	// Update is called once per frame
	void Update () {
		if (PlayerInfo.Instance.allPlayersLoaded()) {
			setupGame();
			enabled = false;
		}
	}
}
