using Framework;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[CustomEditor(typeof(ImageEx), true)]
[CanEditMultipleObjects]
public class ImageExEditor : ImageEditor
{
    SerializedProperty m_grayColor;

    protected override void OnEnable()
    {
        base.OnEnable();

        m_grayColor = serializedObject.FindProperty("m_grayColor");
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

        EditorGUILayout.PropertyField(m_grayColor);

        serializedObject.ApplyModifiedProperties();
    }
}
