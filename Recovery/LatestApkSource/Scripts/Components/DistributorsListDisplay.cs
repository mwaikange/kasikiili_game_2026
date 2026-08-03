using System;
using SimpleJSON;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class DistributorsListDisplay : MonoBehaviour
{
	public GameManager gameManager;

	public GameObject container;

	public GameObject rowPrefab;

	public Color colorGreen;

	public Color colorRed;

	private void Awake()
	{
		gameManager.OnDistributorsList.Subscribe(ShowList).AddTo(this);
	}

	private void ShowList(JSONNode distributors)
	{
		DeleteAllChildren();
		JSONNode.ValueEnumerator enumerator = distributors.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			JSONNode current = enumerator.Current;
			UnityEngine.Object.Instantiate(rowPrefab, container.transform).GetComponent<DistributorRowDisplay>().LoadRow(distColor: (Convert.ToInt32(current["balance"].Value) > 0) ? colorGreen : colorRed, distName: current["name"].Value, distRegion: current["region"].Value, distMobileNumber: current["mobile_number"].Value);
		}
	}

	private void DeleteAllChildren()
	{
		foreach (Transform item in container.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}
}
