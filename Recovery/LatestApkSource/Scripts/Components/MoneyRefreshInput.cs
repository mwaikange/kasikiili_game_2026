using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class MoneyRefreshInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public RouletteManager rouletteManager;

	public TableManager tableManager;

	private void Awake()
	{
	}

	private void OnRouletteStart(bool isMoney)
	{
		backendCmdFactory.GetUserBalance(gameManager, tableManager).Execute();
	}
}
