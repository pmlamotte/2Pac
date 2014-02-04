using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardRenderer : MonoBehaviour {

	public GameObject filledCell;

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
					toAdd += ( Board.isOpen( x - 1 + j, y + 1 - i ) ) ? "0" : "1";
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


	// Use this for initialization
	public void CreateBoard () {
		for ( int y = 0; y < Board.Height; y++ )
		{
			for ( int x = 0; x < Board.Width; x++ )
			{
				createCell( x, y, !Board.isOpen( x, y ) );
			}
		}

		// make sure entire board is visible.
		Camera cam = GameObject.Find( "Camera" ).GetComponent<Camera>();
		if ( !cam.isOrthoGraphic )
		{
			// todo logic could be simpler given that we have similar triangles...

			cam.transform.position = new Vector3( Board.Width / 2f, Board.Height / 2f, -10 );
			
			Ray rayToLeft = cam.ScreenPointToRay( new Vector3( -1, 0, 0 ) );
			Ray rayToTop = cam.ScreenPointToRay( new Vector3( 0, 1, 0 ) );

			// the z distance required to see the left and top
			float leftZ = 0;
			float topZ = 0;

			Plane gamePlane = new Plane( new Vector3( 0, 0, -1 ), new Vector3( 0, 0, 0 ) );
			float leftDistance;
			gamePlane.Raycast( rayToLeft, out leftDistance );
			
			Vector3 leftWorldPoint = rayToLeft.GetPoint( leftDistance );
			float leftAngle = Mathf.Atan( Mathf.Abs( leftWorldPoint.x - Board.Width / 2.0f ) / 10f );
			leftZ = ( Board.Width + 2 ) / 2 / Mathf.Tan( leftAngle );

			
			float topDistance;
			gamePlane.Raycast( rayToTop, out topDistance );
			
			Vector3 topWorldPoint = rayToTop.GetPoint( topDistance );
			float topAngle = Mathf.Atan( Mathf.Abs( topWorldPoint.y - Board.Height / 2.0f ) / 10f );
			topZ = ( Board.Height + 2 ) / 2 / Mathf.Tan( topAngle );

			cam.transform.position = new Vector3( Board.Width / 2.0f, Board.Height / 2.0f, -Mathf.Max( topZ, leftZ ) );
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
