using System;
using System.Collections;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class StartRoundCmd : ICommand
{
	private readonly RouletteManager _rouletteManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	public StartRoundCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		_rouletteManager = rouletteManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
	}

	public void Execute()
	{
		if (_rouletteManager.tableActive.Value && _rouletteManager.gameActive.Value)
		{
			if (_tableManager.buttonsSelected.Count <= 0)
			{
				Debug.Log("There is no bet in table");
				return;
			}
			Debug.Log("Starting your round!");
			_rouletteManager.tableActive.Value = false;
			Observable.FromCoroutine<Unit>(StartGame).Subscribe();
		}
	}

	private IEnumerator StartGame(IObserver<Unit> observer)
	{
		int winNumber = GetRandomNumber();
		yield return new WaitForSeconds(_roundManager.wheelDelay);
		_roundManager.winNumber.Value = winNumber;
		_roundManager.OnRound.OnNext(winNumber);
		yield return new WaitForSeconds(_roundManager.wheelDuration);
		_roundManager.OnPayment.OnNext(winNumber);
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}

	private int GetRandomNumber()
	{
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[4];
		rNGCryptoServiceProvider.GetBytes(array);
		return Math.Abs(BitConverter.ToInt32(array, 0) % 13);
	}
}
