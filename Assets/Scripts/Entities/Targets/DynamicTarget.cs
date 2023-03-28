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
            onHealthChanged += CheckPathFollower;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onHealthChanged -= CheckPathFollower;
        }

        private void CheckPathFollower()
        {
            _pathFollower.enabled = health > 0;
        }
    }
}