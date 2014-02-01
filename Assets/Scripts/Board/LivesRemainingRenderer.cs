using UnityEngine;
using System.Collections;

public class LivesRemainingRenderer : MonoBehaviour {

	public GUIStyle textStyle;
	void OnGUI()
	{
		GUIContent text = new GUIContent("Lives : " + GameData.Instance.PlayerLives );

		
		GUILayout.BeginArea( new Rect( 0, 0, Screen.width, 100 ) );
		GUILayout.Label( text, textStyle, new GUILayoutOption[]{GUILayout.Width(Screen.width), GUILayout.Height( 100 )} );
		
		GUILayout.EndArea();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
