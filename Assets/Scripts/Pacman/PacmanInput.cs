using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PacmanInput : MonoBehaviour {

	IntVector2 oldDirection = new IntVector2(0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameProperties.isSinglePlayer && GetComponent<PacmanData>().playerNum != GameProperties.myPlayer.id) {
			return;
		}

		IntVector2 newDirection = new IntVector2(0,0);
		// compute the attempted direction
		if ( Input.GetKey( KeyCode.LeftArrow ) ) newDirection = new IntVector2( newDirection.x -1, newDirection.y );
		else if ( Input.GetKey( KeyCode.RightArrow ) ) newDirection = new IntVector2( newDirection.x + 1, newDirection.y );
		else if ( Input.GetKey( KeyCode.DownArrow ) ) newDirection = new IntVector2( newDirection.x, newDirection.y - 1);
		else if ( Input.GetKey( KeyCode.UpArrow ) ) newDirection = new IntVector2( newDirection.x, newDirection.y + 1 );

		if (newDirection != oldDirection) {
			GetComponent<PacmanMover>().updateDirection(newDirection.x, newDirection.y);
			if (!GameProperties.isSinglePlayer && !Network.isServer) {
				GetComponent<PacmanMover>().networkView.RPC("updateDirection", RPCMode.Server, newDirection.x, newDirection.y);
			}
			oldDirection = newDirection;
		}
	}
}
