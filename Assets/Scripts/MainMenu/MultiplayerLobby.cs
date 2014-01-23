using UnityEngine;
using System.Collections;

public class MultiplayerLobby : MonoBehaviour {

	private HostData[] hostList;
	private const string GAME_NAME = "2Pac";
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(GAME_NAME);
	}

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI() {
		if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts")) {
			RefreshHostList();
		}

		if (hostList != null)
		{
			for (int i = 0; i < hostList.Length; i++)
			{
				if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) {
					//JoinServer(hostList[i]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
