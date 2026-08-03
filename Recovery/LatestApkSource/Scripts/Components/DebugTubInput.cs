using UnityEngine;
using ViewModel;

namespace Components;

public class DebugTubInput : MonoBehaviour
{
	public RoundManager roundManager;

	private static readonly float[] Values = new float[7] { 999f, 5000f, 20000f, 50000f, 100000f, 180000f, 2000001f };

	private void Awake()
	{
		float value = Values[0];
		roundManager.probabilityTub.Value = value;
	}

	public void ChangeLetter(int number)
	{
		float value = Values[number];
		roundManager.probabilityTub.Value = value;
	}
}
