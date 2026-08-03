using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutHistoryListInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	private void Awake()
	{
		gameManager.OnCashoutSuccess.Subscribe(delegate
		{
			GetData();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		GetData();
	}

	private void GetData()
	{
		backendCmdFactory.GetAwaitingCashout(gameManager).Execute();
		backendCmdFactory.GetCashoutHistoryList(gameManager).Execute();
	}
}
