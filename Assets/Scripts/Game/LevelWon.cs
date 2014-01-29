using UnityEngine;
using System.Collections;

public class LevelWon : MonoBehaviour {

	bool handledLevel = false;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag("Pellet") == null && !handledLevel) {
			handledLevel = true;
			// game was won
			GameData.Instance.level++;
			if (!GameProperties.isSinglePlayer && Network.isServer) {
				ServerComms.Instance.LoadLevel("Networked", GameData.Instance.level);
			} else if (GameProperties.isSinglePlayer) {
				Application.LoadLevel("Networked");
			}
		}
	}
}
