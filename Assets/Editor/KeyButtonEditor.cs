using Asteroids.UI.Views;
using UnityEditor;
using UnityEditor.UI;

namespace Asteroids.Editor
{
    [CustomEditor(typeof(KeyButton), true)]
    [CanEditMultipleObjects]
    public class KeyButtonEditor : ButtonEditor
    {
        private SerializedProperty _isCancelButtonEnabled;

        protected override void OnEnable()
        {
            base.OnEnable();
            _isCancelButtonEnabled = serializedObject.FindProperty("_isCancelButtonEnabled");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_isCancelButtonEnabled);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
