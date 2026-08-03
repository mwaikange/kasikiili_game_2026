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
    public class DebugTubInput : MonoBehaviour
    {
        public RoundManager roundManager;

        private static readonly float[] Values = new float[]
        {
            999,
            5000,
            20000,
            50000,
            100000,
            180000,
            2000001
        };
        
        void Awake()
        {
            var tub = Values[0];
            roundManager.probabilityTub.Value = tub;
        }
        
        public void ChangeLetter(int number)
        {
            var tub = Values[number];
            roundManager.probabilityTub.Value = tub;
        }
    }
}