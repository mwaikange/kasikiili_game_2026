using ViewModel;

namespace Commands;

public class GameStartCmd : ICommand
{
	private GameManager _gameManager;

	private LoaderManager _loaderManager;

	public GameStartCmd(GameManager gameManager)
	{
		_gameManager = gameManager;
		_loaderManager = gameManager.loader;
	}

	public void Execute()
	{
		_gameManager.Reset();
	}
}
