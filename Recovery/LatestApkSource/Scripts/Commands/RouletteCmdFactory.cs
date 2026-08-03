using UnityEngine;
using ViewModel;

namespace Commands;

[CreateAssetMenu(fileName = "RouletteCmdFactory", menuName = "Factory/RouletteCmdFactory", order = 0)]
public class RouletteCmdFactory : ScriptableObject
{
	public RouletteStartCmd StartGame(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		return new RouletteStartCmd(rouletteManager, roundManager, tableManager);
	}

	public RouletteStateCmd TurnRouletteState(RouletteManager rouletteManager, AudioManager audioManager, RouletteState gameState)
	{
		return new RouletteStateCmd(rouletteManager, audioManager, gameState);
	}
}
