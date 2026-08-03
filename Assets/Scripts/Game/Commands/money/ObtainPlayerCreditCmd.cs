using Infrastructure;
using UniRx;
using ViewModel;

namespace Commands
{
    public class ObtainPlayerCreditCmd : ICommand
    {
        private readonly TableManager _tableManager;
        private readonly PlayerCashGateway _playerCashGateway;

        public ObtainPlayerCreditCmd(TableManager tableManager, PlayerCashGateway playerCashGateway)
        {
            _tableManager = tableManager;
            _playerCashGateway = playerCashGateway;
        }

        public void Execute()
        {
            _tableManager.cashManager.ResetMoney();
            _tableManager.cashManager.PaymentSystem(100);
            // _playerCashGateway.GetPlayerCredits(_tableManager.cashManager)
            //     .Subscribe();
        }
    }
}