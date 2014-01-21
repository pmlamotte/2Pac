using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class OnStart : MonoBehaviour {

	public GameObject filledCell;
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
			"1000000000000000001",
			"1011010111110101101",
			"1000010001000100001",
			"1111011101011101111",
			"1111010000000101111",
			"1000000111110000001",
			"1111111111111111111" };

		bool[,] board = new bool[sboard.Length,sboard[0].Length];
		for ( int i = 0; i < board.GetLength(0); i++ )
		{
			for (int j = 0; j < board.GetLength(1); j++ )
			{
				board[i,j] = sboard[i].ToCharArray()[j] == '1';
				createCell( j, i, board[i,j] );
			}
		}
		OnStart.board = new GameBoard( board );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
