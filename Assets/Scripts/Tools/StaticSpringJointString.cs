using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class StaticSpringJointString : MonoBehaviour
    {
        public Rigidbody2D firstCorner;
        public Rigidbody2D secondCorner;
        [Space] 
        [Min(2)] 
        public int segments;
        [Space]
        public bool autoConfigureDistance;
        [Min(0.005f)]
        public float distance = 1;
        [Range(0, 1)]
        public float dumpingRatio;
        [Min(0)]
        public float frequency = 1;
        [Min(0.0001f)]
        public float mass = 1;
        [Space]
        [SerializeField]
        private List<GameObject> nodes = new();

        private readonly List<SpringJoint2D> _springJoint2Ds = new();

        private void Awake()
        {
            var nodeCount = segments - 1;
            var lastRb = firstCorner;
        
            SpringJoint2D springJoint2D;
        
            for (var i = 0; i < nodeCount; i++)
            {
                var node = new GameObject($"Node {i}")
                {
                    transform =
                    {
                        parent = transform
                    }
                };

                // SpringJoint2D settings
                springJoint2D = node.AddComponent<SpringJoint2D>();
                springJoint2D.connectedBody = lastRb;
                springJoint2D = SpringJointSetting(springJoint2D);
                _springJoint2Ds.Add(springJoint2D);
            
                // Rigidbody2D settings
                var rb2D = node.GetComponent<Rigidbody2D>();
                rb2D.mass = mass;
                lastRb = rb2D;

                nodes.Add(node);
            }
            // Last SpringJoint2D settings
            springJoint2D = nodes[nodeCount - 1].AddComponent<SpringJoint2D>();
            springJoint2D.connectedBody = secondCorner;
            SpringJointSetting(springJoint2D);
            _springJoint2Ds.Add(springJoint2D);
        }

        private SpringJoint2D SpringJointSetting(SpringJoint2D springJoint2D)
        {
            springJoint2D.autoConfigureDistance = autoConfigureDistance;
            springJoint2D.distance = distance;
            springJoint2D.dampingRatio = dumpingRatio;
            springJoint2D.frequency = frequency;
            return springJoint2D;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(firstCorner.position, secondCorner.position);
            
            Gizmos.color = Color.green;
            foreach (var springJoint2D in _springJoint2Ds)
            {
                Gizmos.DrawLine(springJoint2D.gameObject.transform.position, 
                    springJoint2D.connectedBody.position);
            }
        }
    }
}
