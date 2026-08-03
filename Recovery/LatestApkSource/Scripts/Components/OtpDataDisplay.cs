using TMPro;
using UnityEngine;

namespace Components;

public class OtpDataDisplay : MonoBehaviour
{
	public TMP_InputField otpField;

	private void Reset()
	{
		otpField.text = string.Empty;
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
