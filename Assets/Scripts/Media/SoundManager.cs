using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {
	
	private static AudioClip PelletEatSound;
	private static AudioClip SpookyGhostSound;
	private static AudioClip GhostEatSound;
	public static AudioSource SoundPlayer;
	
	public void PelletEat()
	{
		SoundPlayer.volume = .5f;
		SoundPlayer.PlayOneShot( PelletEatSound );
		SoundPlayer.volume = 1.0f;
	}

	// todo
	public void PowerPelletEat()
	{
		PelletEat();
	}

	public void SpookyGhost()
	{
		SoundPlayer.PlayOneShot( SpookyGhostSound );
	}

	public void GhostEat()
	{
		SoundPlayer.PlayOneShot( GhostEatSound );
	}

	// Use this for initialization
	void Awake () {
		SoundPlayer = this.gameObject.AddComponent<AudioSource>();
		PelletEatSound = Resources.Load<AudioClip>( "Media/Sounds/PelletEat" );
		SpookyGhostSound = Resources.Load<AudioClip>( "Media/Sounds/Spookhouse" );
		GhostEatSound = Resources.Load<AudioClip>( "Media/Sounds/GhostEat" );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
