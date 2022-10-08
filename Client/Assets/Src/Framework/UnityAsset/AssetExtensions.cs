
namespace Framework
{
    using Event;

    namespace UnityAsset
    {
        public static class AssetExtensions
        {
            /// <summary>
            /// 加载UI
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="action"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static AsyncAsset LoadUI(this AssetManager self, string path, System.Action<bool, AsyncAsset> complete, bool async = true)
            {
                string relativePath = Const.LOADUI + path + ".prefab";
                return LoadAsset(self, relativePath, complete, async, App.abMode);
            }

            /// <summary>
            /// 加载UI
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="action"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static AsyncAsset LoadLua(this AssetManager self, string path, System.Action<bool, AsyncAsset> complete, bool async = true)
            {
                string relativePath = Const.LOADLUA + path + ".bytes";
#if UNITY_EDITOR
                if (!App.abLua)
                {
                    relativePath = "LuaSrc/" + path + ".lua";
                }
#endif
                return LoadAsset(self, relativePath, complete, async, App.abLua);
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static AsyncAsset LoadAsset(this AssetManager self, string path, System.Action<bool, AsyncAsset> complete, bool async = true, bool abModeOrLua = true)
            {
                AsyncAsset asyncAsset = null;
                if (abModeOrLua)
                {
                    path = path.ToLower();
                    if (async)
                    {
                        asyncAsset = self.AssetBundleAsyncLoad(path, complete);
                    }
                    else
                    {
                        asyncAsset = self.AssetBundleLoad(path);
                        complete?.Invoke(string.IsNullOrEmpty(asyncAsset.error), asyncAsset);
                    }
                }
#if UNITY_EDITOR
                else
                {
                    path = "Assets/" + path;
                    EditorAsset unityAsset = new EditorAsset(path);
                    unityAsset.AsyncLoad();

                    asyncAsset = new AsyncAsset(path, unityAsset, complete);
                    asyncAsset.Complete();
                }
#endif
                return asyncAsset;
            }

            /// <summary>
            /// 加载文本资源
            /// </summary>
            /// <param name="self"></param>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            /// <returns></returns>
            public static AsyncAsset LoadFileAsset(this AssetManager self, string path, System.Action<bool, AsyncAsset> complete, bool async = true, bool abModeOrLua = true)
            {
                AsyncAsset asyncAsset = null;
                if (abModeOrLua)
                {
                    path = path.ToLower();
                    if (async)
                    {
                        asyncAsset = self.FileAssetAsyncLoad(path, complete);
                    }
                    else
                    {
                        asyncAsset = self.FileAssetLoad(path);
                        complete?.Invoke(string.IsNullOrEmpty(asyncAsset.error), asyncAsset);
                    }
                }
#if UNITY_EDITOR
                else
                {
                    path = "Assets/" + path;
                    EditorAsset unityAsset = new EditorAsset(path);
                    unityAsset.AsyncLoad();

                    asyncAsset = new AsyncAsset(path, unityAsset, complete);
                    asyncAsset.Complete();
                }
#endif
                return asyncAsset;
            }
        }
    }
}