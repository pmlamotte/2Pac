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
	public Dictionary<IntVector2, List<IntVector2>> PossibleDirectionsMap {get; private set;}
	public Dictionary<IntVector2, int> DirectionIndex {get; private set;}


	public Dictionary<int, Warp> WarpPoints { get; private set; }
	private GameObject[] players;
	private GameObject[] ghosts;
	
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
		PossibleDirectionsMap = new Dictionary<IntVector2, List<IntVector2>>();
		DirectionIndex = new Dictionary<IntVector2, int>();
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
				}

				if ( Accessor.isOpen( j, i ) )
				{
					// determine if intersection
					List<IntVector2> possibleDirections = new List<IntVector2>();
					foreach ( IntVector2 dir in Constants.directions )
					{
						if ( Accessor.isOpen( dir + new IntVector2( j, i ) ) )
						{
							possibleDirections.Add ( dir );
						}
					}
					if ( possibleDirections.Count > 2 )
					{
						this.PossibleDirectionsMap.Add( new IntVector2( j, i ), possibleDirections );
						this.DirectionIndex.Add( new IntVector2( j, i ), 0 );
					}
				}

			}
		}

		foreach( IntVector2 intersection in this.PossibleDirectionsMap.Keys )
		{
			List<IntVector2> possibleDirections = this.PossibleDirectionsMap[intersection];
			
			int bestIndex = 0;
			int shortestDistance = int.MaxValue;
			for ( int i = 0; i < possibleDirections.Count; i++ )
			{
				IntVector2 thisDirection = possibleDirections[i];
				foreach ( IntVector2 playerSpawn in PlayerSpawns.Values )
				{
					int thisDistance = IntVector2.OrthogonalDistance( playerSpawn, intersection + thisDirection );
					if ( thisDistance < shortestDistance )
					{
						bestIndex = i;
						shortestDistance = thisDistance;
					}
				}
			}
			this.DirectionIndex[intersection] = bestIndex;
		}

		this.Height = board.GetLength(0);
		this.Width = board.GetLength(1);
	}
	
	public static BoardData getBoardData() {
		return GameObject.FindObjectOfType<BoardData>();
	}

	public GameObject[] getPlayers() {
		if ( players == null )
		{
			players = GameObject.FindGameObjectsWithTag("Pacman");
		}
		return players;
	}

	public GameObject[] getGhosts() {
		if ( ghosts == null )
		{
			ghosts = GameObject.FindGameObjectsWithTag("Ghost");
		}
		return ghosts;
	}
}
