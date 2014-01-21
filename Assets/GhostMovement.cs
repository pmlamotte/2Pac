using UnityEngine;
using System.Collections.Generic;

public class GhostMovement : MonoBehaviour {
	private List<GameObject> players;
	public float maxSpeed = 0f;

	// Use this for initialization
	void Start () {
		players = ((OnStart)(GameObject.Find( "StartUp" ).GetComponent( "OnStart" ))).players;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 toMove = new Vector2(0,0);
		foreach ( GameObject player in players )
		{
			// todo, will move to the last player in list
			Vector2 direction = OnStart.board.moveTowards( transform.position, player.transform.position,  maxSpeed );
			toMove = direction;
		}
		transform.position = OnStart.board.tryMove( transform.position, toMove );

	}
}
