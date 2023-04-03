using DG.Tweening;
using Tools.Follower;
using UnityEngine;

namespace Entities.Targets
{
    [RequireComponent(typeof(PathFollower))]
    public class DynamicTarget : Target
    {
        [Header("Dynamic settings")] 
        public Transform flipTransform;
        private PathFollower _pathFollower;
        
        protected override void Awake()
        {
            base.Awake();
            _pathFollower = GetComponent<PathFollower>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnHealthChanged += CheckPathFollower;
            _pathFollower.OnMovingLeft += MoveLeft;
            _pathFollower.OnMovingRight += MoveRight;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnHealthChanged -= CheckPathFollower;
            _pathFollower.OnMovingLeft -= MoveLeft;
            _pathFollower.OnMovingRight -= MoveRight;
        }

        private void MoveLeft()
        {
            var newScale = flipTransform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            flipTransform.localScale = newScale;
        }
        
        private void MoveRight()
        {
            var newScale = flipTransform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            flipTransform.localScale = newScale;
        }
        
        private void CheckPathFollower()
        {
            _pathFollower.enabled = health > 0;
        }
    }
}