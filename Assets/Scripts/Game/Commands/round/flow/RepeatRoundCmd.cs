using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using ViewModel;
using Random = System.Random;

namespace Commands
{
    public class RepeatRoundCmd : ICommand
    {
        private TableManager _tableManager;

        public RepeatRoundCmd(TableManager tableManager)
        {
            _tableManager = tableManager;
        }

        public void Execute()
        {
            if (_tableManager.buttonsLast.Count <= 0)
            {
                Debug.Log("There is no past table!");
                return;
            }
            _tableManager.buttonsSelected = _tableManager.buttonsLast;
            _tableManager.buttonsSelected = _tableManager.buttonsSelected.Distinct().ToList();

            foreach (var button in _tableManager.buttonsSelected)
            {
                button.Refresh.OnNext(true);
            }
        }
    }
}