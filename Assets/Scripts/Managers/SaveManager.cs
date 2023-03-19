using System.Collections.Generic;
using Tools.ScriptableObjects;
using Tools.ScriptableObjects.References;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public List<IntReference> intReferences;
        public List<LongReference> longReferences;
        public List<FloatReference> floatReferences;
        public List<BoolReference> boolReferences;
        public List<StringReference> stringReferences;

        private void OnEnable()
        {
            Debug.Log("Start loading...");
            Load();
            Debug.Log("End loading...");
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                return;
            }
            OnDisable();
        }
        
        private void OnDisable()
        {
            Debug.Log("Start saving...");
            Save();
            Debug.Log("End saving...");
        }

        private void Save()
        {
            foreach (var intReference in intReferences)
            {
                PlayerPrefs.SetInt(intReference.name, intReference.Value);
                Debug.Log($"{intReference.name} saving: {intReference.Value}");
            }
            foreach (var longReference in longReferences)
            {
                PlayerPrefs.SetString(longReference.name, longReference.Value.ToString());
                Debug.Log($"{longReference.name} saving: {longReference.Value}");
            }
            foreach (var floatReference in floatReferences)
            {
                PlayerPrefs.SetFloat(floatReference.name, floatReference.Value);
                Debug.Log($"{floatReference.name} saving: {floatReference.Value}");
            }
            foreach (var boolReference in boolReferences)
            {
                PlayerPrefs.SetInt(boolReference.name, boolReference.Value ? 1 : 0);
                Debug.Log($"{boolReference.name} saving: {boolReference.Value}");
            }
            foreach (var stringReference in stringReferences)
            {
                PlayerPrefs.SetString(stringReference.name, stringReference.Value);
                Debug.Log($"{stringReference.name} saving: {stringReference.Value}");
            }
            PlayerPrefs.Save();
        }
        
        private void Load()
        {
            foreach (var intReference in intReferences)
            {
                intReference.Value = PlayerPrefs.GetInt(intReference.name);
                Debug.Log($"{intReference.name} loading: {intReference.Value}");
            }
            foreach (var longReference in longReferences)
            {
                longReference.Value =
                    long.TryParse(PlayerPrefs.GetString(longReference.name), out var value) ? value : 0;
                Debug.Log($"{longReference.name} loading: {longReference.Value}");
            }
            foreach (var floatReference in floatReferences)
            {
                floatReference.Value = PlayerPrefs.GetFloat(floatReference.name);
                Debug.Log($"{floatReference.name} loading: {floatReference.Value}");
            }
            foreach (var boolReference in boolReferences)
            {
                boolReference.Value = PlayerPrefs.GetInt(boolReference.name) != 0;
                Debug.Log($"{boolReference.name} loading: {boolReference.Value}");
            }
            foreach (var stringReference in stringReferences)
            {  
                stringReference.Value = PlayerPrefs.GetString(stringReference.name);
                Debug.Log($"{stringReference.name} loading: {stringReference.Value}");
            }
        }
    }
}