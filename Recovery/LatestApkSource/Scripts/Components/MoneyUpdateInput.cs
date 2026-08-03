using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class MoneyUpdateInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public RouletteManager rouletteManager;

	public TableManager tableManager;

	private void Awake()
	{
	}

	private void OnRouletteRefresh(bool isMoney)
	{
	}
}
