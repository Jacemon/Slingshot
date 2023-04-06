using Managers;
using UnityEngine;

namespace Entities.Targets
{
    public class BossTarget : Target
    {
        [Header("Boss target stage settings")]
        [Range(0, 1), Tooltip("A value from 1 to 0, which means after what percentage of health the first stage begins")]
        public float firstStage = 1.0f;
        [Range(0, 1), Tooltip("A value from 1 to 0, which means after what percentage of health the second stage begins")]
        public float secondStage = 0.7f;
        [Range(0, 1), Tooltip("A value from 1 to 0, which means after what percentage of health the third stage begins")]
        public float thirdStage = 0.3f;
        
        private static readonly int Stage = Animator.StringToHash("Stage");

        protected override void CheckHealth()
        {
            healthBar.Health = health;
            if (health <= 0)
            {
                Animator.SetInteger(Stage, 4);
                
                MoneyManager.DepositMoney(money);
                gameObject.layer = LayerMask.NameToLayer("RearMiddle");
                
                LateDestroy();
                Debug.Log($"Boss {name} defeated");
            } 
            else if (health <= maxHealth * thirdStage)
            {
                Animator.SetInteger(Stage, 3);
                
                Debug.Log($"Boss {name} -> stage 3");
            }
            else if (health <= maxHealth * secondStage)
            {
                Animator.SetInteger(Stage, 2);
                
                Debug.Log($"Boss {name} -> stage 2");
            }
            else if (health <= maxHealth * firstStage)
            {
                Animator.SetInteger(Stage, 1);
            
                Debug.Log($"Boss {name} -> stage 1");
            }
            else
            {
                Debug.Log($"Boss {name} in calm");
            }
        }
    }
}