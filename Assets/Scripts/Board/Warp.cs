using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Warp {

	public IntVector2 input;
	public IntVector2 output;
	private IntVector2 _outDirection;
	public IntVector2 outDirection { get {return _outDirection;} set { _outDirection = value; computeOffset(); }}
	public IntVector2 outOffset;

	private void computeOffset() {
		if (outDirection == Direction.UP) {
			outOffset = new IntVector2(0, -Constants.BoardCellRadius + 1);
		} else if (outDirection == Direction.RIGHT) {
			outOffset = new IntVector2(-Constants.BoardCellRadius + 1, 0);
		} else if (outDirection == Direction.DOWN) {
			outOffset = new IntVector2(0, Constants.BoardCellRadius);
		} else if (outDirection == Direction.LEFT) {
			outOffset = new IntVector2(Constants.BoardCellRadius, 0);
		}
	}
}
