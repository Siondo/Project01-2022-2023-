using System.IO;
using UnityEngine;

namespace Framework
{
    public class PathUtil
    {
        /// <summary>
        /// 得到资源文件夹路径(绝对路径,以Assets目录结尾)
        /// </summary>
        public static string dataPath => GetPath(Application.dataPath);

        /// <summary>
        /// StreamingAssets文件夹路径(绝对路径,以StreamingAssets目录结尾)
        /// </summary>
        public static string streamingAssetsPath => GetPath(Application.streamingAssetsPath);

        /// <summary>
        /// 得到持久化路径
        /// </summary>
        public static string persistentDataPath => GetPath(Application.persistentDataPath);

        /// <summary>
        /// 打包资源
        /// </summary>
        public static string serverDataPath => dataPath.Replace("Assets", "ServerData/") + Util.GetPlatform();

        /// <summary>
        /// 清单文件配置路径
        /// </summary>
        public static string manifestConfigPath => dataPath + "/Manifest.json";

        /// <summary>
        /// 清单映射文件配置路径
        /// </summary>
        public static string manifestMappingConfigPath => dataPath + "/ManifestMapping.json";

        /// <summary>
        /// 获取版本信息配置路径
        /// </summary>
        public static string versionConfigPath => "Assets/LaunchConfig.asset";

        /// <summary>
        /// 持久化数据URL
        /// </summary>
        public static string persistentDataUrl
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return "jar:file://" + persistentDataPath + "/";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return "file://" + persistentDataPath + "/";
                }
                else
                {
                    return "file:///" + persistentDataPath + "/";
                }
            }
        }

        /// <summary>
        /// 流式数据URL
        /// </summary>
        public static string streamingAssetsUrl
        {
            get
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return streamingAssetsPath + "/";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return "file://" + streamingAssetsPath + "/";
                }
                else
                {
                    return "file://" + streamingAssetsPath + "/";
                }
            }
        }

        /// <summary>
        /// 得到真实的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRealUrl(string path)
        {
            if (path.StartsWith("http"))
            {
                return path;
            }
            // 选择从沙盒路径还是流式路径加载清单
            path = path.Replace("assets/", "");
            bool sandbox = File.Exists(Path.Combine(persistentDataPath, path));
            return Path.Combine((sandbox ? persistentDataPath : streamingAssetsPath), path);
        }

        /// <summary>
        /// 得到一致性路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPath(string path)
        {
            return path.Replace(@"\", @"/").Replace(@"\\", @"/");
        }

        /// <summary>
        /// 连接路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 得到路径名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 得到文件名不带扩展
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }


        /// <summary>
        /// 得到扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);   
        }

    }
}
