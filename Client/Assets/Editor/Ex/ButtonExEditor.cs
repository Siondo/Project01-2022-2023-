using UnityEngine;
using UnityEditor;

namespace Framework
{
	[CustomEditor(typeof(ButtonEx), true)]
	[CanEditMultipleObjects]
	public class ButtonExEditor : UnityEditor.UI.SelectableEditor
	{
		private SerializedProperty m_OnClickProperty;
		private SerializedProperty m_clickIntervalTime;
		private SerializedProperty m_normal;
		private SerializedProperty m_disabled;
		private SerializedProperty m_isButtonScale;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_OnClickProperty = base.serializedObject.FindProperty("m_onClick");
			m_clickIntervalTime = base.serializedObject.FindProperty("m_clickIntervalTime");
			m_normal = base.serializedObject.FindProperty("m_normal");
			m_disabled = base.serializedObject.FindProperty("m_disabled");
			m_isButtonScale = base.serializedObject.FindProperty("m_isButtonScale");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();
			serializedObject.Update();
			EditorGUILayout.PropertyField(m_OnClickProperty);

			GUILayout.Space(6);
			Color c = GUI.contentColor;
			GUI.contentColor = Color.yellow;
			GUILayout.Label("Extend", new GUIStyle("dockarea"), GUILayout.ExpandWidth(true));
			GUI.contentColor = c;

			EditorGUILayout.PropertyField(m_clickIntervalTime);
			EditorGUILayout.PropertyField(m_normal);
			EditorGUILayout.PropertyField(m_disabled);
			EditorGUILayout.PropertyField(m_isButtonScale);
            if (m_isButtonScale.boolValue)
            {
				EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_clickDownScale"));
				EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_normalScale"));
			}

			serializedObject.ApplyModifiedProperties(); 
		}
	}
}
