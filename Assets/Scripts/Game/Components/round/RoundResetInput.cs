using System.Collections;
using System.Collections.Generic;
using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewModel;

namespace Components
{
    public class RoundResetInput : MonoBehaviour
    {
        public RoundCmdFactory roundCmdFactory;
        public RouletteManager rouletteManager;
        public RoundManager roundManager;
        public TableManager tableManager;
        
        private void Awake()
        {
            roundManager.OnReset
                .Subscribe(OnPaymentStarter)
                .AddTo(this);
        }
        
        private void OnPaymentStarter(bool reset)
        {
            roundCmdFactory.ResetRound(rouletteManager, roundManager, tableManager).Execute();
        }
    }
}