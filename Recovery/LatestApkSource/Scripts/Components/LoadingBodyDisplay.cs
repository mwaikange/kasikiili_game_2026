using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;

namespace Components;

public class LoadingBodyDisplay : MonoBehaviour
{
	public TextMeshProUGUI dialogLabel;

	public AudioSource audioSource;

	[TextArea(2, 3)]
	public string[] dialogConfig;

	public int loop = 15;

	private void Start()
	{
		Dialog();
		Observable.Interval(TimeSpan.FromSeconds(loop)).Subscribe(delegate
		{
			Dialog();
		}).AddTo(this);
	}

	private void Dialog()
	{
		StopAllCoroutines();
		if (dialogLabel.IsActive())
		{
			StartCoroutine(DialogStep());
		}
	}

	private IEnumerator DialogStep()
	{
		string text = dialogConfig[UnityEngine.Random.Range(0, dialogConfig.Length)];
		dialogLabel.text = text;
		yield return new WaitForSeconds(loop);
	}
}
