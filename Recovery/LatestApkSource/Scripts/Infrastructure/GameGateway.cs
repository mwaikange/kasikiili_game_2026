using System;
using System.Collections;
using System.Net;
using SimpleJSON;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure;

public class GameGateway : ApiBase
{
	public GameGateway(SecurityGateway security)
		: base(security)
	{
	}

	public IObservable<string> GetTotalUserBalance(GameManager gameManager, TableManager tableManager)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => GetAllUserBalance(observer, gameManager, tableManager));
	}

	private IEnumerator GetAllUserBalance(IObserver<string> observer, GameManager gameManager, TableManager tableManager)
	{
		string uri = gameManager.urlData + "/getallusersbalance";
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
}
