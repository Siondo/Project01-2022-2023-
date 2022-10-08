using UnityEditor;

namespace Framework
{
    [InitializeOnLoad]
    public sealed class AppLoad
    {
        static EditorApplication.CallbackFunction update;
        static AppLoad()
        {
            int Cnt = EditorApplication.update.GetInvocationList().Length;
            for (int i = 0; i < Cnt; ++i)
            {
                EditorApplication.update -= Update;
            }
            EditorApplication.update += Update;
        }

        static void Update()
        {
            update?.Invoke();
        }

        public static void AddUpdate(EditorApplication.CallbackFunction e)
        {
            if (null != update)
            {
                RemoveUpdate(e);
                update += e;
            }
            else
            {
                update = e;
            }
        }

        public static void RemoveUpdate(EditorApplication.CallbackFunction e)
        {
            if (null != update)
            {
                int Cnt = update.GetInvocationList().Length;
                for (int i = 0; i < Cnt; ++i)
                {
                    update -= e;
                }
            }
        }
    }
}