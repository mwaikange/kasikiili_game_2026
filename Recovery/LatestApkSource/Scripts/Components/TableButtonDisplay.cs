using TMPro;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class TableButtonDisplay : MonoBehaviour
{
	public TableButton tableButton;

	public GameObject flareObject;

	public TextMeshProUGUI numberText;

	private Color _defaultColorText;

	private void Awake()
	{
		if (tableButton == null)
		{
			Debug.LogWarning("Table Button Not Found!");
			return;
		}
		flareObject.SetActive(value: false);
		_defaultColorText = numberText.color;
		numberText.text = tableButton.betValue.ToString();
		tableButton.isSelected.Subscribe(OnButtonSelect).AddTo(this);
		tableButton.Refresh.Subscribe(OnButtonSelect).AddTo(this);
	}

	private void OnButtonSelect(bool selected)
	{
		if (selected)
		{
			Select();
		}
		else
		{
			UnSelect();
		}
	}

	private void Select()
	{
		flareObject.SetActive(value: true);
		numberText.color = Color.white;
	}

	private void UnSelect()
	{
		flareObject.SetActive(value: false);
		numberText.color = _defaultColorText;
	}
}
