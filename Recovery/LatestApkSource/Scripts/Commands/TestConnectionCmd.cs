using System;
using Infrastructure;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class TestConnectionCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly ApiGateway _apiGateway;

	public TestConnectionCmd(GameManager gameManager, ApiGateway apiGateway)
	{
		_gameManager = gameManager;
		_apiGateway = apiGateway;
	}

	public void Execute()
	{
		_apiGateway.GetConnection(_gameManager).DoOnError(delegate(Exception error)
		{
			_gameManager.errorManager.OnError.OnNext(error);
		}).DoOnError(delegate
		{
			_gameManager.OnConnectionSuccess.OnNext(value: false);
		})
			.DoOnError(Debug.LogError)
			.Do(ConnectionSuccess)
			.Subscribe();
	}

	private void ConnectionSuccess(Unit unit)
	{
		_gameManager.OnApplicationStart.OnNext(value: true);
		_gameManager.OnConnectionSuccess.OnNext(value: true);
	}
}
