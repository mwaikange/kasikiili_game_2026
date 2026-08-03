using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class PaymentCreditDisplay : MonoBehaviour
{
	public TableManager tableMoney;

	[Header("Counting")]
	public TextMeshProUGUI numberLabel;

	public int countFps = 30;

	public float duration = 1f;

	public string format = "N0";

	private int _value;

	private Coroutine _coroutine;

	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			UpdateText(value);
			_value = value;
		}
	}

	private void Start()
	{
		tableMoney.cashManager.currentCredit.Subscribe(OnBetChange).AddTo(this);
	}

	private void UpdateText(int newValue)
	{
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		if (!(base.gameObject == null))
		{
			_coroutine = StartCoroutine(CountText(newValue));
		}
	}

	private IEnumerator CountText(int newValue)
	{
		WaitForSeconds Wait = new WaitForSeconds(1f / (float)countFps);
		int previousValue = _value;
		int stepAmount = ((newValue - previousValue >= 0) ? Mathf.CeilToInt((float)(newValue - previousValue) / ((float)countFps * duration)) : Mathf.FloorToInt((float)(newValue - previousValue) / ((float)countFps * duration)));
		if (previousValue < newValue)
		{
			while (previousValue < newValue)
			{
				previousValue += stepAmount;
				if (previousValue > newValue)
				{
					previousValue = newValue;
				}
				numberLabel.SetText(previousValue.ToString(format));
				yield return Wait;
			}
			yield break;
		}
		while (previousValue > newValue)
		{
			previousValue += stepAmount;
			if (previousValue < newValue)
			{
				previousValue = newValue;
			}
			numberLabel.SetText(previousValue.ToString(format));
			yield return Wait;
		}
	}

	private void OnBetChange(int value)
	{
		Value = value;
	}
}
