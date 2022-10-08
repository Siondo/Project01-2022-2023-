using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Framework
{
    using Event;
    using Singleton;
    namespace UnityAsset
    {
        /// <summary>
        /// 场景管理
        /// </summary>
        public class SpriteManager : MonoBehaviourSingleton<SpriteManager>
        {
            class SpriteEvent : UnityEvent<Sprite> { }
            #region Variable
            /// <summary>
            /// UI精灵列表
            /// </summary>
            Dictionary<string, Sprite> m_sprites;

            /// <summary>
            /// 异步资源
            /// </summary>
            Dictionary<string, AsyncAsset> m_asyncAssets;

            Dictionary<string, SpriteEvent> m_event;
            #endregion

            #region Property
            public Dictionary<string, AsyncAsset> asyncAssets
            {
                get
                {
                    return m_asyncAssets;
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public SpriteManager()
                : base()
            {
                m_sprites = new Dictionary<string, Sprite>(1 << 2);
                m_asyncAssets = new Dictionary<string, AsyncAsset>(1 << 2);
                m_event = new Dictionary<string, SpriteEvent>();
            }

            /// <summary>
            /// 加载精灵
            /// </summary>
            /// <param name="path"></param>
            /// <param name="action"></param>
            public void LoadSprite(string path, UnityAction<Sprite> action)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Debug.LogError("SpriteManager.LoadSprite path == " + path);
                    return;
                }
                if (!m_event.ContainsKey(path))
                {
                    m_event.Add(path, new SpriteEvent());
                }
                m_event[path].AddListener(action);

                //存在精灵直接使用
                if (m_sprites.ContainsKey(path) && null != m_sprites[path])
                {
                    m_event[path].Invoke(m_sprites[path]);
                    m_event[path].RemoveAllListeners();
                }
                //还没加载过此精灵，需要加载使用
                else if (!m_asyncAssets.ContainsKey(path))
                {
                    var asyncAsset = LuaHelper.LoadAssetFromPool(path, (result, async) => {
                        if (result)
                        {
                            Texture2D t2d = async.mainAsset as Texture2D;
                            if (null != t2d)
                            {
                                Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), Vector2.zero);
                                if (m_sprites.ContainsKey(path))
                                {
                                    m_sprites[path] = sprite;
                                }
                                else
                                {
                                    m_sprites.Add(path, sprite);
                                }
                                m_event[path].Invoke(m_sprites[path]);
                                m_event[path].RemoveAllListeners();
                            }
                        }
                    }, true);
                    m_asyncAssets.Add(path, asyncAsset);
                }
            }
            #endregion
        }
    }
}