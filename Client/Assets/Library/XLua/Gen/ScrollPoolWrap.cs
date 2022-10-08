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
    public class ScrollPoolWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ScrollPool);
			Utils.BeginObjectRegister(type, L, translator, 0, 14, 8, 8);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetUpdateCallBack", _m_SetUpdateCallBack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInitFinish", _m_OnInitFinish);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitPool", _m_InitPool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReloadConfig", _m_ReloadConfig);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetListPivot", _m_SetListPivot);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetListAtIndex", _m_SetListAtIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTargetPos", _m_GetTargetPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitListSize", _m_InitListSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetStartIndex", _m_GetStartIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurView", _m_GetCurView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitCardPosAndName", _m_InitCardPosAndName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMaxPoolSize", _m_GetMaxPoolSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPadding", _m_SetPadding);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReInit", _m_ReInit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Scroll", _g_get_Scroll);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollRect", _g_get_scrollRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollList", _g_get_scrollList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "prefab", _g_get_prefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "padding", _g_get_padding);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "locationOffset", _g_get_locationOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cardSize", _g_get_cardSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateCallBack", _g_get_updateCallBack);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Scroll", _s_set_Scroll);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scrollRect", _s_set_scrollRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scrollList", _s_set_scrollList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "prefab", _s_set_prefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "padding", _s_set_padding);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "locationOffset", _s_set_locationOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cardSize", _s_set_cardSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateCallBack", _s_set_updateCallBack);
            
			
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
					
					var gen_ret = new ScrollPool();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPool constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUpdateCallBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<int, UnityEngine.GameObject> _callBack = translator.GetDelegate<System.Action<int, UnityEngine.GameObject>>(L, 2);
                    
                    gen_to_be_invoked.SetUpdateCallBack( _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnInitFinish(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.OnInitFinish( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitPool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _objNum = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.InitPool( _objNum );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _objNum = LuaAPI.xlua_tointeger(L, 2);
                    int _targetIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.InitPool( _objNum, _targetIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPool.InitPool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReloadConfig(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReloadConfig(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetListPivot(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetListPivot(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetListAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetListAtIndex( _index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTargetPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetTargetPos( _index );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitListSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _num = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.InitListSize( _num );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStartIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetStartIndex(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetCurView(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitCardPosAndName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.GameObject _card = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.InitCardPosAndName( _index, _card );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMaxPoolSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetMaxPoolSize(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPadding(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _left = LuaAPI.xlua_tointeger(L, 2);
                    int _right = LuaAPI.xlua_tointeger(L, 3);
                    int _top = LuaAPI.xlua_tointeger(L, 4);
                    int _bottom = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetPadding( _left, _right, _top, _bottom );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReInit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Scroll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Scroll);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scrollRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scrollList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_prefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.prefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_padding(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.padding);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_locationOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.locationOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cardSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.cardSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.updateCallBack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Scroll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Scroll = (UnityEngine.UI.ScrollRect)translator.GetObject(L, 2, typeof(UnityEngine.UI.ScrollRect));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.scrollRect = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scrollList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.scrollList = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_prefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.prefab = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_padding(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                ScrollPool.Padding gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.padding = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_locationOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.locationOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cardSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.cardSize = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateCallBack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPool gen_to_be_invoked = (ScrollPool)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.updateCallBack = translator.GetDelegate<System.Action<int, UnityEngine.GameObject>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
