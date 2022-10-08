using UnityEngine;
using UnityEditor;

namespace Framework
{
	[CustomEditor(typeof(ToggleEx), true)]
	[CanEditMultipleObjects]
	public class ToggleExEditor : UnityEditor.UI.ToggleEditor
    {
        SerializedProperty m_backgroundProperty;
        SerializedProperty m_checkmarkProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_backgroundProperty = serializedObject.FindProperty("m_background");
            m_checkmarkProperty = serializedObject.FindProperty("m_checkmark");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.Update();

            GUILayout.Space(6);
            Color c = GUI.contentColor;
            GUI.contentColor = Color.yellow;
            GUILayout.Label("Extend", new GUIStyle("dockarea"), GUILayout.ExpandWidth(true));
            GUI.contentColor = c;

            EditorGUILayout.PropertyField(m_backgroundProperty);
            EditorGUILayout.PropertyField(m_checkmarkProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
