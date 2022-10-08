using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;

namespace Framework
{
    namespace Pool
    {
        [CustomEditor(typeof(PoolManager), true)]
        public class PoolManagerInspector : Editor
        {
            private Dictionary<string, IPool> m_pool;
            private int m_index = 0;
            private PoolManager m_target = null;

            protected void OnEnable()
            {
                m_target = serializedObject.targetObject as PoolManager;
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

                m_pool = m_target.GetAllPools();
                if (null != m_pool && m_pool.Count > 0)
                {
                    GUILayout.BeginVertical("ObjectPickerPreviewBackground");
                    GUILayout.Label("PoolManager", "PreButtonGreen");
                    m_index = 0;
                    OnGroupTitleGUI(0, "Index", "Name", "PoolType", "Count");
                    foreach (var pool in m_pool)
                    {
                        OnGroupGUI(0, m_index++.ToString(), pool.Key, pool.Value.poolType.ToString(), pool.Value.Count.ToString());
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

                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(80));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(160));
                GUILayout.FlexibleSpace();
                GUILayout.Space(8);
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[2], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(160));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[3], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(80));
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

                GUILayout.Label(args[0], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(80));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[1], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(160));
                GUILayout.FlexibleSpace();
                GUILayout.Space(8);
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[2], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(160));
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel(args[3], "Font.Clip", GUILayout.Height(HEIGHT), GUILayout.MinWidth(80));
                GUILayout.EndHorizontal();
                GUILayout.Space(1);
            }
        }
    }
}
