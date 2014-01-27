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
			// spawn ghosts
			Network.Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
