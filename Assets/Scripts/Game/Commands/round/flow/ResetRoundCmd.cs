using System;
using System.Collections;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using ViewModel;
using Random = System.Random;

namespace Commands
{
    public class ResetRoundCmd : ICommand
    {
        private readonly RouletteManager _rouletteManager;
        private readonly RoundManager _roundManager;
        private readonly TableManager _tableManager;

        public ResetRoundCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
        {
            _rouletteManager = rouletteManager;
            _roundManager = roundManager;
            _tableManager = tableManager;
        }

        public void Execute()
        {
            Debug.Log("Reset your round!");
            ResetRound();
        }
        
        private void ResetRound()
        {
            Observable.FromCoroutine<Unit>(ResetSequence)
                .Subscribe();
        }
        
        IEnumerator ResetSequence(IObserver<Unit> observer)
        {
            // Reset Money
            _tableManager.cashManager.PaymentSystem(_tableManager.cashManager.currentPayment.Value);
            _tableManager.cashManager.currentPayment.Value = 0;
            yield return new WaitForSeconds(_roundManager.roundResetDelay);
            // Reset Table
            var buttons = _tableManager.buttonsSelected.ToArray();
            foreach(var button in buttons)
            {
                button.isSelected.Value = false;
            }
            // Reset Parameters
            _tableManager.buttonsLast = _tableManager.buttonsSelected;
            _tableManager.buttonsSelected.Clear();
            // Reset Round
            _rouletteManager.tableActive.Value = true;
            _roundManager.OnClear.OnNext(true);
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}