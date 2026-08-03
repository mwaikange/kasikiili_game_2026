using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundStartMotionDisplay : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public GameObject flare;

	private void Awake()
	{
		rouletteManager.tableActive.Subscribe(OnTableActive).AddTo(this);
		rouletteManager.gameActive.Subscribe(OnGameActive).AddTo(this);
	}

	private void OnTableActive(bool start)
	{
		if (!rouletteManager.gameActive.Value)
		{
			flare.SetActive(value: false);
		}
		else
		{
			flare.SetActive(!start);
		}
	}

	private void OnGameActive(bool isActive)
	{
		flare.SetActive(!rouletteManager.tableActive.Value);
	}
}
