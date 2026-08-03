using UnityEngine;

namespace ViewModel;

public abstract class AbstractAudio : ScriptableObject
{
	public abstract void Play(AudioSource audioSource);

	public abstract AudioTypeOf GetTypeOf();

	public abstract AudioClip GetClip();

	public abstract void SetVolume(float volume);

	public abstract void Stop(AudioSource audioSource);
}
