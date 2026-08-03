using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundAudioDisplay : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public RoundManager roundManager;

	public AudioManager audioManager;

	public AbstractAudio fxAudio;

	public AudioSource audioSource;

	private void Awake()
	{
		roundManager.OnResetFinished.Subscribe(OnResetRound).AddTo(this);
	}

	private void OnResetRound(bool clear)
	{
		if (clear && rouletteManager.tableActive.Value && rouletteManager.gameActive.Value)
		{
			audioManager.Play(fxAudio, audioSource);
		}
	}

	public void Click()
	{
		if (rouletteManager.tableActive.Value && rouletteManager.gameActive.Value)
		{
			audioManager.Play(fxAudio, audioSource);
		}
		else if (!rouletteManager.tableActive.Value && rouletteManager.gameActive.Value)
		{
			audioManager.Play(fxAudio, audioSource);
		}
	}
}
