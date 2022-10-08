using Boo.Lang;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
	[CustomPropertyDrawer(typeof(LaunchObject))]
	public class LaunchObjectPropertyDrawer : PropertyDrawer
	{
		struct PropertyName
		{
			public string title;
			public string name;
		}

		const float LABLE_WIDTH = 108;
		const float SPACING_Y = 2;

		readonly PropertyName[] PROPERTYNAME = new PropertyName[]
		{
			new PropertyName(){ title = "Name", name = "m_name" },
			new PropertyName(){ title = "Product Name", name = "m_productName" },
			new PropertyName(){ title = "Bundle Identifier", name = "m_bundleIdentifier" },
			new PropertyName(){ title = "Version*", name = "m_version" },
			new PropertyName(){ title = "Asset Verison*", name = "m_assetVersion" },
			new PropertyName(){ title = "Version Code", name = "m_bundleVersionCode" },
			new PropertyName(){ title = "Define Sysbols", name = "m_scriptingDefineSymbols" },
			new PropertyName(){ title = "Url", name = "m_url" },
			new PropertyName(){ title = "开启新手引导?", name = "m_openGuide" },
			new PropertyName(){ title = "开启资源更新?", name = "m_openUpdate" },
			new PropertyName(){ title = "开启日志&GM工具?", name = "m_logLevel" },
			new PropertyName(){ title = "AB_Mode?", name = "m_abMode" },
			new PropertyName(){ title = "AB_LUA?", name = "m_abLua" },
			new PropertyName(){ title = "Debug_Mode?", name = "m_debugMode" },
			new PropertyName(){ title = "Check_Mode?", name = "m_checkMode" },
		};

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight + SPACING_Y;
			for (int i = 0; i < PROPERTYNAME.Length; ++i)
			{
				SerializedProperty arrayElement = property.FindPropertyRelative(PROPERTYNAME[i].name);
				height += EditorGUI.GetPropertyHeight(arrayElement, GUIContent.none, arrayElement.isExpanded) + SPACING_Y;
			}
			return height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new EditorGUI.PropertyScope(position, label, property))
			{
				Rect rect = new Rect(position)
				{
					x = position.x,
					y = position.y,
					height = EditorGUIUtility.singleLineHeight + SPACING_Y
				};
				Color c = GUI.contentColor;
				GUI.contentColor = Color.green;
				GUI.Label(rect, label);
				GUI.contentColor = c;

				Rect titleRect = new Rect(rect)
				{
					y = rect.y,
					width = LABLE_WIDTH,
				};
				Rect contentRect = new Rect(titleRect)
				{
					x = titleRect.x + titleRect.width,
					width = rect.width - titleRect.width,
				};
				//*************************************************************
				SerializedProperty assetProperty = null;
				for (int i = 0; i < PROPERTYNAME.Length; ++i)
				{
					GUILayout.BeginHorizontal();
					titleRect.y += EditorGUIUtility.singleLineHeight + SPACING_Y;
					contentRect.y += EditorGUIUtility.singleLineHeight + SPACING_Y;

					GUI.Label(titleRect, PROPERTYNAME[i].title);
					assetProperty = property.FindPropertyRelative(PROPERTYNAME[i].name);
					EditorGUI.PropertyField(contentRect, assetProperty, new GUIContent(""));
					GUILayout.EndHorizontal();
				}
			}
		}
	}

	[CustomEditor(typeof(LaunchConfig))]
	public class LaunchConfigInspector : Editor
	{
		private ReorderableList m_reorderableList;
		private void OnEnable()
		{
			SerializedProperty property = serializedObject.FindProperty("m_list");
			m_reorderableList = new ReorderableList(serializedObject, property, true, true, true, true);
			m_reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 4;
			m_reorderableList.drawHeaderCallback = (Rect rect) =>
			{
				GUI.Label(rect, "LaunchConfig");
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
				EditorGUI.PropertyField(rect, item, new GUIContent(string.Format("Index {0}", index)));
			};

			//Delete
			m_reorderableList.onRemoveCallback = (ReorderableList reorderableList) =>
			{
				if (EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
				{
					ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
				}
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			m_reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}