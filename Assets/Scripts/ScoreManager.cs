using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreLabel;
    public int projectileCount = 0;
    public TextMeshProUGUI projectileLabel;

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
                AddScore(target.points);
                Debug.Log($"{target.targetName} +{target.points}c");
                Destroy(destroyableGameObject);
            },
            "Projectile" => () =>
            {
                AddProjectile();
                Debug.Log($"{destroyableGameObject.GetComponent<Projectile>().projectileName} +1p");
                Destroy(destroyableGameObject);
            },
            _ => () => Debug.LogError($"Action for {destroyableGameObject.name}" +
                                      $"({destroyableGameObject.tag}) is not assigned")
        };
    }

    public void AddScore(int additionalScore)
    {
        score += additionalScore;
        scoreLabel.text = score.ToString();
    }
    
    public void AddProjectile(int additionalProjectileCount = 1)
    {
        projectileCount += additionalProjectileCount;
        projectileLabel.text = projectileCount.ToString();
    }

    public int GetProjectile(int takingProjectileCount = 1)
    {
        int returnCount;
        if (projectileCount - takingProjectileCount < 0)
        {
            returnCount = projectileCount;
            projectileCount = 0;
        }
        else
        {
            returnCount = takingProjectileCount;
            projectileCount -= takingProjectileCount;
        }

        return returnCount;
    }
}
