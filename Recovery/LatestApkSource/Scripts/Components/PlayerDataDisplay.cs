using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace Components;

public class PlayerDataDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public Text mobileNumberLabel;

	public Text regionLabel;

	private void Start()
	{
		if (!(gameManager.UserData == null))
		{
			mobileNumberLabel.text = gameManager.UserData["mobile_number"].Value;
			regionLabel.text = gameManager.UserData["region"].Value;
		}
	}
}
