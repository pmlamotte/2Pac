using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerSetup : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
		if (Network.isServer) {

			foreach (PlayerInfo.Player player in PlayerInfo.Instance.players) {
				GameObject pacPlayer = (GameObject)Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
				pacPlayer.GetComponent<PacmanData>().setPlayerNum(player.id);
			}

			// todo this should be stored elsewhere, TBD
			// todo determine spawn location of ghosts
			int numGhosts = 1;

			for ( int i = 0; i < numGhosts; i++ )
			{
				// spawn ghosts
				Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
