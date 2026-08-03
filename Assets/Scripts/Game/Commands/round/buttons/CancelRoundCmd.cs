using System;
using System.Security.Cryptography;
using UnityEngine;
using ViewModel;
using Random = System.Random;

namespace Commands
{
    public class CancelRoundCmd : ICommand
    {
        private readonly RouletteManager _rouletteManager;
        private readonly RoundManager _roundManager;
        private readonly TableManager _tableManager;

        public CancelRoundCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
        {
            _rouletteManager = rouletteManager;
            _roundManager = roundManager;
            _tableManager = tableManager;
        }

        public void Execute()
        {
            if (!_rouletteManager.tableActive.Value || !_rouletteManager.gameActive.Value) return;
            var buttons = _tableManager.buttonsSelected.ToArray();
            foreach(var button in buttons)
            {
                button.Refresh.OnNext(false);
            }
        }
    }
}