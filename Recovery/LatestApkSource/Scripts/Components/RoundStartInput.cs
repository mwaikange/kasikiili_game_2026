using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundStartInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public RoundCmdFactory roundCmdFactory;

	public RouletteManager rouletteManager;

	public RoundManager roundManager;

	public TableManager tableManager;

	private bool _startWasPressed;

	private float _lastClickTime;

	private void Start()
	{
		gameManager.OnConnectionSuccess.Subscribe(StartInput).AddTo(this);
	}

	public void Click()
	{
		if (Time.time - _lastClickTime <= 1f)
		{
			return;
		}
		_lastClickTime = Time.time;
		if (rouletteManager.tableActive.Value && rouletteManager.gameActive.Value)
		{
			if (tableManager.buttonsSelected.Count <= 0)
			{
				Debug.Log("There is no bet in table");
			}
			else if (!_startWasPressed)
			{
				_startWasPressed = true;
				backendCmdFactory.TestGameConnection(gameManager).Execute();
			}
		}
	}

	private void StartInput(bool isSuccess)
	{
		_startWasPressed = false;
		if (!isSuccess)
		{
			gameManager.errorManager.OnAlertError.OnNext("Connection failed. Check your internet connection and try again.");
			return;
		}
		backendCmdFactory.GetTotalUserBalance(gameManager, tableManager).Execute();
		roundCmdFactory.StartRound(rouletteManager, roundManager, tableManager).Execute();
	}
}
