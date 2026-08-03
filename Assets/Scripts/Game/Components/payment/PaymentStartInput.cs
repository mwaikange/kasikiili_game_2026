using System;
using System.Collections;
using System.Collections.Generic;
using Commands;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Components
{
    public class PaymentStartInput : MonoBehaviour
    {
        public RoundCmdFactory roundCmdFactory;
        public RoundManager roundManager;
        public TableManager tableManager;
        
        private void Awake()
        {
            roundManager.OnPayment
                .Subscribe(OnPaymentStarter)
                .AddTo(this);
        }

        private void OnPaymentStarter(int credits)
        {
            roundCmdFactory.PaymentRound(roundManager, tableManager).Execute();
        }
    }
}
