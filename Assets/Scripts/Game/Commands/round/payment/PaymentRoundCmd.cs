using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using ViewModel;
using Random = UnityEngine.Random;

namespace Commands
{
    public class PaymentRoundCmd : ICommand
    {
        private readonly RoundManager _roundManager;
        private readonly TableManager _tableManager;
        private static readonly int[] ProbabilityNumbers = { 10, 50, 1000, 2000, 25, 100 };
        
        public PaymentRoundCmd(RoundManager roundManager, TableManager tableManager)
        {
            _roundManager = roundManager;
            _tableManager = tableManager;
            
        }

        public void Execute()
        {
            if (_tableManager.CheckIfPlayerWin(_roundManager.winNumber.Value))
            {
                Debug.Log("WIN! The number " + _roundManager.winNumber.Value + " is in table!");
                // Calculate probability
                var tub = GetRandomTub();
                // DISABLED TO DEBUG
                // var tub = 2000001;
                var rwmProbability = new RwmProbability(tub);
                var probability = rwmProbability.SelectRwm();
                _roundManager.probabilityTub.Value = tub;
                _roundManager.probabilityNumber.Value = probability;
                _roundManager.probabilityPla.Value = rwmProbability._pla;
                _roundManager.probabilityAfa.Value = rwmProbability._afa;
                _roundManager.probabilityLetter.Value = rwmProbability._letter;
                WinRound(probability);
            }
            else
            {
                Debug.Log("LOST! There is no win bet!");
                LostRound();
            }
        }

        private void WinRound(int probability)
        {
            var winButtons = _tableManager.GetWinButtons(_roundManager.winNumber.Value);
            var winButtonsPayment = winButtons.Sum(button => button.betValue);

            // Calculate payment
            var payment = (winButtonsPayment * probability);
            _tableManager.cashManager.currentPayment.Value = payment + winButtonsPayment;
            // Execute game sequence
            Observable.FromCoroutine<Unit>(observer => WinSequence(observer, payment, probability))
                .Subscribe();
        }

        IEnumerator WinSequence(IObserver<Unit> observer, int payment, int probability)
        {
            // Probability calculator
            yield return new WaitForSeconds(_roundManager.probabilityDelay);
            // Probability animation
            _roundManager.OnProbability.OnNext(probability);
            yield return new WaitForSeconds(_roundManager.probabilityDuration);
            
            if (probability == 1000 || probability == 2000)
            {
                // Jackpot probability
                yield return new WaitForSeconds(_roundManager.probabilityJackpotDelay);
                _roundManager.OnJackpot.OnNext(probability);
                yield return new WaitForSeconds(_roundManager.probabilityJackpotDuration);
            }
            yield return new WaitForSeconds(_roundManager.moneyDelay);
            // Show user money
            _roundManager.OnWin.OnNext(payment);
            yield return new WaitForSeconds(_roundManager.moneyDuration);
            // Reset round game
            _roundManager.OnReset.OnNext(true);
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }

        private void LostRound()
        {
            // ReSharper disable once ConvertClosureToMethodGroup
            Observable.FromCoroutine<Unit>(observer => LostSequence(observer))
                .Subscribe();
        }

        IEnumerator LostSequence(IObserver<Unit> observer)
        {
            _roundManager.OnReset.OnNext(true);
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
            yield return null;
        }

        float GetRandomTub()
        {
            var values = new float[]
            {
                999,
                5000,
                20000,
                50000,
                100000,
                180000,
                2000001
            };
            return values[Random.Range(0, values.Length-1)];
        }
    }
}