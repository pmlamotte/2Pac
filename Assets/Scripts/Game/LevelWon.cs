using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class LevelWon : MonoBehaviour {
	public GUIStyle textStyle;
	
	bool handledLevel = false;
	float winScreenRemaining = 5.0f;
	private static float ShakeDistance = 1.5f;
	private static float ShakeRot = 4.0f;
	private static float lastCall = -1;


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag("Pellet") == null && !handledLevel && winScreenRemaining <= 0 ) {
			handledLevel = true;
			// game was won
			GameData.Instance.level++;
			if (!GameProperties.isSinglePlayer && Network.isServer) {
				ServerComms.Instance.LoadLevel("Networked", GameData.Instance.level);
			} else if (GameProperties.isSinglePlayer) {
				Application.LoadLevel("Networked");
			}
		}
	}

	void OnGUI()
	{
		if ( GameObject.FindGameObjectWithTag("Pellet") == null )
		{
			Time.timeScale = 0;
			winScreenRemaining -= (Time.realtimeSinceStartup - lastCall );
			GUIContent text = new GUIContent("VICTORY!");
			
			GUIStyle watStyle = new GUIStyle(textStyle);
			watStyle.alignment = TextAnchor.MiddleCenter;

			float x = (float)( ShakeDistance * ( Constants.random.NextDouble() * 2 - 1.0 ) );
			float y = (float)( ShakeDistance * ( Constants.random.NextDouble() * 2 - 1.0 ) );
			float rot = (float) ( ShakeRot * ( Constants.random.NextDouble() * 2 - 1.0 ) );
			GUIUtility.RotateAroundPivot( rot, new Vector2( Screen.width / 2, Screen.height / 2 ) );

			GUILayout.BeginArea( new Rect( x, y, Screen.width, Screen.height ) );
			GUILayout.Label( text, watStyle, new GUILayoutOption[]{GUILayout.Width(Screen.width), GUILayout.Height( Screen.height )} );
			GUILayout.EndArea();
		}
		lastCall = Time.realtimeSinceStartup;
	}

}
