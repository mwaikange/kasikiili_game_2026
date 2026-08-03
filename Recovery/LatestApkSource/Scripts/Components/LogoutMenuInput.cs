using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class LogoutMenuInput : MonoBehaviour
{
	public GameCmdFactory gameCmdFactory;

	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public AudioManager audioManager;

	private void Start()
	{
		gameManager.OnLogoutSuccess.Subscribe(OnLogoutSuccess).AddTo(this);
	}

	private void OnLogoutSuccess(bool success)
	{
		if (success)
		{
			gameCmdFactory.ChangeScene(gameManager, audioManager, GameState.Menu).Execute();
		}
	}

	public void OnClick()
	{
		backendCmdFactory.PostUserLogout(gameManager).Execute();
	}
}
