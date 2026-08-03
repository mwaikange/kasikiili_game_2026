using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components;

public class SignUpDataDisplay : MonoBehaviour
{
	public TMP_InputField refCodeField;

	public Dropdown regionField;

	public TMP_InputField mobileNoField;

	public TMP_InputField passwordField;

	public Toggle termsField;

	public void Reset()
	{
		refCodeField.text = string.Empty;
		mobileNoField.text = string.Empty;
		passwordField.text = string.Empty;
		termsField.isOn = false;
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
