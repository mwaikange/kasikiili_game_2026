using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

[RequireComponent(typeof(SignUpDataDisplay))]
public class SignUpButtonInput : MonoBehaviour
{
	public MenuManager menuManager;

	public GameManager gameManager;

	public TMP_InputField refCodeField;

	public Dropdown regionField;

	public TMP_InputField mobileNoField;

	public TMP_InputField passwordField;

	public TMP_InputField referralField;

	public Toggle termsField;

	public BackendCmdFactory backendCmdFactory;

	public Button signUpButton;

	private void Start()
	{
		menuManager.OnSignUp.Subscribe(SignUp).AddTo(this);
	}

	private void SignUp(bool isSuccess)
	{
		signUpButton.interactable = !isSuccess;
		if (!isSuccess)
		{
			GetComponent<SignUpDataDisplay>().Reset();
		}
		backendCmdFactory.PostUserSignUp(gameManager, menuManager, mobileNoField.text, passwordField.text, regionField.options[regionField.value].text).Execute();
	}

	public void OnClick()
	{
		if (ValidSignUp())
		{
			signUpButton.interactable = false;
			backendCmdFactory.CheckMobileNumber(gameManager, menuManager, mobileNoField.text).Execute();
		}
	}

	private bool ValidSignUp()
	{
		if (mobileNoField.text.Length < 4 || !mobileNoField.text.StartsWith("264"))
		{
			gameManager.errorManager.OnAlertError.OnNext("Please enter valid mobile number. Example: 264 81...");
			return false;
		}
		if (passwordField.text.Length < 4)
		{
			gameManager.errorManager.OnAlertError.OnNext("Please enter valid password. More than 4 digits.");
			return false;
		}
		if (regionField.value == 0)
		{
			gameManager.errorManager.OnAlertError.OnNext("Please select a region.");
			return false;
		}
		if (!termsField.isOn)
		{
			gameManager.errorManager.OnAlertError.OnNext("Please confirm age and terms and conditions.");
			return false;
		}
		return true;
	}

	private void OnEnable()
	{
		menuManager.isOtpValid.Value = false;
	}
}
