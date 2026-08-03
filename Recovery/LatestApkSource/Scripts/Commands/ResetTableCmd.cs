using System;
using System.Collections;
using UniRx;
using ViewModel;

namespace Commands;

public class ResetTableCmd : ICommand
{
	private readonly RouletteManager _rouletteManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	public ResetTableCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		_rouletteManager = rouletteManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
	}

	public void Execute()
	{
		ResetRound();
	}

	private void ResetRound()
	{
		Observable.FromCoroutine<Unit>(ResetSequence).Subscribe();
	}

	private IEnumerator ResetSequence(IObserver<Unit> observer)
	{
		_tableManager.cashManager.currentBet.Value = 0;
		_tableManager.cashManager.currentTub.Value = 0;
		TableButton[] array = _tableManager.buttonsSelected.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].isSelected.Value = false;
		}
		_tableManager.buttonsLast = _tableManager.buttonsSelected;
		_tableManager.buttonsSelected.Clear();
		_rouletteManager.tableActive.Value = true;
		_roundManager.OnResetFinished.OnNext(value: true);
		yield return null;
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}
}
