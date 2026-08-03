using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class GetUserCashoutHistoryListCmd
{
	private readonly GameManager _gameManager;

	private readonly PlayerGateway _playerGateway;

	public GetUserCashoutHistoryListCmd(GameManager gameManager, PlayerGateway playerGateway)
	{
		_gameManager = gameManager;
		_playerGateway = playerGateway;
	}

	public void Execute()
	{
		_playerGateway.GetUserCashHistory(_gameManager).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
	}

	private async void Cache(string distrStr)
	{
		await CacheUser(distrStr);
	}

	private async Task CacheUser(string distrStr)
	{
		JSONNode value = await Task.Run(() => JSON.Parse(distrStr));
		_gameManager.OnCashoutHistoryList.OnNext(value);
	}
}
