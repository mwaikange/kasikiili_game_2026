using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
// using Controllers;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "CashManager", menuName = "Scriptable/Manager/CashManager")]
    public class CashManager : ScriptableObject
    {
        // Player credits
        public IntReactiveProperty currentCredit = new IntReactiveProperty();
        // Current bet in table
        public IntReactiveProperty currentBet = new IntReactiveProperty();
        // Current money that player win
        public IntReactiveProperty currentPayment = new IntReactiveProperty();

        void AddCash(int cashWinner)
        {
            Debug.Log("Cashing add is " + cashWinner);
            currentCredit.Value += cashWinner;
        }
        
        void SubstrateCash(int cashLost)
        {
            if(cashLost < 0) 
            {
                cashLost = cashLost * -1;
            }

            currentCredit.Value -= cashLost;

            if (currentCredit.Value < 0)
            {
                currentCredit.Value = 0;
            }
        }

        // Operations in player bet
        void AddBet(int betSum)
        {
            currentBet.Value  += betSum;
        }
        void SubstrateBet(int betRest)
        {
            currentBet.Value  -= betRest;
        }

        // Public methods
        public void ResetMoney()
        {
            currentBet.Value = 0;
            currentPayment.Value = 0;
            currentCredit.Value = 0;
        }
        
        public bool AddCheckBet(int valueBet)
        {
            // Check if the bet is possible
            if (valueBet <= currentCredit.Value  && valueBet != 0)
            {
                SubstrateCash(valueBet);
                AddBet(valueBet);
                return true;
            }
            return false;
        }
        public void RemoveCheckBet(int valueBet)
        {
            // Delete bet of the table
            SubstrateBet(valueBet);
            AddCash(valueBet);
        }
        
        // Public Payment   
        public void PaymentSystem(int payment)
        {      
            // If the player win when the round finish game will pay.
            // If not win it will stay with the same money without the bet.
            if(payment > 0)
                AddCash(payment);
                
            Debug.Log($"Player credit will add {payment} credits!");
        }
    }
}
