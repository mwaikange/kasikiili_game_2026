using System;
using Commands;
using TMPro;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutPlayerInput : MonoBehaviour
{
	public GameManager gameManager;

	public TableManager tableManager;

	public TMP_InputField inputField;

	public BackendCmdFactory backendCmdFactory;

	public void OnClick()
	{
		if (inputField.text == "")
		{
			gameManager.errorManager.OnAlertError.OnNext("Amount is empty. Please enter amount.");
			return;
		}
		int num = Convert.ToInt32(inputField.text);
		if (num < 100 || num > tableManager.cashManager.currentCredit.Value)
		{
			gameManager.errorManager.OnAlertError.OnNext("Amount entered either exceeds or is below the specified limit.");
			return;
		}
		backendCmdFactory.PostUserCashout(gameManager, num).Execute();
		inputField.text = "";
	}
}
