using System;
using ViewModel;

namespace Commands
{
    public class ButtonClickCmd : ICommand
    {
        private TableManager _tableManager;
        private TableButton _tableButton;

        public ButtonClickCmd(TableManager tableManager, TableButton tableButton)
        {
            _tableManager = tableManager;
            _tableButton = tableButton;
        }

        public void Execute()
        {
            if (_tableButton.isSelected.Value)
            {
                UnSelect();
            }
            else
            {
                Select();
            }
        }
        
        private void Select()
        {
            if (!_tableManager.cashManager.AddCheckBet(_tableButton.betValue)) return;
            _tableManager.buttonsSelected.Add(_tableButton);
            _tableButton.isSelected.Value = true;
        }
        private void UnSelect()
        {
            _tableManager.buttonsSelected.Remove(_tableButton);
            _tableButton.isSelected.Value = false;
            _tableManager.cashManager.RemoveCheckBet(_tableButton.betValue);
        }
    }
}
