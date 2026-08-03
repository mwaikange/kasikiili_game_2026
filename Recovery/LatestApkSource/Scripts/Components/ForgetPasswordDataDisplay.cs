using TMPro;
using UnityEngine;

namespace Components;

public class ForgetPasswordDataDisplay : MonoBehaviour
{
	public TMP_InputField mobileNoField;

	public TMP_InputField otpField;

	public TMP_InputField passwordField;

	public TMP_InputField reEnterPasswordField;

	private void Reset()
	{
		mobileNoField.text = string.Empty;
		mobileNoField.interactable = true;
		otpField.interactable = false;
		otpField.text = string.Empty;
		passwordField.interactable = false;
		passwordField.text = string.Empty;
		reEnterPasswordField.interactable = false;
		reEnterPasswordField.text = string.Empty;
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
