using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "CashManager", menuName = "Scriptable/Manager/CashManager")]
public class CashManager : ScriptableObject
{
	public IntReactiveProperty currentTub = new IntReactiveProperty();

	public IntReactiveProperty currentCredit = new IntReactiveProperty();

	public IntReactiveProperty currentBet = new IntReactiveProperty();

	private void AddCash(int cashWinner)
	{
		currentCredit.Value += cashWinner;
	}

	private void SubstrateCash(int cashLost)
	{
		if (cashLost < 0)
		{
			cashLost *= -1;
		}
		currentCredit.Value -= cashLost;
		if (currentCredit.Value < 0)
		{
			currentCredit.Value = 0;
		}
	}

	private void AddBet(int betSum)
	{
		currentBet.Value += betSum;
	}

	private void SubstrateBet(int betRest)
	{
		currentBet.Value -= betRest;
	}

	public void ResetMoney()
	{
		currentBet.Value = 0;
		currentCredit.Value = 0;
		currentTub.Value = 0;
	}

	public bool AddCheckBet(int valueBet)
	{
		if (valueBet <= currentCredit.Value && valueBet != 0)
		{
			SubstrateCash(valueBet);
			AddBet(valueBet);
			return true;
		}
		return false;
	}

	public void RemoveCheckBet(int valueBet)
	{
		SubstrateBet(valueBet);
		AddCash(valueBet);
	}
}
