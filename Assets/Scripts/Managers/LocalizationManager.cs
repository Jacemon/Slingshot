using Tools;
using UnityEngine;

namespace Managers
{
    public class LocalizationManager : MonoBehaviour
    {
        public void SetLanguage(string languageName)
        {
            NotDestroyableLocalization.SetLanguage(languageName);
        }
    }
}
