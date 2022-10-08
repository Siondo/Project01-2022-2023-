using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Framework
{
    /// <summary>
    /// 启动监视器
    /// </summary>
    [CustomEditor(typeof(Launch))]
    public class LaunchEditor : Editor
    {
        enum ChangeType
        {
            ProductName,
            BundleIdentifier,
            Version,
            BundleVersionCode,
        }

        public enum BuildType
        {
            None,
            BuildForApp,
            BuildForUpdate
        }

        /// <summary>
        /// 当前实例对象
        /// </summary>
        private static LaunchEditor m_instance = null;

        /// <summary>
        /// 版本信息
        /// </summary>
        private static LaunchConfig m_config = null;

        /// <summary>
        /// 名字表
        /// </summary>
        private List<string> m_nameList = new List<string>();

        /// <summary>
        /// 目标对象
        /// </summary>
        private Launch m_target;

        /// <summary>
        /// 是否需要保存
        /// </summary>
        private bool m_isNeedSave = false;

        /// <summary>
        /// 执行的方式
        /// </summary>
        private static BuildType m_buildType = BuildType.None;

        /// <summary>
        /// 颜色
        /// </summary>
        private Color m_bgColor = Color.white;
        private Color m_tempColor = Color.white;

        /// <summary>
        /// 得到选择的配置
        /// </summary>
        private LaunchObject getSelectConfig
        {
            get
            {
                int index = m_config.selectIndex > m_config.getList.Length ? m_config.getList.Length - 1 : m_config.selectIndex;
                return m_config.getList[index];
            }
        }

        /// <summary>
        /// 选中的配置用于缓存
        /// </summary>
        private LaunchObject selectConfig
        {
            get; set;
        }

        /// <summary>
        /// 更新名字列表
        /// </summary>
        private void UpdateNameList()
        {
            m_nameList.Clear();
            foreach (var data in m_config.getList)
            {
                m_nameList.Add(data.Name);
            }
        }

        /// <summary>
        /// OnEnable
        /// </summary>
        private void OnEnable()
        {
            m_target = serializedObject.targetObject as Launch;
            // 读取配置数据
            m_config = LaunchConfig.CreateConfig();
            // 更新名字列表
            UpdateNameList();
        }

        /// <summary>
        /// OnInspectorGUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            m_bgColor = GUI.backgroundColor;
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            {
                EditorGUI.BeginChangeCheck();
                {
                    serializedObject.Update();
                    if (null != m_config && m_config.getList.Length > 0)
                    {
                        // 模式
                        var index = m_config.advancedMode ? 1 : 0;
                        string value = string.Empty;
                        var selected = GUILayout.Toolbar(index, new string[] { "编辑器模式", "高级模式" });
                        if (index != selected)
                        {
                            m_isNeedSave = true;
                            m_config.advancedMode = 1 == selected;
                        }
                        selectConfig = getSelectConfig;
                        if (m_config.advancedMode)
                        {
                            // 产品名字
                            value = EditorGUILayout.TextField("Product Name", selectConfig.productName);
                            if (value != selectConfig.productName || !GetSettings(ChangeType.ProductName).Equals(value) || m_target.data.productName != selectConfig.productName)
                            {
                                m_isNeedSave = true;
                                selectConfig.productName = value;
                                ChangeSettings(ChangeType.ProductName, selectConfig.productName);
                            }
                            // 全平台包名
                            value = EditorGUILayout.TextField("Bundle Identifier", selectConfig.bundleIdentifier);
                            if (value != selectConfig.bundleIdentifier || !GetSettings(ChangeType.BundleIdentifier).Equals(value))
                            {
                                m_isNeedSave = true;
                                selectConfig.bundleIdentifier = value;
                                ChangeSettings(ChangeType.BundleIdentifier, selectConfig.bundleIdentifier);
                            }
                            // 版本
                            value = EditorGUILayout.TextField("Version*", selectConfig.version);
                            if (value != selectConfig.version || !GetSettings(ChangeType.Version).Equals(value))
                            {
                                m_isNeedSave = true;
                                selectConfig.version = value;
                                ChangeSettings(ChangeType.Version, selectConfig.version);
                            }

                            // 版本Code
#if UNITY_ANDROID
                            int bundleVersionCode = EditorGUILayout.IntField("Bundle Version Code", selectConfig.bundleVersionCode);
                            if (bundleVersionCode != selectConfig.bundleVersionCode || !GetSettings(ChangeType.BundleVersionCode).Equals(bundleVersionCode))
                            {
                                m_isNeedSave = true;
                                selectConfig.bundleVersionCode = bundleVersionCode;
                                ChangeSettings(ChangeType.BundleVersionCode, selectConfig.bundleVersionCode);
                            }
#elif UNITY_IOS
                            int buildNumber = EditorGUILayout.IntField("BuildNumber", selectConfig.bundleVersionCode);
                            if (buildNumber != selectConfig.bundleVersionCode || !GetSettings(ChangeType.BundleVersionCode).Equals(buildNumber))
                            {
                                m_isNeedSave = true;
                                selectConfig.bundleVersionCode = buildNumber;
                                ChangeSettings(ChangeType.BundleVersionCode, selectConfig.bundleVersionCode);
                            }
#else
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.BeginHorizontal();
                            int buildNumber = EditorGUILayout.IntField("BuildNumber", selectConfig.bundleVersionCode);
                            EditorGUILayout.LabelField("*仅Android或iOS有效");
                            if (buildNumber != selectConfig.bundleVersionCode || !GetSettings(ChangeType.BundleVersionCode).Equals(buildNumber))
                            {
                                m_isNeedSave = true;
                                selectConfig.bundleVersionCode = buildNumber;
                                ChangeSettings(ChangeType.BundleVersionCode, selectConfig.bundleVersionCode);
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.EndDisabledGroup();
#endif
                        }

                        // 资源版本,用于资源更新
                        value = EditorGUILayout.TextField("Asset Version*", selectConfig.assetVersion);
                        if (value != selectConfig.assetVersion || m_target.data.assetVersion != selectConfig.assetVersion)
                        {
                            m_isNeedSave = true;
                            selectConfig.assetVersion = value;
                        }

                        // 平台、宏定义
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.TextField("Platform", EditorUserBuildSettings.activeBuildTarget.ToString());
                        EditorGUI.EndDisabledGroup();
                        GUILayout.BeginHorizontal();
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.TextField("Scripting Define Symbol", selectConfig.scriptingDefineSymbols);
                        value = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                        EditorGUI.EndDisabledGroup();
                        if (selectConfig.scriptingDefineSymbols != value)
                        {
                            //PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, selectConfig.scriptingDefineSymbols);
                        }
                        GUILayout.EndHorizontal();

                        // 设置配置选项组
                        selected = EditorGUILayout.Popup("服务器配置", m_config.selectIndex, m_nameList.ToArray());
                        if (selected != m_config.selectIndex)
                        {
                            m_isNeedSave = true;
                            m_config.selectIndex = selected;
                        }

                        // 脚本
                        EditorGUI.BeginDisabledGroup(true);
                        SerializedProperty property = serializedObject.GetIterator();
                        if (property.NextVisible(true))
                        {
                            EditorGUILayout.PropertyField(property, new GUIContent("Script"), true, new GUILayoutOption[0]);
                        }
                        EditorGUI.EndDisabledGroup();

                        // 其它字段属性
                        EditorGUI.BeginDisabledGroup(selected != 0);
                        {
                            // 中心地址
                            value = EditorGUILayout.TextField("Url", selectConfig.url);
                            if (value != selectConfig.url || m_target.data.url != selectConfig.url)
                            {
                                m_isNeedSave = true;
                                selectConfig.url = value;
                            }

                            // 是否开启引导
                            bool bValue = EditorGUILayout.Toggle("开启新手引导?", selectConfig.openGuide);
                            if (bValue != selectConfig.openGuide || m_target.data.openGuide != selectConfig.openGuide)
                            {
                                m_isNeedSave = true;
                                selectConfig.openGuide = bValue;
                            }

                            // 是否开启更新模式
                            bValue = EditorGUILayout.Toggle("开启资源更新?", selectConfig.openUpdate);
                            if (bValue != selectConfig.openUpdate || m_target.data.openUpdate != selectConfig.openUpdate)
                            {
                                m_isNeedSave = true;
                                selectConfig.openUpdate = bValue;
                            }

                            // 是否开启日志
                            Debugger.LogLevel logLevel = (Debugger.LogLevel)EditorGUILayout.EnumPopup("开启日志&GM工具?", selectConfig.logLevel);
                            if (logLevel != selectConfig.logLevel || m_target.data.logLevel != selectConfig.logLevel)
                            {
                                m_isNeedSave = true;
                                selectConfig.logLevel = logLevel;
                            }

                            // 是否AB_Mode?
                            bValue = EditorGUILayout.Toggle("AB_Mode?", selectConfig.abMode);
                            if (bValue != selectConfig.abMode || m_target.data.abMode != selectConfig.abMode)
                            {
                                m_isNeedSave = true;
                                selectConfig.abMode = bValue;
                            }

                            // 是否AB_LUA?
                            bValue = EditorGUILayout.Toggle("AB_LUA?", selectConfig.abLua);
                            if (bValue != selectConfig.abLua || m_target.data.abLua != selectConfig.abLua)
                            {
                                m_isNeedSave = true;
                                selectConfig.
                                     abLua = bValue;
                            }

                            // 是否DEBUG_MODE?
                            bValue = EditorGUILayout.Toggle("DEBUG_MODE?", selectConfig.debugMode);
                            if (bValue != selectConfig.debugMode || m_target.data.debugMode != selectConfig.debugMode)
                            {
                                m_isNeedSave = true;
                                selectConfig.debugMode = bValue;
                            }

                            // 是否CHECK_MODE?
                            bValue = EditorGUILayout.Toggle("CHECK_MODE?", selectConfig.checkMode);
                            if (bValue != selectConfig.checkMode || m_target.data.checkMode != selectConfig.checkMode)
                            {
                                m_isNeedSave = true;
                                selectConfig.checkMode = bValue;
                            }
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUI.BeginDisabledGroup(selected == 0);
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("复制到自定义...", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                            {
                                m_isNeedSave = true;
                                m_config.getList[0].productName = selectConfig.productName;
                                m_config.getList[0].bundleIdentifier = selectConfig.bundleIdentifier;
                                m_config.getList[0].version = selectConfig.version;
                                m_config.getList[0].assetVersion = selectConfig.assetVersion;
                                m_config.getList[0].bundleVersionCode = selectConfig.bundleVersionCode;
                                m_config.getList[0].scriptingDefineSymbols = selectConfig.scriptingDefineSymbols;
                                m_config.getList[0].url = selectConfig.url;
                                m_config.getList[0].openGuide = selectConfig.openGuide;
                                m_config.getList[0].openUpdate = selectConfig.openUpdate;
                                m_config.getList[0].logLevel = selectConfig.logLevel;
                                m_config.getList[0].abMode = selectConfig.abMode;
                                m_config.getList[0].abLua = selectConfig.abLua;
                                m_config.getList[0].debugMode = selectConfig.debugMode;
                                m_config.getList[0].checkMode = selectConfig.checkMode;
                                m_config.selectIndex = 0;
                            }
                            if (GUILayout.Button("编辑配置...", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                            {
                                Selection.activeObject = m_config;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("清除沙盒资源", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                        {
                            BuildTool.DeletePersistentData();
                        }
                        if (GUILayout.Button("清除Prefs沙盒版本", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                        {
                            PlayerPrefs.DeleteKey(Const.SANDBOX_VERSION);
                        }
                        if (GUILayout.Button("清除Prefs缓存", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                        {
                            PlayerPrefs.DeleteAll();
                        }
                        EditorGUILayout.EndHorizontal();

                        if (m_config.advancedMode)
                        {
                            m_tempColor = Color.yellow;
                            m_tempColor.a = 0.38f;
                            GUI.backgroundColor = m_tempColor;
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("打开沙盒目录", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.38F)))
                            {
                                BuildTool.OpenPersistentData();
                            }
                            if (GUILayout.Button("打开热更目录", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.68F)))
                            {
                                BuildTool.OpenServerDataPath();
                            }
                            if (GUILayout.Button("删除ServerData目录", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.68F)))
                            {
                                BuildTool.DeleteExportDirectory(PathUtil.serverDataPath);
                            }
                            EditorGUILayout.EndHorizontal();
                            m_tempColor = Color.red;
                            m_tempColor.a = 0.23f;
                            GUI.backgroundColor = m_tempColor;
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("打包资源(出包)", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.68F)) && m_buildType == BuildType.None)
                            {
                                m_buildType = BuildType.BuildForApp;
                                AppLoad.AddUpdate(Update);
                            }
                            if (GUILayout.Button("打包资源(热更)", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.68F)) && m_buildType == BuildType.None)
                            {
                                m_buildType = BuildType.BuildForUpdate;
                                AppLoad.AddUpdate(Update);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    if (m_isNeedSave && !Application.isPlaying)
                    {
                        m_isNeedSave = false;
                        m_config.SaveConfig();
                        SaveToLaunch(m_target.data, selectConfig);
                        AssetDatabase.Refresh();
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.EndChangeCheck();
            }
            EditorGUI.EndDisabledGroup();
        }

        void Update()
        {
            if (BuildType.None != m_buildType)
            {
                AppLoad.RemoveUpdate(Update);
                try
                {
                    DateTime lastTime = DateTime.Now;
                    if (m_buildType == BuildType.BuildForApp)
                    {
                        BuildTool.Build();
                    }
                    else if (m_buildType == BuildType.BuildForUpdate)
                    {
                        BuildTool.BuildHotUpdate();
                    }
                    if (BuildTool.buildSuccessful)
                    {
                        Debug.Log(string.Format("[{0}] BUILD SUCCESSFUL，use {1:F2} s!", m_buildType.ToString(), DateTime.Now.Subtract(lastTime).TotalSeconds));
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                m_buildType = BuildType.None;
            }

        }

        /// <summary>
        /// 保存版本配置
        /// </summary>
        public static void SaveToLaunch(AppInfo data, LaunchObject config, bool hasProperties = true)
        {
            if (!Application.isPlaying)
            {
                data.productName = config.productName;
                data.assetVersion = config.assetVersion;
                data.scriptingDefineSymbols = config.scriptingDefineSymbols;
                data.url = config.url;
                data.openGuide = config.openGuide;
                data.openUpdate = config.openUpdate;
                data.logLevel = config.logLevel;
                data.abMode = config.abMode;
                data.abLua = config.abLua;
                data.debugMode = config.debugMode;
                data.checkMode = config.checkMode;
                if (hasProperties && m_instance)
                {
                    m_instance.serializedObject.ApplyModifiedProperties();
                }

                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                EditorSceneManager.SaveOpenScenes();
            }
        }

        /// <summary>
        /// 改变设置
        /// </summary>
        private void ChangeSettings(ChangeType type, object args)
        {
            switch (type)
            {
                case ChangeType.ProductName:
                    {
                        PlayerSettings.productName = args.ToString();
                    }
                    break;
                case ChangeType.BundleIdentifier:
                    {
                        string applicationIdentifier = args.ToString();
#if UNITY_ANDROID
                        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, applicationIdentifier);
#elif UNITY_IOS
                        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, applicationIdentifier);
#else
                        PlayerSettings.applicationIdentifier = applicationIdentifier;
#endif
                    }
                    break;
                case ChangeType.Version:
                    {
                        PlayerSettings.bundleVersion = args.ToString();
                    }
                    break;
                case ChangeType.BundleVersionCode:
                    {
#if UNITY_ANDROID
                        PlayerSettings.Android.bundleVersionCode = (int)args;
#elif UNITY_IOS
                        PlayerSettings.iOS.buildNumber = args.ToString();
#else
                        PlayerSettings.macOS.buildNumber = args.ToString();
#endif
                    }
                    break;
            }
        }

        /// <summary>
        /// 得到设置信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetSettings(ChangeType type)
        {
            object obj = null;

            switch (type)
            {
                case ChangeType.ProductName:
                    {
                        obj = PlayerSettings.productName;
                    }
                    break;
                case ChangeType.BundleIdentifier:
                    {
#if UNITY_ANDROID
                        obj = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
#elif UNITY_IOS
                        obj = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
#else
                        obj = PlayerSettings.applicationIdentifier;
#endif
                    }
                    break;
                case ChangeType.Version:
                    {
                        obj = PlayerSettings.bundleVersion;
                    }
                    break;
                case ChangeType.BundleVersionCode:
                    {
#if UNITY_ANDROID
                        obj = PlayerSettings.Android.bundleVersionCode;
#elif UNITY_IOS
                        obj = int.Parse(PlayerSettings.iOS.buildNumber);
#else
                        obj = int.Parse(PlayerSettings.macOS.buildNumber);
#endif
                    }
                    break;
            }

            return obj;
        }
    }
}