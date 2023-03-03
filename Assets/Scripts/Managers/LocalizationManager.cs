using Lean.Localization;
using Tools;
using UnityEngine;

namespace Managers
{
    public class LocalizationManager : MonoBehaviour
    {
        public void SetLanguage(string languageName)
        {
            if (NotDestroyable.TryGetComponent("LeanLocalization", out LeanLocalization leanLocalization))
            {
                leanLocalization.SetCurrentLanguage(languageName);
            }
        }
    }
}
