using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

namespace Framework
{
    public class Util
    {
        /// <summary>
        /// 目标平台名
        /// </summary>
        public static string GetPlatform()
        {
            string platform = "PC";
#if UNITY_ANDROID
            platform = "Android";
#elif UNITY_IOS
            platform = "iOS";
#endif
            return platform;
        }

        /// <summary>
        /// 简单的加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] SimpleEncrypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = (byte)(byte.MaxValue - bytes[i]);
            }
            return bytes;
        }

        /// <summary>
        /// 简单的解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] SimpleDecrypt(byte[] bytes)
        {
            return SimpleEncrypt(bytes);
        }

        /// <summary>
        /// 字符串简单加密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string SimpleEncrypt(string encrypt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(encrypt);
            bytes = SimpleEncrypt(bytes);
            encrypt = Encoding.Unicode.GetString(bytes);

            return encrypt;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string SimpleDecrypt(string encrypt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(encrypt);
            bytes = SimpleDecrypt(bytes);
            encrypt = Encoding.Unicode.GetString(bytes);
            return encrypt;
        }

        /// <summary>
        /// 写入字节到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 写入文本到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void WriteAllText(string path, string text)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, text);
        }

        /// <summary>
        /// 得到MD5值
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMD5(byte[] bytes)
        {
            string md5Value = string.Empty;
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytes);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                md5Value = sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return md5Value;
        }

        /// <summary>
        /// 得到MD5值
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMD5(string file)
        {
            if (File.Exists(file))
            {
                return GetMD5(File.ReadAllBytes(file));
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到字符串的MD5
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string GetTextMD5(string txt)
        {
            if (txt.Length > 0)
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(txt);
                return GetMD5(bytes);
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到持久化字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(GetTextMD5(key), defaultValue);
        }

        /// <summary>
        /// 得到持久化整型数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(GetTextMD5(key), defaultValue);
        }

        /// <summary>
        /// 得到持久化浮点数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(GetTextMD5(key), defaultValue);
        }

        /// <summary>
        /// 得到持久化布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetString(GetTextMD5(key), defaultValue.ToString()).Equals(bool.TrueString);
        }

        /// <summary>
        /// 设置持久化字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(GetTextMD5(key), value);
        }

        /// <summary>
        /// 设置持久化整型数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(GetTextMD5(key), value);
        }

        /// <summary>
        /// 设置持久化浮点数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(GetTextMD5(key), value);
        }

        /// <summary>
        /// 设置持久化布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetString(GetTextMD5(key), value.ToString());
        }

        /// <summary>
        /// 删除一条持久化数据
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(GetTextMD5(key));
        }

        /// <summary>
        /// 删除所有持久化数据
        /// </summary>
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 保存持久化数据
        /// </summary>
        public static void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 生成32位随机数
        /// </summary>
        /// <returns></returns>
        public static string Get32Random()
        {
            string random32 = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 32);
            return random32;
        }

        /// <summary>
        /// 得到字符串
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetStringBuilder(params string[] args)
        {
            int len = args.Length;
            StringBuilder builder = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                builder.Append(args[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 获取当前秒数
        /// </summary>
        /// <returns></returns>
        public static Int32 Now()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime DateNow()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 得到设备唯一ID
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceUniqueIdentifier()
        {
            string deviceId = string.Empty;
            string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            if (deviceUniqueIdentifier.Length == 32)
            {
                deviceId += deviceUniqueIdentifier.Substring(0, 8);
                deviceId += "-" + deviceUniqueIdentifier.Substring(8, 4);
                deviceId += "-" + deviceUniqueIdentifier.Substring(12, 4);
                deviceId += "-" + deviceUniqueIdentifier.Substring(16, 4);
                deviceId += "-" + deviceUniqueIdentifier.Substring(20, 12);
            }
            return deviceId;
        }
    }
}
