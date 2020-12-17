using UnityEditor;

namespace Bullets {
    
    [CustomEditor(typeof(Bullet))]
    public class BulletEditor : Editor {
        private SerializedProperty damage;
        private SerializedProperty speed;
        private SerializedProperty isSplash;
        private SerializedProperty radius;
        private SerializedProperty lifetime;
        
        private void OnEnable() {
            damage = serializedObject.FindProperty("damage");
            speed = serializedObject.FindProperty("speed");
            isSplash = serializedObject.FindProperty("isSplash");
            radius = serializedObject.FindProperty("radius");
            lifetime = serializedObject.FindProperty("lifetime");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.Slider(lifetime, 1, 15);
            EditorGUILayout.Slider(speed, 1, 10);
            EditorGUILayout.PropertyField(damage);
            isSplash.boolValue = EditorGUILayout.Toggle("Splash Damage" ,isSplash.boolValue);
            if (isSplash.boolValue) {
                EditorGUILayout.Slider(radius, 1, 10);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}