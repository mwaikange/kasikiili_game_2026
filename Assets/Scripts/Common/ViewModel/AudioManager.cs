using System;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using ViewModel;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "AudioManager", menuName = "Global/Audio Manager", order = 0)]
    public class AudioManager : ScriptableObject
    {
        [Header("Audio Global")] 
        public float masterDefault;
        public FloatReactiveProperty masterVolume = new FloatReactiveProperty();
        public AbstractAudio masterMusic;
    
        [Header("Audio control")]
        public BoolReactiveProperty musicState = new BoolReactiveProperty();
        public BoolReactiveProperty fxState = new BoolReactiveProperty();
    
        public void Play(AbstractAudio abstractAudio, AudioSource source)
        {
            switch (abstractAudio.GetTypeOf())
            {
                case AudioTypeOf.Fx:
                    if (this.fxState.Value) abstractAudio.Play(source);
                    break;
                case AudioTypeOf.Music:
                    if (this.musicState.Value) abstractAudio.Play(source);
                    break;
                default:
                    abstractAudio.Play(source);
                    break;
            }
        }
    
        public void PlayMasterMusic(AudioSource audioSource)
        {
            masterMusic.SetVolume(this.masterVolume.Value);
            masterMusic.Play(audioSource);
        }
    }
    
    public enum AudioTypeOf
    {
        Fx,
        Music
    }
}