using System;
using UnityEngine;

namespace ViewModel;

[Serializable]
public class AudioConfiguration
{
	public bool loop;

	public bool mute;

	[Range(0f, 1f)]
	public float volume = 1f;

	[Range(-3f, 3f)]
	public float pitchMin = 1f;

	[Range(-3f, 3f)]
	public float pitchMax = 1f;

	[Range(-1f, 1f)]
	public float panStereo;

	[Range(0f, 1.1f)]
	public float reverbZoneMix = 1f;

	public void ApplyTo(AudioSource audioSource)
	{
		audioSource.loop = loop;
		audioSource.mute = mute;
		audioSource.volume = volume;
		audioSource.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
		audioSource.panStereo = panStereo;
		audioSource.reverbZoneMix = reverbZoneMix;
	}
}
