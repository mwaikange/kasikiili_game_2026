using System;
using System.Collections;
using System.Net;
using SimpleJSON;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

namespace Infrastructure;

public class UserGateway : ApiBase
{
	public UserGateway(SecurityGateway security)
		: base(security)
	{
	}

	public IObservable<string> PostLogin(GameManager gameManager, string mobileNumber, string password)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNumber);
		data.Add("password", password);
		return Observable.FromCoroutine((IObserver<string> observer) => PostUserLogin(observer, gameManager, data));
	}

	private IEnumerator PostUserLogin(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/login";
		Debug.Log(gameManager.urlData + "/login");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.downloadHandler.text));
			yield break;
		}
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(www.downloadHandler.text)["data"];
		IObservable<string> source2 = SecurityGateway.PostDecrypt(gameManager.urlData, jSONNode);
		ObservableYieldInstruction<string> decryptedResult = source2.ToYieldInstruction();
		yield return decryptedResult;
		observer.OnNext(decryptedResult.Result);
		observer.OnCompleted();
	}

	public IObservable<Unit> PostLogout(GameManager gameManager)
	{
		return Observable.FromCoroutine((IObserver<Unit> observer) => PostUserLogout(observer, gameManager));
	}

	private IEnumerator PostUserLogout(IObserver<Unit> observer, GameManager gameManager)
	{
		string uri = gameManager.urlData + "/logout";
		string value = "Bearer " + gameManager.userAccessToken;
		WWWForm formData = new WWWForm();
		using UnityWebRequest www = UnityWebRequest.Post(uri, formData);
		www.SetRequestHeader("Authorization", value);
		yield return www.SendWebRequest();
		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
			observer.OnError(new WebException(www.error));
			yield break;
		}
		Debug.Log("www.downloadHandler.text: " + www.downloadHandler.text);
		observer.OnNext(Unit.Default);
		PlayerPrefs.DeleteKey("mobile_no");
		PlayerPrefs.DeleteKey("password");
		observer.OnCompleted();
	}

	public IObservable<string> PostSignUp(GameManager gameManager, string mobileNo, string password, string region)
	{
		JSONObject data = new JSONObject();
		data.Add("first_name", "user_" + mobileNo);
		data.Add("last_name", "user_" + mobileNo);
		data.Add("fcm_token", "N/A");
		data.Add("user_device_id", "N/A");
		data.Add("mobile_number", mobileNo);
		data.Add("password", password);
		data.Add("region", region.ToUpper());
		return Observable.FromCoroutine((IObserver<string> observer) => PostUserSignUp(observer, gameManager, data));
	}

	private IEnumerator PostUserSignUp(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/register";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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

	public IObservable<string> PostCheckMobileNumber(GameManager gameManager, string mobileNo)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNo);
		return Observable.FromCoroutine((IObserver<string> observer) => CheckMobileNumber(observer, gameManager, data));
	}

	private IEnumerator CheckMobileNumber(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/checkmobilenumber";
		Debug.Log(gameManager.urlData + "/checkmobilenumber");
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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

	public IObservable<string> PostOtpNumber(GameManager gameManager, string mobileNo)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNo);
		return Observable.FromCoroutine((IObserver<string> observer) => SendOtp(observer, gameManager, data));
	}

	private IEnumerator SendOtp(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/sendotp";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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

	public IObservable<string> PostValidOtp(GameManager gameManager, string mobileNo, string otp)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNo);
		data.Add("otp", otp);
		return Observable.FromCoroutine((IObserver<string> observer) => VerifyOtp(observer, gameManager, data));
	}

	private IEnumerator VerifyOtp(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/verifyotp";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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

	public IObservable<string> ChangeUserPassword(GameManager gameManager, string mobileNo, string newPassword)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNo);
		data.Add("new_password", newPassword);
		return Observable.FromCoroutine((IObserver<string> observer) => ChangePassword(observer, gameManager, data));
	}

	private IEnumerator ChangePassword(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/forgetPassword";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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

	public IObservable<string> RegisterOtpNumber(GameManager gameManager, string mobileNo)
	{
		JSONObject data = new JSONObject();
		data.Add("mobile_number", mobileNo);
		return Observable.FromCoroutine((IObserver<string> observer) => RegisterOtp(observer, gameManager, data));
	}

	private IEnumerator RegisterOtp(IObserver<string> observer, GameManager gameManager, JSONNode data)
	{
		IObservable<string> source = SecurityGateway.PostEncrypt(gameManager.urlData, data);
		ObservableYieldInstruction<string> encryptedResult = source.ToYieldInstruction();
		yield return encryptedResult;
		string result = encryptedResult.Result;
		Debug.Log("encryptedInfo: " + result);
		string uri = gameManager.urlData + "/registerotp";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("info", result);
		using UnityWebRequest www = UnityWebRequest.Post(uri, wWWForm);
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
}
