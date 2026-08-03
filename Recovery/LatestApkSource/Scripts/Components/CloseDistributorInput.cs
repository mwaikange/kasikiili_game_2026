using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class CloseDistributorInput : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public GameObject distriMenu;

	public TableManager tableManager;

	public AudioManager audioManager;

	public RouletteCmdFactory rouletteCmdFactory;

	public void OnClick()
	{
		distriMenu.SetActive(value: false);
		if (tableManager.cashManager.currentCredit.Value >= rouletteManager.gameLimit)
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Cashout).Execute();
		}
		else
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
		}
	}
}
