using Commands;
using UnityEngine;
using ViewModel;

namespace Components;

public class MoneyStartInput : MonoBehaviour
{
	public RoundCmdFactory roundCmdFactory;

	public GameManager gameManager;

	public RouletteManager rouletteManager;

	public TableManager tableManager;

	private void Awake()
	{
	}
}
