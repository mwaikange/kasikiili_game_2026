using System;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class ValidOtpNumberCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly MenuManager _menuManager;

	private readonly string _mobileNo;

	private readonly string _otpNumber;

	private readonly UserGateway _userGateway;

	public ValidOtpNumberCmd(GameManager gameManager, MenuManager menuManager, string mobileNo, string otpNumber, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_menuManager = menuManager;
		_mobileNo = mobileNo;
		_otpNumber = otpNumber;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_userGateway.PostValidOtp(_gameManager, _mobileNo, _otpNumber).DoOnError(HandleLoginError).DoOnError(Debug.LogError)
			.Do(delegate
			{
				SignUpFlow();
			})
			.Subscribe();
	}

	private void SignUpFlow()
	{
		_menuManager.OnOtpValid.OnNext(value: true);
	}

	private void HandleLoginError(Exception error)
	{
		if (error.Message.Contains("html") || error.Message.Contains("HTTP"))
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Not found. Please check your network/data connection."));
		}
		else if (error.Message.Length > 3)
		{
			string value = JSON.Parse(error.Message)["message"].Value + ". Please enter valid OTP. Check your SMS for OTP.";
			_gameManager.errorManager.OnAlertError.OnNext(value);
		}
		else
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Unknown Error."));
		}
	}
}
