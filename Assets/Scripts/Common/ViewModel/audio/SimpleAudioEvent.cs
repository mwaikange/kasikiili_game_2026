using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "SimpleAudioEvent", menuName = "Event/Audio/Simple", order = 0)]
    public class SimpleAudioEvent : AbstractAudio 
    {
        public AudioTypeOf audioTypeOf;
        public AudioClip[] clips;
        public AudioConfiguration configuration;

        public override void Play(AudioSource audioSource)
        {
            if(clips.Length == 0 || audioSource == null)
                return;

            configuration.ApplyTo(audioSource);
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
        public override AudioTypeOf GetTypeOf()
        {
            return audioTypeOf;
        }
        public override void SetVolume(float volume)
        {
            configuration.volume = volume;
        }
        public override AudioClip GetClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }
        public override void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}