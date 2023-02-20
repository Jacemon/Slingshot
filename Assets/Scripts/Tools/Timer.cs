﻿using UnityEngine;

namespace Tools
{
    public class Timer : MonoBehaviour
    {
        [Header("Settings")]
        public bool timerDone = true;
        public bool timerOn;
        public float delay;

        private void Update()
        {
            if (!timerOn) return;
            timerDone = false;
            delay -= Time.deltaTime;
        
            if (delay > 0) return;
            timerDone = true;
            timerOn = false;
        }

        public void SetBiggerDelay(float otherDelay)
        {
            if (delay < otherDelay)
            {
                delay = otherDelay;
            }
        }
    
        public void SetDelay(float newDelay)
        {
            delay = newDelay;
        }
    
        public void AddDelay(float additionalDelay)
        {
            delay += additionalDelay;
        }
    }
}