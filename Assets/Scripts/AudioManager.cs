using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource sfxAudioSource;
	public AudioSource musicAudioSource;

	public AudioClip deathAudio;
	public AudioClip collisionAudio;
	public AudioClip shootAudio;
	public AudioClip stepAudio;
	public AudioClip victoryAudio;
	public AudioClip selectAudio;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void PlayAudio(AudioClip audioClip)
	{
		if (sfxAudioSource != null)
		{
			sfxAudioSource.PlayOneShot(audioClip);
		}
	}
}
