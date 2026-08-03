using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutRequiredDisplay : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public GameObject cashoutMenu;

	public GameObject gameMenu;

	private void Start()
	{
		rouletteManager.OnCashoutRequired.Subscribe(delegate
		{
			OnClick();
		}).AddTo(this);
	}

	public void OnClick()
	{
		cashoutMenu.SetActive(value: true);
		gameMenu.SetActive(value: false);
	}
}
