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
    public class FrameworkPathUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Framework.PathUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 9, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRealUrl", _m_GetRealUrl_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPath", _m_GetPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Combine", _m_Combine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPathName", _m_GetPathName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFileNameWithoutExtension", _m_GetFileNameWithoutExtension_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetExtension", _m_GetExtension_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "dataPath", _g_get_dataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingAssetsPath", _g_get_streamingAssetsPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "persistentDataPath", _g_get_persistentDataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "serverDataPath", _g_get_serverDataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "manifestConfigPath", _g_get_manifestConfigPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "manifestMappingConfigPath", _g_get_manifestMappingConfigPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "versionConfigPath", _g_get_versionConfigPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "persistentDataUrl", _g_get_persistentDataUrl);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "streamingAssetsUrl", _g_get_streamingAssetsUrl);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Framework.PathUtil();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Framework.PathUtil constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRealUrl_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Framework.PathUtil.GetRealUrl( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Framework.PathUtil.GetPath( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Combine_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path1 = LuaAPI.lua_tostring(L, 1);
                    string _path2 = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = Framework.PathUtil.Combine( _path1, _path2 );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPathName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Framework.PathUtil.GetPathName( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFileNameWithoutExtension_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Framework.PathUtil.GetFileNameWithoutExtension( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtension_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Framework.PathUtil.GetExtension( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.dataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingAssetsPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.streamingAssetsPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_persistentDataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.persistentDataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_serverDataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.serverDataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_manifestConfigPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.manifestConfigPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_manifestMappingConfigPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.manifestMappingConfigPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_versionConfigPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.versionConfigPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_persistentDataUrl(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.persistentDataUrl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_streamingAssetsUrl(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Framework.PathUtil.streamingAssetsUrl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
