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
            _pathFollower.MovingLeft += MoveLeft;
            _pathFollower.MovingRight += MoveRight;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _pathFollower.MovingLeft -= MoveLeft;
            _pathFollower.MovingRight -= MoveRight;
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

        protected override void CheckHealth()
        {
            base.CheckHealth();
            _pathFollower.enabled = health > 0;
        }
    }
}