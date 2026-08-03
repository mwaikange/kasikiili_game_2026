using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutAwaitingDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public TextMeshProUGUI awaitingLabel;

	private void Awake()
	{
		gameManager.OnCashoutAwaiting.Subscribe(delegate(int value)
		{
			awaitingLabel.text = value.ToString();
		}).AddTo(this);
	}
}
