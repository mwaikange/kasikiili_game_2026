using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class OtpPopUpDisplay : MonoBehaviour
{
	public MenuManager menuManager;

	public GameObject otpPopUp;

	private void Awake()
	{
		menuManager.OnOtpPopUp.Subscribe(delegate(bool isActive)
		{
			otpPopUp.SetActive(isActive);
		}).AddTo(this);
	}
}
