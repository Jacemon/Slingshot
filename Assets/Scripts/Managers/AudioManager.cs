using Tools.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour, ISavable
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
        
        private void Awake()
        {
            GlobalEventManager.onLoad += LoadData;
            GlobalEventManager.onSave += SaveData;
        }

        private void OnDestroy()
        {
            GlobalEventManager.onLoad -= LoadData;
            GlobalEventManager.onSave -= SaveData;
        }
        
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

        public void SaveData()
        {
            Debug.Log("AudioManager saving");
            //PlayerPrefs.SetInt("master", masterVolumeSwitch ? 1 : 0);
            PlayerPrefs.SetInt("music", musicVolumeSwitch ? 1 : 0);
            PlayerPrefs.SetInt("effects", effectsVolumeSwitch ? 1 : 0);
        }

        public void LoadData()
        {
            //masterVolumeSwitch = PlayerPrefs.GetInt("master") != 0;
            musicVolumeSwitch = PlayerPrefs.GetInt("music") != 0;
            effectsVolumeSwitch = PlayerPrefs.GetInt("effects") != 0;
            Debug.Log("AudioManager was loaded");
        }

        public void ReloadData() { }
    }
}