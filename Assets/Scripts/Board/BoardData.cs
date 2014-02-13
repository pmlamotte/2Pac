using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using System.Text.RegularExpressions;

public class BoardData : MonoBehaviour {



	public bool[,] board;
	public Dictionary<int, IntVector2> GhostSpawns {get; private set;}
	public Dictionary<int, IntVector2> PlayerSpawns {get; private set;}
	public HashSet<IntVector2> Pellets {get; private set;}
	public HashSet<IntVector2> PowerPellets {get; private set;}
	public Dictionary<int, Warp> WarpPoints { get; private set; }
	
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
		
		if ( !Pellets.Contains( pos.location ) )
		{
			Pellets.Add( pos.location );
		}
		
		Pellets.Add(pos.location);
	}

	private void createPowerPellet( BoardLocation pos )
	{
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;
		
		PowerPellets.Add(pos.location);
	}

	public void CreateBoard()
	{
		GhostSpawns = new Dictionary<int, IntVector2>();
		PlayerSpawns = new Dictionary<int, IntVector2>();
		Pellets = new HashSet<IntVector2>();
		PowerPellets = new HashSet<IntVector2>();
		WarpPoints = new Dictionary<int, Warp>();

		Debug.Log("Loading level: " + GameData.Instance.level);
		TextAsset boardAsset = (TextAsset) Resources.Load( "Levels/Level" + ((GameData.Instance.level % 2) + 1), typeof( TextAsset ) );
		
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
				// GHOST SPAWN
				if ( token.StartsWith( "G" ) )
				{
					token = token.Substring( 1 );
					int ghostNum = int.Parse( token );
					GhostSpawns.Add( ghostNum, new IntVector2( j, i ) );
				}
				// Power pellet 
				else if ( token.StartsWith( "PP"  ) )
				{
					createPowerPellet( new BoardLocation( new IntVector2( j, i ), new IntVector2( 0, 0 ) ) );
				}
				// PLAYER SPAWN
				else if ( token.StartsWith( "P" ) )
				{
					token = token.Substring( 1 );
					int playerNum = int.Parse( token );
					PlayerSpawns.Add( playerNum, new IntVector2( j, i ) );
				}
				// WARP INPUT
				else if (token.StartsWith("W"))
				{
					token = token.Substring(1);
					int id = int.Parse(token[0] + "");
					Warp warp;
					if (!WarpPoints.ContainsKey(id)) {
						warp = new Warp();
						WarpPoints.Add(id, warp);
					} else {
						warp = WarpPoints[id];
					}
					warp.input = new IntVector2(j, i);
				}
				// WARP OUTPUT
				else if (token.StartsWith("O")) {
					token = token.Substring(1);
					int id = int.Parse(token[0] + "");
					Warp warp;
					if (!WarpPoints.ContainsKey(id)) {
						warp = new Warp();
						WarpPoints.Add(id, warp);
					} else {
						warp = WarpPoints[id];
					}
					warp.output = new IntVector2(j, i);
					token = token.Substring(1);
					warp.outDirection = Direction.getDirection(token);
				}
				else if ( Accessor.isOpen( j, i ) )
				{
					// place pellets
					createPellet( new BoardLocation( new IntVector2( j, i ), new IntVector2( 0, 0 ) ) );
						// removed, only one pellet per tile
						//IntVector2 check = new IntVector2( j, i ) + dir;
					
					//foreach ( IntVector2 dir in Constants.directions )
					//{


						//if ( Accessor.isOpen( check.x, check.y ) )
						//{
							// pelet goes there
//							createPellet( new BoardLocation( new IntVector2( j, i ), dir * ( 2 * Constants.BoardCellRadius / 3 ) ) );
						//}
					//}
				}
			}
		}
		
		
		this.Height = board.GetLength(0);
		this.Width = board.GetLength(1);
	}
	
	public static BoardData getBoardData() {
		return GameObject.FindObjectOfType<BoardData>();
	}

	public GameObject[] getPlayers() {
		return GameObject.FindGameObjectsWithTag("Pacman");
	}

	public GameObject[] getGhosts() {
		return GameObject.FindGameObjectsWithTag("Ghost");
	}
}
