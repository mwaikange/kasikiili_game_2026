using UnityEngine;

namespace Components;

public class FlowLinkInput : MonoBehaviour
{
	public string url;

	public void OnClick()
	{
		Application.OpenURL("https://" + url);
	}
}
