using System.Collections.Generic;
using Tools.ScriptableObjects.Slingshot;
using Tools.ScriptableObjects.Slingshot.SlingshotSkins;
using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        public SlingshotDisplay slingshot;
        [Space]
        public List<BaseSlingshotSkin> slingshotSkins;

        public void Awake()
        {
            slingshot.baseSlingshotSkin = slingshotSkins[Random.Range(0, slingshotSkins.Count)];
            slingshot.ReloadData();
            
            Debug.Log($"Skin {slingshot.baseSlingshotSkin.name} set");
        }
    }
}