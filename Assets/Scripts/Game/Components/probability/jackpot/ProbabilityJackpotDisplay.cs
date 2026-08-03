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
    public class ProbabilityJackpotDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public GameObject effect;

        private void Awake()
        {
            effect.SetActive(false);
            roundManager.OnJackpot
                .Subscribe(OnWinNumberReceived)
                .AddTo(this);
        }

        private void OnWinNumberReceived(int value)
        {
            if (roundManager.probabilityNumber.Value != 1000 && roundManager.probabilityNumber.Value != 2000) return;
            effect.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(Effect());
        }

        IEnumerator Effect()
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(roundManager.probabilityDuration * 3);
            effect.SetActive(false);
        }
    }
}
