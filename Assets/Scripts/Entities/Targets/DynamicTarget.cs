using Tools.Follower;
using UnityEngine;

namespace Entities.Targets
{
    [RequireComponent(typeof(PathFollower))]
    public class DynamicTarget : Target
    {
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
            _pathFollower.onMovingLeft += MoveLeft;
            _pathFollower.onMovingRight += MoveRight;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnHealthChanged -= CheckPathFollower;
            _pathFollower.onMovingLeft -= MoveLeft;
            _pathFollower.onMovingRight -= MoveRight;
        }

        private void MoveLeft()
        {
            var newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
        
        private void MoveRight()
        {
            var newScale = transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
        
        private void CheckPathFollower()
        {
            _pathFollower.enabled = health > 0;
        }
    }
}