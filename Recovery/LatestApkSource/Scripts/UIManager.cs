using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Transform toggleCredential;

	[SerializeField]
	private TMP_InputField mobileNumberInputField;

	[SerializeField]
	private TMP_InputField passwordInputField;

	private bool canSaveLoginCredentials;

	public static UIManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		canSaveLoginCredentials = PlayerPrefs.GetInt("credential") == 1;
		ChangeCredentialToggleStatus(canSaveLoginCredentials);
	}

	private void Start()
	{
		if (!PlayerPrefs.HasKey("mobile_no") && !PlayerPrefs.HasKey("password"))
		{
			mobileNumberInputField.text = "";
			passwordInputField.text = "";
		}
		else
		{
			mobileNumberInputField.text = PlayerPrefs.GetString("mobile_no");
			passwordInputField.text = PlayerPrefs.GetString("password");
		}
	}

	public void ToggleCredentialStatus()
	{
		canSaveLoginCredentials = !canSaveLoginCredentials;
		ChangeCredentialToggleStatus(canSaveLoginCredentials);
		if (canSaveLoginCredentials)
		{
			PlayerPrefs.SetInt("credential", 1);
		}
		else
		{
			PlayerPrefs.SetInt("credential", 0);
		}
	}

	private void ChangeCredentialToggleStatus(bool isOn)
	{
		toggleCredential.GetChild(0).gameObject.SetActive(!isOn);
		toggleCredential.GetChild(1).gameObject.SetActive(isOn);
	}

	internal bool CanSaveCredentials()
	{
		return canSaveLoginCredentials;
	}
}
