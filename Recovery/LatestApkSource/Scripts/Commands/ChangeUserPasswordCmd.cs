using System;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class ChangeUserPasswordCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly MenuManager _menuManager;

	private readonly string _mobileNo;

	private readonly string _newPassword;

	private readonly UserGateway _userGateway;

	public ChangeUserPasswordCmd(GameManager gameManager, MenuManager menuManager, string mobileNo, string newPassword, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_menuManager = menuManager;
		_mobileNo = mobileNo;
		_newPassword = newPassword;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_gameManager.loaderManager.OnLoading.OnNext(value: true);
		_userGateway.ChangeUserPassword(_gameManager, _mobileNo, _newPassword).DoOnError(HandleLoginError).DoOnError(Debug.LogError)
			.Do(delegate
			{
				ForgetPasswordFlow();
			})
			.Delay(TimeSpan.FromMilliseconds(3000.0))
			.Do(delegate
			{
				_gameManager.loaderManager.loadingQueue--;
				if (_gameManager.loaderManager.loadingQueue <= 0)
				{
					_gameManager.loaderManager.OnLoading.OnNext(value: false);
				}
			})
			.Subscribe();
	}

	private void ForgetPasswordFlow()
	{
		_menuManager.OnSuccessPopUp.OnNext(new PopUp
		{
			Title = "Successful",
			Subtitle = "Hello, " + _mobileNo + "!",
			Message = "You have successfully change password. Please login to continue."
		});
		_menuManager.OnFlowChange.OnNext(MenuType.Login);
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
