using UnityEngine;
using System.Collections;

public class Events {

	/**
	 * Triggered when any other player has connected to
	 * the server.
	 **/
	public const string PLAYER_CONNECTED = "PLAYER_CONNECTED";

	/**
	 * Triggered when any other player has disconnected from
	 * the server.
	 **/
	public const string PLAYER_DISCONNECTED = "PLAYER_DISCONNECTED";

	/**
	 * When the client's ID was set. This only refers to the current
	 * running instance, other clients who have their ID set will not
	 * trigger this event.
	 **/
	public const string PLAYER_ID_SET = "PLAYER_ID_SET";

	/**
	 * When a player toggles their ready state
	 **/
	public const string PLAYER_READY_CHANGED = "PLAYER_READY_CHANGED";
}
