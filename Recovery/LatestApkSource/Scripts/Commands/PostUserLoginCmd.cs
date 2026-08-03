using System;
using System.Threading.Tasks;
using Infrastructure;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Commands;

public class PostUserLoginCmd : ICommand
{
	private readonly GameManager _gameManager;

	private readonly string _mobileNo;

	private readonly string _password;

	private readonly UserGateway _userGateway;

	public PostUserLoginCmd(GameManager gameManager, string mobileNo, string password, UserGateway userGateway)
	{
		_gameManager = gameManager;
		_mobileNo = mobileNo;
		_password = password;
		_userGateway = userGateway;
	}

	public void Execute()
	{
		_gameManager.loaderManager.OnLoading.OnNext(value: true);
		_userGateway.PostLogin(_gameManager, _mobileNo, _password).DoOnError(HandleLoginError).DoOnError(Debug.LogError)
			.Do(Cache)
			.Delay(TimeSpan.FromMilliseconds(3000.0))
			.Do(delegate
			{
				_gameManager.loaderManager.loadingQueue--;
				if (_gameManager.loaderManager.loadingQueue <= 0)
				{
					_gameManager.loaderManager.OnLoading.OnNext(value: false);
				}
				if (UIManager.Instance.CanSaveCredentials())
				{
					PlayerPrefs.SetString("mobile_no", _mobileNo);
					PlayerPrefs.SetString("password", _password);
				}
			})
			.Subscribe();
	}

	private void HandleLoginError(Exception error)
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
			string value = JSON.Parse(error.Message)["message"].Value + ". Please contact support.";
			_gameManager.errorManager.OnAlertError.OnNext(value);
		}
		else
		{
			_gameManager.errorManager.OnError.OnNext(new Exception("Unknown Error."));
		}
	}

	private async void Cache(string userStr)
	{
		await CacheUser(userStr);
	}

	private async Task CacheUser(string userStr)
	{
		JSONNode jSONNode = await Task.Run(() => JSON.Parse(userStr));
		_gameManager.userId = jSONNode["user_id"];
		_gameManager.userAccessToken = jSONNode["accesstoken"];
		_gameManager.UserData = jSONNode;
		PlayerPrefs.SetString("refferal_link", jSONNode["referral_link"]);
		PlayerPrefs.SetString("mobile_number", jSONNode["mobile_number"]);
		PlayerPrefs.SetString("user_id", jSONNode["user_id"]);
		_gameManager.OnLoginSuccess.OnNext(value: true);
	}
}
