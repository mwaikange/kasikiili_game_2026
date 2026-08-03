using Infrastructure;
using UnityEngine;
using ViewModel;

namespace Commands;

[CreateAssetMenu(fileName = "RoundCmdFactory", menuName = "Factory/RoundCmdFactory", order = 0)]
public class RoundCmdFactory : ScriptableObject
{
	public StartRoundCmd StartRound(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		return new StartRoundCmd(rouletteManager, roundManager, tableManager);
	}

	public CancelRoundCmd CancelRound(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		return new CancelRoundCmd(rouletteManager, roundManager, tableManager);
	}

	public PaymentRoundCmd PaymentRound(GameManager gameManager, RoundManager roundManager, TableManager tableManager, Loading loading, ErrorManager errorManager)
	{
		return new PaymentRoundCmd(gameManager, roundManager, tableManager, new PlayerGateway(new SecurityGateway()), loading, errorManager);
	}

	public ResetRoundCmd ResetRound(GameManager gameManager, RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		return new ResetRoundCmd(gameManager, rouletteManager, roundManager, tableManager, new PlayerGateway(new SecurityGateway()));
	}

	public ResetTableCmd RefreshRound(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
	{
		return new ResetTableCmd(rouletteManager, roundManager, tableManager);
	}
}
