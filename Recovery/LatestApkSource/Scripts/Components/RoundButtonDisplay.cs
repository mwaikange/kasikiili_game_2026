using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class RoundButtonDisplay : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public Button buttonObj;

	public void Awake()
	{
		rouletteManager.gameActive.Subscribe(OnGameActive).AddTo(this);
	}

	private void OnGameActive(bool isActive)
	{
		buttonObj.interactable = isActive;
	}
}
