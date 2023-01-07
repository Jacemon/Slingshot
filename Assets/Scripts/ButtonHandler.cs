using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void SpawnRock()
    {
        ProjectileManager.SpawnProjectile("Rock");
    }
}
