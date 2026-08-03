using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class CloseCashoutInput : MonoBehaviour
{
	public GameManager gameManager;

	public RoundManager roundManager;

	public RouletteManager rouletteManager;

	public GameObject cashoutMenu;

	public TableManager tableManager;

	public AudioManager audioManager;

	public RouletteCmdFactory rouletteCmdFactory;

	public void OnClick()
	{
		cashoutMenu.SetActive(value: false);
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
