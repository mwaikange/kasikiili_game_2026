using System;
using System.Collections;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class TableWheelDisplay : MonoBehaviour
{
	[Header("Manager")]
	public RoundManager roundManager;

	[Header("Audio")]
	public AudioManager audioManager;

	public AudioSource audioSource;

	public AbstractAudio wheelFx;

	[Header("Wheel")]
	public GameObject rotatorFlare;

	public float stepQuick = 0.1f;

	public float stepDecreaseHigh = 0.02f;

	public float stepDecreaseLow = 0.05f;

	public float stepDecreaseSlow = 0.01f;

	private const float RotatorAngle = 27.692307f;

	private static readonly int[] WheelNumbers = new int[13]
	{
		0, 4, 11, 6, 7, 2, 9, 8, 1, 10,
		3, 12, 5
	};

	private void Awake()
	{
		rotatorFlare.SetActive(value: false);
		roundManager.OnRound.Subscribe(OnWinNumberReceived).AddTo(this);
		roundManager.OnResetFinished.Subscribe(OnClearRound).AddTo(this);
	}

	private void OnClearRound(bool clear)
	{
		if (clear)
		{
			rotatorFlare.SetActive(value: false);
		}
	}

	private void OnWinNumberReceived(int winNumber)
	{
		rotatorFlare.SetActive(value: true);
		Debug.Log("Winning number is " + winNumber);
		audioManager.Play(wheelFx, audioSource);
		StopAllCoroutines();
		StartCoroutine(DecreaseTime(winNumber, roundManager.wheelDuration));
	}

	private IEnumerator DecreaseTime(int numberWin, float totalTime)
	{
		SetFlareRotatorPosition(numberWin);
		float step = stepQuick;
		float middleTime = totalTime / 2f;
		float thirdTime = totalTime / 3f;
		float timeLeft = totalTime;
		Debug.Log("START WHEEL!");
		while (timeLeft >= 0f)
		{
			step += stepDecreaseSlow;
			if (timeLeft <= middleTime && timeLeft > thirdTime)
			{
				step += stepDecreaseLow;
			}
			else if (timeLeft <= thirdTime)
			{
				step += stepDecreaseHigh;
			}
			timeLeft -= step;
			rotatorFlare.transform.Rotate(new Vector3(0f, 0f, 27.692307f));
			yield return new WaitForSeconds(step);
		}
		SetFlareFinalRotatorPosition(numberWin);
		Debug.Log("END WHEEL!");
	}

	private void SetFlareRotatorPosition(int numberWin)
	{
		float num = (float)GetArrayPosition(numberWin) * 27.692307f - 27.692307f;
		if (num < 0f)
		{
			num = 332.30768f;
		}
		rotatorFlare.transform.eulerAngles = new Vector3(0f, 0f, num);
	}

	private void SetFlareFinalRotatorPosition(int numberWin)
	{
		float z = (float)GetArrayPosition(numberWin) * 27.692307f;
		rotatorFlare.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	private int GetArrayPosition(int numberWin)
	{
		return Array.FindIndex(WheelNumbers, (int row) => row == numberWin);
	}
}
