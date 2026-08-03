using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class GetUserBalanceCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly TableManager _tableManager;

	private readonly PlayerGateway _playerGateway;

	public GetUserBalanceCmd(GameManager gameManager, TableManager tableManager, PlayerGateway playerGateway)
	{
		_gameManager = gameManager;
		_tableManager = tableManager;
		_playerGateway = playerGateway;
	}

	public void Execute()
	{
		_playerGateway.GetPlayerBalance(_gameManager, _gameManager.userId).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
	}

	private async void Cache(string userStr)
	{
		await CacheUser(userStr);
	}

	private async Task CacheUser(string userStr)
	{
		int asInt = (await Task.Run(() => JSON.Parse(userStr)))[0]["balance"].AsInt;
		_tableManager.cashManager.currentCredit.Value = asInt;
	}
}
