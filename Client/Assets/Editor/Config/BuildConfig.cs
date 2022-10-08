using UnityEngine;
using UnityEditor;
using System.IO;

namespace Framework
{
    [System.Serializable]
    public class BuildObject
    {
        [SerializeField]
        private bool m_select = false;

        [SerializeField]
        private string m_description = string.Empty;

        [SerializeField]
        private Object m_asset = null;

        [SerializeField]
        private string m_searchPattern = string.Empty;

        [SerializeField]
        private SearchOption m_searchOption = SearchOption.TopDirectoryOnly;


        public bool select => m_select;

        public Object asset => m_asset;

        public string searchPattern => m_searchPattern;

        public SearchOption searchOption => m_searchOption;
    }

    [System.Serializable]
    public class BuildConfig : ScriptableObject
    {
        [SerializeField]
        public BuildObject[] m_list = new BuildObject[0];

        public BuildObject[] getList => m_list;

        [MenuItem("Tools/Create/BuildConfig")]
        private static void CreateConfig()
        {
            string filePath = "Assets/BuildConfig.asset";
            if (null == AssetDatabase.LoadMainAssetAtPath(filePath))
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildConfig>(), filePath);
            }
        }
    }
}