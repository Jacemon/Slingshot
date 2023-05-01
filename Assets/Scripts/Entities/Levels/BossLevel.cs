using System.Collections.Generic;
using Entities.Levels.Generators;
using Entities.Targets;
using Entities.Targets.Bosses;
using Tools;
using Tools.Interfaces;
using UnityEngine;

namespace Entities.Levels
{
    [DisallowMultipleComponent]
    public class BossLevel : Level, IReloadable
    {
        public BossTarget boss;
        public float respawnTime = 5;
        public Vector2 spawnPoint;
        public IntHealthBar bossHealthBar;
        
        private BossTarget _bossTarget;

        private PointGenerator _pointGenerator;

        protected override void Awake()
        {
            _pointGenerator = new PointGenerator
            {
                parent = transform,
                points = new List<Vector2> { spawnPoint },
                randomTargets = new List<Target> { boss },
                level = levelNumber,
                regenerateTime = respawnTime
            };
            
            base.Awake();
        }

        private void OnEnable()
        {
            _pointGenerator.OnGenerated += Reload;
        }

        private void OnDisable()
        {
            _pointGenerator.OnGenerated -= Reload;
        }

        public override void StartGenerate() // TODO: mb add _pointGenerator to generators?
        {
            boss.healthBar = bossHealthBar;
            
            _pointGenerator.StartGenerate();

            Reload();
        }
        
        public override void StopGenerate() { 
            _pointGenerator.StopGenerate();
        }

        public void Reload()
        {
            _bossTarget = (BossTarget)_pointGenerator.generatedTargets[0];
            _bossTarget.healthBar = bossHealthBar;
        }
    }
}