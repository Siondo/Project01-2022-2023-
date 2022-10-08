using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.Networking;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using CSObjectWrapEditor;
using XLua;

public class CmdBuildTool
{
    /// <summary>
    /// 默认的开始场景
    /// </summary>
    const string START_SCENE = "Assets/Res/Scene/Start.unity";

    public static void CmdBuildAndroid()
    {
        string workspace = string.Empty;
        string buildMode = string.Empty;
        string server = string.Empty;
        bool clearCache = false;
        string version = string.Empty;
        string versionCode = string.Empty;
        string assetVersion = string.Empty;
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 12)
        {
            workspace = args[7];
            buildMode = args[8];
            server = args[9];
            bool.TryParse(args[10], out clearCache);
            string[] array = args[11].Split(':');
            version = array[0];
            versionCode = array[1];
            assetVersion = array[2];

            server = server.Split('[')[0];
            var config = Framework.LaunchConfig.CreateConfig();
            int selectIndex = 0;
            if (int.TryParse(server.Replace(" ", ""), out selectIndex))
            {
                config.selectIndex = selectIndex;
            }
            var selectConfig = config.getSelectConfig;
            EditorSceneManager.OpenScene(START_SCENE, OpenSceneMode.Single);
            GameObject go = GameObject.Find(typeof(Launch).Name);
            if (null != go)
            {
                Selection.activeGameObject = go;
                Launch component = go.GetComponent<Launch>();
                if (null != component)
                {
                    //设置版本
                    selectConfig.version = version;
                    PlayerSettings.bundleVersion = selectConfig.version;
                    int bundleVersionCode = 0;
                    if (int.TryParse(versionCode, out bundleVersionCode))
                    {
                        selectConfig.bundleVersionCode = bundleVersionCode;
                    }
#if UNITY_ANDROID
                    PlayerSettings.Android.bundleVersionCode = selectConfig.bundleVersionCode;
#elif UNITY_IOS
                    PlayerSettings.iOS.buildNumber = selectConfig.bundleVersionCode.ToString();
#endif
                    selectConfig.assetVersion = assetVersion;

                    //清理ServerData目录
                    if (clearCache)
                    {
                        Framework.BuildTool.DeleteExportDirectory(Framework.PathUtil.serverDataPath);
                    }
                    Framework.LaunchEditor.SaveToLaunch(component.data, selectConfig);

                    //设置宏定义
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, selectConfig.scriptingDefineSymbols);

                    Debug.LogError("=============== Build Info Start ===============");
                    Debug.Log("workspace: " + workspace);
                    Debug.Log("buildMode: " + buildMode);
                    Debug.Log("server: " + server);
                    Debug.Log("version: " + selectConfig.version);
                    Debug.Log("bundleVersionCode: " + selectConfig.bundleVersionCode);
                    Debug.Log("assetVersion: " + selectConfig.assetVersion);
                    Debug.Log("scriptingDefineSymbols: " + selectConfig.scriptingDefineSymbols);
                    Debug.Log("url: " + selectConfig.url);
                    Debug.LogError("=============== Build Info End ===============");

                    //开始打包
                    Framework.BuildTool.Build(workspace, buildMode);
                }
            }
        }
    }
}

namespace Framework
{
    using IO;
    using JsonFx;
    public class BuildTool
    {
        /// <summary>
        /// AssetBundle File Info
        /// </summary>
        struct ABFI
        {
            public string md5;
            public long size;
        }

        /// <summary>
        /// 打包配置文件
        /// </summary>
        const string BUILD_CONFIG_PATH = "Assets/BuildConfig.asset";

        /// <summary>
        /// 默认的开始场景
        /// </summary>
        const string START_SCENE = "Assets/Res/Scene/Start.unity";

        /// <summary>
        /// 当前Build的目标平台
        /// </summary>
        public static BuildTarget currentBuildTarget => EditorUserBuildSettings.activeBuildTarget;

        /// <summary>
        /// 打包选项
        /// </summary>
        public static BuildAssetBundleOptions assetBundleOptions => BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 执行成功
        /// </summary>
        public static bool buildSuccessful
        {
            get;set;
        }

        /// <summary>
        /// 构建配置
        /// </summary>
        public static BuildConfig buildConfig
        {
            get; set;
        }

        /// <summary>
        /// 工作空间
        /// </summary>
        public static string workspace
        {
            get; set;
        }

        /// <summary>
        /// 打包模式
        /// </summary>
        public static string buildMode
        {
            get; set;
        }

        /// <summary>
        /// 打开持久化路径
        /// </summary>
        public static void OpenPersistentData()
        {
            string persistentDataPath = PathUtil.persistentDataPath.Replace("/", "\\");
            if (Directory.Exists(persistentDataPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", persistentDataPath);
            }
            else
            {
                Debug.LogError(string.Format("路径[{0}]不存在", persistentDataPath));
            }
        }

        /// <summary>
        /// 删除持久化数据
        /// </summary>
        public static void DeletePersistentData()
        {
            if (Directory.Exists(PathUtil.persistentDataPath))
            {
                Directory.Delete(PathUtil.persistentDataPath, true);
            }
        }

        /// <summary>
        /// 打开热更目录
        /// </summary>
        public static void OpenServerDataPath()
        {
            string outputVersionPath = PathUtil.dataPath.Replace("Assets", "ServerData").Replace("/", "\\");
            if (Directory.Exists(outputVersionPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", outputVersionPath);
            }
            else
            {
                Debug.LogError(string.Format("路径[{0}]不存在", outputVersionPath));
            }
        }

        /// <summary>
        /// 删除导出的资源目录
        /// </summary>
        public static void DeleteExportDirectory(string exportPath)
        {
            if (Directory.Exists(exportPath))
            {
                Directory.Delete(exportPath, true);
            }
        }

        /// <summary>
        /// 打平台包
        /// </summary>
        public static void Build(string workspace = "", string buildMode = "")
        {
            BuildTool.workspace = workspace;
            BuildTool.buildMode = buildMode;
            GameObject go = GameObject.Find(typeof(Launch).Name);
            if (null != go)
            {
                Launch c = go.GetComponent<Launch>();
                if (null != c)
                {
#if UNITY_ANDROID
                    BuildAndroid(c.data);
#elif UNITY_IOS
                    BuildIOS(c.data);
#else
                    BuildPC(c.data);
#endif
                }
            }
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="info"></param>
        /// <param name="buildFunc"></param>
        /// <param name="path"></param>
        private static void Build(AppInfo info, Action<string[], string, BuildOptions> buildFunc, string path = "?*")
        {
            buildSuccessful = false;
            bool CommandLine = !string.IsNullOrWhiteSpace(path) && !path.StartsWith("?");
            if (!CommandLine)
            {
                path = path.Replace("?", "");
                string[] array = path.Split('*');
                if (1 == array.Length)
                {
                    path = EditorUtility.SaveFolderPanel("Build " + array[0], Application.dataPath.Replace("Assets", ""), array[0]);
                }
                else if (2 == array.Length)
                {
                    path = EditorUtility.SaveFilePanel("Build " + array[0], Application.dataPath.Replace("Assets", ""), string.Empty, array[1]);
                }
            }
            if (!string.IsNullOrWhiteSpace(path))
            {
                //生成lua桥接文件
                Generator.ClearAll();
                Generator.GenAll();
                //xlua热修复注入
                Hotfix.HotfixInject();
                //资源打包
                BuildAsset(info);

                //构建工程选项
                BuildOptions buildOptions = BuildOptions.None;
                if (info.debugMode)
                {
                    buildOptions = BuildOptions.Development | BuildOptions.SymlinkLibraries;
                    if (currentBuildTarget != BuildTarget.StandaloneWindows)
                    {
                        buildOptions = buildOptions | BuildOptions.ConnectWithProfiler;
                    }
                }

                //打包
                string[] scenes = UpdateScene();
                buildFunc?.Invoke(scenes, path, buildOptions);

                if (!CommandLine)
                {
                    try
                    {
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        proc.StartInfo.FileName = "explorer.exe";
                        //打开资源管理器
                        DirectoryInfo apkOutputInfo = new DirectoryInfo(path);
                        proc.StartInfo.Arguments = @"/select," + apkOutputInfo.FullName;
                        //选中
                        proc.Start();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }

        /// <summary>
        /// PC打包
        /// </summary>
        /// <param name="info"></param>
        /// <param name="path"></param>
        private static void BuildPC(AppInfo info, string path = "?PC")
        {
            Action<string[], string, BuildOptions> buildFunc = (scenes, localtionPathName, buildOptions) => {
                //构建打包目录
                string[] array = PlayerSettings.applicationIdentifier.Split('.');
                string productName = array[array.Length - 1];
                localtionPathName = PathUtil.Combine(localtionPathName, productName);
                if (Directory.Exists(localtionPathName))
                {
                    Directory.Delete(localtionPathName, true);
                }
                Directory.CreateDirectory(localtionPathName);

                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = scenes;
                buildPlayerOptions.locationPathName = PathUtil.Combine(localtionPathName, productName + ".exe");
                buildPlayerOptions.target = currentBuildTarget;
                buildPlayerOptions.options = buildOptions;
                BuildPipeline.BuildPlayer(buildPlayerOptions);
            };
            Build(info, buildFunc, path);
        }

        /// <summary>
        /// 安卓打包
        /// </summary>
        public static void BuildAndroid(AppInfo info, string path = "?Android*apk")
        {
            if (!string.IsNullOrWhiteSpace(workspace))
            {
                path = string.Format("{0}/{1}.apk", workspace, DateTime.Now.Millisecond);
            }
            Action<string[], string, BuildOptions> buildFunc = (scenes, localtionPathName, buildOptions) => {
                EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
                EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
                buildPlayerOptions.scenes = scenes;
                buildPlayerOptions.locationPathName = "Gradle";
                buildPlayerOptions.target = BuildTarget.Android;
                buildPlayerOptions.options = BuildOptions.None;
                BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

                Thread.Sleep(500);
                BuildSummary summary = report.summary;
                if (summary.result == BuildResult.Succeeded)
                {
                    string[] array = PlayerSettings.applicationIdentifier.Split('.');
                    string productName = array[array.Length - 1];
                    string outputPath = PathUtil.Combine(summary.outputPath, productName);
                    if (Directory.Exists(outputPath))
                    {
                        Directory.Delete(outputPath, true);
                    }
                    string srcOutputPath = PathUtil.Combine(summary.outputPath, App.productName);
                    string srcSamplePath = Path.GetFullPath(srcOutputPath);
                    string desSamplePath = Path.GetFullPath(outputPath);
                    if (!srcSamplePath.Equals(desSamplePath))
                    {
                        Directory.Move(srcOutputPath, outputPath);
                    }
                    Export(outputPath, info, localtionPathName);
                }
                else if (summary.result == BuildResult.Failed)
                {
                    Debug.LogError("Build Failed");
                }
            };
            Build(info, buildFunc, path);
        }

        /// <summary>
        /// iOS打包
        /// </summary>
        public static void BuildIOS(AppInfo info, string path = "")
        {
            //Build(info, null, path);
        }

        public static void Export(string outputPath, AppInfo info, string apkPath)
        {
            outputPath = PathUtil.GetPath(outputPath);
            DirectoryInfo outputInfo = new DirectoryInfo(outputPath);
            if (outputInfo.Exists)
            {
                outputInfo.Refresh();
            }
            //删除导出的java路径
            string javaPath = Path.Combine(outputPath, "src/main/java");
            FileUtil.DeleteFileOrDirectory(javaPath);

            //使用模板替换
            string templatePath = Application.dataPath.Replace("Assets", "Gradle/Template").Replace("//", @"\");
            if (Directory.Exists(templatePath))
            {
                string[] filePaths = Directory.GetFiles(templatePath, "*.*", SearchOption.AllDirectories);
                string source = string.Empty, dest = string.Empty, relativePath = string.Empty;
                foreach (var filePath in filePaths)
                {
                    source = filePath.Replace("//", @"\");
                    if (source.Contains(@"\.gradle\") || source.Contains(@"\.idea\") || source.Contains(@"\build\"))
                    {
                        continue;
                    }

                    relativePath = source.Substring(templatePath.Length);
                    relativePath = PathUtil.GetPath(relativePath);
                    dest = outputPath + relativePath;

                    //创建目标目录
                    DirectoryInfo directoryInfo = new DirectoryInfo(dest);
                    CreateDirectory(directoryInfo.Parent);

                    //拷贝文件
                    FileUtil.DeleteFileOrDirectory(dest);
                    FileUtil.CopyFileOrDirectory(source, dest);
                }
            }
            if (outputInfo.Exists)
            {
                outputInfo.Refresh();
            }
            //Android工程替换
            AndroidProjectReplace(info, outputPath);
            //导出apk
            //存在工作空间，则拷贝到工作空间
            if (string.IsNullOrWhiteSpace(workspace))
            {
                ExportApk(outputPath, info, apkPath);
            }
            else
            {
                string assembleFile = info.debugMode ? "assembleDebug.bat" : "assembleRelease.bat";

                string filePath = PathUtil.Combine(workspace, "gradlew.bat");
                //创建目标目录
                DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                CreateDirectory(directoryInfo.Parent);
                var file = File.CreateText(filePath);
                file.WriteLine("@echo off");
                file.WriteLine();
                file.WriteLine("d:");
                file.WriteLine(string.Format("cd {0}", outputPath));
                file.WriteLine(string.Format("call {0}", assembleFile));
                file.Close();
            }
        }

        /// <summary>
        /// 导出APK
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="info"></param>
        /// <param name="apkPath"></param>
        private static void ExportApk(string outputPath, AppInfo info, string apkPath)
        {
            const float TIME = 23;
            string assembleFile = info.debugMode ? "assembleDebug.bat" : "assembleRelease.bat";
            float startTime = Time.realtimeSinceStartup;
            StringBuilder stringBuilder = new StringBuilder();
            string lastLog = string.Empty;
            bool runEnd = false;
            string strOuputError = string.Empty;

            Thread thread = new Thread(() => {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                //设置要启动的应用程序
                startInfo.FileName = "cmd.exe";
                //是否使用操作系统shell启动
                startInfo.UseShellExecute = false;
                // 接受来自调用程序的输入信息
                startInfo.RedirectStandardInput = true;
                //输出信息
                startInfo.RedirectStandardOutput = true;
                // 输出错误
                startInfo.RedirectStandardError = true;
                //不显示程序窗口
                startInfo.CreateNoWindow = true;

                //启动程序
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(startInfo);
                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine(string.Format(@"{0} &exit", Path.Combine(outputPath, assembleFile)));
                p.StandardInput.AutoFlush = true;

                var output = p.StandardOutput;
                bool logStart = false;
                while (!output.EndOfStream)
                {
                    string log = output.ReadLine();
                    if (string.IsNullOrWhiteSpace(log))
                    {
                        continue;
                    }
                    if (log.Contains("&exit"))
                    {
                        logStart = true;
                        continue;
                    }
                    if (logStart)
                    {
                        lastLog = log;
                        stringBuilder.AppendLine(lastLog);
                    }
                    Thread.Sleep(20);
                }
                strOuputError = p.StandardError.ReadToEnd();

                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();
                //线程运行结束
                runEnd = true;
            });
            thread.Start();

            float progress = 0;
            while (!runEnd)
            {
                progress = (Time.realtimeSinceStartup - startTime) / TIME;
                progress = Mathf.Min(progress, 1.0f);
                EditorUtility.DisplayProgressBar("Export APK", lastLog, progress);
                Thread.Sleep(20);
            }

            string strOuput = stringBuilder.ToString();
            if (strOuput.Contains("BUILD SUCCESSFUL in") && !strOuputError.Contains("BUILD FAILED in"))
            {
                DirectoryInfo outputInfo = new DirectoryInfo(outputPath);
                if (outputInfo.Exists)
                {
                    outputInfo.Refresh();
                }
                string[] apkPaths = Directory.GetFiles(outputPath, "*.apk", SearchOption.AllDirectories);
                if (apkPaths.Length > 0)
                {
                    string format = info.debugMode ? "debug" : "release";
                    foreach (var path in apkPaths)
                    {
                        if (path.Contains(format))
                        {
                            FileUtil.DeleteFileOrDirectory(apkPath);
                            FileUtil.CopyFileOrDirectory(path, apkPath);
                            break;
                        }
                    }
                }
                lastLog = "BUILD SUCCESSFUL";
                EditorUtility.DisplayProgressBar("Export APK", lastLog, progress);
                Thread.Sleep(300);
                Debug.Log(lastLog);
                buildSuccessful = true;
            }
            else
            {
                lastLog = "BUILD FAILED";
                EditorUtility.DisplayProgressBar("Export APK", lastLog, progress);
                Thread.Sleep(300);
                Debug.LogError(strOuputError);
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryInfo"></param>
        private static void CreateDirectory(DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                CreateDirectory(directoryInfo.Parent);
                Directory.CreateDirectory(directoryInfo.FullName);
            }
        }

        /// <summary>
        /// Android工程替换
        /// </summary>
        /// <param name="output"></param>
        private static void AndroidProjectReplace(AppInfo info, string output)
        {
            //Android工程替换
            string[] filePaths = Directory.GetFiles(output, "*.*", SearchOption.AllDirectories);
            filePaths = filePaths.Where(f => !f.Contains(@"\build\") && (f.EndsWith("build.gradle") || f.EndsWith("strings.xml") || f.EndsWith("AndroidManifest.xml"))).ToArray();

            //build.gradle替换包名和版本信息
            string path = FindFile(filePaths, "build.gradle");
            string txt = string.Empty;
            if (!string.IsNullOrWhiteSpace(path))
            {
                txt = File.ReadAllText(path);
                RegexReplace(ref txt, "applicationId '.+'", string.Format("applicationId '{0}'", PlayerSettings.applicationIdentifier));
                RegexReplace(ref txt, "versionCode [0-9]+", string.Format("versionCode {0}", PlayerSettings.Android.bundleVersionCode));
                RegexReplace(ref txt, "versionName '.+'", string.Format("versionName '{0}'", PlayerSettings.bundleVersion));
                File.WriteAllText(path, txt);
            }

            //strings.xml替换
            path = FindFile(filePaths, "strings.xml");
            if (!string.IsNullOrWhiteSpace(path))
            {
                txt = File.ReadAllText(path);
                string productName = PlayerPrefs.GetString("ProductName", PlayerSettings.productName);
                RegexReplace(ref txt, "name=\"app_name\">.+<", string.Format("name=\"app_name\">{0}<", productName));
                File.WriteAllText(path, txt);
            }

            //AndroidManifest.xml替换
            path = FindFile(filePaths, "AndroidManifest.xml");
            if (!string.IsNullOrWhiteSpace(path))
            {
                txt = File.ReadAllText(path);
                RegexReplace(ref txt, "package=\".+\"", string.Format("package=\"{0}\"", PlayerSettings.applicationIdentifier));
                RegexReplace(ref txt, "<meta-data android:name=\"jjj.release\" android:value=\".+\" />",
                    string.Format("<meta-data android:name=\"jjj.release\" android:value=\"{0}\" />", info.debugMode ? "False" : "True"));
                File.WriteAllText(path, txt);
            }
        }

        /// <summary>
        /// 找到文件
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string FindFile(string[] filePaths, string fileName)
        {
            string path = string.Empty;
            foreach (var filePath in filePaths)
            {
                if (filePath.EndsWith(fileName) && Path.GetFileName(filePath).Equals(fileName))
                {
                    path = filePath;
                    break;
                }
            }
            return path;
        }

        /// <summary>
        /// 替换规则
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="pattern"></param>
        /// <param name="value"></param>
        private static void RegexReplace(ref string txt, string pattern, string value)
        {
            Match m = Regex.Match(txt, pattern);
            if (m.Success)
            {
                txt = txt.Replace(m.Value, value);
            }
            else
            {
                Debug.LogError(string.Format("{0}: {1}, {2}", pattern, value, txt));
            }
        }

        /// <summary>
        /// 热更新
        /// </summary>
        public static void BuildHotUpdate()
        {
            buildSuccessful = false;
            GameObject go = GameObject.Find(typeof(Launch).Name);
            if (null != go)
            {
                Launch c = go.GetComponent<Launch>();
                if (null != c)
                {
                    //热更新
                    BuildAsset(c.data, true);
                }
            }
        }

        [MenuItem("Tools/Update/AssetBundle")]
        private static void UpdateAssetBundle()
        {
            string scenePath = string.Empty;
            var scene =  EditorSceneManager.GetActiveScene();
            if (!PathUtil.GetPath(scene.path).EndsWith(START_SCENE))
            {
                scenePath = scene.path;
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene(START_SCENE);
            }

            GameObject go = GameObject.Find(typeof(Launch).Name);
            if (null != go)
            {
                Launch c = go.GetComponent<Launch>();
                if (null != c)
                {
                    BuildAsset(c.data);
                }
            }

            if (!string.IsNullOrWhiteSpace(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }

        /// <summary>
        /// 创建图集并更新
        /// </summary>
        [MenuItem("Tools/Update/Atlas")]
        private static void CreateAtlasAndUpdate()
        {
            string textureDirectory = PathUtil.Combine(PathUtil.dataPath, "Res/UI/Texture");
            string[] directory = Directory.GetDirectories(textureDirectory, "*.Atlas", SearchOption.AllDirectories);
            for (int i = 0; i < directory.Length; ++i)
            {
                directory[i] = PathUtil.GetPath(directory[i]);
                string relativePath = directory[i].Replace(PathUtil.dataPath, "Assets");
                string spriteAtlasPath = relativePath.Replace(".Atlas", ".spriteatlas");
                SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(spriteAtlasPath);
                if (null == spriteAtlas)
                {
                    //创建新的图集
                    spriteAtlas = new SpriteAtlas();
                    AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath);

                    SpriteAtlasPackingSettings spriteAtlasPackingSettings = SpriteAtlasExtensions.GetPackingSettings(spriteAtlas);
                    spriteAtlasPackingSettings.enableTightPacking = false;
                    spriteAtlasPackingSettings.padding = 2;
                    spriteAtlasPackingSettings.enableRotation = false;
                    spriteAtlas.SetPackingSettings(spriteAtlasPackingSettings);
                    SpriteAtlasTextureSettings spriteAtlasTextureSettings = SpriteAtlasExtensions.GetTextureSettings(spriteAtlas);
                    spriteAtlasTextureSettings.sRGB = true;
                    SpriteAtlasExtensions.SetIncludeInBuild(spriteAtlas, false);
                    spriteAtlas.SetTextureSettings(spriteAtlasTextureSettings);

                    var obj = AssetDatabase.LoadMainAssetAtPath(relativePath);
                    UnityEngine.Object[] objects = new UnityEngine.Object[] { obj };
                    SpriteAtlasExtensions.Add(spriteAtlas, objects);

                    spriteAtlas.SetIncludeInBuild(true);
                }
            }
            SpriteAtlasUtility.PackAllAtlases(currentBuildTarget);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 打包资源
        /// </summary>
        private static void BuildAsset(AppInfo info, bool hotUpdate = false)
        {
            //更新Lua
            string newDirectory = PathUtil.dataPath + "/Res/Lua";
            string[] paths = Directory.GetFiles(newDirectory, "*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; ++i)
            {
                FileUtil.DeleteFileOrDirectory(paths[i]);
            }
            paths = Directory.GetDirectories(newDirectory, "*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; ++i)
            {
                FileUtil.DeleteFileOrDirectory(paths[i]);
            }
            string luaDirectory = PathUtil.dataPath + "/LuaSrc";
            paths = Directory.GetFiles(luaDirectory, "*.lua", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; ++i)
            {
                string newPath = Path.ChangeExtension(PathUtil.GetPath(paths[i]).Replace(luaDirectory, newDirectory), ".bytes");
                string directory = Path.GetDirectoryName(newPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var bytes = Util.SimpleEncrypt(File.ReadAllBytes(paths[i]));
                File.WriteAllBytes(newPath, bytes);
            }
            //设置图集并Pack图集
            CreateAtlasAndUpdate();

            //打包
            if (!Directory.Exists(PathUtil.serverDataPath))
            {
                Directory.CreateDirectory(PathUtil.serverDataPath);
            }
            //打包资源
            BuildAssetBundleAsset();
            ManifestConfig remote = GetRemoteManifestConfig(info.url);
            string assetVersion = GetNextAssetVersion(remote.assetVersion, info.assetVersion);
            BuildManifestFile(PathUtil.serverDataPath, assetVersion);
            if (!hotUpdate)
            {
                CopyAssetBundle(PathUtil.serverDataPath);
            }
            if (hotUpdate)
            {
                string zipPath = CopyUpdateAssetBundle(PathUtil.serverDataPath, remote, assetVersion);
                if (!string.IsNullOrEmpty(zipPath))
                {
                    Debug.Log(string.Format("HOT UPDATE ASSET PATH [{0}]", zipPath));
                }
            }
            DeleteManifestFile();
            Debug.Log("Build AssetBundles Finished!");
            buildSuccessful = true;
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        /// <returns></returns>
        private static string[] UpdateScene()
        {
            string dataPath = PathUtil.dataPath;
            string path = PathUtil.Combine(dataPath, "Res/Scene");
            string[] scene = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(scene.Length);
            for (int i = 0; i < scene.Length; ++i)
            {
                path = PathUtil.GetPath(scene[i]).Replace(dataPath, "Assets");
                if (PathUtil.GetPath(path).EndsWith(START_SCENE))
                {
                    scenes.Insert(0, new EditorBuildSettingsScene(path, true));
                }
                else
                {
                    scenes.Add(new EditorBuildSettingsScene(path, true));
                }
            }
            EditorBuildSettings.scenes = scenes.ToArray();

            return new string[] { START_SCENE };
        }

        /// <summary>
        /// 得到MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static ABFI GetABFI(string path)
        {
            ABFI ab = new ABFI();
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                ab.size = Mathf.CeilToInt((fs.Length / 1024f));
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                ab.md5 = sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ab;
        }

        /// <summary>
        /// 得到清单文件
        /// </summary>
        /// <returns></returns>
        private static ManifestConfig GetManifest(string output)
        {
            string filePath = PathUtil.GetPath(output);
            filePath += "/" + filePath.Split('/')[filePath.Split('/').Length - 1];
            ManifestConfig manifestConfig = null;
            if (File.Exists(filePath))
            {
                manifestConfig = new ManifestConfig();
                var bundle = AssetBundle.LoadFromFile(filePath);
                AssetBundleManifest abManifest = bundle.LoadAsset("assetbundlemanifest") as AssetBundleManifest;
                string[] bundleNames = abManifest.GetAllAssetBundles();
                for (int i = 0; i < bundleNames.Length; ++i)
                {
                    Manifest manifest = new Manifest();
                    manifest.name = bundleNames[i];
                    ABFI ab = GetABFI(PathUtil.Combine(output, bundleNames[i]));
                    manifest.MD5 = ab.md5;
                    manifest.size = ab.size;
                    foreach (var dependenciesName in abManifest.GetDirectDependencies(bundleNames[i]))
                    {
                        manifest.dependencies.Add(dependenciesName);
                    }
                    manifestConfig.Add(manifest);
                }
                bundle.Unload(true);
            }

            // 添加图集依赖
            List<string> temp = new List<string>();
            int index = 0;
            filePath = PathUtil.dataPath + "/res/ui/prefab";
            string[] array = Directory.GetFiles(filePath, "*.prefab", SearchOption.AllDirectories);
            for (int i = 0; i < array.Length; ++i)
            {
                temp.Clear();
                array[i] = PathUtil.GetPath(array[i]);
                array[i] = array[i].Replace(PathUtil.dataPath, "Assets");
                string[] dep = AssetDatabase.GetDependencies(array[i]);
                for (int j = 0; j < dep.Length; ++j)
                {
                    if (dep[j].Contains("Assets/Res/UI/Texture/") && dep[j].Contains(".Atlas/") && (dep[j].EndsWith(".png") || dep[j].EndsWith(".jpg")))
                    {
                        index = dep[j].IndexOf(".Atlas/");
                        dep[j] = (dep[j].Substring(0, index) + "." + Const.ASSETBUNDLEVARIANT).Replace("Assets/", "").ToLower();
                        if (!temp.Contains(dep[j]))
                        {
                            temp.Add(dep[j]);
                        }
                    }
                }
                if (temp.Count > 0)
                {
                    array[i] = array[i].Replace("Assets/", "").Replace(".prefab", "." + Const.ASSETBUNDLEVARIANT).ToLower();
                    var data = manifestConfig.Get(array[i]);
                    if (data != null)
                    {
                        foreach (var v in temp)
                        {
                            if (!data.dependencies.Contains(v))
                            {
                                data.dependencies.Add(v);
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError(string.Format("为预制体添加依赖错误：{0}", array[i]));
                    }
                }
            }
            return manifestConfig;
        }

        /// <summary>
        /// 得到资源版本
        /// </summary>
        /// <returns></returns>
        private static long GetAssetVersion()
        {
            long version = 100000000;
            version *= DateTime.Now.Year;
            version += DateTime.Now.Month * 1000000 + DateTime.Now.Day * 10000 + DateTime.Now.Hour * 100 + DateTime.Now.Minute;
            return version;
        }

        /// <summary>
        /// 打包AB资源
        /// </summary>
        private static void BuildAssetBundleAsset()
        {
            //根据BuildConfig添加到组
            if (null == buildConfig)
            {
                buildConfig = AssetDatabase.LoadAssetAtPath<BuildConfig>(BUILD_CONFIG_PATH);
            }
            foreach (var config in buildConfig.getList)
            {
                if (config.select)
                {
                    string path = AssetDatabase.GetAssetPath(config.asset);
                    path = PathUtil.GetPath(PathUtil.Combine(PathUtil.dataPath, path.Substring(7)));
                    List<string> filePath = new List<string>(Directory.GetFiles(path, "*", config.searchOption));
                    filePath = filePath.FindAll((arg1) => { return !arg1.EndsWith(".meta"); });
                    for (int i = 0; i < filePath.Count; ++i)
                    {
                        filePath[i] = PathUtil.GetPath(filePath[i]).Replace(PathUtil.dataPath, "Assets");
                    }

                    string[] patternArray = config.searchPattern.Split(';');
                    List<string> patternList = new List<string>(patternArray);
                    for (int i = 0; i < patternList.Count; ++i)
                    {
                        if (patternList[i].StartsWith("~"))
                        {
                            string temp = patternList[i];
                            patternList.RemoveAt(i);
                            patternList.Insert(0, temp);
                        }
                    }
                    patternArray = patternList.ToArray();
                    foreach (var pattern in patternArray)
                    {
                        if (!string.IsNullOrEmpty(pattern))
                        {
                            string[] array = pattern.Split(':');
                            string patternDirectory = 1 == array.Length ? string.Empty : array[0];
                            for (int i = filePath.Count - 1; i >= 0; --i)
                            {
                                //剔除文件 ~test.prefab 或 ~.prefab
                                if (1 == array.Length && array[0].StartsWith("~") && filePath[i].EndsWith(array[0].Substring(1)))
                                {
                                    filePath.RemoveAt(i);
                                    continue;
                                }
                                //剔除文件夹 ~:Test
                                else if (2 == array.Length && array[0].Equals("~") && filePath[i].Contains("/" + array[1] + "/"))
                                {
                                    filePath.RemoveAt(i);
                                    continue;
                                }
                                if ((1 == array.Length && filePath[i].EndsWith(array[0])) ||
                                    (2 == array.Length && filePath[i].Contains("/" + array[0] + "/") && filePath[i].EndsWith(array[1])) ||
                                    (3 == array.Length && filePath[i].Contains("/" + array[0] + "/") && filePath[i].EndsWith(array[1])))
                                {
                                    AssetImporter importer = AssetImporter.GetAtPath(filePath[i]);
                                    string assetBundleName = filePath[i];
                                    if (3 == array.Length && !string.IsNullOrEmpty(array[2]))
                                    {
                                        int index = assetBundleName.IndexOf("/" + array[0] + "/");
                                        assetBundleName = assetBundleName.Substring(0, index);
                                        assetBundleName = PathUtil.Combine(assetBundleName, array[2]);
                                    }
                                    string extension = PathUtil.GetExtension(assetBundleName);
                                    if (!string.IsNullOrEmpty(extension))
                                    {
                                        assetBundleName = assetBundleName.Replace(extension, "");
                                    }
                                    extension = Const.ASSETBUNDLEVARIANT;
                                    assetBundleName = assetBundleName.Replace("Assets/", "");
                                    importer.SetAssetBundleNameAndVariant(assetBundleName, extension);
                                    filePath.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
            }
            buildConfig = null;
            AssetDatabase.SaveAssets();
            BuildPipeline.BuildAssetBundles(PathUtil.serverDataPath, assetBundleOptions, currentBuildTarget);
            RemoveAssetBundleName();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 移除AssetBundleName
        /// </summary>
        private static void RemoveAssetBundleName()
        {
            string[] names = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < names.Length; ++i)
            {
                AssetDatabase.RemoveAssetBundleName(names[i], true);
            }
        }

        /// <summary>
        /// 清单文件
        /// </summary>
        /// <param name="assetVersion"></param>
        private static void BuildManifestFile(string output, string assetVersion)
        {
            ManifestConfig manifestConfig = GetManifest(output);
            // 写入Manifest
            if (manifestConfig != null)
            {
                // *************************************************************************
                // 生成映射表
                ManifestMappingConfig config = new ManifestMappingConfig();
                bool START = false;
                foreach (var path in manifestConfig.data.Keys)
                {
                    string[] lines = File.ReadAllLines(output + "/" + path + ".manifest");
                    START = false;

                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Assets:"))
                        {
                            START = true;
                            continue;
                        }
                        else if (line.StartsWith("Dependencies:"))
                        {
                            break;
                        }
                        if (START)
                        {
                            config.Add(PathUtil.GetPath(line).Replace("- Assets/", "").ToLower(), path);
                        }
                    }
                }
                File.WriteAllText(PathUtil.manifestMappingConfigPath, JsonWriter.Serialize(config));
                // 刷新
                AssetDatabase.Refresh();
                // Build清单映射文件
                AssetBundleBuild[] builds = new AssetBundleBuild[1];
                builds[0].assetBundleName = "manifestmapping";
                builds[0].assetBundleVariant = null;
                builds[0].assetNames = new string[1] { PathUtil.manifestMappingConfigPath.Replace(PathUtil.dataPath, "Assets") };
                BuildPipeline.BuildAssetBundles(output, builds, assetBundleOptions, currentBuildTarget);
                // *************************************************************************
                // 写入到文件
                Manifest manifest = new Manifest();
                manifest.name = "manifestmapping";
                ABFI ab = GetABFI(Path.Combine(output, manifest.name));
                manifest.MD5 = ab.md5;
                manifest.size = ab.size;
                manifestConfig.Add(manifest);
                manifestConfig.version = App.version;
                manifestConfig.assetVersion = assetVersion;
                File.WriteAllText(PathUtil.manifestConfigPath, JsonWriter.Serialize(manifestConfig));
                // 刷新
                AssetDatabase.Refresh();
                // Build清单文件
                builds = new AssetBundleBuild[2];
                builds[0].assetBundleName = "manifest";
                builds[0].assetBundleVariant = null;
                builds[0].assetNames = new string[1] { PathUtil.manifestConfigPath.Replace(PathUtil.dataPath, "Assets") };
                BuildPipeline.BuildAssetBundles(output, builds, assetBundleOptions, currentBuildTarget);
            }
        }

        /// <summary>
        /// 拷贝资源包
        /// </summary>
        private static void CopyAssetBundle(string output)
        {
            // 拷贝资源
            if (Directory.Exists(PathUtil.streamingAssetsPath))
            {
                FileUtil.DeleteFileOrDirectory(PathUtil.streamingAssetsPath);
            }

            AssetDatabase.Refresh();
            FileUtil.CopyFileOrDirectory(output, PathUtil.streamingAssetsPath);
            string[] filePaths = Directory.GetFiles(PathUtil.streamingAssetsPath, "*.manifest", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                FileUtil.DeleteFileOrDirectory(filePath);
            }
            string destPath = PathUtil.Combine(PathUtil.streamingAssetsPath, Util.GetPlatform());
            if (File.Exists(destPath))
            {
                FileUtil.DeleteFileOrDirectory(destPath);
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 得到远程清单文件
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <returns></returns>
        private static ManifestConfig GetRemoteManifestConfig(string remoteUrl)
        {
            ManifestConfig remote = new ManifestConfig();

            string secrect = "37574B9964DE8D1B5D0DDEAF8C64321G";
            string uid = "0";
            string random32 = Util.Get32Random();
            string nocestime = Util.Now().ToString();
            const string GET_APP_INFO = "/configure/info";
            string version = App.version;

            string str = Util.GetStringBuilder(string.Empty, uid, random32, nocestime, secrect);
            string md5Str = Util.GetTextMD5(str);
            md5Str = md5Str.ToUpper();

            Dictionary<string, string> requestHeader = new Dictionary<string, string>();
            requestHeader.Add("platform", App.platform);
            requestHeader.Add("version", version);
            requestHeader.Add("uid", uid);
            requestHeader.Add("deviceId", App.deviceId);
            requestHeader.Add("channel", string.Empty);
            requestHeader.Add("deviceBrand", App.deviceBrand);
            requestHeader.Add("nonce", random32);
            requestHeader.Add("noncestime", nocestime);
            requestHeader.Add("signature", md5Str);

            WWWForm form = new WWWForm();
            remoteUrl += GET_APP_INFO;
            using (UnityWebRequest webRequest = UnityWebRequest.Post(remoteUrl, form))
            {
                webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                foreach (var v in requestHeader)
                {
                    webRequest.SetRequestHeader(v.Key, v.Value);
                }
                webRequest.SendWebRequest();
                while (!webRequest.isDone) ;
                if (webRequest.responseCode != 200L || webRequest.isHttpError || webRequest.isNetworkError || !string.IsNullOrWhiteSpace(webRequest.error))
                {
                    Debug.LogErrorFormat("URL:[{0}] Fail! ", remoteUrl);
                    return remote;
                }
                else
                {
                    Dictionary<string, object> dict = JsonReader.Deserialize<Dictionary<string, object>>(webRequest.downloadHandler.text);
                    if (dict.ContainsKey("status") && (int)dict["status"] == 200)
                    {
                        dict = dict["data"] as Dictionary<string, object>;
                        remoteUrl = dict["url"].ToString();
                    }
                    else
                    {
                        return remote;
                    }
                }
            }

            string verInfoUrl = PathUtil.Combine(remoteUrl, Util.GetPlatform()) + "/v" + version + ".json";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(verInfoUrl))
            {
                webRequest.SendWebRequest();
                while (!webRequest.isDone) ;
                if (webRequest.responseCode != 200L || webRequest.isHttpError || webRequest.isNetworkError || !string.IsNullOrWhiteSpace(webRequest.error))
                {
                    Debug.LogErrorFormat("URL:[{0}] Fail! ", verInfoUrl);
                    return remote;
                }
                else
                {
                    Dictionary<string, object> dict = JsonReader.Deserialize<Dictionary<string, object>>(webRequest.downloadHandler.text);
                    if (dict.ContainsKey("assetDir"))
                    {
                        string assetDir = dict["assetDir"].ToString();
                        remoteUrl = PathUtil.Combine(remoteUrl, Util.GetPlatform()) + "/" + assetDir;
                    }
                }
            }

            // 获取远程清单
            if (!string.IsNullOrEmpty(remoteUrl))
            {
                string url = remoteUrl + "/manifest";
                var webRequest = UnityWebRequest.Get(url);
                webRequest.SendWebRequest();
                while (!webRequest.isDone) ;
                if (string.IsNullOrEmpty(webRequest.error) && webRequest.downloadProgress == 1F)
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromMemory(webRequest.downloadHandler.data);
                    TextAsset text = assetBundle.LoadAsset(Path.GetFileNameWithoutExtension(url)) as TextAsset;
                    remote = JsonReader.Deserialize<ManifestConfig>(text.text);
                    assetBundle.Unload(true);
                }
                webRequest.Dispose();
            }
            return remote;
        }

        /// <summary>
        /// 得到下一个资源版本
        /// </summary>
        /// <param name="remoteAssetVersion"></param>
        /// <returns></returns>
        private static string GetNextAssetVersion(string remoteAssetVersion, string localAssetVersion)
        {
            string assetVersion = string.Empty;
            // 如果远程版本与当前设定资源版本一样则，自动加一个版本
            if (string.IsNullOrEmpty(remoteAssetVersion))
            {
                remoteAssetVersion = localAssetVersion;
            }
            long aV = 0;
            if (long.TryParse(remoteAssetVersion, out aV))
            {
                assetVersion = (++aV).ToString();
            }
            else
            {
                string[] lV = assetVersion.Split('.');
                string[] rV = remoteAssetVersion.Split('.');
                if (lV.Length == rV.Length && !assetVersion.Equals(remoteAssetVersion))
                {
                    for (int i = 0; i < rV.Length; ++i)
                    {
                        if (int.Parse(lV[i]) > int.Parse(rV[i]))
                        {
                            assetVersion = string.Join(".", lV);
                            break;
                        }
                        else if (int.Parse(lV[i]) == int.Parse(rV[i]))
                        {
                            continue;
                        }
                        rV[i] = (int.Parse(rV[rV.Length - 1]) + 1).ToString();
                        assetVersion = string.Join(".", rV);
                    }
                }
                else
                {
                    rV[rV.Length - 1] = (int.Parse(rV[rV.Length - 1]) + 1).ToString();
                    assetVersion = string.Join(".", rV);
                }
            }
            return assetVersion;
        }

        /// <summary>
        /// 拷贝更新资源包
        /// </summary>
        private static string CopyUpdateAssetBundle(string output, ManifestConfig remote, string assetVersion)
        {
            string version = App.version;
            string dest = output.Substring(0, output.Length - Util.GetPlatform().Length) + "UpdateData/" + Util.GetPlatform() + "/v" + version;
            string zipPath = string.Empty;

            ManifestConfig local = JsonReader.Deserialize<ManifestConfig>(File.ReadAllText(PathUtil.manifestConfigPath));
            if (local != null)
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                AssetDatabase.Refresh();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (ZipOutputStream zip = new ZipOutputStream(stream))
                    {
                        zip.SetComment(string.Format("version:{0}\nassetVersion:{1}", version, assetVersion));
                        Action<string> action = (name) =>
                        {
                            ZipEntry entry = new ZipEntry("v" + version + "/" + name);
                            entry.DateTime = new DateTime();
                            entry.DosTime = 0;
                            zip.PutNextEntry(entry);

                            string filepPath = output + "/" + name;
                            var bytes = File.ReadAllBytes(filepPath);
                            zip.Write(bytes, 0, bytes.Length);
                        };
                        action("manifest");
                        foreach (var data in local.data.Values)
                        {
                            if (remote.Contains(data.name) && remote.Get(data.name).MD5 == data.MD5)
                            {
                                continue;
                            }
                            action(data.name);
                        }
                        zip.Finish();
                        zip.Flush();

                        var fileBytes = new byte[stream.Length];
                        Array.Copy(stream.GetBuffer(), fileBytes, fileBytes.Length);

                        string md5 = Util.GetMD5(fileBytes);
                        zipPath = string.Format("{0}/{1}_{2}.zip", dest, assetVersion, md5);
                        File.WriteAllBytes(zipPath, fileBytes);
                    }
                }
            }
            return zipPath;
        }

        /// <summary>
        /// 删除清单文件
        /// </summary>
        private static void DeleteManifestFile()
        {
            // 删除文件
            //FileUtil.DeleteFileOrDirectory(PathUtil.manifestConfigPath);
            //FileUtil.DeleteFileOrDirectory(PathUtil.manifestMappingConfigPath);
        }
    }
}
