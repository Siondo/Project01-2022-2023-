using UnityEngine;
using UnityEditor;

namespace SiondoStudio
{
    public class EditorTools : Editor
    {
        private readonly static string FbxTargetPath = "Assets/Res/Model/";

        [MenuItem("Assets/FPX模型转UnityPrefab[停用!!!]")]
        [System.Obsolete]
        public static void OnAsyncPrefab()
        {
            var selectionList = Selection.gameObjects;
            if (selectionList.Length > 0)
            {
                var selectionPath = AssetDatabase.GetAssetPath(selectionList[0]);
                if (selectionPath.Contains(FbxTargetPath))
                {
                    for (int i = 0; i < selectionList.Length; i++)
                    {
                        //查找物体
                        var prefabName = selectionList[i].name;
                        var gameObj = AssetDatabase.LoadAssetAtPath<GameObject>(FbxTargetPath + "Fbx/" + prefabName + ".fbx");
                        var fbx = PrefabUtility.InstantiatePrefab(gameObj) as GameObject;
                        fbx.name = "model";
                        fbx.AddComponent<Rigidbody>();

                        //创建空物体, 将模型放入其中
                        var prefab = new GameObject(prefabName);
                        fbx.transform.SetParent(prefab.transform);

                        //创建Prefab
                        var targetPath = FbxTargetPath + prefabName + ".prefab";
                        PrefabUtility.CreatePrefab(targetPath, prefab);
                        DestroyImmediate(prefab);

                        //显示进度
                        EditorUtility.DisplayProgressBar(EditorUtils.Title, string.Format("已经创建 {0}/{1} 正在处理: ({2})", i, selectionList.Length, prefabName), 
                            (float)i / selectionList.Length);//更新进度条的值
                    }

                    EditorUtils.DeBug(SLogType.Log, "成功创建/刷新 {0} 个预制体", new object[] { selectionList.Length });      
                    EditorUtility.ClearProgressBar();
                }
                else EditorUtils.DeBug(SLogType.Error, "必须是此路径下的 '{0}' 格式为FPX的模型", new object[] { FbxTargetPath });
            }
            else EditorUtils.DeBug(SLogType.Error, "至少选一个FPX模型");
        }

    }
}

