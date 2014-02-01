using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {
	
	private static AudioClip PelletEatSound;
	private static AudioClip SpookyGhostSound;
	public static AudioSource SoundPlayer;
	
	public void PelletEat()
	{
		SoundPlayer.PlayOneShot( PelletEatSound );
	}
	
	public void SpookyGhost()
	{
		SoundPlayer.PlayOneShot( SpookyGhostSound );
	}

	// Use this for initialization
	void Awake () {
		SoundPlayer = this.gameObject.AddComponent<AudioSource>();
		PelletEatSound = Resources.Load<AudioClip>( "Media/Sounds/PelletEat" );
		SpookyGhostSound = Resources.Load<AudioClip>( "Media/Sounds/Spookhouse" );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
