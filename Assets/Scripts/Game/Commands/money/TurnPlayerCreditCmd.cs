using Infrastructure;
using UniRx;
using ViewModel;

namespace Commands
{
    public class TurnPlayerCreditCmd : ICommand
    {
        private readonly int _credits;
        private readonly TableManager _tableManager;
        private readonly PlayerCashGateway _playerCashGateway;

        public TurnPlayerCreditCmd(int credits, TableManager tableManager, PlayerCashGateway playerCashGateway)
        {
            _credits = credits;
            _tableManager = tableManager;
            _playerCashGateway = playerCashGateway;
        }

        public void Execute()
        {
            _playerCashGateway.SetPlayerCredits(_credits, _tableManager.cashManager)
                .Subscribe();
        }
    }
}