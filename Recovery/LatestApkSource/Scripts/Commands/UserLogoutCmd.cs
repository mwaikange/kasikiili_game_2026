using System;
using Infrastructure;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class UserLogoutCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly UserGateway _userGateway;

	public UserLogoutCmd(GameManager gameManager, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_gameManager.loaderManager.OnLoading.OnNext(value: true);
		_userGateway.PostLogout(_gameManager).DoOnError(delegate(Exception error)
		{
			_gameManager.errorManager.OnError.OnNext(error);
		}).DoOnError(Debug.LogError)
			.Do(delegate
			{
				CloseGame();
			})
			.Subscribe();
	}

	private void CloseGame()
	{
		_gameManager.OnLogoutSuccess.OnNext(value: true);
		_gameManager.loaderManager.loadingQueue--;
		if (_gameManager.loaderManager.loadingQueue <= 0)
		{
			_gameManager.loaderManager.OnLoading.OnNext(value: false);
		}
	}
}
