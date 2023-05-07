using System;
using UnityEngine;

namespace Tools.ScriptableObjects.References
{
    [Serializable]
    public abstract class ValueReference<T> : ScriptableObject
    {
        [SerializeField]
        private T value;

        public Action OnValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke();
            }
        }
    }
}