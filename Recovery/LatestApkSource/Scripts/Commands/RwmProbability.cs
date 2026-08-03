using UnityEngine;

namespace Commands;

public class RwmProbability
{
	public float _afa;

	public float _pla;

	public string _letter;

	public RwmProbability(float tub)
	{
		_pla = tub * 0.6f;
		_afa = tub * 0.4f;
		Debug.Log("Total user balance is " + tub);
		Debug.Log("60% PLA is " + _pla);
		Debug.Log("40% AFA is " + _afa);
	}

	public int SelectRwm()
	{
		int result = 0;
		if (_pla >= 0f && _pla <= 999f)
		{
			_letter = "a";
			Debug.Log("RWA is a!");
			result = 10;
		}
		else if (_pla >= 1000f && _pla <= 5000f)
		{
			_letter = "b";
			Debug.Log("RWA is b!");
			result = ((!(Random.Range(0f, 1f) <= 0.65f)) ? 25 : 10);
		}
		else if (_pla >= 5001f && _pla <= 20000f)
		{
			_letter = "c";
			Debug.Log("RWA is c!");
			float num = Random.Range(0f, 1f);
			result = ((num <= 0.55f) ? 10 : ((!(num <= 0.85f)) ? 50 : 25));
		}
		else if (_pla >= 20001f && _pla <= 50000f)
		{
			_letter = "d";
			Debug.Log("RWA is d!");
			float num2 = Random.Range(0f, 1f);
			result = ((num2 <= 0.5f) ? 10 : ((num2 <= 0.75f) ? 25 : ((!(num2 <= 0.85f)) ? 100 : 50)));
		}
		else if (_pla >= 50001f && _pla <= 100000f)
		{
			_letter = "e";
			Debug.Log("RWA is e!");
			float num3 = Random.Range(0f, 1f);
			result = ((num3 <= 0.45f) ? 10 : ((num3 <= 0.65f) ? 25 : ((num3 <= 0.7f) ? 50 : ((!(num3 <= 0.9f)) ? 1000 : 100))));
		}
		else if (_pla >= 100001f && _pla <= 180000f)
		{
			_letter = "f";
			Debug.Log("RWA is f!");
			float num4 = Random.Range(0f, 1f);
			result = ((num4 <= 0.4f) ? 10 : ((num4 <= 0.55f) ? 25 : ((num4 <= 0.6f) ? 50 : ((num4 <= 0.75f) ? 100 : ((!(num4 <= 0.9f)) ? 2000 : 1000)))));
		}
		else if (_pla >= 180001f)
		{
			_letter = "g";
			Debug.Log("RWA is g!");
			float num5 = Random.Range(0f, 1f);
			result = ((num5 <= 0.35f) ? 10 : ((num5 <= 0.5f) ? 25 : ((num5 <= 0.55f) ? 50 : ((num5 <= 0.7f) ? 100 : ((!(num5 <= 0.85f)) ? 2000 : 1000)))));
		}
		return result;
	}
}
