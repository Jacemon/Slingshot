using Tools.ScriptableObjects.Shop.ShopItems;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CurveShopItem))]
    public class LinearCurveEditor : UnityEditor.Editor // TODO: CurveShopItemEditor избавиться от IntLinearCurve(Editor) и написать CustomPropertyDrawer 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
    
            GUILayout.Space(10);
        
            var linearCurve = ((CurveShopItem)target).itemCostCurve;

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