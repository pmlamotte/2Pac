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
	public BoardLocation _lastBoardLocation = new BoardLocation(new IntVector2( 0, 0), new IntVector2(0,0));
	public BoardLocation lastBoardLocation
	{	
		get
		{
			return _lastBoardLocation;
		} 
		set
		{
			_lastBoardLocation = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		BoardLocation newLocation= new BoardLocation(new IntVector2(0,0), new IntVector2(0,0));
		int maxSpeed = 0;
		IntVector2 direction = new IntVector2(0,0);
		BoardLocation lastBoardLocation = new BoardLocation(new IntVector2( 0, 0), new IntVector2(0,0));

		if (stream.isWriting)
		{
			newLocation = this.boardLocation;
			maxSpeed = this.maxSpeed;
			direction = this.direction;
			lastBoardLocation = this.lastBoardLocation;

			newLocation.Serialize( stream );
			stream.Serialize( ref maxSpeed );
			direction.Serialize( stream );
			lastBoardLocation.Serialize( stream );
		}
		else
		{
			newLocation.DeSerialize( stream );
			stream.Serialize( ref maxSpeed );
			direction.DeSerialize( stream );
			lastBoardLocation.DeSerialize( stream );
			
			this.boardLocation = newLocation;
			this.maxSpeed = maxSpeed;
			this.direction = direction;
			this.lastBoardLocation = lastBoardLocation;
		}
	}
}
