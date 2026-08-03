using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class ProbabilityAudioDisplay : MonoBehaviour
{
	public RoundManager roundManager;

	public AudioManager audioManager;

	public AudioSource audioSource;

	public AbstractAudio fxAudio;

	private void Awake()
	{
		roundManager.OnProbability.Subscribe(OnWinNumberReceived).AddTo(this);
	}

	private void OnWinNumberReceived(int value)
	{
		audioManager.Play(fxAudio, audioSource);
	}
}
