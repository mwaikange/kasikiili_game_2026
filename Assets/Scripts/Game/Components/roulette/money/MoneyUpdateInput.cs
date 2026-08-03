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
    public class MoneyUpdateInput : MonoBehaviour
    {
        public MoneyCmdFactory moneyCmdFactory;
        public RouletteManager rouletteManager;
        public TableManager tableManager;
        
        private void Awake()
        {
            rouletteManager.OnRefresh
                .Subscribe(OnRouletteRefresh)
                .AddTo(this);
        }

        private void OnRouletteRefresh(bool isMoney)
        {
            moneyCmdFactory.TurnPlayerCredit(tableManager).Execute();
        }
    }
}