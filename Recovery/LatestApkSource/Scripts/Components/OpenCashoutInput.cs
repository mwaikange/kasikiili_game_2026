using UnityEngine;
using ViewModel;

namespace Components;

public class OpenCashoutInput : MonoBehaviour
{
	public RoundManager roundManager;

	public GameObject cashoutMenu;

	public GameObject gameMenu;

	public void OnClick()
	{
		roundManager.OnReset.OnNext(value: true);
		cashoutMenu.SetActive(value: true);
		gameMenu.SetActive(value: false);
	}
}
