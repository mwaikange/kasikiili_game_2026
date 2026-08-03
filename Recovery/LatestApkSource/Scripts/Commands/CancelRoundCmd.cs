using ViewModel;

namespace Commands;

public class CancelRoundCmd : ICommand
{
	private readonly RouletteManager _rouletteManager;

	private readonly RoundManager _roundManager;

	private readonly TableManager _tableManager;

	public CancelRoundCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		_rouletteManager = rouletteManager;
		_roundManager = roundManager;
		_tableManager = tableManager;
	}

	public void Execute()
	{
		if (_rouletteManager.tableActive.Value && _rouletteManager.gameActive.Value)
		{
			TableButton[] array = _tableManager.buttonsSelected.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Refresh.OnNext(value: false);
			}
		}
	}
}
