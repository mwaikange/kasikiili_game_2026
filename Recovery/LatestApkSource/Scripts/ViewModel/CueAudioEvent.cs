using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "MultipleAudioEvent", menuName = "Event/Audio/Cue", order = 0)]
public class CueAudioEvent : AbstractAudio
{
	public AudioTypeOf audioTypeOf;

	public AudioClip[] clips;

	public float delayBetween;

	public AudioConfiguration configuration;

	public override void Play(AudioSource audioSource)
	{
		if (!(audioSource == null))
		{
			configuration.ApplyTo(audioSource);
			Observable.FromCoroutine((IObserver<Unit> observer) => PlayImagesEvent(observer, audioSource)).Subscribe().AddTo(audioSource.gameObject);
		}
	}

	public override AudioTypeOf GetTypeOf()
	{
		return audioTypeOf;
	}

	public override AudioClip GetClip()
	{
		return clips[UnityEngine.Random.Range(0, clips.Length)];
	}

	public override void SetVolume(float volume)
	{
		configuration.volume = volume;
	}

	private IEnumerator PlayImagesEvent(IObserver<Unit> observer, AudioSource audioSource)
	{
		AudioClip[] array = clips;
		foreach (AudioClip audioClip in array)
		{
			if (audioSource == null)
			{
				break;
			}
			audioSource.clip = audioClip;
			audioSource.Play();
			yield return new WaitForSeconds(audioClip.length + delayBetween);
		}
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}

	public override void Stop(AudioSource audioSource)
	{
		if (!(audioSource == null))
		{
			audioSource.Stop();
		}
	}
}
