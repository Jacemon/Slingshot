using UnityEngine;

namespace Tools.ScriptableObjects.References
{
    [CreateAssetMenu(fileName = "StringReference", menuName = "Custom/Reference/String Reference")]
    public class StringReference : ValueReference<string> { }
}