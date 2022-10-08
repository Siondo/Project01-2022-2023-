using UnityEngine;
using System.IO;
using XLua;

namespace Framework
{
    namespace UI
    {
        /// <summary>
        /// UIBase
        /// </summary>
        [System.Serializable]
        public class UIToLua : UIReference
        {
            /// <summary>
            /// Lua文件路径
            /// </summary>
            [SerializeField] protected string m_path = string.Empty;

            /// <summary>
            /// 得到脚本
            /// </summary>
            /// <returns></returns>
            public LuaTable GetScriptTable()
            {
                string scriptName = gameObject.name;
                if (scriptName.StartsWith("@"))
                {
                    scriptName = scriptName.Substring(1, scriptName.Length - 1);
                }
                if (!(m_path.EndsWith("/") || m_path.EndsWith(@"\")))
                {
                    scriptName = string.Empty;
                }
                LuaTable table = Lua.instance.GetScript(m_path + scriptName);
                table.SetInPath<LuaTable>("ui", GetReferenceTable());
                table.SetInPath("uiBase", this);
                return table;
            }
        }
    }
}