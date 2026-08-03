using System;
using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class FlowMenuDisplay : MonoBehaviour
{
	public MenuManager menuManager;

	public GameObject loginMenu;

	public GameObject registerMenu;

	public GameObject forgotPasswordMenu;

	[SerializeField]
	private TextMeshProUGUI loginPageVersion;

	[SerializeField]
	private TextMeshProUGUI registerPageVersion;

	[SerializeField]
	private TextMeshProUGUI forgetPasswordPageVersion;

	private void Awake()
	{
		menuManager.OnFlowChange.Subscribe(OnSuccess).AddTo(this);
		string text = "Version " + Application.version + " | © Copyright of Kasikili Virtual Gaming cc | 2025";
		loginPageVersion.text = text;
		registerPageVersion.text = text;
		forgetPasswordPageVersion.text = text;
	}

	private void OnSuccess(MenuType menu)
	{
		switch (menu)
		{
		case MenuType.Login:
			loginMenu.SetActive(value: true);
			registerMenu.SetActive(value: false);
			forgotPasswordMenu.SetActive(value: false);
			break;
		case MenuType.Register:
			loginMenu.SetActive(value: false);
			registerMenu.SetActive(value: true);
			forgotPasswordMenu.SetActive(value: false);
			break;
		case MenuType.ForgotPassword:
			loginMenu.SetActive(value: false);
			registerMenu.SetActive(value: false);
			forgotPasswordMenu.SetActive(value: true);
			break;
		default:
			throw new ArgumentOutOfRangeException("menu", menu, null);
		}
	}
}
