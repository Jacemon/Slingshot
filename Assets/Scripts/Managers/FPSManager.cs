using TMPro;
using UnityEngine;

public class FpsManager : MonoBehaviour
{
    public TextMeshProUGUI fpsLabel;

    private void Start()
    {
        Application.targetFrameRate = int.MaxValue;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        var fps = (int)(1.0f/Time.deltaTime);
        fpsLabel.text = fps.ToString();
    }
}