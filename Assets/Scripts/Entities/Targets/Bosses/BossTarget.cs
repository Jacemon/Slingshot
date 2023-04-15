using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Targets.Bosses
{
    public abstract class BossTarget : Target //TODO: abstract
    {
        public List<Stage> stages;
        [ReadOnlyInspector]
        public Stage currentStage;

        public Action OnStageChanged;
        
        [Serializable]
        public class Stage
        {
            [Range(0, 1)]
            public float startHealthPercent;
            public UnityEvent stageAction;

            #region Equality

            public static bool operator ==(Stage stage1,Stage stage2)
            {
                return stage1 != null && stage1.Equals(stage2);
            }

            public static bool operator !=(Stage stage1, Stage stage2)
            {
                return !(stage1 == stage2);
            }
            
            protected bool Equals(Stage other)
            {
                return startHealthPercent.Equals(other.startHealthPercent);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof(Stage)) return false;
                return Equals((Stage)obj);
            }
            
            public override int GetHashCode()
            {
                return startHealthPercent.GetHashCode();
            }
            
            #endregion
        }

        protected override void CheckHealth()
        {
            healthBar.Health = health;

            for (var i = 0; i < stages.Count; i++)
            {
                if (stages[i].startHealthPercent >= (float)health / maxHealth) continue;
                ChangeStage(stages[i - 1]);
                break;
            }
        }

        private void ChangeStage(Stage stage)
        {
            if (currentStage.Equals(stage)) return;
            
            OnStageChanged?.Invoke();
            
            currentStage = stage;
            currentStage.stageAction?.Invoke();

            Debug.Log($"{targetName} Boss -> stage (<{currentStage.startHealthPercent})");
        }
    }
}