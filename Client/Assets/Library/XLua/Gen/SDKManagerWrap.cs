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
    public class SDKManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SDKManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 20, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEventObject", _m_OnEventObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLogin", _m_OnLogin);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnAutoLogin", _m_OnAutoLogin);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoginBack", _m_LoginBack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoginSuccessed", _m_LoginSuccessed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RequestPermission", _m_RequestPermission);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RewardVideoAdShow", _m_RewardVideoAdShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RewardVideoAdShowFinished", _m_RewardVideoAdShowFinished);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InterstitialAdShow", _m_InterstitialAdShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InterstitialAdShowFinished", _m_InterstitialAdShowFinished);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BannerAdShow", _m_BannerAdShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BannerAdShowFinished", _m_BannerAdShowFinished);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BannerAdHide", _m_BannerAdHide);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SplashAdShow", _m_SplashAdShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SplashAdShowFinished", _m_SplashAdShowFinished);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NativeAdShow", _m_NativeAdShow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NativeAdShowFinished", _m_NativeAdShowFinished);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendTenjinEvent", _m_SendTenjinEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstallAPK", _m_InstallAPK);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QuitAPP", _m_QuitAPP);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "InstanceSDK", _m_InstanceSDK_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "instance", _g_get_instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new SDKManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SDKManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstanceSDK_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    SDKManager.InstanceSDK(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEventObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _eventkey = LuaAPI.lua_tostring(L, 2);
                    string _eventValue = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.OnEventObject( _eventkey, _eventValue );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _eventkey = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OnEventObject( _eventkey );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to SDKManager.OnEventObject!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLogin(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    SDKManager.LoginType _loginType;translator.Get(L, 2, out _loginType);
                    System.Action<SDKBase.LoginCode> _callback = translator.GetDelegate<System.Action<SDKBase.LoginCode>>(L, 3);
                    
                    gen_to_be_invoked.OnLogin( _loginType, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnAutoLogin(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    SDKManager.LoginType _loginType;translator.Get(L, 2, out _loginType);
                    System.Action<SDKBase.LoginCode> _callback = translator.GetDelegate<System.Action<SDKBase.LoginCode>>(L, 3);
                    
                    gen_to_be_invoked.OnAutoLogin( _loginType, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoginBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoginBack( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoginSuccessed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoginSuccessed(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RequestPermission(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _permission = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.RequestPermission( _permission );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RewardVideoAdShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _scenario = LuaAPI.lua_tostring(L, 2);
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    gen_to_be_invoked.RewardVideoAdShow( _scenario, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RewardVideoAdShowFinished(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.RewardVideoAdShowFinished( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InterstitialAdShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _scenario = LuaAPI.lua_tostring(L, 2);
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    gen_to_be_invoked.InterstitialAdShow( _scenario, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InterstitialAdShowFinished(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.InterstitialAdShowFinished( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BannerAdShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _scenario = LuaAPI.lua_tostring(L, 2);
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    gen_to_be_invoked.BannerAdShow( _scenario, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BannerAdShowFinished(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.BannerAdShowFinished( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BannerAdHide(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BannerAdHide(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SplashAdShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.SplashAdShow( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SplashAdShowFinished(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SplashAdShowFinished( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NativeAdShow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _callback = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.NativeAdShow( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NativeAdShowFinished(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.NativeAdShowFinished( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendTenjinEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SendTenjinEvent( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstallAPK(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _filePath = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.InstallAPK( _filePath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QuitAPP(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SDKManager gen_to_be_invoked = (SDKManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.QuitAPP( _msg );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, SDKManager.instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
