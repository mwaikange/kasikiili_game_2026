using System;
using System.Collections;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class ProbabilityNumberDisplay : MonoBehaviour
{
	[Header("Round")]
	public RoundManager roundManager;

	public GameObject[] lights;

	[Header("Duration")]
	public float stepQuick = 0.1f;

	private int _currentPos;

	private static readonly int[] ProbabilityNumbers = new int[6] { 10, 50, 1000, 2000, 25, 100 };

	private void Awake()
	{
		TurnOffAll();
		roundManager.OnProbability.Subscribe(OnProbabilityNumber).AddTo(this);
		roundManager.OnResetFinished.Subscribe(OnClear).AddTo(this);
	}

	private void OnClear(bool clear)
	{
		if (clear)
		{
			TurnOffAll();
		}
	}

	private void OnProbabilityNumber(int probabilityNumber)
	{
		Debug.Log("Probability number is " + probabilityNumber);
		StopAllCoroutines();
		StartCoroutine(DecreaseTime(probabilityNumber, roundManager.probabilityDuration));
	}

	private IEnumerator DecreaseTime(int probabilityNumber, float totalTime)
	{
		TurnOffAll();
		float step = stepQuick;
		float timeLeft = totalTime;
		while (timeLeft >= 0f)
		{
			if (_currentPos >= ProbabilityNumbers.Length)
			{
				_currentPos = 0;
			}
			lights[_currentPos].SetActive(value: true);
			timeLeft -= step;
			yield return new WaitForSeconds(step);
			lights[_currentPos].SetActive(value: false);
			_currentPos++;
		}
		TurnOffAll();
		SetFlareFinalRotatorPosition(probabilityNumber);
	}

	private void TurnOffAll()
	{
		_currentPos = 0;
		GameObject[] array = lights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	private int GetArrayPosition(int probability)
	{
		return Array.FindIndex(ProbabilityNumbers, (int row) => row == probability);
	}

	private void SetFlareFinalRotatorPosition(int numberWin)
	{
		int arrayPosition = GetArrayPosition(numberWin);
		lights[arrayPosition].SetActive(value: true);
	}
}
