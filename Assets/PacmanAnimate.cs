using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PacmanAnimate : MonoBehaviour {


	public float maxSpeed = .01f;
	public Vector2 direction = new Vector2( -1, 0 );
	public float score = 0;
	public Vector3 spawnPosition = new Vector3(1.5f,1.5f,0);
	public GUIText text;


	public void hitByGhost( GameObject ghost )
	{
		this.transform.position = spawnPosition;

		score -= 10;
	}

	void Start () {
	}
	
	void Update () {

		if (networkView.isMine) {
			if ( text != null )
			{
				score += (.1f * Time.deltaTime );

				string newText = this.text.text.Substring( 0, this.text.text.IndexOf(':') );
				newText += ": " + ((int) score );
				this.text.text = newText;
			}
			// make game frame rate independent
			float maxSpeed = this.maxSpeed * (1000 * Time.deltaTime );

			Vector3 startPos = transform.position;
			Vector2 newDirection = new Vector2( 0, 0 );

			// compute the attempted direction
			if ( Input.GetKey( KeyCode.LeftArrow ) ) newDirection.x -= 1;
			else if ( Input.GetKey( KeyCode.RightArrow ) ) newDirection.x += 1;
			else if ( Input.GetKey( KeyCode.DownArrow ) ) newDirection.y -= 1;
			else if ( Input.GetKey( KeyCode.UpArrow ) ) newDirection.y += 1;

			if ( newDirection.x + newDirection.y != 0 )
			{
				// decide if attempted direction is a valid one based on whether or not it would effect mr
				// pacman were it executed
				newDirection.Normalize( ); 
				newDirection.Scale( new Vector2( maxSpeed, maxSpeed ) );
				Vector3 posAfter = OnStart.board.tryMove( transform.position, newDirection );

				if ( Vector3.Distance( posAfter, startPos ) >= .001 )
				{
					// valid velocity change.
					direction = newDirection;
				}
			}

			direction.Normalize( );
			direction *= maxSpeed;

			transform.position = OnStart.board.tryMove( transform.position, direction );

			// rotate to face direction traveling 
			bool[] point = new bool[]{direction.x > 0, direction.y > 0, direction.x < 0, direction.y < 0 };
			for ( int i = 0; i < 4; i++ )
			{
				if ( point[i] )
				{
					transform.rotation = Quaternion.Euler( new Vector3(0, 0, i * 90 ));
				}
			}

			Animator a = (Animator) GetComponent( "Animator" );

			// if not moving turn off the animation
			a.enabled = Vector3.Distance( transform.position, startPos ) >= maxSpeed / 2;
		}
	}
}
