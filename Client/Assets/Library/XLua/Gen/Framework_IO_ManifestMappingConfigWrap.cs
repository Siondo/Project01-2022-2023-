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
    public class FrameworkIOManifestMappingConfigWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Framework.IO.ManifestMappingConfig);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Add", _m_Add);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryAdd", _m_TryAdd);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Get", _m_Get);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Contains", _m_Contains);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "data", _g_get_data);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "data", _s_set_data);
            
			
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
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Framework.IO.ManifestMappingConfig();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2))
				{
					bool _isPrepare = LuaAPI.lua_toboolean(L, 2);
					
					var gen_ret = new Framework.IO.ManifestMappingConfig(_isPrepare);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<System.Collections.Generic.Dictionary<string, string>>(L, 2))
				{
					System.Collections.Generic.Dictionary<string, string> _data = (System.Collections.Generic.Dictionary<string, string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, string>));
					
					var gen_ret = new Framework.IO.ManifestMappingConfig(_data);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Framework.IO.ManifestMappingConfig constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Add(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    string _assetBundleName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.Add( _assetName, _assetBundleName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryAdd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    string _assetBundleName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.TryAdd( _assetName, _assetBundleName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Get( _assetName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Contains(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Contains( _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_data(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.data);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_data(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Framework.IO.ManifestMappingConfig gen_to_be_invoked = (Framework.IO.ManifestMappingConfig)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.data = (System.Collections.Generic.Dictionary<string, string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
