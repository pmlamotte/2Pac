using UnityEngine;
using System.Collections;

public class PacmanAnimate : MonoBehaviour {


	public float maxSpeed = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKey( KeyCode.LeftArrow ) )
		{
			Vector3 newpos = transform.position;
			newpos.x -= maxSpeed;
			transform.position = newpos;
			transform.localRotation = Quaternion.Euler( 0, 0, 180 );
		}
		if ( Input.GetKey( KeyCode.RightArrow ) )
		{
			Vector3 newpos = transform.position;
			newpos.x += maxSpeed;
			transform.position = newpos;
			transform.localRotation = Quaternion.Euler( 0, 0, 0 );
		}
		if ( Input.GetKey( KeyCode.UpArrow ) )
		{
			Vector3 newpos = transform.position;
			newpos.y += maxSpeed;
			transform.position = newpos;
			transform.localRotation = Quaternion.Euler( 0, 0, 90 );
		}
		if ( Input.GetKey( KeyCode.DownArrow ) )
		{
			Vector3 newpos = transform.position;
			newpos.y -= maxSpeed;
			transform.position = newpos;
			transform.localRotation = Quaternion.Euler( 0, 0, 270 );
		}

	}
}
