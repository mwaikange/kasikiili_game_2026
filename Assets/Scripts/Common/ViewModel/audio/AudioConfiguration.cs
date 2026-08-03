using UniRx;
using UnityEngine;

namespace ViewModel
{
    [System.Serializable]
    public class AudioConfiguration
    {
        public bool loop = false;
        public bool mute = false;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(-3f, 3f)] public float pitchMin = 1f;
        [Range(-3f, 3f)] public float pitchMax = 1f;
        [Range(-1f, 1f)] public float panStereo = 0f;
        [Range(0f, 1.1f)] public float reverbZoneMix = 1f;
		
        public void ApplyTo(AudioSource audioSource)
        {
            audioSource.loop = this.loop;
            audioSource.mute = this.mute;
            audioSource.volume = this.volume;
            audioSource.pitch = Random.Range(this.pitchMin, this.pitchMax);
            audioSource.panStereo = this.panStereo;
            audioSource.reverbZoneMix = this.reverbZoneMix;
        }
    }
}