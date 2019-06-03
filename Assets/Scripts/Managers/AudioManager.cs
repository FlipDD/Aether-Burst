using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
	public Sound[] sounds;
	public float sfxMultiplier = 1;
	public float musicMultiplier = 1;

	private void Awake() 
	{
		DontDestroyOnLoad(gameObject);

		if (i == null)
			i = this;
		else
			Destroy(gameObject);
		// i = this;
		
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
		}
	}

	public void Play (string name, Vector3 pos, bool looping = false)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.volume = s.volume * sfxMultiplier;
		s.source.spatialBlend = 1;
		s.source.rolloffMode = AudioRolloffMode.Linear;
		s.source.minDistance = 0;
		s.source.maxDistance = 300;
		s.source.loop = looping;
		s.source.transform.position = pos;
		s.source.Play();
	}

	public void PlayBackground (string name, bool looping = true)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.volume = musicMultiplier;
		s.source.loop = looping;
		s.source.Play();
	}

	public void ChangeVolume(string name, float multiplier)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.volume = musicMultiplier * multiplier;
	}

	public void Stop (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.Stop();
	}

	public void StopAll()
	{
		foreach (Sound s in sounds)
			s.source.Stop();
	}

    public void SetSound(float value) => musicMultiplier = value;
	public void SetSfx(float value) => sfxMultiplier = value;

}
