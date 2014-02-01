using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : Singleton<GameData> {

	private Dictionary<int, float> scores = new Dictionary<int, float>();
	public int level = 0;
	public int PlayerLives = 5;


	public float getScore(int id) {
		if (!scores.ContainsKey(id)) {
			return 0;
		}
		return scores[id];
	}

	public void setScore(int id, float score) {
		if (!scores.ContainsKey(id)) {
			scores.Add(id, score);
		}
		scores[id] = score;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Clear()
	{
		scores = new Dictionary<int, float>();
		level = 0;
		PlayerLives = 5;
	}


}
