using System;
using System.Collections;
using System.Linq;
using Infrastructure;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class PaymentRoundCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	private readonly PlayerGateway _playerGateway;

	private static readonly int[] ProbabilityNumbers = new int[6] { 10, 50, 1000, 2000, 25, 100 };

	private readonly Loading _loading;

	private readonly ErrorManager _errorManager;

	public PaymentRoundCmd(GameManager gameManager, RoundManager roundManager, TableManager tableManager, PlayerGateway playerGateway, Loading loading, ErrorManager errorManager)
	{
		_gameManager = gameManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
		_playerGateway = playerGateway;
		_loading = loading;
		_errorManager = errorManager;
	}

	public void Execute()
	{
		int credits = _tableManager.cashManager.currentCredit.Value;
		int win = 0;
		int lost = 0;
		Debug.Log("Win Number: " + _roundManager.winNumber.Value);
		if (_tableManager.CheckIfPlayerWin(_roundManager.winNumber.Value))
		{
			Debug.Log("Player Win Round");
			Singleton.Instance.apiManager.CallProbabilityAPI(delegate(bool status, string message)
			{
				if (status)
				{
					int probabilityNum = GetProbabilityNum();
					win += WinRound(probabilityNum);
					lost += _tableManager.cashManager.currentBet.Value;
				}
				else
				{
					_errorManager.Show(message);
					lost += LostRound();
				}
				PostBalance(credits, Convert.ToInt32(win), Convert.ToInt32(lost));
			});
		}
		else
		{
			Debug.Log("Player Lost Round");
			lost += LostRound();
			PostBalance(credits, Convert.ToInt32(win), Convert.ToInt32(lost));
		}
	}

	private void PostBalance(int credits, int win, int lost)
	{
		Debug.Log("Won: " + win + " Lost: " + lost);
		int amount = credits + win;
		Debug.Log("Balance: " + amount);
		_playerGateway.PostBalance(_gameManager, amount).DoOnError(Debug.LogError).Do(Debug.Log)
			.Subscribe();
	}

	private int WinRound(int probability)
	{
		int num = _tableManager.GetWinButtons(_roundManager.winNumber.Value).Sum((TableButton button) => button.betValue);
		int gamePayment = num * probability;
		Observable.FromCoroutine((IObserver<Unit> observer) => WinSequence(observer, gamePayment, probability)).Subscribe();
		return gamePayment;
	}

	private int GetProbabilityNum()
	{
		int rwm = Singleton.Instance.dataManager.probabilityData.data.rwm;
		_roundManager.probabilityNumber.Value = rwm;
		_roundManager.probabilityLetter.Value = Singleton.Instance.dataManager.probabilityData.data.label;
		return rwm;
	}

	private int LostRound()
	{
		Observable.FromCoroutine<Unit>(LostSequence).Subscribe();
		return _tableManager.cashManager.currentBet.Value;
	}

	private IEnumerator WinSequence(IObserver<Unit> observer, int payment, int probability)
	{
		yield return new WaitForSeconds(_roundManager.probabilityDelay);
		_roundManager.OnProbability.OnNext(probability);
		yield return new WaitForSeconds(_roundManager.probabilityDuration);
		if (probability == 50 || probability == 100 || probability == 1000 || probability == 2000)
		{
			yield return new WaitForSeconds(_roundManager.probabilityJackpotDelay);
			_roundManager.OnJackpot.OnNext(probability);
			yield return new WaitForSeconds(_roundManager.probabilityJackpotDuration);
		}
		yield return new WaitForSeconds(_roundManager.moneyDelay);
		_roundManager.OnWin.OnNext(payment);
		yield return new WaitForSeconds(_roundManager.moneyDuration);
		_roundManager.OnReset.OnNext(value: true);
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}

	private IEnumerator LostSequence(IObserver<Unit> observer)
	{
		yield return new WaitForSeconds(1f);
		_roundManager.OnReset.OnNext(value: true);
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
		yield return null;
	}
}
