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

namespace Framework
{
    using IO;
    using JsonFx;
    public class ExcelTool
    {
        /// <summary>
        /// 更新配置
        /// </summary>
        [MenuItem("Tools/Update/MarkConfig")]
        private static void CreateAtlasAndUpdate()
        {
            string path = Directory.GetCurrentDirectory() + "/Tools/Excel";

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
                string makePath = path + "/Make.bat";
                makePath = Path.GetFullPath(makePath);
                bool b = File.Exists(makePath);
                p.StandardInput.WriteLine(string.Format(@"{0} &exit", makePath));
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

                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();
                //线程运行结束
                runEnd = true;
            });
            thread.Start();

            while (!runEnd)
            {
                Thread.Sleep(20);
            }
            string logPath = path + "/log.txt";
            logPath = Path.GetFullPath(logPath);
            string strOuput = File.ReadAllText(logPath);
            if (strOuput.Contains("Mark fail:"))
            {
                lastLog = "Make Confie failed";
                Thread.Sleep(300);
                Debug.LogError(strOuput);
            }
            else
            {
                lastLog = "Make Config successful";
                Thread.Sleep(300);
                Debug.Log(lastLog);
            }
            AssetDatabase.Refresh();
        }
    }
}
