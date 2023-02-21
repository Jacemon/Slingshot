using UnityEngine;
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
            Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

            _backgroundImageWidth = background.rectTransform.rect.width;
        
            _backgroundImage = Instantiate(background, screenCenter, 
                Quaternion.identity, gameObject.transform);
            _auxiliaryBackgroundImage = Instantiate(background, screenCenter + new Vector2(_backgroundImageWidth, 0), 
                Quaternion.identity, gameObject.transform);
        
            background.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_backgroundImage.transform.position.x <= -_backgroundImageWidth / 2.0f)
            {
                _backgroundImage.transform.position += new Vector3(2 * _backgroundImageWidth, 0);
            }
            if (_auxiliaryBackgroundImage.transform.position.x <= -_backgroundImageWidth / 2.0f)
            {
                _auxiliaryBackgroundImage.transform.position += new Vector3(2 * _backgroundImageWidth, 0);
            }

            _backgroundImage.transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0);
            _auxiliaryBackgroundImage.transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0);
        }
    }
}
