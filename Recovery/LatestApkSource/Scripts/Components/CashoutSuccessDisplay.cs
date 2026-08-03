using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutSuccessDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public GameObject successMenu;

	private void Start()
	{
		gameManager.OnCashoutSuccess.Subscribe(delegate
		{
			Success();
		}).AddTo(this);
	}

	private void Success()
	{
		successMenu.SetActive(value: true);
	}
}
