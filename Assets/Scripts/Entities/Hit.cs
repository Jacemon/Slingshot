using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Hit : MonoBehaviour
    {
        public float lifeSpan;

        public IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(lifeSpan);
            Destroy(gameObject);
        }
    }
}