using TMPro;
using UnityEngine;

namespace Managers
{
    public class TargetManager : MonoBehaviour
    {
        public int score;
        public TextMeshProUGUI scoreLabel;

        private void Awake()
        {
            GlobalEventManager.OnTargetHitCart.AddListener(TargetHitCart);
            GlobalEventManager.OnTargetSpawned.AddListener(TargetSpawned);
        
            AddScore(0);
        }
    
        private void AddScore(int additionalScore)
        {
            score += additionalScore;
            scoreLabel.text = score.ToString();
        }
    
        private void TargetHitCart(Target target)
        {
            AddScore(target.points);
            Destroy(target.gameObject);
        }

        private void TargetSpawned(Target target)
        {
            Debug.Log($"{target.name} was spawned");
        }
    }
}