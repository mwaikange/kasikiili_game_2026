using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class RouletteStartInput : MonoBehaviour
{
	public RouletteCmdFactory rouletteCmdFactory;

	public RouletteManager rouletteManager;

	public AudioManager audioManager;

	public RoundManager roundManager;

	public TableManager tableManager;

	public void Start()
	{
		rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
		rouletteCmdFactory.StartGame(rouletteManager, roundManager, tableManager).Execute();
	}
}
