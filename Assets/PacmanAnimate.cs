using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PacmanAnimate : MonoBehaviour {


	public int maxSpeed = 10;
	public IntVector2 direction = new IntVector2( -1, 0 );
	public float score = 0;
	public int playerNum = 0;
	public BoardLocation spawnPosition = new BoardLocation( new IntVector2( 1, 1), new IntVector2(0,0) );
	public BoardLocation boardLocation {get; set;}

	public void hitByGhost( GameObject ghost )
	{
		this.boardLocation = spawnPosition.Clone();
		SendMessage("PacmanHit");
	}

	[RPC] public void setPlayerNum(int num) {
		playerNum = num;
		if (networkView.isMine) {
			networkView.RPC("setPlayerNum", RPCMode.Others, num);
		}
	}
	
	void Update () {

		if (networkView.isMine) {
			// make game frame rate independent
			int maxSpeed = this.maxSpeed * ((int)(1000 * Time.deltaTime ));

			BoardLocation startPos = boardLocation.Clone();
			IntVector2 newDirection = new IntVector2( 0, 0 );


			// compute the attempted direction
			if ( Input.GetKey( KeyCode.LeftArrow ) ) newDirection = new IntVector2( newDirection.x -1, newDirection.y );
			else if ( Input.GetKey( KeyCode.RightArrow ) ) newDirection = new IntVector2( newDirection.x + 1, newDirection.y );
			else if ( Input.GetKey( KeyCode.DownArrow ) ) newDirection = new IntVector2( newDirection.x, newDirection.y - 1);
			else if ( Input.GetKey( KeyCode.UpArrow ) ) newDirection = new IntVector2( newDirection.x, newDirection.y + 1 );

			if ( newDirection.x + newDirection.y != 0 )
			{
				// decide if attempted direction is a valid one based on whether or not it would effect mr
				// pacman were it executed
				newDirection.Normalize( ); 
				newDirection *= maxSpeed;
				BoardLocation posAfter = OnStart.board.tryMove( startPos, newDirection );

				if ( BoardLocation.SqrDistance( posAfter, startPos ) > 0 )
				{
					// valid velocity change.
					direction = newDirection;
				}
			}

			direction.Normalize( );
			direction *= maxSpeed;

			boardLocation = OnStart.board.tryMove( boardLocation, direction );
			
			// rotate to face direction traveling 
			bool[] point = new bool[]{direction.x > 0, direction.y > 0, direction.x < 0, direction.y < 0 };
			for ( int i = 0; i < 4; i++ )
			{
				if ( point[i] )
				{
					transform.rotation = Quaternion.Euler( new Vector3(0, 0, i * 90 ));
				}
			}
			this.transform.position = OnStart.board.convertToRenderPos( this.boardLocation );

			Animator a = (Animator) GetComponent( "Animator" );

			// if not moving turn off the animation
			a.enabled = BoardLocation.SqrDistance( boardLocation, startPos ) > 0;
		}
		

	}
}
