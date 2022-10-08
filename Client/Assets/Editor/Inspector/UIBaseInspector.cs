using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;

namespace Framework
{
    namespace UI
    {
        [CustomEditor(typeof(UIBase), true)]
        public class UIBaseInspector : UIToLuaInspector
        {
            struct PropertyName
            {
                public string title;
                public string name;
            }

            readonly PropertyName[] PROPERTYNAME = new PropertyName[]
            {
                new PropertyName(){ title = "UILayer", name = "m_uiLayer" },
                new PropertyName(){ title = "UIType", name = "m_uiType" },
                new PropertyName(){ title = "UIDestroyMode", name = "m_destroyMode" },
                new PropertyName(){ title = "UIShowOrHideMode", name = "m_showOrHideMode" },
            };

            private UIBase m_target = null;

            protected override void OnEnable()
            {
                m_titleName = "ToReference";
                m_target = serializedObject.targetObject as UIBase;
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

                for (int i = 0; i < PROPERTYNAME.Length; ++i)
                {
                    property = serializedObject.FindProperty(PROPERTYNAME[i].name);
                    EditorGUILayout.PropertyField(property, new GUIContent(PROPERTYNAME[i].title));
                }
                property = serializedObject.FindProperty("m_sortOrder");
                EditorGUILayout.PropertyField(property, new GUIContent("SortOrder"));

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
