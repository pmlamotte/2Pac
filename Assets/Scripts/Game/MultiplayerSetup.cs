using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerSetup : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
		PacmanData player = (PacmanData)((GameObject) Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<PacmanData>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
