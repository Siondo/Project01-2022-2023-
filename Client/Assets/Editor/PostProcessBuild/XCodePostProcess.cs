#if UNITY_EDITOR && UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
	using UnityEditor.XCodeEditor;
#endif

public static class XCodePostProcess
{
	[PostProcessBuild(100)]
	public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if (target == BuildTarget.iOS)
		{

		}
	}
}
#endif