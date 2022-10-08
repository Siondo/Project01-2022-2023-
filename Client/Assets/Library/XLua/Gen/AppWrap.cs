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
    public class AppWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(App);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 29, 8);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Update", _m_Update_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "productName", _g_get_productName);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "innerVersion", _g_get_innerVersion);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "version", _g_get_version);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "assetVersion", _g_get_assetVersion);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "url", _g_get_url);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "openGuide", _g_get_openGuide);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "openUpdate", _g_get_openUpdate);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "logEnabled", _g_get_logEnabled);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "logLevel", _g_get_logLevel);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "abMode", _g_get_abMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "abLua", _g_get_abLua);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "debugMode", _g_get_debugMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "checkMode", _g_get_checkMode);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "platformTag", _g_get_platformTag);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "scriptingDefineSymbols", _g_get_scriptingDefineSymbols);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "manifest", _g_get_manifest);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "manifestMapping", _g_get_manifestMapping);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "assetDir", _g_get_assetDir);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "address", _g_get_address);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "newVersionDownloadUrl", _g_get_newVersionDownloadUrl);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "newVersionContent", _g_get_newVersionContent);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "platform", _g_get_platform);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "nextState", _g_get_nextState);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "dataNetwork", _g_get_dataNetwork);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "token", _g_get_token);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "deviceId", _g_get_deviceId);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "deviceBrand", _g_get_deviceBrand);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "host", _g_get_host);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "port", _g_get_port);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "assetVersion", _s_set_assetVersion);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "url", _s_set_url);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "manifest", _s_set_manifest);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "manifestMapping", _s_set_manifestMapping);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "nextState", _s_set_nextState);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "token", _s_set_token);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "host", _s_set_host);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "port", _s_set_port);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new App();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to App constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    AppInfo _data = (AppInfo)translator.GetObject(L, 1, typeof(AppInfo));
                    
                    App.Init( _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _jsonText = LuaAPI.lua_tostring(L, 1);
                    
                    App.Update( _jsonText );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_productName(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.productName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_innerVersion(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.innerVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_version(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.version);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetVersion(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.assetVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_url(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.url);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_openGuide(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.openGuide);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_openUpdate(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.openUpdate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_logEnabled(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.logEnabled);
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
			    translator.Push(L, App.logLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_abMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.abMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_abLua(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.abLua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_debugMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.debugMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_checkMode(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.checkMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_platformTag(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.platformTag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scriptingDefineSymbols(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.scriptingDefineSymbols);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_manifest(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, App.manifest);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_manifestMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, App.manifestMapping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetDir(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.assetDir);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_address(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.address);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_newVersionDownloadUrl(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.newVersionDownloadUrl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_newVersionContent(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.newVersionContent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_platform(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.platform);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_nextState(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.nextState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dataNetwork(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, App.dataNetwork);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_token(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.token);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deviceId(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.deviceId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deviceBrand(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.deviceBrand);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_host(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, App.host);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_port(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, App.port);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_assetVersion(RealStatePtr L)
        {
		    try {
                
			    App.assetVersion = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_url(RealStatePtr L)
        {
		    try {
                
			    App.url = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_manifest(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    App.manifest = (Framework.IO.ManifestConfig)translator.GetObject(L, 1, typeof(Framework.IO.ManifestConfig));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_manifestMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    App.manifestMapping = (Framework.IO.ManifestMappingConfig)translator.GetObject(L, 1, typeof(Framework.IO.ManifestMappingConfig));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_nextState(RealStatePtr L)
        {
		    try {
                
			    App.nextState = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_token(RealStatePtr L)
        {
		    try {
                
			    App.token = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_host(RealStatePtr L)
        {
		    try {
                
			    App.host = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_port(RealStatePtr L)
        {
		    try {
                
			    App.port = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
