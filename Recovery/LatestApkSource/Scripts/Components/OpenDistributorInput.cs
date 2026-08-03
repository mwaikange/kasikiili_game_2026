using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class OpenDistributorInput : MonoBehaviour
{
	public GameManager gameManager;

	public RouletteManager rouletteManager;

	public GameObject distributorMenu;

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
		distributorMenu.SetActive(value: true);
		gameMenu.SetActive(value: false);
	}
}
