using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Components;

public class CashoutHistoryRowDisplay : MonoBehaviour
{
	public Text amountLabel;

	public Text statusLabel;

	public Text transactionDateLabel;

	public Text transactionIdLabel;

	public void LoadRow(string amount, string status, string transactionDate, string transactionId)
	{
		amountLabel.text = amount;
		statusLabel.text = status;
		transactionDateLabel.text = PerformDate(transactionDate);
		transactionIdLabel.text = transactionId;
	}

	private string PerformDate(string dateStr)
	{
		return DateTime.Parse(dateStr).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
	}
}
