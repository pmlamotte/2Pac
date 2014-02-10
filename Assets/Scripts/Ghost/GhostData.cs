using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GhostData : BoardObject {

	private BoardObject _Data;
	private BoardObject Data
	{
		get
		{
			if ( _Data == null )
			{
				_Data = GetComponent<BoardObject>();
			}
			return _Data;
		}
		 set { throw new NotImplementedException(); }
	}
	
	public int ghostNumber {get; set;}
	
	public bool HasTurned = false;

	public LinkedList<int> PlayersCanEat {get; set;}



	public GhostData( )
	{
		PlayersCanEat = new LinkedList<int>();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
