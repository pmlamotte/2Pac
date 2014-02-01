using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SinglePlayerSetup : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable() {
		GameObject.FindObjectOfType<Level>().InitializeLevel();
		GetComponent<LevelWon>().enabled = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
