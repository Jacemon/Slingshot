using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class SpringJointString : MonoBehaviour
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
        private readonly List<Rigidbody2D> _rigidbody2Ds = new();

        private Rigidbody2D _lastRb;

        private void RecalculateNodes()
        {
            foreach (var node in nodes)
            {
                Destroy(node);
            }
            nodes.Clear();
            _springJoint2Ds.Clear();
            _rigidbody2Ds.Clear();
        
            var nodeCount = segments - 1;
            _lastRb = firstCorner;
        
            SpringJoint2D springJoint2D;
        
            for (var i = 0; i < nodeCount; i++)
            {
                var node = new GameObject($"Node {i}");
                node.transform.parent = transform;
            
                var rb2D = node.AddComponent<Rigidbody2D>();
                rb2D.mass = mass;
                _rigidbody2Ds.Add(rb2D);
            
                springJoint2D = node.AddComponent<SpringJoint2D>();
                springJoint2D.connectedBody = _lastRb;
                _springJoint2Ds.Add(springJoint2D);
            
                _lastRb = rb2D;
                nodes.Add(node);
            }
            springJoint2D = nodes[nodeCount - 1].AddComponent<SpringJoint2D>();
            springJoint2D.connectedBody = secondCorner;
            _springJoint2Ds.Add(springJoint2D);
        }

        private void Update()
        {
            if (nodes.Count != segments - 1)
            {
                RecalculateNodes();
            }
        
            foreach (var springJoint2D in _springJoint2Ds)
            {
                springJoint2D.autoConfigureDistance = autoConfigureDistance;
                springJoint2D.distance = distance;
                springJoint2D.dampingRatio = dumpingRatio;
                springJoint2D.frequency = frequency;
            }

            foreach (var rb2D in _rigidbody2Ds)
            {
                rb2D.mass = mass;
            }
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
