using Infrastructure;
using UnityEngine;
using ViewModel;

namespace Commands;

[CreateAssetMenu(fileName = "BackendCmdFactory", menuName = "Factory/BackendCmdFactory", order = 0)]
public class BackendCmdFactory : ScriptableObject
{
	public TestConnectionCmd TestConnection(GameManager gameManager)
	{
		return new TestConnectionCmd(gameManager, new ApiGateway(new SecurityGateway()));
	}

	public TestGameConnectionCmd TestGameConnection(GameManager gameManager)
	{
		return new TestGameConnectionCmd(gameManager, new ApiGateway(new SecurityGateway()));
	}

	public UserLogoutCmd PostUserLogout(GameManager gameManager)
	{
		return new UserLogoutCmd(gameManager, new UserGateway(new SecurityGateway()));
	}

	public PostUserLoginCmd PostUserLogin(GameManager gameManager, string mobileNo, string password)
	{
		return new PostUserLoginCmd(gameManager, mobileNo, password, new UserGateway(new SecurityGateway()));
	}

	public PostUserSignUpCmd PostUserSignUp(GameManager gameManager, MenuManager menuManager, string mobileNo, string password, string region)
	{
		return new PostUserSignUpCmd(gameManager, menuManager, mobileNo, password, region, new UserGateway(new SecurityGateway()));
	}

	public CheckMobileNumberCmd CheckMobileNumber(GameManager gameManager, MenuManager menuManager, string mobileNo)
	{
		return new CheckMobileNumberCmd(gameManager, menuManager, mobileNo, new UserGateway(new SecurityGateway()));
	}

	public SendOtpNumberCmd SendOtpNumber(GameManager gameManager, MenuManager menuManager, string mobileNo)
	{
		return new SendOtpNumberCmd(gameManager, menuManager, mobileNo, new UserGateway(new SecurityGateway()));
	}

	public RegisterOtpNumberCmd RegisterOtpNumber(GameManager gameManager, MenuManager menuManager, string mobileNo)
	{
		return new RegisterOtpNumberCmd(gameManager, menuManager, mobileNo, new UserGateway(new SecurityGateway()));
	}

	public ValidOtpNumberCmd ValidateOtpNumber(GameManager gameManager, MenuManager menuManager, string mobileNo, string otpNo)
	{
		return new ValidOtpNumberCmd(gameManager, menuManager, mobileNo, otpNo, new UserGateway(new SecurityGateway()));
	}

	public ChangeUserPasswordCmd ChangeUserPassword(GameManager gameManager, MenuManager menuManager, string mobileNo, string newPassword)
	{
		return new ChangeUserPasswordCmd(gameManager, menuManager, mobileNo, newPassword, new UserGateway(new SecurityGateway()));
	}

	public GetUserBalanceCmd GetUserBalance(GameManager gameManager, TableManager tableManager)
	{
		return new GetUserBalanceCmd(gameManager, tableManager, new PlayerGateway(new SecurityGateway()));
	}

	public GetTotalUserBalanceCmd GetTotalUserBalance(GameManager gameManager, TableManager tableManager)
	{
		return new GetTotalUserBalanceCmd(gameManager, tableManager, new GameGateway(new SecurityGateway()));
	}

	public PostUserCashoutCmd PostUserCashout(GameManager gameManager, int amount)
	{
		return new PostUserCashoutCmd(gameManager, amount, new PlayerGateway(new SecurityGateway()));
	}

	public GetDistributorsListCmd GetDistributorsList(GameManager gameManager)
	{
		return new GetDistributorsListCmd(gameManager, new DistributorsGateway(new SecurityGateway()));
	}

	public GetUserCashoutHistoryListCmd GetCashoutHistoryList(GameManager gameManager)
	{
		return new GetUserCashoutHistoryListCmd(gameManager, new PlayerGateway(new SecurityGateway()));
	}

	public GetUserAwaitingCashout GetAwaitingCashout(GameManager gameManager)
	{
		return new GetUserAwaitingCashout(gameManager, new PlayerGateway(new SecurityGateway()));
	}
}
