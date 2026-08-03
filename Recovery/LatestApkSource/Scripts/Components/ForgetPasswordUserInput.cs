using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class ForgetPasswordUserInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	public MenuManager menuManager;

	public TMP_InputField mobileNoField;

	public TMP_InputField otpField;

	public TMP_InputField passwordField;

	public TMP_InputField reEnterPasswordField;

	public Button nextButton;

	public Text nextButtonText;

	private void Start()
	{
		menuManager.OnOtpSent.Subscribe(OtpSent).AddTo(this);
		menuManager.OnOtpValid.Subscribe(OtpValid).AddTo(this);
	}

	private void OtpSent(bool isSuccess)
	{
		if (isSuccess)
		{
			menuManager.currentForgetPasswordStage.Value = ForgetPasswordStage.Otp;
			mobileNoField.interactable = false;
			otpField.interactable = true;
			nextButtonText.text = "Validate Otp";
		}
	}

	private void OtpValid(bool isSuccess)
	{
		if (isSuccess)
		{
			menuManager.currentForgetPasswordStage.Value = ForgetPasswordStage.Password;
			mobileNoField.interactable = false;
			otpField.interactable = false;
			passwordField.interactable = true;
			reEnterPasswordField.interactable = true;
			menuManager.isOtpValid.Value = true;
			nextButtonText.text = "Change Password";
		}
	}

	public void OnClick()
	{
		switch (menuManager.currentForgetPasswordStage.Value)
		{
		case ForgetPasswordStage.MobileNo:
			if (mobileNoField.text.Length < 4 || !mobileNoField.text.StartsWith("264"))
			{
				gameManager.errorManager.OnAlertError.OnNext("Please enter valid mobile number. Example: 264 81...");
			}
			else
			{
				backendCmdFactory.SendOtpNumber(gameManager, menuManager, mobileNoField.text).Execute();
			}
			break;
		case ForgetPasswordStage.Otp:
			if (otpField.text.Length < 4)
			{
				gameManager.errorManager.OnAlertError.OnNext("Please enter valid OTP.");
			}
			else
			{
				backendCmdFactory.ValidateOtpNumber(gameManager, menuManager, mobileNoField.text, otpField.text).Execute();
			}
			break;
		case ForgetPasswordStage.Password:
			if (passwordField.text.Length < 4)
			{
				gameManager.errorManager.OnAlertError.OnNext("Please enter valid password.");
			}
			else if (passwordField.text != reEnterPasswordField.text)
			{
				gameManager.errorManager.OnAlertError.OnNext("Password and re-enter password does not match.");
			}
			else
			{
				backendCmdFactory.ChangeUserPassword(gameManager, menuManager, mobileNoField.text, passwordField.text).Execute();
			}
			break;
		}
	}

	private void Reset()
	{
		nextButtonText.text = "Next";
		nextButton.interactable = true;
		menuManager.currentForgetPasswordStage.Value = ForgetPasswordStage.MobileNo;
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
