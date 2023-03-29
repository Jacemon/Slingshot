using System.Collections.Generic;
using System.ComponentModel;
using Entities.Levels.Generators;
using Tools.Interfaces;
using UnityEngine;

namespace Entities.Levels
{
    [DisallowMultipleComponent]
    public class Level : MonoBehaviour, IGenerator
    {
        public int levelNumber;
        [SerializeReference]
        public List<IGenerator> generators = new();

        private void Awake()
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