using System;
using Components;
using ViewModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace Commands
{

    [CreateAssetMenu(fileName = "RoundCmdFactory", menuName = "Factory/RoundCmdFactory", order = 0)]
    public class RoundCmdFactory : ScriptableObject 
    {
        public StartRoundCmd StartRound(RouletteManager rouletteManager, RoundManager roundManager,TableManager tableManager)
        {
            return new StartRoundCmd(rouletteManager, roundManager, tableManager);
        }
        
        public CancelRoundCmd CancelRound(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
        {
            return new CancelRoundCmd(rouletteManager, roundManager, tableManager);
        }

        public PaymentRoundCmd PaymentRound(RoundManager roundManager, TableManager tableManager)
        {
            return new PaymentRoundCmd(roundManager, tableManager);
        }
        
        public ResetRoundCmd ResetRound(RouletteManager rouletteManager, RoundManager roundManager, TableManager tableManager)
        {
            return new ResetRoundCmd(rouletteManager, roundManager, tableManager);
        }
    }
}