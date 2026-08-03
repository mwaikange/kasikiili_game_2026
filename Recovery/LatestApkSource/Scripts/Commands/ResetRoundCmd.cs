using System;
using System.Collections;
using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class ResetRoundCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly RouletteManager _rouletteManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	private readonly PlayerGateway _playerGateway;

	public ResetRoundCmd(GameManager gameManager, RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager, PlayerGateway playerGateway)
	{
		_gameManager = gameManager;
		_rouletteManager = rouletteManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
		_playerGateway = playerGateway;
	}

	public void Execute()
	{
		_rouletteManager.gameActive.Value = false;
		Observable.FromCoroutine<Unit>(ResetRound).Subscribe();
	}

	private IEnumerator ResetRound(IObserver<Unit> observer)
	{
		yield return new WaitForSeconds(3f);
		_playerGateway.GetPlayerBalance(_gameManager, _gameManager.userId).DoOnError(Debug.LogError).Do(Cache)
			.Subscribe();
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}

	private async void Cache(string userStr)
	{
		await CacheUser(userStr);
	}

	private async Task CacheUser(string userStr)
	{
		int asInt = (await Task.Run(() => JSON.Parse(userStr)))[0]["balance"].AsInt;
		_tableManager.cashManager.currentCredit.Value = asInt;
		Observable.FromCoroutine<Unit>(ResetSequence).Subscribe();
	}

	private IEnumerator ResetSequence(IObserver<Unit> observer)
	{
		yield return null;
		_tableManager.cashManager.currentBet.Value = 0;
		_tableManager.cashManager.currentTub.Value = 0;
		TableButton[] array = _tableManager.buttonsSelected.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].isSelected.Value = false;
		}
		_tableManager.buttonsLast = _tableManager.buttonsSelected;
		_tableManager.buttonsSelected.Clear();
		switch (_rouletteManager.lastState)
		{
		case RouletteState.Game:
			_rouletteManager.gameActive.Value = true;
			_rouletteManager.tableActive.Value = true;
			_roundManager.OnResetFinished.OnNext(value: true);
			break;
		case RouletteState.Cashout:
			_rouletteManager.gameActive.Value = false;
			_rouletteManager.tableActive.Value = false;
			break;
		case RouletteState.Pause:
			_rouletteManager.gameActive.Value = false;
			_rouletteManager.tableActive.Value = false;
			break;
		case RouletteState.Unavailable:
			_rouletteManager.gameActive.Value = false;
			_rouletteManager.tableActive.Value = false;
			break;
		}
		Debug.Log($"Game new balance is {_tableManager.cashManager.currentCredit.Value}");
		Debug.Log("Ending/resetting your round!");
	}
}
