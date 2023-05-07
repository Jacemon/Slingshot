using UnityEngine;

namespace Tools.Follower
{
    public class TouchFollower : Follower
    {
        private Camera _camera;

        protected override void Awake()
        {
            _camera = Camera.main;
            base.Awake();
        }

        protected override void Update()
        {
            followPoint = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            base.Update();
        }
    }
}