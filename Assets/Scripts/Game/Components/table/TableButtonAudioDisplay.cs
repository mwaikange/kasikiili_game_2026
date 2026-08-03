using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewModel;

namespace Components
{
    public class TableButtonAudioDisplay : MonoBehaviour
    {
        public TableManager tableManager;
        public TableButton tableButton;
        
        public AudioManager audioManager;
        public AudioSource source;
        public AbstractAudio fxAudio;

        void Awake()
        {
            if (tableButton == null)
            {
                Debug.LogWarning("Table Button Not Found!");
                return;
            }

            tableButton.isSelected
                .Subscribe(OnButtonSelect)
                .AddTo(this);
        }

        private void OnButtonSelect(bool selected)
        {
            if (!selected) return;
            audioManager.Play(fxAudio, source);
        }
    }
}