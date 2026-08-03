using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class TableButtonAudioDisplay : MonoBehaviour
{
	public TableManager tableManager;

	public TableButton tableButton;

	public AudioManager audioManager;

	public AudioSource source;

	public AbstractAudio fxAudio;

	private void Awake()
	{
		if (tableButton == null)
		{
			Debug.LogWarning("Table Button Not Found!");
		}
		else
		{
			tableButton.isSelected.Subscribe(OnButtonSelect).AddTo(this);
		}
	}

	private void OnButtonSelect(bool selected)
	{
		if (selected)
		{
			audioManager.Play(fxAudio, source);
		}
	}
}
