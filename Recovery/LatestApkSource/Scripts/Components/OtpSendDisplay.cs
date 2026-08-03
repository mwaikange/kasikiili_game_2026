using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class OtpSendDisplay : MonoBehaviour
{
	public MenuManager menuManager;

	public Button otpButton;

	private void Awake()
	{
		menuManager.isOtpValid.Subscribe(delegate(bool isActive)
		{
			otpButton.interactable = isActive;
		}).AddTo(this);
	}
}
