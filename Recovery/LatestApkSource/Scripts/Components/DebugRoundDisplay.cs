using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class DebugRoundDisplay : MonoBehaviour
{
	public RoundManager roundManager;

	public TextMeshProUGUI tub;

	public TextMeshProUGUI pla;

	public TextMeshProUGUI afa;

	public TextMeshProUGUI multiplier;

	public TextMeshProUGUI letter;

	public TextMeshProUGUI win;

	private void Awake()
	{
		roundManager.probabilityPla.Subscribe(OnPla).AddTo(this);
		roundManager.probabilityAfa.Subscribe(OnAfa).AddTo(this);
		roundManager.probabilityTub.Subscribe(OnTub).AddTo(this);
		roundManager.probabilityNumber.Subscribe(OnNumber).AddTo(this);
		roundManager.probabilityLetter.Subscribe(OnLetter).AddTo(this);
		roundManager.winNumber.Subscribe(OnWin).AddTo(this);
	}

	private void OnLetter(string letterString)
	{
		letter.text = "(A to G) SELECT = " + letterString;
	}

	private void OnPla(float plaNumber)
	{
		pla.text = "(70%) PLA = " + plaNumber;
	}

	private void OnAfa(float afaNumber)
	{
		afa.text = "(30%) AFA = " + afaNumber;
	}

	private void OnTub(float tubNumber)
	{
		tub.text = "(100%) TUB = " + tubNumber;
	}

	private void OnNumber(int probability)
	{
		multiplier.text = "MULT = " + probability;
	}

	private void OnWin(int winNumber)
	{
		win.text = "ROULETTE = " + winNumber;
	}
}
