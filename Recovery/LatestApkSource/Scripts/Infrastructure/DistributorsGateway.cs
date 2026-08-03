using System;
using System.Collections;
using System.Net;
using SimpleJSON;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure;

public class DistributorsGateway : ApiBase
{
	public DistributorsGateway(SecurityGateway security)
		: base(security)
	{
	}

	public IObservable<string> GetDistributorsList(GameManager gameManager)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => DistributorsList(observer, gameManager));
	}

	private IEnumerator DistributorsList(IObserver<string> observer, GameManager gameManager)
	{
		string uri = gameManager.urlData + "/getdistributorslist";
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
		observer.OnNext(decryptedResult.Result);
		observer.OnCompleted();
	}
}
