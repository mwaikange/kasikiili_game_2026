using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class GetDistributorsListCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly DistributorsGateway _distributorsGateway;

	public GetDistributorsListCmd(GameManager gameManager, DistributorsGateway distributorsGateway)
	{
		_gameManager = gameManager;
		_distributorsGateway = distributorsGateway;
	}

	public void Execute()
	{
		_distributorsGateway.GetDistributorsList(_gameManager).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
	}

	private async void Cache(string distrStr)
	{
		await CacheUser(distrStr);
	}

	private async Task CacheUser(string distrStr)
	{
		JSONNode value = await Task.Run(() => JSON.Parse(distrStr));
		_gameManager.OnDistributorsList.OnNext(value);
	}
}
