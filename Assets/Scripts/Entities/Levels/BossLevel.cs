using System.Collections.Generic;
using Entities.Levels.Generators;
using Entities.Targets;
using Entities.Targets.Bosses;
using Tools;
using UnityEngine;

namespace Entities.Levels
{
    [DisallowMultipleComponent]
    public class BossLevel : Level
    {
        public BossTarget boss;
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
            boss.healthBar = bossHealthBar;
            
            var pointGenerator = new PointGenerator()
            {
                parent = transform,
                points = new List<Vector2> { spawnPoint },
                randomTargets = new List<Target> { boss },
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