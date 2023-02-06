using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public GameObject[] targets;
    
    public int amount;
    public Vector2 centerPoint;
    public float radius;
    
    [SerializeField]
    private List<Vector2> existingCoordinates;

    private void Awake()
    {
        GenerateTargets();
    }

    // todo Доделать, когда мишени будут относиться к уровню
    public void GenerateTargets()
    {
        // Расчёт координат
        int remainingAmount = amount;
        Vector2 newCoordinate;
        
        while (remainingAmount != 0) {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float distance = Random.Range(0, radius);
            newCoordinate = centerPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > 2 * radius))
            {
                continue;
            }
            remainingAmount--;
            existingCoordinates.Add(newCoordinate);
        }
        
        // Создание мишеней
        existingCoordinates.ForEach(coordinate => Instantiate(targets[Random.Range(0, targets.Length - 1)], 
            coordinate, Quaternion.identity));
    }
}
