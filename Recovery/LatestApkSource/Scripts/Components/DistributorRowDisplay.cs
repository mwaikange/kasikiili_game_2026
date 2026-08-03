using UnityEngine;
using UnityEngine.UI;

namespace Components;

public class DistributorRowDisplay : MonoBehaviour
{
	public Text nameLabel;

	public Text regionLabel;

	public Image statusImage;

	public Text mobileNumberLabel;

	public void LoadRow(string distName, string distRegion, string distMobileNumber, Color distColor)
	{
		nameLabel.text = distName;
		regionLabel.text = distRegion;
		mobileNumberLabel.text = distMobileNumber;
		statusImage.color = distColor;
	}
}
