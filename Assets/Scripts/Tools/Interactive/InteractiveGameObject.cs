using UnityEngine;

namespace Tools.Interactive
{
    public class InteractiveGameObject : MonoBehaviour, IInteractive

    {
        public virtual void SetInteractive(bool interactive) { }
    }
}