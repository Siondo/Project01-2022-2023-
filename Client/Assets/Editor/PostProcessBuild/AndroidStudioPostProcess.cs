#if UNITY_EDITOR && UNITY_ANDROID
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

public class AndroidStudioPostProcess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		if (bool.TrueString == PlayerPrefs.GetString("BuildAndroid"))
		{
			return;
		}
		if (!report.summary.outputPath.EndsWith(".apk"))
		{
			//删除目录
			string outputPath = Path.Combine(report.summary.outputPath, Application.productName);
			FileUtil.DeleteFileOrDirectory(outputPath);
		}
	}

	public void OnPostprocessBuild(BuildReport report)
	{
		if (bool.TrueString == PlayerPrefs.GetString("BuildAndroid"))
		{
			return;
		}
		//通过BuildSettings界面导包
		if (!report.summary.outputPath.EndsWith(".apk"))
		{
			//BuildTool.Export(report.summary.outputPath, false);
		}
	}
}
#endif