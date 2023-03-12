using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    [RequireComponent(typeof(Toggle))]
    public class CustomToggle : MonoBehaviour
    {
        public Image graphicTarget;
        public Sprite onSprite;
        public Sprite offSprite;

        private void Awake()
        {
            OnToggleChanged(GetComponent<Toggle>().isOn);
        }

        public void OnToggleChanged(bool value)
        {
            graphicTarget.sprite = value ? onSprite : offSprite;
        }
    }
}
