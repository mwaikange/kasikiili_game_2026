using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class CashoutHistoryListDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public GameObject container;

	public GameObject rowPrefab;

	private void Awake()
	{
		gameManager.OnCashoutHistoryList.Subscribe(ShowList).AddTo(this);
	}

	private void ShowList(JSONNode distributors)
	{
		DeleteAllChildren();
		JSONNode.ValueEnumerator enumerator = distributors.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			JSONNode current = enumerator.Current;
			Object.Instantiate(rowPrefab, container.transform).GetComponent<CashoutHistoryRowDisplay>().LoadRow(current["amount"].Value, current["status"].Value, current["trascantion_date"].Value, current["distributor"].Value);
		}
	}

	private void DeleteAllChildren()
	{
		foreach (Transform item in container.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}
}
