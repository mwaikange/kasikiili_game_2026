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
    public class DebugWinInput : MonoBehaviour
    {
        public RoundManager roundManager;

        void Awake()
        {
            roundManager.winNumber.Value = Convert.ToInt32(0);
        }
        
        public void ChangeNumber(int number)
        {
            Debug.Log(Convert.ToInt32(number));
            roundManager.winNumber.Value = Convert.ToInt32(number);
        }
    }
}