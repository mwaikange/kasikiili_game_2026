using UnityEngine;
using ViewModel;

namespace Components;

public class ForgotPasswordInput : MonoBehaviour
{
	public GameManager gameManager;

	public void OnClick()
	{
		gameManager.errorManager.OnAlertError.OnNext("This feature is not yet available. If you want to reset your password, please contact support.");
	}
}
