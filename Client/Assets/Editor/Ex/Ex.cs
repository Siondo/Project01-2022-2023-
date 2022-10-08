using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Ex
{
    static void CreateEx(string assetPath, int type)
    {
        if (type == 1)
            assetPath = string.Format("Assets/Res/UI/Prefab/Golobal/Ex/{0}.prefab", assetPath);
        else if(type == 2)
            assetPath = string.Format("Assets/Res/UI/Prefab/Golobal/Ex/UItemplate/{0}.prefab", assetPath);

        GameObject activeGameObject = Selection.activeGameObject;
        if (activeGameObject != null)
        {
            Object o = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (o != null)
            {
                GameObject go = GameObject.Instantiate(o) as GameObject;
                go.name = go.name.Substring(0, go.name.Length - "(Clone)".Length);
                go.transform.SetParent(activeGameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                Selection.activeObject = go;
            }
        }
        else SiondoStudio.EditorUtils.DeBug(SiondoStudio.SLogType.Warn, "请选择一个UI父物体");
    }

    [MenuItem("GameObject/UI/Component/ImageEx")]
    static void CreateImageEx()
    {
        CreateEx("ImageEx", 1);
    }

    [MenuItem("GameObject/UI/Component/TextEx")]
    static void CreateTextEx()
    {
        CreateEx("TextEx", 1);
    }

    [MenuItem("GameObject/UI/Component/ButtonEx")]
    static void CreateButtonEx()
    {
        CreateEx("ButtonEx", 1);
    }

    [MenuItem("GameObject/UI/Component/ToggleEx")]
    static void CreateToggleEx()
    {
        CreateEx("ToggleEx", 1);
    }

    [MenuItem("GameObject/UI/Component/SliderEx")]
    static void CreateSliderEx()
    {
        CreateEx("SliderEx", 1);
    }

    [MenuItem("GameObject/UI/Component/InputFieldEx")]
    static void CreateInputFieldEx()
    {
        CreateEx("InputFieldEx", 1);
    }

    [MenuItem("GameObject/UI/Component/AutoExpendIcon-Button")]
    static void CreateToggleExAutoExpendIconButton()
    {
        CreateEx("AutoExpendIcon-Button", 1);
    }

    //UI模板
    [MenuItem("GameObject/UI/UItemplate/PopupTemplate")]
    static void CreateUIPopupTemplate()
    {
        CreateEx("UIPopupTemplate", 2);
    }
}
