
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
    public class ProbabilityJackpotAudioDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public AudioManager audioManager;
        public AudioSource audioSource;
        public AbstractAudio fxAudio;

        private void Awake()
        {
            roundManager.OnJackpot
                .Subscribe(OnWinNumberReceived)
                .AddTo(this);
        }

        private void OnWinNumberReceived(int value)
        {
            if (roundManager.probabilityNumber.Value != 1000 && roundManager.probabilityNumber.Value != 2000) return;
            audioManager.Play(fxAudio, audioSource);
        }
    }
}
