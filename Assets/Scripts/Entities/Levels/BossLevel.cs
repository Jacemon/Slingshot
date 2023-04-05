using System.Collections.Generic;
using Entities.Levels.Generators;
using Entities.Targets;
using Tools;
using UnityEngine;

namespace Entities.Levels
{
    [DisallowMultipleComponent]
    public class BossLevel : Level
    {
        public GameObject boss;
        public Vector2 spawnPoint;
        public IntHealthBar bossHealthBar;
        
        private BossTarget _bossTarget;

        private void OnEnable()
        {
            _bossTarget.OnHealthChanged += Reload;
        }

        private void OnDisable()
        {
            _bossTarget.OnHealthChanged -= Reload;
        }

        public override void Generate()
        {
            if (boss.TryGetComponent(out BossTarget bossTarget))
            {
                bossTarget.healthBar = bossHealthBar;
            }
            
            var pointGenerator = new PointGenerator()
            {
                parent = transform,
                points = new List<Vector2> { spawnPoint },
                randomTargets = new List<GameObject> { boss },
                level = levelNumber
            };
            generators.Add(pointGenerator);
            generators[0].Generate();

            Reload();
        }

        public void Reload()
        {
            _bossTarget = (BossTarget)((PointGenerator)generators[0]).generatedTargets[0];
        }
    }
}