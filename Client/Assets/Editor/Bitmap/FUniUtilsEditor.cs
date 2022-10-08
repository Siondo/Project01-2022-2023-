using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FFramework.UnityEditor
{
	public static class FUniUtilsEditor
	{

		public static void ClearLog()
		{
			var logType = typeof(Editor).Assembly.GetType("UnityEditorInternal.LogEntries");
			logType.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		public static string TrimToUniPath(string str)
		{
			return str.Substring(Application.dataPath.Length - 6);
		}
	}
}
