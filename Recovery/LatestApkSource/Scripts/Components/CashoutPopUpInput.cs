using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutPopUpInput : MonoBehaviour
{
	public GameManager gameManager;

	public RoundManager roundManager;

	private void Start()
	{
		gameManager.OnCashoutSuccess.Subscribe(delegate
		{
			Success();
		}).AddTo(this);
	}

	private void Success()
	{
		roundManager.OnReset.OnNext(value: true);
	}
}
