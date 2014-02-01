using UnityEngine;
using System.Collections;

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
			GameObject cell = (GameObject) Instantiate( filledCell );
			cell.transform.position = new Vector3(x + .5f,y + .5f, 0f);
		}
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
