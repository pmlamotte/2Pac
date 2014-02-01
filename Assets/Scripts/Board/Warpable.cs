using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Warpable : MonoBehaviour {

	private BoardObject obj;
	private BoardData data;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		obj = GetComponent<BoardObject>();
		data = BoardData.getBoardData();
	}
	
	// Update is called once per frame
	void Update () {

		BoardLocation location = obj.boardLocation;
		IntVector2 pos = location.location;
		foreach (Warp warp in data.WarpPoints.Values) {
			if (pos.Equals (warp.input)) {
				obj.boardLocation = new BoardLocation(warp.output.Clone(), warp.outOffset.Clone());
				break;
			}
		}
	}
}
