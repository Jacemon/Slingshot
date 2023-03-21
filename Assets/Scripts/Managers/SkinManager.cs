using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<SlingshotSkinBoolReference> slingshotSkins;
        public StringReference currentSkin;

        [Serializable]
        public class SlingshotSkinBoolReference
        {
            public BaseSlingshotSkin baseSlingshotSkin;
            public BoolReference boolReference;
        }
        
        private void Awake()
        {
            PutOnSkin(currentSkin.Value);
        }

        private void OnEnable()
        {
            foreach (var slingshotSkin in slingshotSkins)
            {
                slingshotSkin.boolReference.onValueChanged += () => PutOnSkin(slingshotSkin.baseSlingshotSkin.slingshotName);
            }
        }
        
        private void OnDisable()
        {
            foreach (var slingshotSkin in slingshotSkins)
            {
                slingshotSkin.boolReference.onValueChanged -= () => PutOnSkin(slingshotSkin.baseSlingshotSkin.slingshotName);
            }
        }

        private void PutOnSkin(string skinName)
        {
            var slingshotSkin = slingshotSkins.FirstOrDefault(slingshotSkin =>
                slingshotSkin.baseSlingshotSkin.slingshotName == skinName && slingshotSkin.boolReference.Value);
            if (slingshotSkin?.baseSlingshotSkin != null)
            {
                slingshot.baseSlingshotSkin = slingshotSkin.baseSlingshotSkin;
                slingshot.ReloadData();
                
                currentSkin.Value = slingshot.baseSlingshotSkin.name;
                Debug.Log($"Skin {slingshot.baseSlingshotSkin.name} was set");
            }
            else
            {
                Debug.Log("Skin was not set");
            }
        }
    }
}