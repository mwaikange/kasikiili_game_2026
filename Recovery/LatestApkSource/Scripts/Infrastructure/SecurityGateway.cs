using System;
using System.Collections;
using System.Net;
using SimpleJSON;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Infrastructure;

public class SecurityGateway
{
	public IObservable<string> PostEncrypt(string baseUrl, JSONNode jsonNode)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => EncryptData(observer, baseUrl, jsonNode));
	}

	private IEnumerator EncryptData(IObserver<string> observer, string baseUrl, JSONNode jsonNode)
	{
		string uri = baseUrl + "/encryption";
		WWWForm wWWForm = new WWWForm();
		JSONNode.KeyEnumerator enumerator = jsonNode.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string current = enumerator.Current;
			string fieldName = current ?? throw new ArgumentNullException("key");
			string value = jsonNode[current].Value;
			wWWForm.AddField(fieldName, value);
		}
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
		yield return www.SendWebRequest();
		Debug.Log(">>>>>>");
		Debug.Log(www.downloadHandler.text);
		Debug.Log(">>>>>>");
		Debug.Log(www.result);
		Debug.Log(">>>>>>");
		Debug.Log(www.error);
		Debug.Log(">>>>>>");
		if (www.result != UnityWebRequest.Result.Success)
		{
			observer.OnError(new WebException(www.error));
			yield break;
		}
		JSONNode jSONNode = JSON.Parse(www.downloadHandler.text)["info"];
		observer.OnNext(jSONNode);
		observer.OnCompleted();
	}

	public IObservable<string> PostDecrypt(string baseUrl, string data)
	{
		return Observable.FromCoroutine((IObserver<string> observer) => DecryptData(observer, baseUrl, data));
	}

	private IEnumerator DecryptData(IObserver<string> observer, string baseUrl, string data)
	{
		string uri = baseUrl + "/decryption";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", data);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			observer.OnError(new WebException(www.error));
			yield break;
		}
		string text = www.downloadHandler.text;
		observer.OnNext(text);
		observer.OnCompleted();
	}
}
