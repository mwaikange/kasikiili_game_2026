using System;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class SendOtpNumberCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly MenuManager _menuManager;

	private readonly string _mobileNo;

	private readonly UserGateway _userGateway;

	public SendOtpNumberCmd(GameManager gameManager, MenuManager menuManager, string mobileNo, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_menuManager = menuManager;
		_mobileNo = mobileNo;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_userGateway.PostOtpNumber(_gameManager, _mobileNo).DoOnError(HandleLoginError).DoOnError(Debug.LogError)
			.Do(delegate
			{
				SignUpFlow();
			})
			.Subscribe();
	}

	private void SignUpFlow()
	{
		_menuManager.OnOtpSent.OnNext(value: true);
	}

	private void HandleLoginError(Exception error)
	{
		if (error.Message.Contains("html") || error.Message.Contains("HTTP"))
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Not found. Please check your network/data connection."));
		}
		else if (error.Message.Length > 3)
		{
			string value = JSON.Parse(error.Message)["message"].Value;
			_gameManager.errorManager.OnAlertError.OnNext(value);
		}
		else
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Unknown Error."));
		}
	}
}
