using UniRx;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class PopupSuccessDisplay : MonoBehaviour
{
	public MenuManager menuManager;

	public GameObject successAnchor;

	public Text successTitle;

	public Text successSubtitle;

	public Text successLabel;

	private void Awake()
	{
		menuManager.OnSuccessPopUp.Subscribe(OnSuccess).AddTo(this);
	}

	private void OnSuccess(PopUp popUp)
	{
		successAnchor.SetActive(value: true);
		successTitle.text = popUp.Title.ToUpper();
		successSubtitle.text = popUp.Subtitle;
		successLabel.text = popUp.Message;
	}
}
