using UnityEngine;
using UnityEditor;

namespace Framework
{
	[CustomEditor(typeof(TextEx))]
	public class TextExEditor : UnityEditor.UI.TextEditor
	{
		SerializedProperty m_vertOptimize;
		SerializedProperty m_showMarkBounds;
		SerializedProperty m_writeIntervalTime;
		SerializedProperty m_onHrefClick;
		SerializedProperty m_imgSize;
		SerializedProperty m_markImg;
		SerializedProperty m_languageId;

		protected override void OnEnable()
		{
			base.OnEnable();

			m_vertOptimize = serializedObject.FindProperty("m_vertOptimize");
			m_showMarkBounds = serializedObject.FindProperty("m_showMarkBounds");
			m_writeIntervalTime = serializedObject.FindProperty("m_writeIntervalTime");
			m_onHrefClick = serializedObject.FindProperty("m_onHrefClick");
			m_imgSize = serializedObject.FindProperty("m_imgSize");
			m_markImg = serializedObject.FindProperty("m_markImg");
			m_languageId = serializedObject.FindProperty("m_languageId");
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

			EditorGUILayout.PropertyField(m_vertOptimize);
			EditorGUILayout.PropertyField(m_showMarkBounds);
			EditorGUILayout.PropertyField(m_writeIntervalTime);
			EditorGUILayout.PropertyField(m_imgSize);
			EditorGUILayout.PropertyField(m_markImg);
			EditorGUILayout.PropertyField(m_languageId);
			EditorGUILayout.PropertyField(m_onHrefClick);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
