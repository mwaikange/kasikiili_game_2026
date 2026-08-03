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
    public class ProbabilityAudioDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public AudioManager audioManager;
        public AudioSource audioSource;
        public AbstractAudio fxAudio;

        private void Awake()
        {
            roundManager.OnProbability
                .Subscribe(OnWinNumberReceived)
                .AddTo(this);
        }

        private void OnWinNumberReceived(int value)
        {
            audioManager.Play(fxAudio, audioSource);
        }
    }
}
