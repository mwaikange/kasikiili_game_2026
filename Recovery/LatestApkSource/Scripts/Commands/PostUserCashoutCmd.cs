using System;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class PostUserCashoutCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly int _amount;

	private readonly PlayerGateway _playerGateway;

	public PostUserCashoutCmd(GameManager gameManager, int amount, PlayerGateway playerGateway)
	{
		_gameManager = gameManager;
		_amount = amount;
		_playerGateway = playerGateway;
	}

	public void Execute()
	{
		_gameManager.loader.OnLoading.OnNext(value: true);
		_playerGateway.PostUserCashout(_gameManager, _amount).DoOnError(HandleCashoutError).DoOnError(Debug.LogError)
			.Do(delegate
			{
				Success();
			})
			.Delay(TimeSpan.FromMilliseconds(1000.0))
			.Do(delegate
			{
				_gameManager.loaderManager.loadingQueue--;
				if (_gameManager.loaderManager.loadingQueue <= 0)
				{
					_gameManager.loaderManager.OnLoading.OnNext(value: false);
				}
			})
			.Subscribe();
	}

	private void HandleCashoutError(Exception error)
	{
		_gameManager.loaderManager.loadingQueue--;
		if (_gameManager.loaderManager.loadingQueue <= 0)
		{
			_gameManager.loaderManager.OnLoading.OnNext(value: false);
		}
		if (error.Message.Contains("html") || error.Message.Contains("HTTP"))
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Not found. Please check your network/data connection."));
		}
		else if (error.Message.Length > 3)
		{
			string value = JSON.Parse(error.Message)["message"].Value + " Action denied.";
			_gameManager.errorManager.OnAlertError.OnNext(value);
		}
		else
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Unknown Error."));
		}
	}

	private void Success()
	{
		_gameManager.OnCashoutSuccess.OnNext(value: true);
	}
}
