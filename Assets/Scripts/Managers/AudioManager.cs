using Tools.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixerGroup mixer;
        [Space]
        [Range(0, 1)]
        public float musicVolume;
        [Range(0, 1)]
        public float effectsVolume;
        
        public BoolReference musicVolumeSwitch;
        public BoolReference effectsVolumeSwitch;
        
        private float _masterVolumeTemp;
        private float _musicVolumeTemp;
        private float _effectsVolumeTemp;

        private const float MinVolume = -80;
        private const float MaxVolume = 0;
        
        private void Update()
        {
            mixer.audioMixer.SetFloat("MusicVolume", PercentToVolume(musicVolumeSwitch.Value ? musicVolume : 0));
            mixer.audioMixer.SetFloat("EffectsVolume", PercentToVolume(effectsVolumeSwitch.Value ? effectsVolume : 0));
        }

        private static float PercentToVolume(float percent)
        {
            return MinVolume + (MaxVolume - MinVolume) * percent;
        }
    }
}