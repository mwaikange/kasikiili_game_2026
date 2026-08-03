using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Global/Audio Manager", order = 0)]
public class AudioManager : ScriptableObject
{
	[Header("Audio Global")]
	public float masterDefault;

	public FloatReactiveProperty masterVolume = new FloatReactiveProperty();

	public AbstractAudio masterMusic;

	[Header("Audio control")]
	public BoolReactiveProperty musicState = new BoolReactiveProperty();

	public BoolReactiveProperty fxState = new BoolReactiveProperty();

	private AbstractAudio absAudio;

	public void Play(AbstractAudio abstractAudio, AudioSource source)
	{
		absAudio = abstractAudio;
		switch (abstractAudio.GetTypeOf())
		{
		case AudioTypeOf.Fx:
			if (fxState.Value)
			{
				abstractAudio.Play(source);
			}
			break;
		case AudioTypeOf.Music:
			if (musicState.Value)
			{
				abstractAudio.Play(source);
			}
			break;
		default:
			abstractAudio.Play(source);
			break;
		}
	}

	public void PlayMasterMusic(AudioSource audioSource)
	{
		masterMusic.SetVolume(masterVolume.Value);
		masterMusic.Play(audioSource);
	}

	public void StopAbstractAudio(AudioSource source)
	{
		absAudio.Stop(source);
	}

	public void StopMasterMusic(AudioSource source)
	{
		masterMusic.Stop(source);
	}
}
