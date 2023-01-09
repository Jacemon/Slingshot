using UnityEngine;

[RequireComponent(typeof(ScoreManager))]
public class TargetManager : MonoBehaviour
{
    private ScoreManager _scoreManager;
    
    private void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        foreach (var target in targets)
        {
            _scoreManager.AddDestroyableGameObject(target);
        }
    }
}