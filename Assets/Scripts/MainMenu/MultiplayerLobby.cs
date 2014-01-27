using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerLobby : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}

	void openServer()
	{
		ReadyUpToGame ready = GetComponent<ReadyUpToGame>();
		ready.enabled = true;
		enabled = false;
		GameProperties.isSinglePlayer = false;
	}

	void OnGUI() {
		if (GUI.Button(new Rect(100, 100, 250, 100), "Refresh Hosts")) {
			ServerManager.Instance.RefreshHostList();
		}
		if (GUI.Button(new Rect(100, 250, 250, 100), "Start Server")) {
			GetComponent<ServerCreate>().enabled = true;
			enabled = false;
		}

		ServerManager manager = ServerManager.Instance;
		HostData[] hostList = manager.hostList;
		if (hostList != null)
		{
			for (int i = 0; i < hostList.Length; i++)
			{
				if (hostList[i].comment.ToLower() == "closed") {
					continue;
				}
				if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) 
				{
					NetworkConnectionError error = manager.JoinServer(hostList[i]);
					Debug.Log(error.ToString());
					if (error != NetworkConnectionError.NoError) {
						ServerManager.Instance.RefreshHostList();
					}
				}
			}
		}
	}

	void OnConnectedToServer() {
		openServer();
	}
	
	void OnFailedToConnect(NetworkConnectionError error) {
		ServerManager.Instance.RefreshHostList();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
