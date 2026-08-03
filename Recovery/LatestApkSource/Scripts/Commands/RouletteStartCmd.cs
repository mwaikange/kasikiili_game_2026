using ViewModel;

namespace Commands;

public class RouletteStartCmd : ICommand
{
	private readonly RouletteManager _rouletteManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	public RouletteStartCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		_rouletteManager = rouletteManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
	}

	public void Execute()
	{
		Reset();
	}

	private void Reset()
	{
		_rouletteManager.tableActive.Value = true;
		_rouletteManager.gameActive.Value = false;
		_tableManager.cashManager.ResetMoney();
		_tableManager.buttonsSelected.Clear();
		_tableManager.buttonsLast.Clear();
		_roundManager.probabilityTub.Value = 0f;
		_roundManager.probabilityNumber.Value = 0;
		_roundManager.probabilityPla.Value = 0f;
		_roundManager.probabilityAfa.Value = 0f;
		_roundManager.probabilityLetter.Value = "-";
		_roundManager.winNumber.Value = 0;
		_roundManager.OnReset.OnNext(value: true);
	}
}
