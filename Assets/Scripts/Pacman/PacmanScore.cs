using UnityEngine;
using System.Collections;

public class PacmanScore : MonoBehaviour {

	public GameObject textPrefab;
	private GUIText text;
	public PacmanData pacmanData;


	// Use this for initialization
	void Start () {
		// players score
		pacmanData = GetComponent<PacmanData>();
		GameObject playerText = (GameObject) Instantiate( textPrefab, new Vector3(0,0,0) , Quaternion.identity );
		text = (GUIText) playerText.GetComponent("GUIText");
	}

	void Awake() {

	}

	public float getScore() {
		return GameData.Instance.getScore(pacmanData.playerNum);
	}

	public void setScore(float score) {
		GameData.Instance.setScore(pacmanData.playerNum, score);
	}

	public void decrementScore(int num) {
		setScore(getScore() - num);
	}

	public void incrementScore(int num) {
		setScore(getScore() + num);
	}

	void PacmanHit() {
		decrementScore(10);
	}

	// Update is called once per frame
	void Update () {
		setScore(getScore() + (.1f * Time.deltaTime ));
		updateScoreText();
	}

	private void updateScoreText() {
		int playerNum = pacmanData.playerNum;
		text.transform.position = new Vector3( 0, 1 - .1f * playerNum, 0 ); 
		text.text = "Player " + playerNum + ": " + (int)getScore();
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		float syncScore = 0;
		if (stream.isWriting)
		{
			syncScore = getScore();
			stream.Serialize(ref syncScore);
		}
		else
		{
			stream.Serialize(ref syncScore);
			setScore(syncScore);
		}
	}
}
