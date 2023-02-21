using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public void SetLanguage(string languageName)
    {
        NotDestroyableLocalization.SetLanguage(languageName);
    }
}
