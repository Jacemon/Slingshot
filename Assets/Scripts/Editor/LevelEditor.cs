using Entities.Levels;
using Entities.Levels.Generators;
using Entities.Levels.Generators.DynamicGenerators;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Level), true)]
    public class LevelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var generators = ((Level)target).generators;
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+Point generator"))
            {
                generators.Add(new PointGenerator());
            }
            if (GUILayout.Button("+Ellipse generator"))
            {
                generators.Add(new EllipseGenerator());
            }
            if (GUILayout.Button("+Rect. generator"))
            {
                generators.Add(new RectangleGenerator());
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+Dyn. Point generator"))
            {
                generators.Add(new DynamicPointGenerator());
            }
            if (GUILayout.Button("+Dyn. Ellipse generator"))
            {
                generators.Add(new DynamicEllipseGenerator());
            }
            if (GUILayout.Button("+Dyn. Rect. generator"))
            {
                generators.Add(new DynamicRectangleGenerator());
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}