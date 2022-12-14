using UnityEngine;
using UnityEditor;

public class StyleViewerWindow : EditorWindow
{
    private Vector2 m_scrollPosition = Vector2.zero;
    private string search = string.Empty;

    [MenuItem("Tools/GUI样式查看器")]
    public static void Init()
    {
        EditorWindow.GetWindow(typeof(StyleViewerWindow));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("单击示例将复制其名到剪贴板", "label");
        GUILayout.FlexibleSpace();
        GUILayout.Label("查找:");
        search = EditorGUILayout.TextField(search);
        GUILayout.EndHorizontal();

        m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
        foreach (GUIStyle style in GUI.skin)
        {
            if (style.name.ToLower().Contains(search.ToLower()))
            {
                GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                GUILayout.Space(7);
                if (GUILayout.Button(style.name, style, GUILayout.Width(300)))
                {
                    EditorGUIUtility.systemCopyBuffer = "\"" + style.name + "\"";
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.SelectableLabel("\"" + style.name + "\"");
                GUILayout.EndHorizontal();
                GUILayout.Space(11);
            }
        }
        GUILayout.EndScrollView();
    }
}