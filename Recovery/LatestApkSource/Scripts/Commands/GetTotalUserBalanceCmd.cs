using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class GetTotalUserBalanceCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly TableManager _tableManager;

	private readonly GameGateway _gameGateway;

	public GetTotalUserBalanceCmd(GameManager gameManager, TableManager tableManager, GameGateway gameGateway)
	{
		_gameManager = gameManager;
		_tableManager = tableManager;
		_gameGateway = gameGateway;
	}

	public void Execute()
	{
		_gameGateway.GetTotalUserBalance(_gameManager, _tableManager).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
	}

	private async void Cache(string userStr)
	{
		await CacheUser(userStr);
	}

	private async Task CacheUser(string userStr)
	{
		int asInt = (await Task.Run(() => JSON.Parse(userStr)))["total_available"].AsInt;
		_tableManager.cashManager.currentTub.Value = asInt;
	}
}
