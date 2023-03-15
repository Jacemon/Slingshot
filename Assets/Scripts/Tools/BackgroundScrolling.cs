using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Tools
{
    public class BackgroundScrolling : MonoBehaviour
    {
        public float scrollSpeed = 0.5f;

        public Image background;
    
        private Image _backgroundImage;
        private Image _auxiliaryBackgroundImage;

        private float _backgroundImageWidth;
    
        private void Start()
        {
            var screenCenter = Vector2.zero;

            _backgroundImageWidth = background.rectTransform.rect.width;
        
            _backgroundImage = Instantiate(background, gameObject.transform);
            _backgroundImage.rectTransform.anchoredPosition = screenCenter;
            _auxiliaryBackgroundImage = Instantiate(background, gameObject.transform);
            _auxiliaryBackgroundImage.rectTransform.anchoredPosition = 
                screenCenter + new Vector2(_backgroundImageWidth, 0);
        
            background.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_backgroundImage.rectTransform.anchoredPosition.x <= -_backgroundImageWidth)
            {
                _backgroundImage.rectTransform.anchoredPosition += new Vector2(2 * _backgroundImageWidth, 0);
            }
            if (_auxiliaryBackgroundImage.rectTransform.anchoredPosition.x <= -_backgroundImageWidth)
            {
                _auxiliaryBackgroundImage.rectTransform.anchoredPosition += new Vector2(2 * _backgroundImageWidth, 0);
            }

            _backgroundImage.rectTransform.anchoredPosition -= new Vector2(scrollSpeed * Time.deltaTime, 0);
            _auxiliaryBackgroundImage.rectTransform.anchoredPosition -= new Vector2(scrollSpeed * Time.deltaTime, 0);
        }
    }
}
