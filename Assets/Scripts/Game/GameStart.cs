using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	public GUIStyle textStyle;
	private bool showStartingScreen = false;
	int num = 3;
	float currTime = 0;
	float lastTime = 0;


	private bool startedUp = false;

	// Use this for initialization
	void Start () {
		if (GameProperties.isSinglePlayer) {
			GetComponent<SinglePlayerSetup>().enabled = true;
		} else {
			GetComponent<MultiplayerSetup>().enabled = true;
		}
	}

	void OnGUI() {

		if (showStartingScreen) {
			int height = (int)(Screen.height * .2f);
			GUI.Box(new Rect(0, Screen.height / 2 - height / 2, Screen.width, height),"");

			GUIContent text = new GUIContent("" + ((num == 0) ? "GO" : num + ""));

			GUIStyle watStyle = new GUIStyle(textStyle);
			int x = 0;
			if (currTime < .2f) {
				x = (int)(currTime / .2f * Screen.width / 2 - watStyle.CalcSize(text).x / 2);
			} else if (currTime >= .2f && currTime <= .8f) {
				watStyle.fontSize += (int)(20 - 20*((Mathf.Abs(.5f - currTime))/.3f));
				x = (int)(Screen.width / 2 - watStyle.CalcSize(text).x / 2);
			} else if (currTime > .8f) {
				x = (int)((currTime - .8f) / .2f * Screen.width/2 - watStyle.CalcSize(text).x / 2 + Screen.width /2);
			}
			GUI.Label(new Rect(x, Screen.height / 2 - watStyle.CalcSize(text).y / 2, 100, 100),text, watStyle);

			currTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			if (currTime >= 1f) {
				currTime = 0f;
				num--;
			}

			if (num < 0) {
				showStartingScreen = false;
				Time.timeScale = 1;
			}
		}
	}

	public void showStartingMessage() {
		num = 3;
		currTime = 0;
		lastTime = Time.realtimeSinceStartup;
		showStartingScreen = true;
	}

	// Update is called once per frame
	void Update () {

		if (!startedUp && PlayerInfo.Instance.allPlayersReady()) {
			startedUp = true;
			showStartingMessage();
		}
	}
}
