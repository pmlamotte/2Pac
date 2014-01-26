using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GameProperties.isSinglePlayer) {
			GetComponent<SinglePlayerSetup>().enabled = true;
		} else {
			GetComponent<MultiplayerSetup>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
