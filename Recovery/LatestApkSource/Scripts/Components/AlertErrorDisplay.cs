using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class AlertErrorDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public GameObject errorHandler;

	public Text messageLabel;

	private void Awake()
	{
		gameManager.errorManager.OnAlertError.Subscribe(OnAlertError).AddTo(this);
	}

	private void OnAlertError(string message)
	{
		errorHandler.SetActive(value: true);
		messageLabel.text = message;
	}
}
