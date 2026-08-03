using System.Collections;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class ProbabilityJackpotDisplay : MonoBehaviour
{
	public RoundManager roundManager;

	public GameObject effect;

	private void Awake()
	{
		effect.SetActive(value: false);
		roundManager.OnJackpot.Subscribe(OnWinNumberReceived).AddTo(this);
	}

	private void OnWinNumberReceived(int value)
	{
		if (roundManager.probabilityNumber.Value == 50 || roundManager.probabilityNumber.Value == 100 || roundManager.probabilityNumber.Value == 1000 || roundManager.probabilityNumber.Value == 2000)
		{
			effect.SetActive(value: false);
			StopAllCoroutines();
			StartCoroutine(Effect());
		}
	}

	private IEnumerator Effect()
	{
		effect.SetActive(value: true);
		yield return new WaitForSeconds(roundManager.probabilityDuration * 3f);
		effect.SetActive(value: false);
	}
}
