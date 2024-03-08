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

        private Tween _regenerator;
        
        public Action Generated;
        public Action NoTargetsRemaining;

        public void StartGenerate()
        {
            Generate();

            GenerateSubscription();
        }

        public void StopGenerate()
        {
            _regenerator?.Kill();
        }

        protected abstract void Generate();

        protected virtual void Regenerate()
        {
            _regenerator = DOVirtual.DelayedCall(regenerateTime, () =>
            {
                StartGenerate();
                Generated?.Invoke();
            });
        }
        
        protected virtual void GenerateSubscription()
        {
            foreach (var target in generatedTargets)
                target.OnHealthChanged += () =>
                {
                    if (target.health > 0) return;
                    generatedTargets.Remove(target);
                    
                    if (generatedTargets.Count == 0)
                    {
                        NoTargetsRemaining?.Invoke();
                        
                        if (autoRegenerate)
                        {
                            Regenerate();
                        }
                    }
                };
        }

        public virtual void DrawGizmos() { }
    }
}