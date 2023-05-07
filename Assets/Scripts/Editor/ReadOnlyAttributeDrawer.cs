using Tools;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            // TODO: if the property is disclosed, the height does not match
            EditorGUI.PropertyField(position, property, label, true); 
            GUI.enabled = true;
        }
    }
}