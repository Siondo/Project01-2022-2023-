using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;

namespace Framework
{
    namespace UI
    {
        [CustomEditor(typeof(UIManager), true)]
        public class UIManagerInspector : Editor
        {
            private UIManager.UIWindowData[] m_windowData;
            private UIManager.UIData[] m_dialogData;
            private UIManager.UIData m_uiData;
            private int m_index = 0;

            struct PropertyName
            {
                public string title;
                public string name;
            }

            readonly PropertyName[] PROPERTYNAME = new PropertyName[]
            {
                new PropertyName(){ title = "UICamera", name = "m_uiCamera" },
                new PropertyName(){ title = "UIRoot", name = "m_uiRoot" },
                new PropertyName(){ title = "Background", name = "m_background" },
                new PropertyName(){ title = "Default", name = "m_default" },
                new PropertyName(){ title = "Popup", name = "m_popup" },
                new PropertyName(){ title = "Top", name = "m_top" },
            };

            private UIManager m_target = null;

            protected void OnEnable()
            {
                m_target = serializedObject.targetObject as UIManager;
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

                EditorGUI.BeginDisabledGroup(Application.isPlaying);
                for (int i = 0; i < PROPERTYNAME.Length; ++i)
                {
                    property = serializedObject.FindProperty(PROPERTYNAME[i].name);
                    EditorGUILayout.PropertyField(property, new GUIContent(PROPERTYNAME[i].title));
                }
                EditorGUI.EndDisabledGroup();

                // 窗口模式
                m_windowData = m_target.GetAllWindow();
                if (m_windowData.Length > 0)
                {
                    GUILayout.BeginVertical("ObjectPickerPreviewBackground");
                    GUILayout.Label("Window", "PreButtonGreen");
                    m_index = 0;
                    OnGroupTitleGUI(0, "Index", "Name", "State");
                    foreach (var window in m_windowData)
                    {
                        m_uiData = m_target.GetUIData(window.name);
                        if (null == m_uiData || null == m_uiData.uiBase || null == window.data)
                        {
                            continue;
                        }

                        OnGroupGUI(0, m_index++.ToString(), window.name, window.data.show ? "Show" : "Hide");
                    }
                    GUILayout.EndVertical();
                }

                // 对话框模式
                m_dialogData = m_target.GetAllDialog();
                if (m_dialogData.Length > 0)
                {
                    GUILayout.BeginVertical("ObjectPickerPreviewBackground");
                    GUILayout.Label("Dialog", "PreButtonGreen");
                    m_index = 0;
                    OnGroupTitleGUI(0, "Index", "Name", "State");
                    foreach (var dialog in m_dialogData)
                    {
                        if (null == dialog.uiBase || dialog.uiBase.uiType == UIType.Window)
                        {
                            continue;
                        }
                        OnGroupGUI(0, m_index++.ToString(), dialog.name, dialog.show ? "Show" : "Hide");
                    }
                    GUILayout.EndVertical();
                }


                serializedObject.ApplyModifiedProperties();
            }

            private void OnGroupTitleGUI(float tab, params string[] args)
            {
                const float HEIGHT = 16;
                GUILayout.BeginHorizontal("Icon.ClipSelected", GUILayout.Height(HEIGHT));
                if (tab > 0)
                {
                    GUILayout.Space(tab);
                }

                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                GUILayout.FlexibleSpace();
                GUILayout.Label(args[2], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }

            private void OnGroupGUI(float tab, params string[] args)
            {
                const float HEIGHT = 16;
                GUILayout.BeginHorizontal("Icon.Clip", GUILayout.Height(HEIGHT));
                if (tab > 0)
                {
                    GUILayout.Space(tab);
                }

                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                GUILayout.FlexibleSpace();
                if (args[2].Equals("Show"))
                {
                    GUILayout.Label(args[2], "sv_label_3", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                }
                else
                {
                    GUILayout.Label(args[2], "sv_label_0", GUILayout.Height(HEIGHT), GUILayout.MinWidth(120));
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }
        }
    }
}
