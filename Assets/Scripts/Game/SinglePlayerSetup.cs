using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SinglePlayerSetup : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject ghostPrefab;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable() {
		SpawnPlayer();
		SpawnGhost();

		GameObject.FindObjectOfType<Level>().InitializeLevel();
	}

	void SpawnPlayer()
	{
		BoardLocation pos = new BoardLocation( new IntVector2(0,0), new IntVector2( 0, 0 ) );
		int x = 1;
		int y = 1;
		
		pos = new BoardLocation( new IntVector2( x, y ), pos.offset );
		
		PacmanData player = (PacmanData)((GameObject) Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity)).GetComponent<PacmanData>();
		
		PacmanData animate = player.GetComponent<PacmanData>();
		animate.Data.maxSpeed = 12;
		animate.Data.boardLocation = pos;
		
	}

	private void SpawnGhost()
	{	
		GhostMover ghost = ((GameObject) Instantiate(ghostPrefab, new Vector3(0,0,0), Quaternion.identity)).GetComponent<GhostMover>();

		GhostMover animate = ghost.GetComponent<GhostMover>();
		animate.Data.maxSpeed = 10;
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
