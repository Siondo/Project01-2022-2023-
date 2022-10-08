using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

namespace Framework
{
    namespace UI
    {
        [CustomPropertyDrawer(typeof(ReferenceComponent))]
        public class ReferenceComponentPropertyDrawer : PropertyDrawer
        {
            const float SPACING_Y = 2;
            static Color GRAY = new Color(169 / 255f, 169 / 255f, 169 / 255f);
            static Color GREEN = new Color(60 / 255f, 179 / 255f, 113 / 255f);

            List<Object> m_objList = new List<Object>();
            Rect[] m_rects = new Rect[3];
            private List<Object> objList => m_objList;
            private Rect[] rects => m_rects;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                float height = EditorGUIUtility.singleLineHeight + SPACING_Y;


                //数据获取
                UIReference uIReference = (property.serializedObject.targetObject as UIReference);
                int index = int.Parse(property.displayName.Replace("Element ", ""));
                ReferenceComponent referenceComponent = uIReference.data[index];
                if (null == referenceComponent.target)
                {
                    return height + EditorGUIUtility.singleLineHeight + SPACING_Y;
                }

                objList.Clear();
                objList.AddRange(referenceComponent.target.GetComponents<Component>());
                objList.Insert(0, referenceComponent.target);
                int rectIndex = 0;
                for (int i = 0; i < objList.Count; ++i)
                {
                    if (ReferenceComponent.Contains(objList[i].GetType().Name))
                    {
                        rectIndex++;
                    }
                }
                int Cnt = Mathf.CeilToInt(rectIndex / 3.0f);

                for (int i = 0; i < Cnt; ++i)
                {
                    height += EditorGUIUtility.singleLineHeight + SPACING_Y;
                }
                return height + SPACING_Y;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                using (new EditorGUI.PropertyScope(position, label, property))
                {
                    Color c = GUI.contentColor;
                    //目标
                    Rect rect = position;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    SerializedProperty targetProperty = property.FindPropertyRelative("m_target");
                    if (targetProperty != null)
                    {
                        EditorGUI.PropertyField(rect, targetProperty, new GUIContent(string.Empty));
                    }
                    //数据获取
                    SerializedProperty propertyList = property.FindPropertyRelative("m_list");
                    int index = int.Parse(property.displayName.Replace("Element ", ""));
                    UIReference uIReference = (property.serializedObject.targetObject as UIReference);
                    ReferenceComponent referenceComponent = uIReference.data[index];
                    if (null == referenceComponent.target)
                    {
                        GUI.contentColor = new Color(255 / 255f, 255 / 255f, 0 / 255f);
                        rect.y += EditorGUIUtility.singleLineHeight + SPACING_Y;
                        GUI.Label(rect, "The target object is null, are you sure ?");
                        GUI.contentColor = c;

                        return;
                    }
                    objList.Clear();
                    objList.AddRange(referenceComponent.target.GetComponents<Component>());
                    objList.Insert(0, referenceComponent.target);

                    //Rect初始化
                    rects[0] = new Rect(rect)
                    {
                        x = rect.x,
                        width = rect.width / 3.0f - 0.25f,
                    };
                    rects[1] = new Rect(rect)
                    {
                        x = rect.x + rect.width / 3.0f + 0.25f,
                        width = rect.width / 3.0f - 0.5f,
                    };
                    rects[2] = new Rect(rect)
                    {
                        x = rect.x + rect.width / 3.0f * 2 + 0.25f,
                        width = rect.width / 3.0f - 0.25f,
                    };
                    //*************************************************************
                    //刷新
                    int rectIndex = 0;
                    c = GUI.backgroundColor;
                    GUILayout.BeginHorizontal();
                    string tempString = string.Empty;
                    for (int i = 0; i < objList.Count; ++i)
                    {
                        tempString = objList[i].GetType().Name;
                        if (ReferenceComponent.Contains(tempString))
                        {
                            bool bContainsObj = referenceComponent.ContainsObj(objList[i]);
                            if (0 == rectIndex % 3)
                            {
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                            }
                            rects[rectIndex%3].y += EditorGUIUtility.singleLineHeight + SPACING_Y;
                            GUI.backgroundColor = bContainsObj ? GREEN : GRAY;
                            if (GUI.Button(rects[rectIndex%3], tempString, GUI.skin.GetStyle("minibuttonmid")))
                            {
                                for (int j = 0; j < propertyList.arraySize; ++j)
                                {
                                    if (propertyList.GetArrayElementAtIndex(j).objectReferenceValue == null)
                                    {
                                        propertyList.DeleteArrayElementAtIndex(j);
                                    }
                                }
                                if (bContainsObj)
                                {
                                    for (int j = 0; j < propertyList.arraySize; ++j)
                                    {
                                        if (propertyList.GetArrayElementAtIndex(j).objectReferenceValue == objList[i])
                                        {
                                            propertyList.DeleteArrayElementAtIndex(j);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    propertyList.InsertArrayElementAtIndex(0);
                                    SerializedProperty sd = propertyList.GetArrayElementAtIndex(0);
                                    sd.objectReferenceValue = objList[i];
                                }
                            }
                            GUI.backgroundColor = c;
                            rectIndex++;
                        }
                    }
                    int Cnt = Mathf.CeilToInt(rectIndex / 3.0f) * 3;
                    for (int i = rectIndex; i < Cnt; ++i)
                    {
                        rects[rectIndex%3].y += EditorGUIUtility.singleLineHeight + SPACING_Y;
                        GUI.Label(rects[rectIndex%3], string.Empty);
                        rectIndex++;
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }


        [CustomPropertyDrawer(typeof(ReferenceParam))]
        public class ReferenceParamPropertyDrawer : PropertyDrawer
        {
            const float SPACING_Y = 2;
            Rect[] m_rects = new Rect[3];
            private Rect[] rects => m_rects;

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                float height = EditorGUIUtility.singleLineHeight * 3 + SPACING_Y + SPACING_Y;
                return height + SPACING_Y;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                using (new EditorGUI.PropertyScope(position, label, property))
                {
                    //目标
                    Rect rect = position;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    //Rect初始化
                    rects[0] = new Rect(rect)
                    {
                        y = rect.y,
                    };
                    rects[1] = new Rect(rect)
                    {
                        y = rects[0].y + rect.height + SPACING_Y,
                    };
                    rects[2] = new Rect(rect)
                    {
                        y = rects[1].y + rect.height + SPACING_Y,
                    };

                    //数据获取
                    int index = int.Parse(property.displayName.Replace("Element ", ""));
                    UIReference uIReference = (property.serializedObject.targetObject as UIReference);
                    ReferenceParam referenceComponent = uIReference.paramData[index];

                    SerializedProperty temp = property.FindPropertyRelative("m_name");
                    EditorGUI.PropertyField(rects[0], temp);

                    temp = property.FindPropertyRelative("m_type");
                    EditorGUI.PropertyField(rects[1], temp);

                    if (referenceComponent.paramType == ReferenceParam.ParamType.Number)
                    {
                        temp = property.FindPropertyRelative("m_double");
                        EditorGUI.PropertyField(rects[2], temp);
                    }
                    else if (referenceComponent.paramType == ReferenceParam.ParamType.String)
                    {
                        temp = property.FindPropertyRelative("m_string");
                        EditorGUI.PropertyField(rects[2], temp);
                    }
                    else if (referenceComponent.paramType == ReferenceParam.ParamType.Boolean)
                    {
                        temp = property.FindPropertyRelative("m_bool");
                        EditorGUI.PropertyField(rects[2], temp);
                    }
                    else if (referenceComponent.paramType == ReferenceParam.ParamType.Color)
                    {
                        temp = property.FindPropertyRelative("m_color");
                        EditorGUI.PropertyField(rects[2], temp);
                    }
                }
            }
        }

        [CustomEditor(typeof(UIReference), true)]
        public class UIReferenceInspector : Editor
        {
            protected ReorderableList m_reorderableList;
            protected ReorderableList m_paramList;

            private SerializedProperty m_elementProperty;
            private SerializedProperty m_targetProperty;

            protected string m_titleName = "ToReference";
            protected virtual void OnEnable()
            {
                SerializedProperty property = serializedObject.FindProperty("m_data");
                m_reorderableList = new ReorderableList(serializedObject, property, true, true, true, true);
                m_reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
                m_reorderableList.drawHeaderCallback = (Rect rect) =>
                {
                    GUI.Label(rect, m_titleName);
                };

                m_reorderableList.elementHeightCallback = (index) =>
                {
                    SerializedProperty arrayElement = property.GetArrayElementAtIndex(index);
                    float height = EditorGUI.GetPropertyHeight(arrayElement, GUIContent.none, arrayElement.isExpanded);
                    return height;
                };

                m_reorderableList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty item = m_reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.PropertyField(rect, item, new GUIContent(string.Empty));
                };

                //Delete
                m_reorderableList.onRemoveCallback = (ReorderableList reorderableList) =>
                {
                    if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
                    }
                };

                //*********************
                //以下为参数传递到Lua使用
                //*********************
                SerializedProperty paramProperty = serializedObject.FindProperty("m_paramData");
                m_paramList = new ReorderableList(serializedObject, paramProperty, true, true, true, true);
                m_paramList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
                m_paramList.drawHeaderCallback = (Rect rect) =>
                {
                    GUI.Label(rect, "ParamToLua");
                };
                m_paramList.elementHeightCallback = (index) =>
                {
                    SerializedProperty arrayElement = paramProperty.GetArrayElementAtIndex(index);
                    float height = EditorGUI.GetPropertyHeight(arrayElement, GUIContent.none, arrayElement.isExpanded);
                    return height;
                };

                m_paramList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty item = m_paramList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, item, new GUIContent(string.Empty));
                };

                //Delete
                //m_paramList.onRemoveCallback = (ReorderableList reorderableList) =>
                //{
                //    if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
                //    {
                //        ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
                //    }
                //};

                m_paramList.onAddCallback = (ReorderableList reorderableList) =>
                {
                    ReorderableList.defaultBehaviours.DoAddButton(reorderableList);
                };
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
                m_reorderableList.DoLayoutList();

                OnButtonGUI();
                m_paramList.DoLayoutList();
                serializedObject.ApplyModifiedProperties();
            }

            /// <summary>
            /// 添加容器按钮
            /// </summary>
            protected void OnButtonGUI()
            {
                if (GUILayout.Button("添加操作的UI容器"))
                {
                    // 获取所有对象
                    List<GameObject> list = new List<GameObject>();
                    CreateContainer((target as UIReference).transform, list);

                    SerializedProperty property = serializedObject.FindProperty("m_data");
                    // 移除空目标
                    for (int i = property.arraySize - 1; i >= 0; --i)
                    {
                        m_elementProperty = property.GetArrayElementAtIndex(i);
                        m_targetProperty = m_elementProperty.FindPropertyRelative("m_target");

                        if (null == m_targetProperty.objectReferenceValue)
                        {
                            property.DeleteArrayElementAtIndex(i);
                        }
                        else if (!list.Contains(m_targetProperty.objectReferenceValue as GameObject))
                        {
                            property.DeleteArrayElementAtIndex(i);
                        }
                        else
                        {
                            var listProperty = m_elementProperty.FindPropertyRelative("m_list");
                            for (int j = listProperty.arraySize - 1; j >= 0; --j)
                            {
                                var e = listProperty.GetArrayElementAtIndex(j);
                                if (e.objectReferenceValue == null)
                                {
                                    listProperty.DeleteArrayElementAtIndex(j);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < list.Count; ++i)
                    {
                        int index = FindIndex(property, i, list[i]);
                        if (index == -1)
                        {
                            property.InsertArrayElementAtIndex(i);
                            m_elementProperty = property.GetArrayElementAtIndex(i);
                            m_targetProperty = m_elementProperty.FindPropertyRelative("m_target");
                            m_targetProperty.objectReferenceValue = list[i];
                        }
                        else
                        {
                            property.MoveArrayElement(index, i);
                        }
                    }
                }
            }

            /// <summary>
            /// 查找索引
            /// </summary>
            /// <param name="property"></param>
            /// <param name="index"></param>
            /// <param name="object"></param>
            /// <returns></returns>
            private int FindIndex(SerializedProperty property, int index, Object @object)
            {
                for (int i = index; i < property.arraySize; ++i)
                {
                    m_elementProperty = property.GetArrayElementAtIndex(i);
                    m_targetProperty = m_elementProperty.FindPropertyRelative("m_target");

                    if (@object == m_targetProperty.objectReferenceValue)
                    {
                        return i;
                    }
                }
                return -1;
            }

            /// <summary>
            /// 创建UI容器
            /// </summary>
            /// <param name="tf"></param>
            /// <param name="list"></param>
            /// <returns></returns>
            private bool CreateContainer(Transform tf, List<GameObject> list)
            {
                bool bResult = true;
                for (int i = 0; i < tf.childCount; i++)
                {
                    var child = tf.GetChild(i);
                    if (child.name.StartsWith("@"))
                    {
                        bResult = !Contain(list, child.name);
                        if (!bResult)
                        {
                            break;
                        }
                        list.Add(child.gameObject); 
                    }
                    if (child.childCount > 0 && null == child.GetComponent<UIReference>())
                    {
                        CreateContainer(child, list);
                    }
                }
                return bResult;
            }

            /// <summary>
            /// 是否包含相同对象名
            /// </summary>
            /// <param name="list"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            private bool Contain(List<GameObject> list, string name)
            {
                bool bContain = false;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].name == name)
                    {
                        bContain = true;
                        break;
                    }
                }

                if (bContain)
                {
                    string tips = string.Format("UI容器里包含相同名字: '{0}'", name);
                    EditorUtility.DisplayDialog("添加操作的UI容器", tips, "知道了");
                }

                return bContain;

            }
        }
    }
}
