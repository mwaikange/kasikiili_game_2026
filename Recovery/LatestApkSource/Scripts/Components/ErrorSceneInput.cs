using System;
using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class ErrorSceneInput : MonoBehaviour
{
	public GameCmdFactory gameCmdFactory;

	public GameManager gameManager;

	public AudioManager audioManager;

	private void Awake()
	{
		gameManager.errorManager.OnError.Subscribe(OnErrorHappen).AddTo(this);
	}

	private void OnErrorHappen(Exception error)
	{
		gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Error).Execute();
		gameManager.errorManager.lastException.Value = error;
	}
}
