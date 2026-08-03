using UnityEngine;
using ViewModel;

namespace Commands;

[CreateAssetMenu(fileName = "GameCmdFactory", menuName = "Factory/GameCmdFactory", order = 0)]
public class GameCmdFactory : ScriptableObject
{
	public GameStartCmd StartGame(GameManager gameManager)
	{
		return new GameStartCmd(gameManager);
	}

	public GameStateCmd ChangeScene(GameManager gameManager, AudioManager audioManager, GameState stateToChange)
	{
		return new GameStateCmd(gameManager, audioManager, stateToChange);
	}
}
