using System;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class PostUserSignUpCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly MenuManager _menuManager;

	private readonly string _mobileNo;

	private readonly string _password;

	private readonly string _region;

	private readonly UserGateway _userGateway;

	public PostUserSignUpCmd(GameManager gameManager, MenuManager menuManager, string mobileNo, string password, string region, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_menuManager = menuManager;
		_mobileNo = mobileNo;
		_password = password;
		_region = region;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_gameManager.loaderManager.OnLoading.OnNext(value: true);
		_userGateway.PostSignUp(_gameManager, _mobileNo, _password, _region).DoOnError(HandleSignUpError).DoOnError(Debug.LogError)
			.Do(VerifyOtp)
			.Delay(TimeSpan.FromMilliseconds(1500.0))
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

	private void VerifyOtp(string userStr)
	{
		_menuManager.OnSuccessPopUp.OnNext(new PopUp
		{
			Title = "Successful",
			Subtitle = "Hello, " + _mobileNo + "!",
			Message = "You have successfully signed up. Please login to continue."
		});
		_menuManager.OnFlowChange.OnNext(MenuType.Login);
	}

	private void HandleSignUpError(Exception error)
	{
		_gameManager.loaderManager.loadingQueue--;
		if (_gameManager.loaderManager.loadingQueue <= 0)
		{
			_gameManager.loaderManager.OnLoading.OnNext(value: false);
		}
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
