using Tools;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(IntLinearCurve))]
    public class IntLinearCurveEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
    
            GUILayout.Space(10);
        
            var linearCurve = (IntLinearCurve)target;

            for (var i = 0; i < linearCurve.curve.length; i++)
            {
                var key = linearCurve.curve.keys[i];
                key.time = Mathf.RoundToInt(key.time);
                key.value = Mathf.RoundToInt(key.value);
                linearCurve.curve.MoveKey(i, key);
                AnimationUtility.SetKeyBroken(linearCurve.curve, i, true);
                AnimationUtility.SetKeyRightTangentMode(linearCurve.curve, i, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyLeftTangentMode(linearCurve.curve, i, AnimationUtility.TangentMode.Linear);
            }
    
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("(Re)Build Curve"))
            {
                linearCurve.Rebuild();
            }
        
            if (GUILayout.Button("Add Corner"))
            {
                linearCurve.AddCorner();
            }
        
            if (GUILayout.Button("Remove Corner"))
            {
                linearCurve.RemoveCorner();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}