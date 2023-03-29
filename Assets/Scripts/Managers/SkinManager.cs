using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Tools.ScriptableObjects.References;
using Tools.ScriptableObjects.Slingshot;
using Tools.ScriptableObjects.Slingshot.SlingshotSkins;
using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        public SlingshotDisplay slingshot;
        [Space]
        [SerializedDictionary("Slingshot skin", "Bool reference")]
        public SerializedDictionary<BaseSlingshotSkin, BoolReference> slingshotSkins;
        public StringReference currentSkin;

        private void Awake()
        {
            PutOnSkin(currentSkin.Value);
        }

        private void OnEnable()
        {
            foreach (var slingshotSkin in slingshotSkins)
            {
                slingshotSkin.Value.OnValueChanged += () => PutOnSkin(slingshotSkin.Key.slingshotName);
            }
        }
        
        private void OnDisable()
        {
            foreach (var slingshotSkin in slingshotSkins)
            {
                slingshotSkin.Value.OnValueChanged -= () => PutOnSkin(slingshotSkin.Key.slingshotName);
            }
        }

        private void PutOnSkin(string skinName)
        {
            var slingshotSkin = slingshotSkins.FirstOrDefault(slingshotSkin =>
                slingshotSkin.Key.slingshotName == skinName && slingshotSkin.Value.Value);
            if (slingshotSkin.Key != null)
            {
                slingshot.baseSlingshotSkin = slingshotSkin.Key;
                slingshot.ReloadData();
                
                currentSkin.Value = slingshot.baseSlingshotSkin.slingshotName;
                Debug.Log($"Skin {skinName} was set");
            }
            else
            {
                Debug.Log($"Skin {skinName} was not set");
            }
        }
    }
}