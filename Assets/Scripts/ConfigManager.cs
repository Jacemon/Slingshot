using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

// НИХЕРНА НЕ РАБОТАЕТ (де)сериализация словаря
// Имя файла конфигурации
public static class ConfigManager 
{
    private static string ConfigFileName = "config.json";
    private static Dictionary<string, string> config = new Dictionary<string, string>();

    public static string GetConfig(string configName)
    {
        if (config.Count == 0)
        {
            ReadConfig();
        }
        return config[configName];
    }

    private static void ReadConfig()
    {
        File.WriteAllText(ConfigFileName, JsonUtility.ToJson(config));
        if (File.Exists(ConfigFileName))
        {
            string configText = File.ReadAllText(ConfigFileName);

            config = JsonUtility.FromJson<Dictionary<string, string>>(configText);
        }
        else
        {
            Debug.LogError("Config file not exist");
        }
    }

    public static class Tag
    {
        public static string Get(string tagName)
        {
            return GetConfig(tagName);
        }
    }
}
