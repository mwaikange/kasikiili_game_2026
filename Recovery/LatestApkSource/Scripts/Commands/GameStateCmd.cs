using UnityEngine;
using ViewModel;

namespace Commands;

public class GameStateCmd : ICommand
{
	private LoaderManager _loaderManager;

	private GameManager _gameManager;

	private AudioManager _audioManager;

	private GameState _stateToChange;

	public GameStateCmd(GameManager gameManager, AudioManager audioManager, GameState stateToChange)
	{
		_gameManager = gameManager;
		_loaderManager = gameManager.loader;
		_audioManager = audioManager;
		_stateToChange = stateToChange;
	}

	public void Execute()
	{
		Debug.Log("Game changed to " + _stateToChange);
		_loaderManager.loadingQueue = 0;
		switch (_stateToChange)
		{
		case GameState.Start:
			Start(GameState.Start);
			break;
		case GameState.Menu:
			Menu(GameState.Menu);
			break;
		case GameState.Game:
			Game(GameState.Game);
			break;
		case GameState.Loading:
			Loading(GameState.Loading);
			break;
		case GameState.Error:
			Error(GameState.Error);
			break;
		default:
			Debug.LogWarning("Not implemented scene! Please scene need to be valid to change.");
			break;
		}
	}

	private void Error(GameState state)
	{
		GameScene gameScene = _gameManager.GetGameScene(state);
		_loaderManager.OpenAdditiveScene(gameScene.index);
		_audioManager.masterVolume.Value = 0.0045f;
	}

	private void Start(GameState state)
	{
		GameScene gameScene = _gameManager.GetGameScene(state);
		_loaderManager.OpenScene(gameScene.index);
	}

	private void Loading(GameState state)
	{
		_gameManager.loader.loadingQueue++;
		if (_gameManager.loader.loadingQueue <= 1)
		{
			GameScene gameScene = _gameManager.GetGameScene(state);
			_loaderManager.OpenAdditiveScene(gameScene.index);
		}
	}

	private void Game(GameState state)
	{
		GameScene gameScene = _gameManager.GetGameScene(state);
		_audioManager.masterVolume.Value = 0.0045f;
		_loaderManager.OpenScene(gameScene.index);
	}

	private void Menu(GameState state)
	{
		GameScene gameScene = _gameManager.GetGameScene(state);
		_audioManager.masterVolume.Value = 0.045f;
		_loaderManager.OpenScene(gameScene.index);
	}
}
