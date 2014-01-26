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
	}

	void OnGUI() {
		if (GUI.Button(new Rect(100, 100, 250, 100), "Refresh Hosts")) {
			ServerManager.Instance.RefreshHostList();
		}
		if (GUI.Button(new Rect(100, 250, 250, 100), "Start Server")) {
				GameHost.Instance.startServer(2, "test: " + Constants.random.Next());
				openServer();
		}

		ServerManager manager = ServerManager.Instance;
		HostData[] hostList = manager.hostList;
		if (hostList != null)
		{
			for (int i = 0; i < hostList.Length; i++)
			{
				if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) 
				{
					manager.JoinServer(hostList[i]);
					openServer();
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
