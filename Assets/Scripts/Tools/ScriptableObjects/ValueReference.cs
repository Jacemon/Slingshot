using System;
using UnityEngine;

namespace Tools.ScriptableObjects
{
    public class ValueReference<T> : ScriptableObject
    {
        [SerializeField]
        private T value;

        public Action onValueChanged;
        
        public T Value
        {
            get => value;
            set
            {
                onValueChanged?.Invoke();
                this.value = value;
            }
        }
    }
}