using TMPro;
using UnityEngine;

public class FpsManager : MonoBehaviour
{
    public TextMeshProUGUI fpsLabel;
    
    private void Update () {
        int fps = (int)(1.0f/Time.deltaTime);
        fpsLabel.text = fps.ToString();
    }
}