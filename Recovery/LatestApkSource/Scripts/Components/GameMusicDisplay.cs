using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class GameMusicDisplay : MonoBehaviour
{
	public AudioManager audioManager;

	public AudioSource audioSource;

	private void Start()
	{
		audioManager.PlayMasterMusic(audioSource);
		audioManager.masterVolume.Subscribe(OnMasterVolumeChange).AddTo(this);
	}

	private void OnMasterVolumeChange(float value)
	{
		audioSource.volume = value;
	}
}
