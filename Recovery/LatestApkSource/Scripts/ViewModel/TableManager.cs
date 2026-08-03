using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "New Table", menuName = "Scriptable/Manager/Table Manager")]
public class TableManager : ScriptableObject
{
	public CashManager cashManager;

	[Header("Runtime")]
	public List<TableButton> buttonsSelected;

	public List<TableButton> buttonsLast = new List<TableButton>();

	public bool CheckIfPlayerWin(int numberWin)
	{
		return buttonsSelected.ToArray().Any((TableButton button) => button.numberParent == numberWin);
	}

	public IEnumerable<TableButton> GetWinButtons(int numberWin)
	{
		return (from button in buttonsSelected.ToArray()
			where button.numberParent == numberWin
			select button).ToArray();
	}
}
