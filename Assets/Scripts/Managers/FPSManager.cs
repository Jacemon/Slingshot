using TMPro;
using UnityEngine;

namespace Managers
{
    public class FpsManager : MonoBehaviour
    {
        public bool showFps;
        public TextMeshProUGUI fpsLabel;

        private void Start()
        {
            Application.targetFrameRate = int.MaxValue;
            QualitySettings.vSyncCount = 0;
        }

        private void Update()
        {
            if (!showFps)
            {
                return;
            }
            var fps = (int)(1.0f/Time.deltaTime);
            fpsLabel.text = fps.ToString();
        }
    }
}