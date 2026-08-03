using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class PaymentCreditDisplay : MonoBehaviour
    {
        public TableManager tableMoney;
        [Header("Counting")]
        public TextMeshProUGUI numberLabel;
        public int countFps = 30;
        public float duration = 1f;
        public string format = "N0";
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                UpdateText(value);
                _value = value;
            }
        }
        private Coroutine _coroutine;

        private void Awake()
        {
            tableMoney.cashManager
                .currentCredit
                .Subscribe(OnBetChange)
                .AddTo(this);
        }
        
        private void UpdateText(int newValue)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
    
            _coroutine = StartCoroutine(CountText(newValue));
        }
        
         private IEnumerator CountText(int newValue)
         {
             WaitForSeconds Wait = new WaitForSeconds(1f / countFps);
             int previousValue = _value;
             int stepAmount;
     
             if (newValue - previousValue < 0)
             {
                 stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFps * duration)); // newValue = -20, previousValue = 0. CountFPS = 30, and Duration = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
             }
             else
             {
                 stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFps * duration)); // newValue = 20, previousValue = 0. CountFPS = 30, and Duration = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
             }
     
             if (previousValue < newValue)
             {
                 while(previousValue < newValue)
                 {
                     previousValue += stepAmount;
                     if (previousValue > newValue)
                     {
                         previousValue = newValue;
                     }
     
                     numberLabel.SetText(previousValue.ToString(format));
     
                     yield return Wait;
                 }
             }
             else
             {
                 while (previousValue > newValue)
                 {
                     previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                     if (previousValue < newValue)
                     {
                         previousValue = newValue;
                     }
     
                     numberLabel.SetText(previousValue.ToString(format));
     
                     yield return Wait;
                 }
             }
         }

        private void OnBetChange(int value)
        {
            Value = value;
        }
    }
}
