using Managers;
using UnityEngine;

namespace Entities.Targets
{
    public class BossTarget : Target
    {
        private static readonly int Stage = Animator.StringToHash("Stage");

        protected override void CheckHealth()
        {
            healthBar.Health = health;
            // TODO: just change animator's animations
            if (health <= 0)
            {
                MoneyManager.DepositMoney(money);
                
                Debug.Log($"Boss {name} defeated");
            } 
            else if (health <= maxHealth * 0.5f)
            {
                _animator.SetInteger(Stage, 2);
                
                Debug.Log($"Boss {name} -> stage 2");
            }
            else
            {
                _animator.SetInteger(Stage, 1);
            
                Debug.Log($"Boss {name} -> stage 1");
            }
        }
    }
}