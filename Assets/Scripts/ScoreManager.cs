using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int projectileCount = 0;

    public void AddDestroyableGameObject(GameObject destroyableGameObject)
    {
        var destroyable = destroyableGameObject.GetComponent<Destroyable>();
        if (destroyable != null)
        {
            destroyable.OnDestroy += GetAdder(destroyableGameObject);
            Debug.Log($"{destroyableGameObject.name} is registered to destroy");
        }
    }

    private Action GetAdder(GameObject destroyableGameObject)
    {
        return destroyableGameObject.tag switch
        {
            "Target" => () =>
            {
                var target = destroyableGameObject.GetComponent<Target>();
                score += target.points;
                Debug.Log($"{target.targetName} + {target.points}");
                Destroy(destroyableGameObject);
            },
            "Thrown Projectile" => () =>
            {
                projectileCount++;
                Debug.Log($"{destroyableGameObject.GetComponent<Projectile>().projectileName} + 1");
                Destroy(destroyableGameObject);
            },
            _ => () => Debug.LogError($"Action for {destroyableGameObject.name}" +
                                      $"({destroyableGameObject.tag}) is not assigned")
        };
    }
}
