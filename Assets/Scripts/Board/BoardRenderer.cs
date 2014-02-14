using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;

public class BoardRenderer : MonoBehaviour {
	
	public GameObject pelletPrefab;
	public GameObject powerPelletPrefab;
	public GameObject directionArrowPrefab;
	

	public GameObject filledCell;
	
	public Dictionary<IntVector2, GameObject> PelletObjects;
	public Dictionary<IntVector2, GameObject> PowerPelletObjects;
	public Dictionary<IntVector2, GameObject> DirectionArrowObjects;

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


	public void createCell( int x, int y, bool fill )
	{
		if ( fill ) 
		{
			string[] match0 = new string[]
			{
				"000",
				"010",
				"000"
			};

			string[] match1 = new string[]
			{
				"00?",
				"011",
				"00?"
			};
			
			string[] match2 = new string[]
			{
				"?0?",
				"111",
				"?0?"
			};
			
			
			string[] match3 = new string[]
			{
				"?0?",
				"011",
				"?11"
			};
			
			string[] match4 = new string[]
			{
				"?0?",
				"111",
				"111"
			};
			
			string[] match5 = new string[]
			{
				"010",
				"111",
				"010"
			};
			
			string[] match6 = new string[]
			{
				"000",
				"111",
				"010"
			};
			
			string[] match7 = new string[]
			{
				"111",
				"111",
				"111"
			};
			
			string[] match8 = new string[]
			{
				"011",
				"111",
				"111"
			};
			
			string[] match9 = new string[]
			{
				"010",
				"111",
				"111"
			};
			
			string[] match10 = new string[]
			{
				"?0?",
				"011",
				"?10"
			};

			List<string[]> matches = new List<string[]>();
			matches.Add( match0 );
			matches.Add( match1 );
			matches.Add( match2 );
			matches.Add( match3 );
			matches.Add( match4 );
			matches.Add( match5 );
			matches.Add( match6 );
			matches.Add( match7 );
			matches.Add( match8 );
			matches.Add( match9 );
			matches.Add( match10 );

			string[] boardMatch = new string[3];
			for ( int i = 0; i < 3; i++ )
			{
				string toAdd = "";
				for ( int j = 0; j < 3; j++ )
				{
					toAdd += ( Accessor.isOpen( x - 1 + j, y + 1 - i ) ) ? "0" : "1";
				}
				boardMatch[i] = toAdd;
			}


			GameObject cell = (GameObject) Instantiate( filledCell );
			int toLoad = -1;
			int rot = 0;
			for ( rot = 0; rot < 4; rot++ )
			{
				for ( int i = 0; i < matches.Count; i++ )
				{
					if ( BoardSectionEquals( matches[i], boardMatch ) )
					{
						toLoad = i;
						break;
					}
				}
				if ( toLoad != -1 ) break;
				boardMatch = rotBoardMatch( boardMatch );
			}
			if ( toLoad == -1 )
			{
				toLoad = 0;
			}

			MeshFilter mesh = cell.GetComponent<MeshFilter>();
			mesh.mesh = (Mesh)Resources.Load( "Media/3DModels/BoardTile" + toLoad, typeof( Mesh ) );
			cell.transform.rotation = Quaternion.Euler( 0, 0, -rot * 90 );
			cell.transform.position = new Vector3(x + .5f,y + .5f, 0f);
		}
	}

	private bool BoardSectionEquals( string[] a, string[] b )
	{
		for ( int i = 0; i < 3; i++ )
		{
			for ( int j = 0; j < 3; j++ )
			{
				if ( a[i][j] == '?' || b[i][j] == '?' )
				{
					continue;
				}
				if ( a[i][j] != b[i][j] )
				{
					return false;
				}
			}
		}
		return true;
	}

	// helper to above. Rotates a 3x3 section of board
	private string[] rotBoardMatch( string[] boardMatch )
	{
		string[] result = new string[3];
		for ( int i = 0; i < 3; i++ )
		{
			string toAdd = "";
			for ( int j = 0; j < 3; j++ )
			{
				int x = j - 1;
				int y = -1 + i;
				int tempx = x;
				x = -y;
				y = tempx;
				toAdd += boardMatch[y + 1][x + 1];
			}
			result[i] = toAdd;
		}
		return result;
	}

	
	private void renderPellet( BoardLocation pos )
	{
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;
		
		Vector3 renderPos = Accessor.convertToRenderPos( pos );
		Quaternion rot = Quaternion.identity;
		GameObject g = null;
		if ( GameProperties.isSinglePlayer )
		{
			g = (GameObject)Instantiate( pelletPrefab, renderPos, rot );
		}
		else
		{
			g = (GameObject)Network.Instantiate( pelletPrefab, renderPos, rot, 0 );
		}

		PelletObjects[pos.location] = g;
	}
	
	private void renderPowerPellet( BoardLocation pos )
	{
		if ( !( GameProperties.isSinglePlayer || Network.isServer ) ) return;
		
		Vector3 renderPos = Accessor.convertToRenderPos( pos );
		Quaternion rot = Quaternion.identity;
		GameObject g = null;
		if ( GameProperties.isSinglePlayer )
		{
			g = (GameObject)Instantiate( powerPelletPrefab, renderPos, rot );
		}
		else
		{
			g = (GameObject)Network.Instantiate( powerPelletPrefab, renderPos, rot, 0 );
		}

		PowerPelletObjects[pos.location] = g;
	}

	private void renderArrow( BoardLocation pos )
	{
		IntVector2 direction = Accessor.Data.PossibleDirectionsMap[pos.location][Accessor.Data.DirectionIndex[pos.location]];


		int directionInt = 0;
		for ( int i = 0; i < 4; i++ )
		{
			if ( direction.Equals( Constants.directions[i] ) )
			{
				directionInt = i;
				break;
			}
		}
		
		Vector3 renderPos = Accessor.convertToRenderPos( pos );
		Quaternion rot = Quaternion.Euler( new Vector3( 0, 0, 90.0f * directionInt) );
		
		GameObject g = null;
		if ( GameProperties.isSinglePlayer )
		{
			g = (GameObject)Instantiate( directionArrowPrefab, renderPos, rot );
		}
		else
		{
			g = (GameObject)Network.Instantiate( directionArrowPrefab, renderPos, rot, 0 );
		}

		DirectionArrowObjects[pos.location] = g;

	}
	
	public void AtePellet(IntVector2 pos)
	{
		if ( GameProperties.isSinglePlayer )
		{
			Destroy( PelletObjects[pos] );
		}
		else
		{
			Network.Destroy( PelletObjects[pos] );
		}
	}
	public void AtePowerPellet(IntVector2 pos)
	{
		if ( GameProperties.isSinglePlayer )
		{
			Destroy( PowerPelletObjects[pos] );
		}
		else
		{
			Network.Destroy( PowerPelletObjects[pos] );
		}
	}

	public void AfterGhostOverIntersection( IntVector2 intersection )
	{
		IntVector2 direction = Accessor.Data.PossibleDirectionsMap[intersection][Accessor.Data.DirectionIndex[intersection]];

		int directionInt = 0;
		for ( int i = 0; i < 4; i++ )
		{
			if ( direction.Equals( Constants.directions[i] ) )
			{
				directionInt = i;
				break;
			}
		}

		Quaternion rot = Quaternion.Euler( new Vector3( 0, 0, 90.0f * directionInt) );
		this.DirectionArrowObjects[intersection].transform.rotation = rot;
	}

	public void AfterPacmanOverIntersection( IntVector2 intersection )
	{
		// for now, do the same as ghost, update the arrow
		AfterGhostOverIntersection( intersection );
	}

	public void CreateBoard () {
		PowerPelletObjects = new Dictionary<IntVector2, GameObject>();
		PelletObjects = new Dictionary<IntVector2, GameObject>();
		DirectionArrowObjects = new Dictionary<IntVector2, GameObject>();

		for ( int y = 0; y < Accessor.Height; y++ )
		{
			for ( int x = 0; x < Accessor.Width; x++ )
			{
				createCell( x, y, !Accessor.isOpen( x, y ) );
			}
		}

		if ( GameProperties.isSinglePlayer || Network.isServer )
		{
			// pellets are networked. 
			// render power pellets
			foreach ( IntVector2 powerPelletPos in this.Accessor.Data.PowerPellets )
			{
				renderPowerPellet( new BoardLocation( powerPelletPos, new IntVector2( 0, 0 ) ) );
			}
			
			// render pellets
			foreach ( IntVector2 pelletPos in this.Accessor.Data.Pellets )
			{
				renderPellet( new BoardLocation( pelletPos, new IntVector2( 0, 0 ) ) );
			}

			// arrows are networked
			foreach ( IntVector2 arrowPos in this.Accessor.Data.DirectionIndex.Keys )
			{
				renderArrow( new BoardLocation( arrowPos, new IntVector2( 0, 0 ) ) );
			}
		}


		// make sure entire board is visible.
		Camera cam = GameObject.Find( "Camera" ).GetComponent<Camera>();
		if ( !cam.isOrthoGraphic )
		{
			// todo logic could be simpler given that we have similar triangles...

			cam.transform.position = new Vector3( Accessor.Width / 2f, Accessor.Height / 2f, -10 );
			
			Ray rayToLeft = cam.ScreenPointToRay( new Vector3( -1, 0, 0 ) );
			Ray rayToTop = cam.ScreenPointToRay( new Vector3( 0, 1, 0 ) );

			// the z distance required to see the left and top
			float leftZ = 0;
			float topZ = 0;

			Plane gamePlane = new Plane( new Vector3( 0, 0, -1 ), new Vector3( 0, 0, 0 ) );
			float leftDistance;
			gamePlane.Raycast( rayToLeft, out leftDistance );
			
			Vector3 leftWorldPoint = rayToLeft.GetPoint( leftDistance );
			float leftAngle = Mathf.Atan( Mathf.Abs( leftWorldPoint.x - Accessor.Width / 2.0f ) / 10f );
			leftZ = ( Accessor.Width + 2 ) / 2 / Mathf.Tan( leftAngle );

			
			float topDistance;
			gamePlane.Raycast( rayToTop, out topDistance );
			
			Vector3 topWorldPoint = rayToTop.GetPoint( topDistance );
			float topAngle = Mathf.Atan( Mathf.Abs( topWorldPoint.y - Accessor.Height / 2.0f ) / 10f );
			topZ = ( Accessor.Height + 2 ) / 2 / Mathf.Tan( topAngle );

			cam.transform.position = new Vector3( Accessor.Width / 2.0f, Accessor.Height / 2.0f, -Mathf.Max( topZ, leftZ ) );
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
