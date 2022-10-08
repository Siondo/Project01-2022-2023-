
namespace Framework
{
	namespace Singleton
	{
		/// <summary>
		/// 单例
		/// </summary>
		public abstract class Singleton<T> where T : class, new()
		{
			#region Function
            /// <summary>
            /// 得到单例
            /// </summary>
            /// <value>The instance.</value>
            public static T instance
			{
				get
				{
					return INSTANCE.g_instance;
				}
			}

			protected Singleton() { Create(); }

            /// <summary>
            /// 创建数据
            /// </summary>
            protected virtual void Create() { UnityEngine.Debug.Log(string.Format("单例 {0} 实例化", typeof(T).Name)); }

            /// <summary>
            /// 清理数据
            /// </summary>
            public virtual void Clear() { }
            #endregion

            sealed class INSTANCE
            {
                #region Variable
                internal static readonly T g_instance = new T();
                #endregion

                #region Function
                static INSTANCE() { }
                #endregion
            }
        }
	}
}