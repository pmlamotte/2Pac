using UnityEngine;
using System.Collections;

public class PlayerDied : MonoBehaviour {

	public GUIStyle playerDiedStyle;
	private LTRect playerRect;
	private bool doAnim = false;
	private bool styleInitialized = true;
	private int playerNum = 0;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable() {
		Messenger<int>.AddListener(Events.PLAYER_HIT, startPlayerHitAnim);
	}

	void OnDisable() {
		Messenger<int>.RemoveListener(Events.PLAYER_HIT, startPlayerHitAnim);
	}

	void OnGUI() {
		if (!styleInitialized) {
			styleInitialized = true;
			playerRect.setStyle(new GUIStyle(playerDiedStyle));
		}

		if (doAnim) {
			int height = (int)(Screen.height * .25f);
			GUI.Box(new Rect(0, Screen.height / 2 - height / 2, Screen.width, height),"");
			Rect rect = new Rect(Screen.width / 2 - playerRect.width / 2, Screen.height / 2 - playerRect.height / 2, playerRect.width, playerRect.height);
			GUI.Label(rect, new GUIContent("PLAYER " + playerNum + " DIED"), playerRect.style);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void startPlayerHitAnim(int playerNum) {
		this.playerNum = playerNum;
		Time.timeScale = 0;
		if (GameData.Instance.PlayerLives <= 0) {
			return;
		}

		doAnim = true;
		playerRect = new LTRect( 0f, 0f, 0f, 0f );
		playerRect.setStyle(new GUIStyle(playerDiedStyle));
		playerRect.setFontScaleToFit(true);
		LeanTween.scale( playerRect, new Vector2(150f,100f), .75f ).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack).setOnComplete(delegate() {
			StartCoroutine(resetGameBoard());
		});
	}

	public IEnumerator resetGameBoard() {
		float time = 0;
		float lastTime = Time.realtimeSinceStartup;
		while (time < 2.0f) {
			time += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			yield return null;
		}

		doAnim = false;
		styleInitialized = false;
		GameObject.FindObjectOfType<BoardAccessor>().resetBoard();
		GameObject.FindObjectOfType<GameStart>().showStartingMessage();
	}
}
