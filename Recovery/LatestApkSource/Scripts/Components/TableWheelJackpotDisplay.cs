using System.Collections;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class TableWheelJackpotDisplay : MonoBehaviour
{
	public RoundManager roundManager;

	public GameObject effect;

	private void Awake()
	{
		effect.SetActive(value: false);
		roundManager.OnJackpot.Subscribe(OnWinNumberReceived).AddTo(this);
		roundManager.OnResetFinished.Subscribe(OnClear).AddTo(this);
	}

	private void OnClear(bool clear)
	{
		if (clear)
		{
			effect.SetActive(value: false);
		}
	}

	private void OnWinNumberReceived(int value)
	{
		if (roundManager.probabilityNumber.Value == 50 || roundManager.probabilityNumber.Value == 100 || roundManager.probabilityNumber.Value == 1000 || roundManager.probabilityNumber.Value == 2000)
		{
			StopAllCoroutines();
			StartCoroutine(DecreaseTime(roundManager.probabilityJackpotDuration));
		}
	}

	private IEnumerator DecreaseTime(float totalTime)
	{
		effect.SetActive(value: true);
		yield return new WaitForSeconds(totalTime);
		effect.SetActive(value: false);
	}
}
