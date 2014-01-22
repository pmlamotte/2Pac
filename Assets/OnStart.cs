using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class OnStart : MonoBehaviour {

	public GameObject filledCell;
	public List<GameObject> players = new List<GameObject>();
	public List<GameObject> ghosts = new List<GameObject>();
	public static GameBoard board;

	
	public void createCell( int x, int y, bool fill )
	{
		if ( fill ) 
		{
			GameObject cell = (GameObject) Instantiate( filledCell );
			cell.transform.position = new Vector3(x + .5f,y + .5f, 0f);
		}
	}
	
	void Start () {
		// todo, this would  be read from a file or something
		string[] sboard = new string[] { 
			"1111111111111111111",
			"1000000001000000001",
			"1011011101011101101",
			"100000000G000000001",
			"1011010111110101101",
			"1000010001000100001",
			"1111011101011101111",
			"1111010000000101111",
			"1000000111110000001",
			"1111111111111111111" };
		OnStart.board = new GameBoard( sboard );

		for ( int i = 0; i < OnStart.board.Height; i++ )
		{
			for ( int j = 0; j < OnStart.board.Width; j++ )
			{
				if ( !OnStart.board.isOpen( j, i ) )
				{
					createCell( j, i, true );
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
