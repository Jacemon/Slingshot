using Tools.Follower;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(PathFollower))]
    public class PathFollowerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var pathFollower = (PathFollower)target;

            pathFollower.pointType = (PathFollower.PointType)EditorGUILayout.EnumPopup("Point type", pathFollower.pointType);

            switch (pathFollower.pointType)
            {
                case PathFollower.PointType.Vector2:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("vector2Points"), true);
                    break;
                case PathFollower.PointType.Vector3:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("vector3Points"), true);
                    break;
                case PathFollower.PointType.GameObject:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gameObjectPoints"), true);
                    break;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("velocity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loops"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clamp"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}