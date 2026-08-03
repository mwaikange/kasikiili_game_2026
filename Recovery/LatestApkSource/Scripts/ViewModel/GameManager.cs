using System.Linq;
using SimpleJSON;
using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "GameManager", menuName = "Manager/Game Manager", order = 0)]
public class GameManager : ScriptableObject
{
	[Header("API")]
	public string urlTest = "http://15.207.43.56:3008";

	public string urlData = "http://13.127.146.55/kaslkili";

	public JSONNode UserData;

	public int userId;

	public string userAccessToken;

	[Header("Meta")]
	public GameScene[] scenes;

	public LoaderManager loader;

	public LoaderManager loaderManager;

	public ErrorManager errorManager;

	public readonly ISubject<bool> OnApplicationStart = new Subject<bool>();

	public readonly ISubject<bool> OnConnectionSuccess = new Subject<bool>();

	public readonly ISubject<bool> OnLoginSuccess = new Subject<bool>();

	public readonly ISubject<bool> OnLogoutSuccess = new Subject<bool>();

	public readonly ISubject<bool> OnCashoutSuccess = new Subject<bool>();

	public readonly ISubject<JSONNode> OnDistributorsList = new Subject<JSONNode>();

	public readonly ISubject<JSONNode> OnCashoutHistoryList = new Subject<JSONNode>();

	public readonly ISubject<int> OnCashoutAwaiting = new Subject<int>();

	public void Reset()
	{
		UserData = null;
		userId = -1;
		userAccessToken = "";
	}

	public GameScene GetGameScene(GameState state)
	{
		return scenes.FirstOrDefault((GameScene scene) => scene.state == state);
	}

	public string GetToken()
	{
		return userAccessToken;
	}

	public void PostToken(string token)
	{
		userAccessToken = token;
		Debug.Log("TokenReceived: " + userAccessToken);
	}
}
