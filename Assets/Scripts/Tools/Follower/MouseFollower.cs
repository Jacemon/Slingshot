using UnityEngine;

namespace Tools.Follower
{
    public class MouseFollower : Follower
    {
        private Camera _camera;

        protected override void Awake()
        {
            _camera = Camera.main;
            base.Awake();
        }

        protected override void Update()
        {
            followPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            base.Update();
        }
    }
}