using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class IntHealthBar : MonoBehaviour
    {
        public Slider healthBarSlider;
        [Space]
        public Canvas healthBarCanvas;
        public bool alwaysVisible;

        public Action OnHealthChanged;

        private int _health;
        private int _maxHealth;
        
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value > 0 ? value : 0;
                
                healthBarSlider.maxValue = _maxHealth;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                if (value >= _maxHealth)
                {
                    _health = _maxHealth;
                    healthBarCanvas.enabled = alwaysVisible;
                } 
                else if (value <= 0)
                {
                    _health = 0;
                    healthBarCanvas.enabled = alwaysVisible;
                }
                else
                {
                    _health = value;
                    healthBarCanvas.enabled = true;
                }
                
                healthBarSlider.value = _health;
                
                OnHealthChanged?.Invoke();
            }
        }
    }
}