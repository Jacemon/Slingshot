using Tools;
using UnityEngine;

namespace Entities.Levels
{
    public class Level : MonoBehaviour, IReloadable
    {
        [HideInInspector]
        public int levelNumber;
        
        public virtual void Reload() { }
    }
}