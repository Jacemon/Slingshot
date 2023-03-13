using UnityEngine;

namespace Tools.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StringReference", menuName = "Custom/Reference/String Reference")]
    public class StringReference : ValueReference<string> { }
}