using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixerGroup mixer;
        [Space]
        [Range(0, 1)]
        public float masterVolume;
        [Range(0, 1)]
        public float musicVolume;
        [Range(0, 1)]
        public float effectsVolume;
        
        public bool masterVolumeSwitch = true;
        public bool musicVolumeSwitch = true;
        public bool effectsVolumeSwitch = true;
        
        private float _masterVolumeTemp;
        private float _musicVolumeTemp;
        private float _effectsVolumeTemp;

        private const float MinVolume = -80;
        private const float MaxVolume = 0;
        
        private void Update()
        {
            mixer.audioMixer.SetFloat("MasterVolume", PercentToVolume(masterVolumeSwitch ? masterVolume : 0));
            mixer.audioMixer.SetFloat("MusicVolume", PercentToVolume(musicVolumeSwitch ? musicVolume : 0));
            mixer.audioMixer.SetFloat("EffectsVolume", PercentToVolume(effectsVolumeSwitch ? effectsVolume : 0));
        }

        private static float PercentToVolume(float percent)
        {
            return MinVolume + (MaxVolume - MinVolume) * percent;
        }
    }
}