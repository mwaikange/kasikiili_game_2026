using Commands;
using TMPro;
using UnityEngine;
using ViewModel;

namespace Components;

public class LoginButtonInput : MonoBehaviour
{
	public GameManager gameManager;

	public TMP_InputField mobileNoField;

	public TMP_InputField passwordField;

	public BackendCmdFactory backendCmdFactory;

	public void OnClick()
	{
		if (IsFormValid())
		{
			backendCmdFactory.PostUserLogin(gameManager, mobileNoField.text, passwordField.text).Execute();
		}
	}

	private bool IsFormValid()
	{
		if (!string.IsNullOrEmpty(mobileNoField.text))
		{
			return !string.IsNullOrEmpty(passwordField.text);
		}
		return false;
	}
}
