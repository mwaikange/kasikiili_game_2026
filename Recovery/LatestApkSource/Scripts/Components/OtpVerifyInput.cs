using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class OtpVerifyInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public MenuManager menuManager;

	public TMP_InputField mobileNoField;

	public TMP_InputField otpField;

	private void Start()
	{
		menuManager.OnOtpValid.Subscribe(SignUpUser).AddTo(this);
	}

	private void SignUpUser(bool isOtpValid)
	{
		if (isOtpValid)
		{
			menuManager.OnSignUp.OnNext(value: true);
		}
	}

	public void OnClick()
	{
		if (otpField.text.Length < 4)
		{
			gameManager.errorManager.OnAlertError.OnNext("Please enter valid OTP. More than 4 digits.");
		}
		else
		{
			backendCmdFactory.ValidateOtpNumber(gameManager, menuManager, mobileNoField.text, otpField.text).Execute();
		}
	}

	private void Reset()
	{
		otpField.text = string.Empty;
		menuManager.isOtpValid.Value = false;
	}

	private void OnEnable()
	{
		Reset();
	}

	private void OnDisable()
	{
		Reset();
	}
}
