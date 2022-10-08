#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class FrameworkDebuggerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Framework.Debugger);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LogException", _m_LogException);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LogFormat", _m_LogFormat);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 3, 3);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Start", _m_Start_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "logEnabled", _g_get_logEnabled);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "logLevel", _g_get_logLevel);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "logHandler", _g_get_logHandler);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "logEnabled", _s_set_logEnabled);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "logLevel", _s_set_logLevel);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "logHandler", _s_set_logHandler);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Framework.Debugger does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Start_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Framework.Debugger.LogLevel _logLevel;translator.Get(L, 1, out _logLevel);
                    
                    Framework.Debugger.Start( _logLevel );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogException(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.Debugger gen_to_be_invoked = (Framework.Debugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Exception _exception = (System.Exception)translator.GetObject(L, 2, typeof(System.Exception));
                    UnityEngine.Object _context = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    
                    gen_to_be_invoked.LogException( _exception, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogFormat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.Debugger gen_to_be_invoked = (Framework.Debugger)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.LogType _logType;translator.Get(L, 2, out _logType);
                    UnityEngine.Object _context = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    string _format = LuaAPI.lua_tostring(L, 4);
                    object[] _args = translator.GetParams<object>(L, 5);
                    
                    gen_to_be_invoked.LogFormat( _logType, _context, _format, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_logEnabled(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Framework.Debugger.logEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_logLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Framework.Debugger.logLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_logHandler(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushAny(L, Framework.Debugger.logHandler);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_logEnabled(RealStatePtr L)
        {
		    try {
                
			    Framework.Debugger.logEnabled = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_logLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Framework.Debugger.LogLevel gen_value;translator.Get(L, 1, out gen_value);
				Framework.Debugger.logLevel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_logHandler(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Framework.Debugger.logHandler = (UnityEngine.ILogHandler)translator.GetObject(L, 1, typeof(UnityEngine.ILogHandler));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
