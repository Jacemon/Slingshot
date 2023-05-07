using UnityEditor;
using UnityEngine;

namespace Editor
{
    // https://forum.unity.com/threads/custompropertydrawer-for-a-class-with-a-generic-type.172013/
    //[CustomPropertyDrawer(typeof(ValueReference<>), true)]
    public class ValueReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            var valueProperty = property.FindPropertyRelative("value"); // TODO: null

            EditorGUI.PropertyField(position, valueProperty, label, true);

            EditorGUI.EndProperty();
        }
    }
}