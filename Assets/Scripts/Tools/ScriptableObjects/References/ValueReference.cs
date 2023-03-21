using System;
using UnityEngine;

namespace Tools.ScriptableObjects.References
{
    [Serializable]
    public abstract class ValueReference<T> : ScriptableObject
    {
        [SerializeField]
        private T value;

        public Action onValueChanged;
        
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                onValueChanged?.Invoke();
            }
        }
    }
}