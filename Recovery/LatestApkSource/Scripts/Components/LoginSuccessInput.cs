using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class LoginSuccessInput : MonoBehaviour
{
	public GameManager gameManager;

	public RouletteManager rouletteManager;

	public AudioManager audioManager;

	public BackendCmdFactory backendCmdFactory;

	public GameCmdFactory gameCmdFactory;

	[SerializeField]
	private ErrorManager errorManager;

	private void Awake()
	{
		gameManager.OnLoginSuccess.Subscribe(OnLoginSuccess).AddTo(this);
	}

	private void OnLoginSuccess(bool isSuccess)
	{
		if (!isSuccess)
		{
			return;
		}
		Singleton.Instance.apiManager.CallFcmAPI();
		Singleton.Instance.apiManager.CallProbabilityAPI(delegate(bool status, string message)
		{
			if (status)
			{
				rouletteManager.gameLimit = Singleton.Instance.dataManager.probabilityData.data.autoJackpot;
				gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Game).Execute();
			}
			else
			{
				errorManager.Show(message);
			}
		});
	}
}
