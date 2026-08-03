using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class OpenMenuInput : MonoBehaviour
{
	public RouletteManager rouletteManager;

	public AudioManager audioManager;

	public RouletteCmdFactory rouletteCmdFactory;

	public GameObject exitMenu;

	public void OnClick()
	{
		if (rouletteManager.tableActive.Value && rouletteManager.gameActive.Value)
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Pause).Execute();
			exitMenu.SetActive(value: true);
		}
	}
}
