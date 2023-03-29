using System;
using System.Collections.Generic;
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
        public List<GameObject> randomTargets;
        public Transform parent;
        public bool autoRegenerate = true;
        [HideInInspector]
        public List<Target> generatedTargets = new();
        [Header("Target settings")]
        public int level;
        [Space]
        public MinMaxCurve minMaxScale = new(1);

        public void Generate()
        {
            StartGenerate();
            
            if (autoRegenerate)
            {
                RegenerateSubscription();
            }
        }

        protected abstract void StartGenerate();

        protected virtual void RegenerateSubscription()
        {
            foreach (var target in generatedTargets)
            {
                target.OnHealthChanged += () =>
                {
                    if (target.health > 0) return;
                    generatedTargets.Remove(target);
                    if (generatedTargets.Count == 0) Generate();
                };
            }
        }
        
        public virtual void DrawGizmos() { }
    }
}