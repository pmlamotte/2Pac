using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class PacmanData : MonoBehaviour {
	
	public float score = 0;
	public int playerNum = 0;

	public BoardObject _Data;
	public BoardObject Data
	{
		get
		{
			if ( _Data == null )
			{
				_Data = GetComponent<BoardObject>();
			}
			return _Data;
		}
		private set { throw new NotImplementedException(); }
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
