using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutBalanceDisplay : MonoBehaviour
{
	public TableManager tableMoney;

	public TextMeshProUGUI numberLabel;

	private void Start()
	{
		tableMoney.cashManager.currentCredit.Subscribe(OnBetChange).AddTo(this);
	}

	private void OnBetChange(int value)
	{
		numberLabel.text = value.ToString();
	}
}
