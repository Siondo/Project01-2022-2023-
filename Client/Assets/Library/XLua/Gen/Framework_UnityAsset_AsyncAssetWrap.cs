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
    public class FrameworkUnityAssetAsyncAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Framework.UnityAsset.AsyncAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 9, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Complete", _m_Complete);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unload", _m_Unload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAllAssets", _m_LoadAllAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Instantiate", _m_Instantiate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Destroy", _m_Destroy);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "assetName", _g_get_assetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "url", _g_get_url);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDone", _g_get_isDone);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "progress", _g_get_progress);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "error", _g_get_error);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "text", _g_get_text);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bytes", _g_get_bytes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mainAsset", _g_get_mainAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "userData", _g_get_userData);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "userData", _s_set_userData);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 4 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<Framework.UnityAsset.UnityAsyncAsset>(L, 3) && translator.Assignable<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 4))
				{
					string _assetName = LuaAPI.lua_tostring(L, 2);
					Framework.UnityAsset.UnityAsyncAsset _unityAsyncAsset = (Framework.UnityAsset.UnityAsyncAsset)translator.GetObject(L, 3, typeof(Framework.UnityAsset.UnityAsyncAsset));
					System.Action<bool, Framework.UnityAsset.AsyncAsset> _action = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 4);
					
					var gen_ret = new Framework.UnityAsset.AsyncAsset(_assetName, _unityAsyncAsset, _action);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Framework.UnityAsset.AsyncAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Complete(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Complete(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.Unload( _unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.Unload(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Framework.UnityAsset.AsyncAsset.Unload!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAllAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.LoadAllAssets(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Instantiate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Instantiate( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.Instantiate(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Framework.UnityAsset.AsyncAsset.Instantiate!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Destroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.Destroy( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.assetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_url(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.url);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDone(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isDone);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.progress);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_error(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.error);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_text(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.text);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bytes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.bytes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mainAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mainAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_userData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.userData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_userData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.UnityAsset.AsyncAsset gen_to_be_invoked = (Framework.UnityAsset.AsyncAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.userData = translator.GetObject(L, 2, typeof(object));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
