using UnityEngine;

[RequireComponent(typeof(ScoreManager))]
public class TargetManager : MonoBehaviour
{
    private ScoreManager _scoreManager;
    
    private void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();

        foreach (var target in GameObject.FindGameObjectsWithTag("Target"))
        {
            _scoreManager.AddDestroyableGameObject(target);
        }
    }
}