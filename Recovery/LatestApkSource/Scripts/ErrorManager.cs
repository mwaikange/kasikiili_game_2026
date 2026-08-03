using UnityEngine;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
	private Text message;

	private void Awake()
	{
		message = base.transform.GetChild(0).GetChild(1).GetChild(1)
			.GetComponent<Text>();
		base.gameObject.SetActive(value: false);
	}

	internal void Show(string message)
	{
		this.message.text = message;
		base.gameObject.SetActive(value: true);
	}

	public void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}
