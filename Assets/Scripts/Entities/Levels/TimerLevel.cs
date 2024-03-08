using System;
using Entities.Levels.Generators;
using Tools;
using UnityEngine;

namespace Entities.Levels
{
    [RequireComponent(typeof(Timer))]
    public class TimerLevel : Level
    {
        public float timeToComplete = 30f;
        
        private Timer _timer;
        private int generatorsLeft;
        
        protected override void Awake()
        {
            base.Awake();

            _timer = GetComponent<Timer>();
        }
        
        private void Start()
        {
            _timer.delay = timeToComplete;

            generatorsLeft = generators.Count;
        }

        private void OnEnable()
        {
            generators?.ForEach(g =>
            {
                ((BaseGenerator)g).NoTargetsRemaining += WinLevel;
            });
            _timer.OnTimerDone += LoseLevel;
        }

        private void OnDisable()
        {
            generators?.ForEach(g =>
            {
                ((BaseGenerator)g).NoTargetsRemaining -= WinLevel;
            });
            _timer.OnTimerDone -= LoseLevel;
        }

        public override void StartLevel()
        {
            _timer.timerOn = true;
        }
        
        public override void  LoseLevel()
        {
            LevelComplete?.Invoke(false);
        }
        
        public override void WinLevel()
        {
            generatorsLeft--;
            if (generatorsLeft == 0)
            {
                _timer.timerOn = false;
                LevelComplete?.Invoke(true);
            }
        }
    }
}