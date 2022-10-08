using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(ScrollRectEx), true)]
    [CanEditMultipleObjects]
    public class ScrollRectExEditor : UnityEditor.UI.ScrollRectEditor
    {
        SerializedProperty m_direction;
        SerializedProperty m_spacing;
        SerializedProperty m_columnOrRow;
        SerializedProperty m_centerShow;
        SerializedProperty m_contentRect;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_direction = serializedObject.FindProperty("m_direction");
            m_spacing = serializedObject.FindProperty("m_spacing");
            m_columnOrRow = serializedObject.FindProperty("m_columnOrRow");
            m_centerShow = serializedObject.FindProperty("m_centerShow");
            m_contentRect = serializedObject.FindProperty("m_contentRect");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            GUILayout.Space(6);
            Color c = GUI.contentColor;
            GUI.contentColor = Color.yellow;
            GUILayout.Label("Extend", new GUIStyle("dockarea"), GUILayout.ExpandWidth(true));
            GUI.contentColor = c;

            EditorGUILayout.PropertyField(m_direction);
            EditorGUILayout.PropertyField(m_spacing);
            EditorGUILayout.PropertyField(m_columnOrRow);
            EditorGUILayout.PropertyField(m_centerShow);
            EditorGUILayout.PropertyField(m_contentRect);

            serializedObject.ApplyModifiedProperties();
        }
    }

}