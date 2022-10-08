using System.IO;
using UnityEditor;
using UnityEngine;
using SiondoStudio;

namespace Framework
{
    [System.Serializable]
    public class MatchObject
    {
        [SerializeField]
        private string sheet = string.Empty;

        [SerializeField]
        private Object config = null;

        public MatchObject(string _sh, Object _obj)
        {
            sheet = _sh;
            config = _obj;
        }
    }

    [System.Serializable]
    public class MatchConfig : ScriptableObject
    {
        [SerializeField]
        public MatchObject[] configList = new MatchObject[0];

        [MenuItem("Tools/Create/策划配置表")]
        private static void CreateConfig()
        {
            var targetConfigPath = "Assets/Res/Conf";
            var filePath = "Assets/Res/MatchConfig.asset";
            if (AssetDatabase.LoadMainAssetAtPath(filePath))
                AssetDatabase.DeleteAsset(filePath);

            var files = Directory.GetFiles(targetConfigPath);
            foreach (var fileName in files)
            {
                if (fileName.Contains(".meta") || fileName.Contains("lang"))
                    continue;

                //var sheet = Path.GetFileName(fileName).Replace(".json", "");
                //var config = AssetDatabase.LoadAssetAtPath<Object>(fileName);
                //configList.Add(new MatchObject(sheet, null));
                EditorUtils.DeBug(SLogType.Log, fileName);
            }

            EditorUtils.DeBug(SLogType.Log, "策划配置表更新完成");
            AssetDatabase.CreateAsset(CreateInstance<MatchConfig>(), filePath);
            AssetDatabase.Refresh();
        }
    }
}
