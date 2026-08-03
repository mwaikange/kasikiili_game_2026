using System;
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
    public class MoneyStartInput : MonoBehaviour
    {
        public MoneyCmdFactory moneyCmdFactory;
        public RouletteManager rouletteManager;
        public TableManager tableManager;
        
        private void Awake()
        {
            rouletteManager.OnStart
                .Subscribe(OnRouletteStart)
                .AddTo(this);
        }

        private void OnRouletteStart(bool isMoney)
        {
            moneyCmdFactory.ObtainPlayerCredit(tableManager).Execute();
        }
    }
}