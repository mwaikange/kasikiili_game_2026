using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundLimitInput : MonoBehaviour
{
	public RouletteCmdFactory rouletteCmdFactory;

	public AudioManager audioManager;

	public RouletteManager rouletteManager;

	public TableManager tableManager;

	public void Awake()
	{
		tableManager.cashManager.currentCredit.Subscribe(OnCreditChanged).AddTo(this);
	}

	private void OnCreditChanged(int credits)
	{
		if (credits >= rouletteManager.gameLimit)
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Cashout).Execute();
		}
		else if (!rouletteManager.gameActive.Value)
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
		}
	}
}
