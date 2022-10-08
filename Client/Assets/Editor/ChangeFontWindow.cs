using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using Framework;

public class ChangeFontWindow : EditorWindow
{
    [MenuItem("Tools/更换字体")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(ChangeFontWindow), true);
    }
    public Font toChange;
    static Font toChangeFont;

    void OnGUI()
    {
        toChange = (Font)EditorGUILayout.ObjectField("请选择目标字体", toChange, typeof(Font), true, GUILayout.MinWidth(100));
        toChangeFont = toChange;
        if (GUILayout.Button("确认更换"))
        {
            Change();
        }
    }
    private static string PREFABPATH; //所有预设存放位置路径
    public static void Change()
    {
        PREFABPATH = Application.dataPath + "\\Res\\UI\\Prefab\\new";
        DirectoryInfo directoryInfo = Directory.CreateDirectory(PREFABPATH);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string assetPath = fileInfos[i].FullName.Replace("\\", "/");
            assetPath = assetPath.Replace(Application.dataPath, "Assets");
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (Object text in objs)
            {
                if (text.GetType() == typeof(TextEx))
                {
                    Text TempText = (Text)text;
                    Undo.RecordObject(TempText, TempText.gameObject.name);
                    TempText.font = toChangeFont;
                    EditorUtility.SetDirty(TempText);
                }
            }
        }

    }
}