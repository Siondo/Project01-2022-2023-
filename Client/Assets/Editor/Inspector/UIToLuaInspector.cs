using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;

namespace Framework
{
    namespace UI
    {
        [CustomEditor(typeof(UIToLua), true)]
        public class UIToLuaInspector : UIReferenceInspector
        {
            protected override void OnEnable()
            {
                m_titleName = "ToLua";
                base.OnEnable();
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                EditorGUI.BeginDisabledGroup(true);
                SerializedProperty property = serializedObject.GetIterator();
                if (property.NextVisible(true))
                {
                    EditorGUILayout.PropertyField(property, new GUIContent("Script"), true, new GUILayoutOption[0]);
                }
                EditorGUI.EndDisabledGroup();

                property = serializedObject.FindProperty("m_path");
                EditorGUILayout.PropertyField(property, new GUIContent("LuaScript"));
                m_reorderableList.DoLayoutList();
                OnButtonGUI();
                m_paramList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
