using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerSetup : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
		GameObject player = (GameObject)Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
		player.GetComponent<PacmanData>().playerNum = GameProperties.myPlayer.id;
		if ( Network.isServer )
		{
			// spawn ghosts
			Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
