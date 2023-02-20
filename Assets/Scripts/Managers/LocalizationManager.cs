using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

namespace Managers
{
    public class LocalizationManager : MonoBehaviour
    {
        public TextMeshProUGUI test;

        private Dictionary<string, string> _localizedTexts = new();

        private const string FolderName = "Localization";
        private static readonly string FolderPath = Path.Combine(Application.streamingAssetsPath, FolderName);
        
        private void Awake()
        {
            try
            {
                test.text = "1";
                var a = "";
                string[] files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, FolderName));
                test.text = "2";
                foreach (string file in files)
                {
                    a += file + '\n';
                }

                test.text = "3";
                
                test.text =
                    a +
                    "\n" + Application.streamingAssetsPath;
            }
            catch (Exception e)
            {
                test.text = e.Message;
            }
        }

        public string GetLocalizedText(string key)
        {
            return _localizedTexts[key];
        }

        public string[] GetLocalizationCodes()
        {
            return GetLocalizationFilePaths().Select(Path.GetFileNameWithoutExtension).ToArray();
        }
        
        public bool SetLocalization(string languageCode)
        {
            var path = $"{FolderPath}/{languageCode}.json";
            var pathh = Path.Combine(FolderPath, $"{languageCode}.json");
            
            if (!File.Exists(path))
            {
                return false;
            }
            var jsonString = File.ReadAllText(path);
            _localizedTexts = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            return true;
        }

        private string[] GetLocalizationFilePaths()
        {
            return Directory.GetFiles(Application.dataPath + "/Languages", "*.json");
        }
    }
}