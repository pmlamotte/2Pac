using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class PacmanMover : MonoBehaviour {
	
	public BoardAccessor _Board;
	public BoardAccessor Board
	{
		get
		{
			if ( _Board == null )
			{
				_Board = GameObject.FindObjectOfType<BoardAccessor>();
			}
			return _Board;
		}
		private set {}
	}

	public PacmanData _Data;
	public PacmanData Data
	{
		get
		{
			if ( _Data == null )
			{
				_Data = GetComponent<PacmanData>();
			}
			return _Data;
		}
		private set { throw new NotImplementedException(); }
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameProperties.isSinglePlayer || networkView.isMine) {
			Debug.Log ("wat");

			// make game frame rate independent
			int maxSpeed = ((int)(1000 * Time.deltaTime * Data.Data.maxSpeed ));
			
			BoardLocation startPos = Data.Data.boardLocation.Clone();
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
				BoardLocation posAfter = Board.tryMove( startPos, newDirection );
				
				if ( BoardLocation.SqrDistance( posAfter, startPos ) > 0 )
				{
					// valid velocity change.
					Data.Data.direction = newDirection;
				}
			}
			
			Data.Data.direction.Normalize( );
			Data.Data.direction *= maxSpeed;
			
			Data.Data.boardLocation = Board.tryMove( Data.Data.boardLocation, Data.Data.direction );
			
			
		}
	}
}
