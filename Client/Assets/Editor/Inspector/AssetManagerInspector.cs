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
        [CustomEditor(typeof(AssetManager), true)]
        public class AssetManagerInspector : Editor
        {
            /// <summary>
            /// 记录已加载完成的所有异步资源
            /// </summary>
            private Dictionary<string, UnityAsyncAsset> m_complete = null;

            private int m_index = 0;
            private Color m_defaultBgColor = Color.gray;
            private Color m_bgColor = new Color(0.1f, 0.8f, 0.1f, 0.6f);
            private Color m_gotoColor = new Color(1, 1, 0, 0.8f);
            private Color m_clickColor = new Color(0.1f, 0.1f, 0.8f, 0.8f);
            private static string m_gotoUrl = string.Empty;
            private static string m_clickUrl = string.Empty;
            private static int m_clickType = 0;
            private AssetManager m_target = null;

            protected void OnEnable()
            {
                m_target = serializedObject.targetObject as AssetManager;
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

                m_complete = m_target.GetAllComplete();
                if (null != m_complete && m_complete.Count > 0)
                {
                    GUILayout.BeginVertical("ObjectPickerPreviewBackground");
                    GUILayout.Label("Completed", "PreButtonGreen");

                    m_index = 0;
                    OnGroupTitleGUI("Index", "Url", "Dep", "Ref", "Use");
                    foreach (var complete in m_complete)
                    {
                        OnGroupGUI(m_index++.ToString(),
                            complete.Value.url,
                            complete.Value.dependentCount.ToString(),
                            complete.Value.referenceCount.ToString(),
                            complete.Value.refObjCount.ToString()
                            );
                        if (complete.Value.url.Equals(m_clickUrl))
                        {
                            if (1 == m_clickType)
                            {
                                var list = complete.Value.GetDependentList();
                                for (int i = 0; i < list.Count; ++i)
                                {
                                    OnSimpleGroupGUI(list[i].url);
                                }
                            }
                            else if (2 == m_clickType)
                            {
                                var list = complete.Value.GetReferenceList();
                                for (int i = 0; i < list.Count; ++i)
                                {
                                    OnSimpleGroupGUI(list[i].url);
                                }
                            }
                            else if (3 == m_clickType)
                            {
                                AsyncAsset asyncAsset = null;
                                var list = complete.Value.GetRefObjList();
                                for (int i = 0; i < list.Count; ++i)
                                {
                                    asyncAsset = list[i] as AsyncAsset;
                                    OnSimpleGroupGUI(asyncAsset.url);
                                }
                            }
                        }
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
                GUILayout.Button(args[2], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58));
                GUILayout.FlexibleSpace();
                GUILayout.Button(args[3], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58));
                GUILayout.FlexibleSpace();
                GUILayout.Button(args[4], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58));
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
                string symbols = (args[1].Equals(m_clickUrl) && 1 == m_clickType) ? "▼" : "";
                if (GUILayout.Button(symbols + args[2], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58)))
                {
                    if (symbols.Length > 0)
                    {
                        m_clickUrl = string.Empty;
                        m_clickType = 0;
                    }
                    else
                    {
                        m_clickUrl = args[1];
                        m_clickType = 1;
                    }
                }
                GUILayout.FlexibleSpace();
                symbols = (args[1].Equals(m_clickUrl) && 2 == m_clickType) ? "▼" : "";
                if (GUILayout.Button(symbols + args[3], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58)))
                {
                    if (symbols.Length > 0)
                    {
                        m_clickUrl = string.Empty;
                        m_clickType = 0;
                    }
                    else
                    {
                        m_clickUrl = args[1];
                        m_clickType = 2;
                    }
                }
                GUILayout.FlexibleSpace(); 
                symbols = (args[1].Equals(m_clickUrl) && 3 == m_clickType) ? "▼" : "";
                if (GUILayout.Button(symbols + args[4], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58)))
                {
                    if (symbols.Length > 0)
                    {
                        m_clickUrl = string.Empty;
                        m_clickType = 0;
                    }
                    else
                    {
                        m_clickUrl = args[1];
                        m_clickType = 3;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }

            private void OnSimpleGroupGUI(params string[] args)
            {
                const float HEIGHT = 18;
                m_defaultBgColor = GUI.backgroundColor;
                GUI.backgroundColor = m_bgColor;
                GUILayout.BeginHorizontal("Icon.Clip", GUILayout.Height(HEIGHT));
                GUI.backgroundColor = m_defaultBgColor;

                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(300));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Goto", "PreButtonBlue", GUILayout.Height(HEIGHT), GUILayout.MinWidth(58)))
                {
                    m_gotoUrl = args[0];
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }
        }
    }
}
