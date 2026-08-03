using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundResetInput : MonoBehaviour
{
	public GameManager gameManager;

	public RoundCmdFactory roundCmdFactory;

	public RouletteManager rouletteManager;

	public RoundManager roundManager;

	public TableManager tableManager;

	private void Awake()
	{
		roundManager.OnReset.Subscribe(OnReset).AddTo(this);
	}

	private void OnReset(bool reset)
	{
		roundCmdFactory.ResetRound(gameManager, rouletteManager, roundManager, tableManager).Execute();
	}
}
