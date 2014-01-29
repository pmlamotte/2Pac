using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Direction {
	public static IntVector2 RIGHT = new IntVector2(1, 0);
	public static IntVector2 DOWN = new IntVector2(0, -1);
	public static IntVector2 LEFT = new IntVector2(-1, 0);
	public static IntVector2 UP = new IntVector2(0, 1);

	public static IntVector2 opposite(IntVector2 dir) {
		if (dir == RIGHT) {
			return LEFT;
		} else if (dir == DOWN) {
			return UP;
		} else if (dir == LEFT) {
			return RIGHT;
		} else if (dir == UP) {
			return DOWN;
		}

		return null;
	}

	public static IntVector2 getDirection(int dir) {
		switch(dir) {
		case 0: return UP;
		case 1: return RIGHT;
		case 2: return DOWN;
		case 3: return LEFT;
		default: return null;
		}
	}

	public static IntVector2 getDirection(string dir) {
		switch(dir.ToUpper()) {
		case "U": return UP;
		case "R": return RIGHT;
		case "D": return DOWN;
		case "L": return LEFT;
		default: return null;
		}
	}

	public static string getDirectionChar(IntVector2 dir) {
		if (dir == UP) {
			return "U";
		} else if (dir == RIGHT) {
			return "R";
		} else if (dir == DOWN) {
			return "D";
		} else if (dir == LEFT) {
			return "L";
		}
		return null;
	}
}
