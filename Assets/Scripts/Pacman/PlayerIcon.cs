using UnityEngine;
using System.Collections;

public class PlayerIcon : MonoBehaviour {

	public PacmanData pacmanData;
	public Camera cam;
	private GUIStyle textStyle;

	// Use this for initialization
	void Start () {

	}

	void Awake() {

		pacmanData = GetComponent<PacmanData>();
		cam = GameObject.FindObjectOfType<Camera>();

	}

	void OnGUI() {
		if (textStyle == null) {
			textStyle = new GUIStyle(GUI.skin.GetStyle("box"));
			textStyle.normal.background = GraphicsUtil.MakeTexture(1, 1, new Color(0.0f, 0.0f, 0.0f, 0.12f));
			textStyle.fontStyle = FontStyle.Bold;
			textStyle.fontSize = 18;
			textStyle.border.right = textStyle.border.left = textStyle.border.top = textStyle.border.bottom = 1;
			textStyle.margin.right = textStyle.margin.left = textStyle.margin.top = textStyle.margin.bottom = 1;
			textStyle.padding.right = textStyle.padding.left = textStyle.padding.top = textStyle.padding.bottom = 1;
			textStyle.contentOffset = new Vector2(0, -3);
			textStyle.clipping = 0;
			switch (pacmanData.playerNum) {
			case 0: break;
			case 1: textStyle.normal.textColor = new Color(1.0f, 0.0f, 0.0f); break;
			case 2: textStyle.normal.textColor = new Color(0.0f, 0.0f, 1.0f); break;
			case 3: textStyle.normal.textColor = new Color(0.0f, 1.0f, 0.0f); break;
			}
		}
		Vector3 pos = cam.WorldToScreenPoint(transform.position);

		string text = "P" + pacmanData.playerNum;
		Vector2 size = textStyle.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(pos.x - size.x / 2 + 2, Screen.height - pos.y - GetComponent<SpriteRenderer>().sprite.rect.height / 2 + 14, size.x - 4, 17), new GUIContent(text), textStyle);
	}

	// Update is called once per frame
	void Update () {
	}
}
