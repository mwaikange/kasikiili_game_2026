using UnityEngine;

namespace Components;

public class FlowCopyInput : MonoBehaviour
{
	public string textToCopy;

	public void CopyText()
	{
		GUIUtility.systemCopyBuffer = "https://" + textToCopy;
	}
}
