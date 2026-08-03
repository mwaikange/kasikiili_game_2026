using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Components;

public class DelayEnableButton : MonoBehaviour
{
	public float delay = 3f;

	public Button delayButton;

	private IDisposable _observer;

	public void OnEnable()
	{
		delayButton.interactable = false;
		_observer = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(delegate
		{
			delayButton.interactable = true;
			_observer.Dispose();
		}).AddTo(this);
	}

	private void OnDisable()
	{
		_observer.Dispose();
	}
}
