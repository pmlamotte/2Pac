using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	private static AudioClip PelletEatSound;
	private static AudioClip SpookyGhostSound;
	public static AudioSource SoundPlayer;
	
	public static void PelletEat()
	{
		SoundPlayer.PlayOneShot( PelletEatSound );
	}
	
	public static void SpookyGhost()
	{
		SoundPlayer.PlayOneShot( SpookyGhostSound );
	}

	// Use this for initialization
	void Start () {

		PelletEatSound = Resources.Load<AudioClip>( "Media/Sounds/PelletEat" );
		SpookyGhostSound = Resources.Load<AudioClip>( "Media/Sounds/Spookhouse" );
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		SoundPlayer = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
