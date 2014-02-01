using UnityEngine;
using System.Collections;

public class LevelLost : MonoBehaviour {

	bool playLosingAnimation = false;
	public GUIStyle textStyle;
	

	// Use this for initialization
	void Start () {
	
	}


	void OnEnable()
	{
		playLosingAnimation = true;
	}


	void OnGUI()
	{
		if ( playLosingAnimation )
		{
			Time.timeScale = 0;
			GUIContent text = new GUIContent("YOU LOST!");
			
			GUIStyle watStyle = new GUIStyle(textStyle);
			watStyle.alignment = TextAnchor.LowerCenter;
			
			GUILayout.BeginArea( new Rect( 0, 0, Screen.width, Screen.height ) );
			GUILayout.BeginVertical();
			GUILayout.Label( text, watStyle, new GUILayoutOption[]{GUILayout.Width(Screen.width), GUILayout.Height( Screen.height / 2 )} );
			
			if ( GameProperties.isSinglePlayer || Network.isServer )
			{
				if (GUILayout.Button("Main Menu")) {
					if (!GameProperties.isSinglePlayer && Network.isServer) {
						ServerComms.Instance.networkView.RPC("ExitGame", RPCMode.Others );
						Network.Disconnect();						
						Application.LoadLevel("MainMenu");
					} else if (GameProperties.isSinglePlayer) {
						Application.LoadLevel("MainMenu");
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	
	// Update is called once per frame
	void Update () {

	
	}
}
