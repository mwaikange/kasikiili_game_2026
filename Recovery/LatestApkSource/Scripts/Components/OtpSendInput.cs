using System;
using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class OtpSendInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public Button sendButton;

	public GameManager gameManager;

	public MenuManager menuManager;

	public TMP_InputField mobileNoField;

	private IDisposable _observer;

	private void OnEnable()
	{
		backendCmdFactory.RegisterOtpNumber(gameManager, menuManager, mobileNoField.text).Execute();
		sendButton.interactable = false;
		_observer = Observable.Interval(TimeSpan.FromSeconds(30.0)).Subscribe(delegate
		{
			if (!sendButton.interactable)
			{
				sendButton.interactable = !sendButton.interactable;
			}
		}).AddTo(this);
	}

	private void OnDisable()
	{
		_observer.Dispose();
	}

	public void OnClick()
	{
		backendCmdFactory.RegisterOtpNumber(gameManager, menuManager, mobileNoField.text).Execute();
	}
}
