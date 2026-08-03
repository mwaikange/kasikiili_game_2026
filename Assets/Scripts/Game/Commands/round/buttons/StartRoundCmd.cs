using System;
using System.Collections;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using ViewModel;
using Random = System.Random;

namespace Commands
{
    public class StartRoundCmd : ICommand
    {
        private readonly RouletteManager _rouletteManager;
        private readonly RoundManager _roundManager;
        private readonly TableManager _tableManager;

        public StartRoundCmd(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
        {
            _rouletteManager = rouletteManager;
            _roundManager = roundManager;
            _tableManager = tableManager;
        }

        public void Execute()
        {
            if (!_rouletteManager.tableActive.Value || !_rouletteManager.gameActive.Value) return;
            if (_tableManager.buttonsSelected.Count <= 0)
            {
                Debug.Log("There is no bet in table");
                return;
            }

            Debug.Log("Starting your round!");
            _rouletteManager.tableActive.Value = false;
            Observable.FromCoroutine<Unit>(StartGame)
                .Subscribe();
        }

        IEnumerator StartGame(IObserver<Unit> observer)
        {
            // DISABLED TO DEBUG
            // var winNumber = _roundManager.winNumber.Value;
            var winNumber = GetRandomNumber(); 
            // var winNumber = 0;
            yield return new WaitForSeconds(_roundManager.wheelDelay);
            _roundManager.winNumber.Value = winNumber;
            _roundManager.OnRound.OnNext(winNumber);
            yield return new WaitForSeconds(_roundManager.wheelDuration);
            _roundManager.OnPayment.OnNext(winNumber);
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
        
        private int GetRandomNumber()
        {
            // RNGCryptoServiceProvider
            // Generate a pseudo-random number between 0 and 13 exclusive
            using var rng = new RNGCryptoServiceProvider();
            var randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            var result = BitConverter.ToInt32(randomNumber, 0);
            result = Math.Abs(result % 13);
            return result;
        }
    }
}