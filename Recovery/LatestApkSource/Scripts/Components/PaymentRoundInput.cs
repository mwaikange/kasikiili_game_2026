using Commands;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Components;

public class PaymentRoundInput : MonoBehaviour
{
	public RoundCmdFactory roundCmdFactory;

	public GameManager gameManager;

	public RoundManager roundManager;

	public TableManager tableManager;

	[SerializeField]
	private Loading loading;

	[SerializeField]
	private ErrorManager errorManager;

	private void Awake()
	{
		roundManager.OnPayment.Subscribe(OnPaymentStarter).AddTo(this);
	}

	private void OnPaymentStarter(int credits)
	{
		roundCmdFactory.PaymentRound(gameManager, roundManager, tableManager, loading, errorManager).Execute();
	}
}
