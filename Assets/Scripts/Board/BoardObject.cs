using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BoardObject : MonoBehaviour {

	public int maxSpeed = 0;
	public IntVector2 _direction = new IntVector2(0,0);
	public IntVector2 direction 
	{
		get 
		{ 
			
			return _direction; 
		}
		set { 
			_direction = value; 
			if ( _direction.x == 0 && _direction.y == 0 && boardLocation.location.x > 1 )
			{
				int x = 0;
				x++;
			}
			
		}
	}

	public BoardLocation _boardLocation = new BoardLocation(new IntVector2( 0, 0), new IntVector2(0,0));
	public BoardLocation boardLocation
	{	
		get
		{
			return _boardLocation;
		} 
		set
		{
			lastBoardLocation = _boardLocation.Clone();
			_boardLocation = value;
		}
	}
	public BoardLocation lastBoardLocation {get; set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
