using System;
using System.Collections.Generic;
using DG.Tweening;
using Entities.Targets;
using Tools.Interfaces;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Entities.Levels.Generators
{
    [Serializable]
    public abstract class BaseGenerator : IGenerator
    {
        [Header("Base settings")]
        public List<Target> randomTargets;
        public Transform parent;
        public bool autoRegenerate = true;
        public float regenerateTime;
        [HideInInspector]
        public List<Target> generatedTargets = new();
        [Header("Target settings")]
        [HideInInspector]
        public int level;
        [Space]
        public MinMaxCurve minMaxScale = new(1);

        public Action OnGenerated;

        private Tween _regenerator;
        
        public void StartGenerate()
        {
            Generate();
            
            if (autoRegenerate)
            {
                RegenerateSubscription();
            }
        }

        protected abstract void Generate();

        protected virtual void RegenerateSubscription()
        {
            foreach (var target in generatedTargets)
            {
                target.OnHealthChanged += () =>
                {
                    if (target.health > 0) return;
                    generatedTargets.Remove(target);
                    if (generatedTargets.Count == 0)
                    {
                        _regenerator = DOVirtual.DelayedCall(regenerateTime, () =>
                        {
                            StartGenerate();
                            OnGenerated?.Invoke();
                        });
                    }
                };
            }
        }

        public void StopGenerate()
        {
            _regenerator?.Kill();
        }
        
        public virtual void DrawGizmos() { }
    }
}