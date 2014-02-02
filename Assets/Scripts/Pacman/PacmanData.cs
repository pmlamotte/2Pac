using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class PacmanData : BoardObject {
	
	public float score = 0;
	public int playerNum = 0;

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

	void Start() {
	}

	[RPC] public void setPlayerNum(int num) {
		playerNum = num;
		if (Network.isServer) {
			networkView.RPC("setPlayerNum", RPCMode.OthersBuffered, num);
		}
	}
	
	void Update () {
	}
}
