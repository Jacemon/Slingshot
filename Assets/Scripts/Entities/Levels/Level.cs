using System;
using System.Collections.Generic;
using Entities.Levels.Generators;
using Tools.Interfaces;
using UnityEngine;

namespace Entities.Levels
{
    [DisallowMultipleComponent]
    public class Level : MonoBehaviour, IGenerator
    {
        public int levelNumber;
        [Header("Generator settings")]
        [SerializeReference]
        public List<IGenerator> generators = new();

        public Action<bool> LevelComplete;
        
        protected virtual void Awake()
        {
            StartGenerate();
        }

        protected virtual void OnDestroy()
        {
            StopGenerate();
        }

        public virtual void StartLevel() { }
        public virtual void WinLevel() { }
        public virtual void LoseLevel() { }
        
        private void OnDrawGizmosSelected()
        {
            generators?.ForEach(g => ((BaseGenerator)g).DrawGizmos());
        }

        public virtual void StartGenerate()
        {
            generators?.ForEach(g =>
            {
                ((BaseGenerator)g).level = levelNumber;
                g.StartGenerate();
            });
        }

        public virtual void StopGenerate()
        {
            generators?.ForEach(g => g.StopGenerate());
        }
    }
}