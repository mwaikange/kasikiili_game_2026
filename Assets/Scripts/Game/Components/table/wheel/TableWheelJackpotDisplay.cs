using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Components
{
    public class TableWheelJackpotDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public GameObject effect;
        
        private void Awake()
        {
            effect.SetActive(false);
            roundManager.OnJackpot
                .Subscribe(OnWinNumberReceived)
                .AddTo(this);
            roundManager.OnClear
                .Subscribe(OnClear)
                .AddTo(this);
        }
        
        private void OnClear(bool clear)
        {
            if(!clear) return;
            effect.SetActive(false);
        }

        private void OnWinNumberReceived(int value)
        {
            if (roundManager.probabilityNumber.Value != 1000 && roundManager.probabilityNumber.Value != 2000) return;
            StopAllCoroutines();
            StartCoroutine(DecreaseTime(roundManager.probabilityJackpotDuration));
        }

        private IEnumerator DecreaseTime(float totalTime)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(totalTime);
            effect.SetActive(false);
        }
    }
}