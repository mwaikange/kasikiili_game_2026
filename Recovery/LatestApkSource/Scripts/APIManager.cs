using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;

public class APIManager : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;

	private string leaderboardURL = "/gamification/leaderboard-mobile/";

	private string prizeDistribution = "/gamification/get-prize-distribution/";

	private string probabilityAutoJackpot = "/coefficients/probabilites-mobile";

	private string saveFcmToken = "/portability/update-fcm-token";

	private string firstDayOfMonth;

	private string lastDayOfMonth;

	private const string apiErrorMsg = "Network / Data issue. Please check your networks/ WiFi";

	private const string connectionErrorMsg = "No Internet Connection";

	private const string nullReponseErrorMsg = "Network / Data issue. Please check your networks/ WiFi";

	private void Awake()
	{
		leaderboardURL += DateTime.Now.Month - 1;
		prizeDistribution += DateTime.Now.Month - 1;
		string text = ((DateTime.Now.Month < 10) ? ("0" + DateTime.Now.Month) : (DateTime.Now.Month.ToString() ?? ""));
		int num = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		string text2 = ((num < 10) ? ("0" + num) : (num.ToString() ?? ""));
		firstDayOfMonth = DateTime.Now.Year + "-" + text + "-01";
		lastDayOfMonth = DateTime.Now.Year + "-" + text + "-" + text2;
	}

	internal void CallLeaderboardAPI(Action<bool, string> callback)
	{
		StartCoroutine(GetLeaderboardResponse(callback));
	}

	private IEnumerator GetLeaderboardResponse(Action<bool, string> callback)
	{
		LeaderboardRequest leaderboardRequest = new LeaderboardRequest();
		leaderboardRequest.targetDate = firstDayOfMonth;
		leaderboardRequest.endDate = lastDayOfMonth;
		leaderboardRequest.mobile_number = PlayerPrefs.GetString("mobile_number");
		UnityWebRequest www = new UnityWebRequest(gameManager.urlData + leaderboardURL, "POST");
		byte[] bytes = new UTF8Encoding().GetBytes(JsonUtility.ToJson(leaderboardRequest));
		www.uploadHandler = new UploadHandlerRaw(bytes);
		www.downloadHandler = new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		if (www.result == UnityWebRequest.Result.ProtocolError)
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
			yield break;
		}
		if (www.result == UnityWebRequest.Result.ConnectionError)
		{
			callback(arg1: false, "No Internet Connection");
			yield break;
		}
		string text = www.downloadHandler.text;
		MonoBehaviour.print(gameManager.urlData + leaderboardURL);
		MonoBehaviour.print(text);
		if (text != null)
		{
			Singleton.Instance.dataManager.leaderboard = JsonUtility.FromJson<LeaderboardResponse>(text);
			callback(arg1: true, "");
		}
		else
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
		}
	}

	internal void CallPrizeDistributionAPI(Action<bool, string> callback)
	{
		StartCoroutine(GetPrizeDistributionResponse(callback));
	}

	private IEnumerator GetPrizeDistributionResponse(Action<bool, string> callback)
	{
		UnityWebRequest www = UnityWebRequest.Get(gameManager.urlData + prizeDistribution);
		www.downloadHandler = new DownloadHandlerBuffer();
		yield return www.SendWebRequest();
		if (www.result == UnityWebRequest.Result.ProtocolError)
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
			yield break;
		}
		if (www.result == UnityWebRequest.Result.ConnectionError)
		{
			callback(arg1: false, "No Internet Connection");
			yield break;
		}
		string text = www.downloadHandler.text;
		if (text != null)
		{
			MonoBehaviour.print(text);
			Singleton.Instance.dataManager.prizeDistributionData = JsonUtility.FromJson<PrizeDistributionData>(text);
			callback(arg1: true, "");
		}
		else
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
		}
	}

	internal void CallProbabilityAPI(Action<bool, string> callback)
	{
		StartCoroutine(GetProbabilityResponse(callback));
	}

	private IEnumerator GetProbabilityResponse(Action<bool, string> callback)
	{
		UnityWebRequest www = UnityWebRequest.Get(gameManager.urlData + probabilityAutoJackpot);
		www.downloadHandler = new DownloadHandlerBuffer();
		yield return www.SendWebRequest();
		if (www.result == UnityWebRequest.Result.ProtocolError)
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
			yield break;
		}
		if (www.result == UnityWebRequest.Result.ConnectionError)
		{
			callback(arg1: false, "No Internet Connection");
			yield break;
		}
		string text = www.downloadHandler.text;
		MonoBehaviour.print("+++++++++++++");
		MonoBehaviour.print(text);
		MonoBehaviour.print("+++++++++++++");
		if (text != null)
		{
			Singleton.Instance.dataManager.probabilityData = JsonUtility.FromJson<ProbabilityData>(text);
			callback(arg1: true, "");
		}
		else
		{
			callback(arg1: false, "Network / Data issue. Please check your networks/ WiFi");
		}
	}

	internal void CallFcmAPI()
	{
		if (!PlayerPrefs.HasKey("fcm_token_updated") && PlayerPrefs.GetString("fcm_token") != "")
		{
			StartCoroutine(GetFcmResponse());
		}
		else
		{
			Debug.Log("Fcm token updated status: " + PlayerPrefs.HasKey("fcm_token_updated"));
		}
	}

	private IEnumerator GetFcmResponse()
	{
		FcmData fcmData = new FcmData();
		fcmData.user_id = PlayerPrefs.GetString("user_id");
		fcmData.fcm_token = PlayerPrefs.GetString("fcm_token");
		UnityWebRequest www = new UnityWebRequest(gameManager.urlData + saveFcmToken, "POST");
		byte[] bytes = new UTF8Encoding().GetBytes(JsonUtility.ToJson(fcmData));
		www.uploadHandler = new UploadHandlerRaw(bytes);
		www.downloadHandler = new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();
		if (www.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log("Something Went Wrong in Fcm Token API");
		}
		else if (www.result == UnityWebRequest.Result.ConnectionError)
		{
			Debug.Log("No Internet Connection in Fcm Token API");
		}
		else if (www.downloadHandler.text != null)
		{
			Debug.Log("Fcm successfully updated!");
			PlayerPrefs.SetString("fcm_token_updated", "");
		}
		else
		{
			Debug.Log("Something Went Wrong in Fcm Token API");
		}
	}
}
