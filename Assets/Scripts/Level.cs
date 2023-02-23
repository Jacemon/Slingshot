using Managers;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int levelNumber;
    [Space] 
    [Header("Target generator settings")]
    public GameObject[] targets;
    public int targetsAmount;
    public Vector2 spawnPoint;
    public float spawnRadius;
    public float spawnSecondRadius;
    public float spaceBetween;

    private void Awake()
    {
        TargetManager.GenerateTargetsByEllipse(targets, targetsAmount, levelNumber, 
            spawnPoint, spawnRadius, spawnSecondRadius, spaceBetween, transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector2 yUp, yDown, xLeft, xRight;
        yUp = yDown = xLeft = xRight = spawnPoint;
        yUp.y += spawnSecondRadius;
        yDown.y -= spawnSecondRadius;
        xRight.x += spawnRadius;
        xLeft.x -= spawnRadius;
        Gizmos.DrawLine(yUp, yDown);
        Gizmos.DrawLine(xRight, xLeft);
    }
}
