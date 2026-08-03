using System;
using Components;
using Infrastructure;
using ViewModel;
using UnityEngine;
using UnityEngine.AI;

namespace Commands
{

    [CreateAssetMenu(fileName = "MoneyCmdFactory", menuName = "Factory/MoneyCmdFactory", order = 0)]
    public class MoneyCmdFactory : ScriptableObject 
    {
        public ObtainPlayerCreditCmd ObtainPlayerCredit(TableManager tableManager)
        {
            return new ObtainPlayerCreditCmd(tableManager, new PlayerCashGateway());
        }
        
        public TurnPlayerCreditCmd TurnPlayerCredit(TableManager tableManager)
        {
            return new TurnPlayerCreditCmd(tableManager.cashManager.currentCredit.Value, tableManager, new PlayerCashGateway());
        }
    }
}