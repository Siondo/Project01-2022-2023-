using UnityEngine;
using UnityEditor;
using System.IO;

namespace Framework
{
    [System.Serializable]
    public class LaunchObject : AppInfo
    {
        [SerializeField]
        private string m_name = string.Empty;

        [SerializeField]
        private string m_bundleIdentifier = string.Empty;

        [SerializeField]
        private string m_version = string.Empty;

        [SerializeField]
        private int m_bundleVersionCode = 1;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string bundleIdentifier
        {
            get { return m_bundleIdentifier; }
            set { m_bundleIdentifier = value; }
        }

        public string version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public int bundleVersionCode
        {
            get { return m_bundleVersionCode; }
            set { m_bundleVersionCode = value; }
        }
    }

    [System.Serializable]
    public class LaunchConfig : ScriptableObject
    {
        [SerializeField]
        public LaunchObject[] m_list = new LaunchObject[] { new LaunchObject() };

        [SerializeField]
        /// <summary>
        /// 选中索引
        /// </summary>
        private int m_selectIndex = 0;


        [SerializeField]
        /// <summary>
        /// 默认非高级模式
        /// </summary>
        private bool m_advancedMode = false;

        public LaunchObject[] getList => m_list;

        public int selectIndex
        {
            get { return m_selectIndex; }
            set { m_selectIndex = value; }
        }

        public bool advancedMode
        {
            get { return m_advancedMode; }
            set { m_advancedMode = value; }
        }

        [MenuItem("Tools/Create/LaunchConfig")]
        public static LaunchConfig CreateConfig()
        {
            string filePath = PathUtil.versionConfigPath;
            LaunchConfig config = AssetDatabase.LoadMainAssetAtPath(filePath) as LaunchConfig;
            if (null == config)
            {
                config = ScriptableObject.CreateInstance<LaunchConfig>();
                var o = config.getList[0];
                o.Name = "自定义";
                o.productName = "一念永恒";
                o.bundleIdentifier = "com.game.forever";
                o.version = "1.0.0";
                o.assetVersion = "1.0.0";
                o.bundleVersionCode = 0;
                o.openGuide = false;
                o.openUpdate = false;
                o.logLevel = Debugger.LogLevel.Log;
                o.abMode = false;
                o.abLua = false;
                o.debugMode = true;
                o.checkMode = false;
                AssetDatabase.CreateAsset(config, filePath);
            }
            return config;
        }

        public void SaveConfig()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        /// <summary>
        /// 得到选中的配置
        /// </summary>
        public LaunchObject getSelectConfig
        {
            get
            {
                int index = selectIndex > getList.Length ? getList.Length - 1 : selectIndex;
                return getList[index];
            }
        }
    }
}