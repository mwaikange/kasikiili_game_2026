using System;
using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class GetUserAwaitingCashout
{
	private readonly GameManager _gameManager;

	private readonly PlayerGateway _playerGateway;

	public GetUserAwaitingCashout(GameManager gameManager, PlayerGateway playerGateway)
	{
		_gameManager = gameManager;
		_playerGateway = playerGateway;
	}

	public void Execute()
	{
		_playerGateway.GetUserAwaitingCashout(_gameManager).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
	}

	private async void Cache(string distrStr)
	{
		await CacheUser(distrStr);
	}

	private async Task CacheUser(string distrStr)
	{
		JSONNode jSONNode = Convert.ToInt32((await Task.Run(() => JSON.Parse(distrStr)))["awaitingcashout"].Value);
		_gameManager.OnCashoutAwaiting.OnNext(jSONNode);
	}
}
