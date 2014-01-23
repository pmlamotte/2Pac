using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BoardData : MonoBehaviour {

	public bool[,] board;
	public IntVector2 ghostSpawn {get; private set;}
	
	public int Height {get; private set;}
	public int Width {get; private set; }
	
	
	public BoardData()
	{
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


		bool[,] board = new bool[sboard.Length, sboard[0].Length];
		for ( int i = 0; i < board.GetLength(0); i++ )
		{
			for (int j = 0; j < board.GetLength(1); j++ )
			{
				if ( sboard[i][j] == '1' )
				{
					board[i,j] = true;
				}
				else 
				{
					if ( sboard[i][j] == 'G' )
					{
						ghostSpawn = new IntVector2( j, i );
					}
				}
			}
		}
		
		this.board = board;
		this.Height = board.GetLength(0);
		this.Width = board.GetLength(1);
	}
	


}
