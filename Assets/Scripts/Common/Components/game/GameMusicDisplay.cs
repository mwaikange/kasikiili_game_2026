using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class GameMusicDisplay : MonoBehaviour
    {
        public AudioManager audioManager;
        public AudioSource audioSource;
        
        void Start()
        {
            audioManager.PlayMasterMusic(audioSource);
            audioManager.masterVolume
                .Subscribe(OnMasterVolumeChange)
                .AddTo(this);
        }

        void OnMasterVolumeChange(float value)
        {
            audioSource.volume = value;
        }
    }
}