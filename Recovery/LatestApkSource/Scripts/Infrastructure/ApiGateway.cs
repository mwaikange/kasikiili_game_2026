using System;
using System.Collections;
using System.Net;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure;

public class ApiGateway : ApiBase
{
	public ApiGateway(SecurityGateway security)
		: base(security)
	{
	}

	public IObservable<Unit> GetConnection(GameManager gameManager)
	{
		return Observable.FromCoroutine((IObserver<Unit> observer) => TestConnection(observer, gameManager));
	}

	private IEnumerator TestConnection(IObserver<Unit> observer, GameManager gameManager)
	{
		string urlData = gameManager.urlData;
		using UnityWebRequest www = UnityWebRequest.Get(urlData);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			observer.OnError(new WebException(www.error));
			yield break;
		}
		Debug.Log(www.downloadHandler.text);
		observer.OnNext(Unit.Default);
		observer.OnCompleted();
	}
}
