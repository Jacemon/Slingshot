using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] targets;
    
    public int amount;
    public Vector2 centerPoint;
    public float spawnRadius;
    public float spaceBetween;
    
    [SerializeField]
    private List<Vector2> existingCoordinates;
    [SerializeField] 
    private int maxTries = 100;
    
    private void Awake()
    {
        GenerateTargets();
    }
    
    public void GenerateTargets()
    {
        // Расчёт координат
        int remainingAmount = amount;
        Vector2 newCoordinate;

        int remainingTries = maxTries;
        while (remainingAmount != 0 && remainingTries > 0) {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float distance = Random.Range(0, spawnRadius);
            newCoordinate = centerPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

            remainingTries--;
            if (!existingCoordinates.TrueForAll(c => Vector2.Distance(c, newCoordinate) > spaceBetween))
            {
                continue;
            }

            remainingTries = maxTries;
            remainingAmount--;
            existingCoordinates.Add(newCoordinate);
        }
        
        // Создание мишеней
        existingCoordinates.ForEach(coordinate =>
        {
            Instantiate(targets[Random.Range(0, targets.Length)],
                coordinate, Quaternion.identity).transform.SetParent(transform);
        });
    }
}
