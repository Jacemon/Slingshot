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

        protected virtual void Awake()
        {
            Generate();
        }

        public virtual void Generate()
        {
            generators?.ForEach(g =>
            {
                ((BaseGenerator)g).level = levelNumber;
                g.Generate();
            });
        }

        private void OnDrawGizmosSelected()
        {
            generators?.ForEach(g => ((BaseGenerator)g).DrawGizmos());
        }
    }
}