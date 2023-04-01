using DG.Tweening;
using Tools.Follower;
using UnityEngine;

namespace Entities.Targets
{
    [RequireComponent(typeof(PathFollower))]
    public class DynamicTarget : Target
    {
        [Header("Dynamic settings")]
        private PathFollower _pathFollower;
        
        protected override void Awake()
        {
            base.Awake();
            _pathFollower = GetComponent<PathFollower>();
        }

        protected override void OnEnable()
        {
            OnHealthChanged += CheckPathFollower;
            _pathFollower.OnMovingLeft += MoveLeft;
            _pathFollower.OnMovingRight += MoveRight;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            OnHealthChanged -= CheckPathFollower;
            _pathFollower.OnMovingLeft -= MoveLeft;
            _pathFollower.OnMovingRight -= MoveRight;
            base.OnDisable();
        }

        protected override void Appear()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(new Vector2(
                        appearScale * 
                        (PathFollower.CheckDirection(_pathFollower.points[^1], _pathFollower.points[0]) ? 1 : -1),
                        appearScale),
                    appearTime)
                .SetEase(Ease.OutExpo);
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