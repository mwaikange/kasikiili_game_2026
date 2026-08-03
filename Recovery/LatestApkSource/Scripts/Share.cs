using UnityEngine;
using UnityEngine.UI;

public class Share : MonoBehaviour
{
	[SerializeField]
	private GameObject referralBox;

	private void Awake()
	{
		referralBox.SetActive(value: false);
	}

	public void GetReferalLink()
	{
		referralBox.transform.GetChild(0).GetChild(1).GetChild(1)
			.GetComponent<Text>()
			.text = PlayerPrefs.GetString("refferal_link");
		referralBox.SetActive(value: true);
	}

	public void CloseReferalLink()
	{
		referralBox.SetActive(value: false);
	}

	public void CopyReferralLink()
	{
		GUIUtility.systemCopyBuffer = PlayerPrefs.GetString("refferal_link");
		CloseReferalLink();
	}
}
