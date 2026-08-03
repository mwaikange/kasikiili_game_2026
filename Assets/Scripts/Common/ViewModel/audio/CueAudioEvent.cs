using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "MultipleAudioEvent", menuName = "Event/Audio/Cue", order = 0)]
    public class CueAudioEvent : AbstractAudio
    {
        public AudioTypeOf audioTypeOf;
        public AudioClip[] clips;
        public float delayBetween;
        public AudioConfiguration configuration;
        
        public override void Play(AudioSource audioSource)
        {
            if(audioSource == null)
                return;
            
            configuration.ApplyTo(audioSource);

            Observable.FromCoroutine<Unit>(observer => PlayImagesEvent(observer, audioSource))
                .Subscribe()
                .AddTo(audioSource.gameObject);
        }
        public override AudioTypeOf GetTypeOf()
        {
            return audioTypeOf;
        }

        public override AudioClip GetClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }

        public override void SetVolume(float volume)
        {
            configuration.volume = volume;
        }
        private IEnumerator PlayImagesEvent(IObserver<Unit> observer, AudioSource audioSource)
        {
            foreach (AudioClip clip in clips)
            {
                if(audioSource == null)
                    break;
            
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(clip.length + delayBetween);
            }  
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }

        public override void Stop(AudioSource audioSource)
        {
            if(audioSource == null)
                return;
                
            audioSource.Stop();
        }
    }
}