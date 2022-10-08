using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using System;

namespace Framework
{
    namespace UnityAsset
    {
        [CustomEditor(typeof(SpriteManager), true)]
        public class SpriteManagerInspector : Editor
        {
            /// <summary>
            /// 记录所有异步资源
            /// </summary>
            private Dictionary<string, AsyncAsset> m_asyncAssets = null;

            private int m_index = 0;
            private Color m_defaultBgColor = Color.gray;
            private Color m_bgColor = new Color(0.1f, 0.8f, 0.1f, 0.6f);
            private Color m_gotoColor = new Color(1, 1, 0, 0.8f);
            private Color m_clickColor = new Color(0.1f, 0.1f, 0.8f, 0.8f);
            private static string m_gotoUrl = string.Empty;
            private static string m_clickUrl = string.Empty;
            private SpriteManager m_target = null;

            protected void OnEnable()
            {
                m_target = serializedObject.targetObject as SpriteManager;
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

                m_asyncAssets = m_target.asyncAssets;
                if (null != m_asyncAssets && m_asyncAssets.Count > 0)
                {
                    GUILayout.BeginVertical("ObjectPickerPreviewBackground");
                    GUILayout.Label("SpriteManager", "PreButtonGreen");

                    m_index = 0;
                    OnGroupTitleGUI("Index", "Name");
                    foreach (var name in m_asyncAssets.Keys)
                    {
                        OnGroupGUI(m_index++.ToString(), name);
                    }
                    GUILayout.EndVertical();
                }

                serializedObject.ApplyModifiedProperties();
            }

            private void OnGroupTitleGUI(params string[] args)
            {
                const float HEIGHT = 18;
                GUILayout.BeginHorizontal("Icon.ClipSelected", GUILayout.Height(HEIGHT));
                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(64));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(300));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }

            private void OnGroupGUI(params string[] args)
            {
                const float HEIGHT = 18;
                m_defaultBgColor = GUI.backgroundColor;
                if (args[1].Equals(m_gotoUrl))
                {
                    GUI.backgroundColor = m_gotoColor;
                }
                else if (args[1].Equals(m_clickUrl))
                {
                    GUI.backgroundColor = m_clickColor;
                }
                GUILayout.BeginHorizontal("Icon.Clip", GUILayout.Height(HEIGHT));
                GUI.backgroundColor = m_defaultBgColor;
                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(64));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(300));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }
        }
    }
}
