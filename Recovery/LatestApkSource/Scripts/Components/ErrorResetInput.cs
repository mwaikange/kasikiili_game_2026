using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class ErrorResetInput : MonoBehaviour
{
	public GameCmdFactory gameCmdFactory;

	public AudioManager audioManager;

	public GameManager gameManager;

	public void OnClick()
	{
		gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Start).Execute();
	}
}
