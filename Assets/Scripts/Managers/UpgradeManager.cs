using UnityEngine;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        public ProjectileManager projectileManager;

        public void UpgradeProjectiles()
        {
            projectileManager.projectileLevel++;
        }

        public void UpgradeSlingshot()
        {
            
        }
    }
}