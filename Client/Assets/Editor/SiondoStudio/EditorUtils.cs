using UnityEngine;

namespace SiondoStudio
{
    public enum SLogType
    {
        Log,  //普通提示
        Warn, //警告提示
        Error //错误提示
    }

    public class EditorUtils
    {
        public static string Title = "编辑器拓展工具";

        public static void DeBug(SLogType type, string message, object[] parmas = null)
        {
            if (parmas != null) message = string.Format(message, parmas);

            switch (type)
            {
                case SLogType.Log:
                    Debug.Log("<color=#238E23>["+ Title + "]</color> <color=#FFFFFF>" + message + "</color>");
                    break;
                case SLogType.Warn:
                    Debug.LogWarning("<color=#FF7F00>["+ Title + "]</color> <color=#FFFFFF>" + message + "</color>");
                    break;
                case SLogType.Error:
                    Debug.LogError("<color=#FF0000>["+ Title + "]</color> <color=#FFFFFF>" + message + "</color>");
                    break;
            }
        }
    }
}
