using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MultiplayerLobby : MonoBehaviour {


	private ServerManager manager = null;
	public ServerManager Manager
	{
		get
		{
			if ( manager == null )
			{
				Manager = GameObject.FindObjectOfType<ServerManager>();
			}
			return manager;
		}
		set 
		{
			manager = value;
		}
	}

	private GameHost host = null;
	public GameHost Host
	{
		get
		{
			if ( host == null )
			{
				Host = GameObject.FindObjectOfType<GameHost>();
			}
			return host;
		}
		set 
		{
			host = value;
		}
	}


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
			BroadcastMessage( "RefreshHostList" );
		}
		if (GUI.Button(new Rect(100, 250, 250, 100), "Start Server")) {
			if ( Manager != null )
			{
				Host.startServer(2, "test");
				openServer();
			}
		}

		if ( Manager != null )
		{
			HostData[] hostList = Manager.hostList;
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) 
					{
						// Host.StartServer();
						openServer();
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
