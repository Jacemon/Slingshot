using Tools.ScriptableObjects.References;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    [RequireComponent(typeof(Toggle))]
    public class ReferencedToggle : MonoBehaviour
    {
        public BoolReference isOn;
        [Space] 
        public Image graphic;
        public Sprite onSprite;
        public Sprite offSprite;

        private Toggle _toggle;
        
        public void Awake()
        {
            _toggle = GetComponent<Toggle>();
            
            ReloadData();
        }

        private void OnEnable()
        {
            isOn.OnValueChanged += ReloadData;
            _toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        
        private void OnDisable()
        {
            isOn.OnValueChanged -= ReloadData;
            _toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
        {
            isOn.Value = value;
        }
        
        private void ReloadData()
        {
            _toggle.isOn = isOn.Value;
            graphic.sprite = isOn.Value ? onSprite : offSprite;
        }
    }
}
