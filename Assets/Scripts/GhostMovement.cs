using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;

public class GhostMovement : MonoBehaviour {
	private List<PacmanAnimate> players;
	public int maxSpeed = 10;
	public BoardLocation boardLocation {get; set;}
	// Use this for initialization
	void Start () {
		if (!networkView.isMine) return;
		players = ((OnStart)(GameObject.Find( "StartUp" ).GetComponent( "OnStart" ))).players;
	}
	
	// Update is called once per frame
	void Update () {
		if (!networkView.isMine) return;
		int maxSpeed = this.maxSpeed * (int)( Time.deltaTime * 1000 );
		BoardLocation startLocation = boardLocation.Clone();

		IntVector2 toMove = new IntVector2(0,0);
		foreach ( PacmanAnimate player in players )
		{
			// todo, will move to the last player in list
			IntVector2 direction = OnStart.board.moveTowards( boardLocation, player.boardLocation, maxSpeed );
			toMove = direction;
		}
		boardLocation = OnStart.board.tryMove( boardLocation, toMove );
		
		foreach ( PacmanAnimate player in players )
		{
			// see if contact
			if ( BoardLocation.SqrDistance( player.boardLocation, this.boardLocation ) < BoardLocation.cellRadius * BoardLocation.cellRadius )
			{
				player.hitByGhost( this.gameObject );
			}
			
		}

		this.transform.position = OnStart.board.convertToRenderPos( this.boardLocation );

	}
}
