using System;
using System.Collections;
using System.Net;
using SimpleJSON;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure;

public class PlayerGateway : ApiBase
{
	public PlayerGateway(SecurityGateway security)
		: base(security)
	{
	}

	public IObservable<string> GetPlayerBalance(GameManager gameManager, int playerId)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => GetBalance(observer, gameManager, playerId));
	}

	private IEnumerator GetBalance(IObserver<string> observer, GameManager gameManager, int playerId)
	{
		string uri = $"{gameManager.urlData}/getuserdata/{playerId}";
		string value = "Bearer " + gameManager.userAccessToken;
		using UnityWebRequest www = UnityWebRequest.Get(uri);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("There an error in GetBalance");
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.error));
			yield break;
		}
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(www.downloadHandler.text)["data"];
		IObservable<string> source = SecurityGateway.PostDecrypt(gameManager.urlData, jSONNode);
		ObservableYieldInstruction<string> decryptedResult = source.ToYieldInstruction();
		yield return decryptedResult;
		Debug.Log("GetBalance");
		Debug.Log(decryptedResult.Result);
		observer.OnNext(decryptedResult.Result);
		observer.OnCompleted();
	}

	public IObservable<string> PostUserCashout(GameManager gameManager, int amount)
	{
		JSONObject data = new JSONObject();
		data.Add("amount", amount);
		return Observable.FromCoroutine((IObserver<string> observer) => Cashout(observer, gameManager, data));
	}

	private IEnumerator Cashout(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/sendreq";
		string value = "Bearer " + gameManager.userAccessToken;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.downloadHandler.text));
		}
		else
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnNext("Successfully");
			observer.OnCompleted();
		}
	}

	public IObservable<string> GetUserCashHistory(GameManager gameManager)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => CashoutHistoryList(observer, gameManager));
	}

	private IEnumerator CashoutHistoryList(IObserver<string> observer, GameManager gameManager)
	{
		string uri = $"{gameManager.urlData}/getusertranscation/{gameManager.userId}";
		string value = "Bearer " + gameManager.userAccessToken;
		using UnityWebRequest www = UnityWebRequest.Get(uri);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("There an error in GetBalance");
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.error));
			yield break;
		}
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(www.downloadHandler.text)["data"];
		IObservable<string> source = SecurityGateway.PostDecrypt(gameManager.urlData, jSONNode);
		ObservableYieldInstruction<string> decryptedResult = source.ToYieldInstruction();
		yield return decryptedResult;
		observer.OnNext(decryptedResult.Result);
		observer.OnCompleted();
	}

	public IObservable<string> GetUserAwaitingCashout(GameManager gameManager)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => CashoutAwaiting(observer, gameManager));
	}

	private IEnumerator CashoutAwaiting(IObserver<string> observer, GameManager gameManager)
	{
		string uri = gameManager.urlData + "/getuserawaitingcashout";
		string value = "Bearer " + gameManager.userAccessToken;
		using UnityWebRequest www = UnityWebRequest.Get(uri);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.error));
			yield break;
		}
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(www.downloadHandler.text)["data"];
		IObservable<string> source = SecurityGateway.PostDecrypt(gameManager.urlData, jSONNode);
		ObservableYieldInstruction<string> decryptedResult = source.ToYieldInstruction();
		yield return decryptedResult;
		Debug.Log("CashoutAwaiting");
		Debug.Log(decryptedResult.Result);
		observer.OnNext(decryptedResult.Result);
		observer.OnCompleted();
	}

	public IObservable<string> PostBalance(GameManager gameManager, int amount)
	{
		JSONObject data = new JSONObject();
		data.Add("amount", amount);
		return Observable.FromCoroutine((IObserver<string> observer) => UpdateUserBalance(observer, gameManager, data));
	}

	private IEnumerator UpdateUserBalance(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/updateuserbalance";
		string value = "Bearer " + gameManager.userAccessToken;
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.downloadHandler.text));
			yield break;
		}
		Debug.Log("updated");
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		observer.OnNext("Operation /updateuserbalance Success");
		observer.OnCompleted();
	}
}
