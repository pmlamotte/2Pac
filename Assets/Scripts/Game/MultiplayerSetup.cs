using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerSetup : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void OnEnable () {
		if (Network.isServer) {

			foreach (PlayerInfo.Player player in PlayerInfo.Instance.players) {
				GameObject pacPlayer = (GameObject)Network.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
				pacPlayer.GetComponent<PacmanData>().setPlayerNum( player.id );
			}

			// todo this should be stored elsewhere, TBD
			// todo determine spawn location of ghosts
			int numGhosts = 2;

			for ( int i = 0; i < numGhosts; i++ )
			{
				// spawn ghosts
				GhostMover mover = ((GameObject)Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0)).GetComponent<GhostMover>();
			}

			
		}
		GameObject.FindObjectOfType<Level>().InitializeLevel();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
