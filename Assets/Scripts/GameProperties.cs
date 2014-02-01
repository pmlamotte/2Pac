using UnityEngine;
using System.Collections;

public class GameProperties : MonoBehaviour {

	public static bool isSinglePlayer = true;
	public static int numGhosts = 2;
	public static int maxPlayers = 0;
	public static PlayerInfo.Player myPlayer; // the current player
	public static string serverName = "";
}
