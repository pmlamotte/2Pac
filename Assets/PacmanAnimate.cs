using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PacmanAnimate : MonoBehaviour {


	public float maxSpeed = .01f;
	public Vector2 direction = new Vector2( -1, 0 );

	void Start () {
	}
	
	void Update () {
		Vector3 startPos = transform.position;
		Vector2 newDirection = new Vector2( 0, 0 );
		
		if ( Input.GetKey( KeyCode.LeftArrow ) ) newDirection.x -= 1;
		else if ( Input.GetKey( KeyCode.RightArrow ) ) newDirection.x += 1;
		else if ( Input.GetKey( KeyCode.DownArrow ) ) newDirection.y -= 1;
		else if ( Input.GetKey( KeyCode.UpArrow ) ) newDirection.y += 1;

		if ( newDirection.x + newDirection.y != 0 )
		{
			newDirection.Normalize( ); 
			float asdfasdf = newDirection.x * maxSpeed;
			newDirection.Scale( new Vector2( maxSpeed, maxSpeed ) );
			Vector3 posAfter = OnStart.board.tryMove( transform.position, newDirection );

			if ( Vector3.Distance( posAfter, startPos ) >= .001 )
			{
				// valid velocity change.\
				direction = newDirection;
			}
		}

		direction.Normalize( );
		direction.Scale( new Vector2( maxSpeed, maxSpeed ) );

		transform.position = OnStart.board.tryMove( transform.position, direction );

		bool[] point = new bool[]{direction.x > 0, direction.y > 0, direction.x < 0, direction.y < 0 };
		for ( int i = 0; i < 4; i++ )
		{
			if ( point[i] )
			{
				transform.rotation = Quaternion.Euler( new Vector3(0, 0, i * 90 ));
			}
		}

	}
}
