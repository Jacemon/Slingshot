using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    public class BackButton : MonoBehaviour
    {
        public UnityEvent onBack;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) onBack!.Invoke();
        }
    }
}