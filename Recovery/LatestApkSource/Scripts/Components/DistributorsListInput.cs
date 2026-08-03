using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class DistributorsListInput : MonoBehaviour
{
	public BackendCmdFactory backendCmdFactory;

	public GameManager gameManager;

	private void OnEnable()
	{
		backendCmdFactory.GetDistributorsList(gameManager).Execute();
	}
}
