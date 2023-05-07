using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Entities
{
    public class Tree : MonoBehaviour
    {
        public List<Collider2D> leavesCollider2Ds;
        public List<Collider2D> trunkCollider2Ds;
        [Space]
        public GameObject leavesHitPrefab;
        public GameObject trunkHitPrefab;
        [Space]
        public AudioSource leavesHit;
        public MinMaxCurve minMaxLeavesHitPitch;
        public AudioSource trunkHit;
        public MinMaxCurve minMaxTrunkHitPitch;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent(out Projectile _)) return;

            if (leavesCollider2Ds.Contains(collision.otherCollider))
            {
                Destroy(collision.gameObject);

                leavesHit.pitch = minMaxLeavesHitPitch.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
                leavesHit.Play();

                Instantiate(leavesHitPrefab, collision.contacts[0].point,
                    Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), transform);
            }
            else if (trunkCollider2Ds.Contains(collision.otherCollider))
            {
                trunkHit.pitch = minMaxTrunkHitPitch.Evaluate(Time.time, Random.Range(0.0f, 1.0f));
                trunkHit.Play();

                Instantiate(trunkHitPrefab, collision.contacts[0].point,
                    Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), transform);
            }

            collision.collider.enabled = false;
        }
    }
}