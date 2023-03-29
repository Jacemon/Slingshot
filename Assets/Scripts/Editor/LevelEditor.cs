using Entities.Levels;
using Entities.Levels.Generators;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var generators = ((Level)target).generators;
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Ellipse generator"))
            {
                generators.Add(new EllipseGenerator());
            }
            if (GUILayout.Button("Add Point generator"))
            {
                generators.Add(new PointGenerator());
            }
            
            GUILayout.EndHorizontal();
        }
    }
}