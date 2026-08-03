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
    public class RoundButtonDisplay : MonoBehaviour
    {
        public RouletteManager rouletteManager;
        public Button buttonObj;

        public void Awake()
        {
            rouletteManager.gameActive
                .Subscribe(OnGameActive)
                .AddTo(this);
        }

        private void OnGameActive(bool isActive)
        {
            buttonObj.interactable = isActive;   
        }
    }
}