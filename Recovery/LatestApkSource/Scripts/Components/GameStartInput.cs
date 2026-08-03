using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class GameStartInput : MonoBehaviour
{
	public GameManager gameManager;

	public AudioManager audioManager;

	public BackendCmdFactory backendCmdFactory;

	public GameCmdFactory gameCmdFactory;

	private void Awake()
	{
	}

	private void Start()
	{
		OnGameStart(isSuccess: true);
	}

	private void OnGameStart(bool isSuccess)
	{
		MonoBehaviour.print("+++++++ " + isSuccess);
		if (isSuccess)
		{
			gameCmdFactory.StartGame(gameManager).Execute();
			gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Menu).Execute();
		}
	}
}
