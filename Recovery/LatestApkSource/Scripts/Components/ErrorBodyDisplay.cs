using TMPro;
using UnityEngine;
using ViewModel;

namespace Components;

public class ErrorBodyDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public TextMeshProUGUI errorLabel;

	private void Start()
	{
		string text = gameManager.errorManager.lastException.Value.Message;
		if (!string.IsNullOrEmpty(text))
		{
			text = char.ToUpper(text[0]) + text.Substring(1);
		}
		errorLabel.text = ((text.Length > 0) ? text : "Unknown errorManager");
	}
}
