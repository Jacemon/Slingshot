using UnityEngine;

namespace Managers.AnimatedUI
{
    public class AnimatedUIToggle : AnimatedUIElement
    {
        [Header("Toggle")]
        public string isEnabledParameterName = "IsOpen";
        
        public void Toggle(bool isOpen)
        {
            UIAnimator.speed = animationSpeed;
            UIAnimator.SetBool(isEnabledParameterName, isOpen);
        }
    }
}