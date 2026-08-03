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
    public class RoundAudioDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public AudioManager audioManager;
        public AbstractAudio fxAudio;
        public AudioSource audioSource;

        void Awake()
        {
            roundManager.OnClear
                .Subscribe(OnResetRound)
                .AddTo(this);
        }

        private void OnResetRound(bool clear)
        {
            if (!clear) return;
            audioManager.Play(fxAudio, audioSource);
        }
        
        public void Click()
        {
            audioManager.Play(fxAudio, audioSource);
        }
    }
}