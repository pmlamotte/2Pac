using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using System.Text.RegularExpressions;

public class BoardData : MonoBehaviour {

	public GameObject pelletPrefab;




	public bool[,] board;
	public Dictionary<int, IntVector2> GhostSpawns {get; private set;}
	public Dictionary<int, IntVector2> PlayerSpawns {get; private set;}
	public Dictionary<IntVector2, List<BoardObject>> Pellets {get; private set;}
	
	public int Height {get; private set;}
	public int Width {get; private set; }

	
	
	public BoardAccessor _Board;
	public BoardAccessor Accessor
	{
		get
		{
			if ( _Board == null )
			{
				_Board = GetComponent<BoardAccessor>();
			}
			return _Board;
		}
		private set {}
	}
	

	private void createPellet( BoardLocation pos )
	{
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;

		if ( !Pellets.ContainsKey( pos.location ) )
		{
			Pellets.Add( pos.location, new List<BoardObject>());
		}

		Vector3 renderPos = Accessor.convertToRenderPos( pos );
		Quaternion rot = Quaternion.identity;
		BoardObject g = null;
		if ( GameProperties.isSinglePlayer )
		{
			g = ((GameObject)Instantiate( pelletPrefab, renderPos, rot )).GetComponent<BoardObject>();
		}
		else
		{
			g = ((GameObject)Network.Instantiate( pelletPrefab, renderPos, rot, 0 )).GetComponent<BoardObject>();
		}
		g.boardLocation = pos;
		Pellets[pos.location].Add( g );
	}

	public void CreateBoard()
	{
		GhostSpawns = new Dictionary<int, IntVector2>();
		PlayerSpawns = new Dictionary<int, IntVector2>();
		Pellets = new Dictionary<IntVector2, List<BoardObject>>();

		TextAsset boardAsset = (TextAsset) Resources.Load( "Levels/Level1", typeof( TextAsset ) );
		
		string[] sboard = Regex.Split( boardAsset.text, "\r\n|\n" );
		string[][] sboardTokens = new string[sboard.Length][];

		for ( int i = 0; i < sboard.Length; i++ )
		{
			sboardTokens[i] = sboard[i].Split('\t');
		}

		
		// init closed spaces
		board = new bool[sboardTokens.Length, sboardTokens[0].Length];
		for ( int i = 0; i < board.GetLength(0); i++ )
		{
			for (int j = 0; j < board.GetLength(1); j++ )
			{
				string token = sboardTokens[sboardTokens.Length - 1 - i][j];
				if ( token.Equals( "1" ) )
				{
					board[i,j] = true;
				}

			}
		}
		
		for ( int i = 0; i < board.GetLength(0); i++ )
		{
			for (int j = 0; j < board.GetLength(1); j++ )
			{
				string token = sboardTokens[sboardTokens.Length - 1 - i][j];
				if ( token.StartsWith( "G" ) )
				{
					token = token.Substring( 1 );
					int ghostNum = int.Parse( token );
					GhostSpawns.Add( ghostNum, new IntVector2( j, i ) );
				}
				else if ( token.StartsWith( "P" ) )
				{
					token = token.Substring( 1 );
					int playerNum = int.Parse( token );
					PlayerSpawns.Add( playerNum, new IntVector2( j, i ) );
				}
				else if ( !board[i,j] )
				{
					// place pellets
					foreach ( IntVector2 dir in Constants.directions )
					{
						createPellet( new BoardLocation( new IntVector2( j, i ), new IntVector2( 0, 0 ) ) );
						IntVector2 check = new IntVector2( j, i ) + dir;
						if ( !board[check.y, check.x] )
						{
							// pelet goes there
							createPellet( new BoardLocation( new IntVector2( j, i ), dir * ( 2 * Constants.BoardCellRadius / 3 ) ) );
						}
					}
				}
			}
		}
		
		this.Height = board.GetLength(0);
		this.Width = board.GetLength(1);
	}
	


}
