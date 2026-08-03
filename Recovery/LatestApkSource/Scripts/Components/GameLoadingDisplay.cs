using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class GameLoadingDisplay : MonoBehaviour
{
	public GameCmdFactory gameCmdFactory;

	public GameManager gameManager;

	public AudioManager audioManager;

	private void Awake()
	{
		gameManager.loader.OnLoading.Subscribe(OnLoadingFinished).AddTo(this);
	}

	private void OnLoadingFinished(bool isActive)
	{
		if (isActive)
		{
			gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Loading).Execute();
			return;
		}
		GameScene gameScene = gameManager.GetGameScene(GameState.Loading);
		gameManager.loader.CloseAdditiveScene(gameScene.index);
	}
}
