using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class ProbabilityJackpotAudioDisplay : MonoBehaviour
{
	public RoundManager roundManager;

	public AudioManager audioManager;

	public AudioSource audioSource;

	public AbstractAudio fxAudio;

	private void Awake()
	{
		roundManager.OnJackpot.Subscribe(OnWinNumberReceived).AddTo(this);
	}

	private void OnWinNumberReceived(int value)
	{
		if (roundManager.probabilityNumber.Value == 50 || roundManager.probabilityNumber.Value == 100 || roundManager.probabilityNumber.Value == 1000 || roundManager.probabilityNumber.Value == 2000)
		{
			audioManager.Play(fxAudio, audioSource);
		}
	}
}
