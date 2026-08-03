using System;
using UnityEngine;
using ViewModel;

namespace Components;

public class DebugWinInput : MonoBehaviour
{
	public RoundManager roundManager;

	private void Awake()
	{
		roundManager.winNumber.Value = Convert.ToInt32(0);
	}

	public void ChangeNumber(int number)
	{
		Debug.Log(Convert.ToInt32(number));
		roundManager.winNumber.Value = Convert.ToInt32(number);
	}
}
