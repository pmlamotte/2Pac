using UnityEngine;
using System.Collections;
using AssemblyCSharp;

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
		if (pacmanData != null) {
			return GameData.Instance.getScore (pacmanData.playerNum);
		}
		return 0;
	}

	public void setScore(float score) {
		if (pacmanData != null) {
			GameData.Instance.setScore (pacmanData.playerNum, score);
		}
	}

	public void decrementScore(float num) {
		setScore(getScore() - num);
	}

	public void incrementScore(float num) {
		setScore(getScore() + num);
	}
	
	[RPC] public void AtePellet()
	{
		SoundManager.Instance.PelletEat();
		incrementScore( Constants.PelletWorth );
	}

	[RPC] public void AtePowerPellet()
	{
		SoundManager.Instance.PowerPelletEat();
		lock( pacmanData )
		{
			pacmanData.PowerPelletLevel++;
		}
		incrementScore( Constants.PelletWorth );

		StartCoroutine( PowerPelletTimeout() );
	}

	private IEnumerator PowerPelletTimeout()
	{
		yield return new WaitForSeconds( Constants.PowerPelletTime );
		lock( pacmanData )
		{
			pacmanData.PowerPelletLevel--;
		}
		if ( pacmanData.PowerPelletLevel <= 0 )
		{
			pacmanData.GhostsEaten = 0;
		}
		
	}

	[RPC] public void killedGhost()
	{
		SoundManager.Instance.GhostEat();
		lock( pacmanData )
		{
			pacmanData.GhostsEaten++;
		}
		incrementScore( Constants.GhostWorth * pacmanData.GhostsEaten );
	}

	void PacmanHit() {
		decrementScore(10);
	}

	// Update is called once per frame
	void Update () {
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
