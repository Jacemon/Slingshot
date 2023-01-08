using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void SpawnRock()
    {
        ProjectileManager.SpawnProjectile("Rock");
    }
}
