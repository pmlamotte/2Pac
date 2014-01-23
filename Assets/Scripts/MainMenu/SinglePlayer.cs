using UnityEngine;
using System.Collections;

public class SinglePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void OnGUI() {
		if (GUI.Button(new Rect(100, 100, 250, 100), "1 Player")) {
			GameProperties.isSinglePlayer = true;
			GameProperties.numStartingPlayers = 1;
			Application.LoadLevel("Networked");
		}
		
		if (GUI.Button(new Rect(100, 300, 250, 100), "2 Player")) {
			// TODO
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
