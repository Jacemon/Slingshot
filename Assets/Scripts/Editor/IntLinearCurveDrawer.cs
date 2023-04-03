using Tools;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(IntLinearCurve))]
    public class IntLinearCurveDrawer : PropertyDrawer
    {
        private const float HorizontalSpacing = 5f; 
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var curve = property.FindPropertyRelative("curve").animationCurveValue;
            var cornerCount = property.FindPropertyRelative("cornerCount").intValue;
            var k = property.FindPropertyRelative("k").intValue;
            var b = property.FindPropertyRelative("b").intValue;

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
        
            var buildButtonRect = new Rect(
                position.x,
                position.y,
                position.width * 0.1f - HorizontalSpacing,
                EditorGUIUtility.singleLineHeight
            );
            var addButtonRect = new Rect(
                position.x + position.width * 0.1f,
                position.y,
                position.width * 0.1f - HorizontalSpacing,
                EditorGUIUtility.singleLineHeight
            );
            var removeButtonRect = new Rect(
                position.x + position.width * 0.2f,
                position.y,
                position.width * 0.1f - HorizontalSpacing,
                EditorGUIUtility.singleLineHeight
            );
            var curveRect = new Rect(
                position.x + position.width * 0.3f,
                position.y,
                position.width * 0.6f,
                EditorGUIUtility.singleLineHeight
            );
            var cornerCountRect = new Rect(
                position.x + position.width * 0.9f + HorizontalSpacing,
                position.y,
                position.width * 0.1f - HorizontalSpacing,
                EditorGUIUtility.singleLineHeight
            );

            var quarterSecondLineRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width * 0.25f,
                EditorGUIUtility.singleLineHeight
            );
            var deltaWidth = HorizontalSpacing * 3 / 4;
            var deltaX = position.width * 0.25f - deltaWidth + HorizontalSpacing;
        
            var equationStartRect = quarterSecondLineRect;
            equationStartRect.width -= deltaWidth;
        
            var kRect = quarterSecondLineRect;
            kRect.x += deltaX;
            kRect.width -= deltaWidth;
        
            var equationEndRect = quarterSecondLineRect;
            equationEndRect.x += deltaX * 2;
            equationEndRect.width -= deltaWidth;
        
            var bRect = quarterSecondLineRect;
            bRect.x += deltaX * 3;
            bRect.width -= deltaWidth;
        
            curve = EditorGUI.CurveField(curveRect, curve);
        
            for (var i = 0; i < curve.length; i++)
            {
                var key = curve.keys[i];
                key.time = Mathf.RoundToInt(key.time);
                key.value = Mathf.RoundToInt(key.value);
                curve.MoveKey(i, key);
                AnimationUtility.SetKeyBroken(curve, i, true);
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.TangentMode.Linear);
            }

            cornerCount = EditorGUI.IntField(cornerCountRect, cornerCount);
        
            k = EditorGUI.IntField(kRect, k);
            b = EditorGUI.IntField(bRect, b);
        
            EditorGUI.LabelField(equationStartRect, "y = ");
            EditorGUI.LabelField(equationEndRect, "* x + ");
        
            if (GUI.Button(buildButtonRect, "B"))
            {
                curve = new AnimationCurve();
                for (var i = 0; i < cornerCount; i++)
                {
                    curve.AddKey(i, i * k + b);
                }
            }
            if (GUI.Button(addButtonRect, "+"))
            {
                cornerCount++;
                curve.AddKey(cornerCount, cornerCount * k + b);
            }
            if (GUI.Button(removeButtonRect, "-"))
            {
                cornerCount--;
                curve.RemoveKey(cornerCount);
            }
        
            EditorGUI.EndProperty();

            property.FindPropertyRelative("curve").animationCurveValue = curve;
            property.FindPropertyRelative("cornerCount").intValue = cornerCount;
            property.FindPropertyRelative("k").intValue = k;
            property.FindPropertyRelative("b").intValue = b;
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
