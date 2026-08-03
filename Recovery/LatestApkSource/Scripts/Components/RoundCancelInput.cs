using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class RoundCancelInput : MonoBehaviour
{
	public RoundCmdFactory gameCmdFactory;

	public RouletteManager rouletteManager;

	public RoundManager roundManager;

	public TableManager tableManager;

	private float _lastClickTime;

	public void Click()
	{
		if (!(Time.time - _lastClickTime <= 1f))
		{
			_lastClickTime = Time.time;
			gameCmdFactory.CancelRound(rouletteManager, roundManager, tableManager).Execute();
		}
	}
}
