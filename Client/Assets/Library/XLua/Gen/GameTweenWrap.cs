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
    public class GameTweenWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameTween);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 55, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Sequence", _m_Sequence_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Kill", _m_Kill_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOComplete", _m_DOComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "To", _m_To_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Punch", _m_Punch_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Shake", _m_Shake_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ToAlpha", _m_ToAlpha_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ToArray", _m_ToArray_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOMove", _m_DOMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOMoveX", _m_DOMoveX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOMoveY", _m_DOMoveY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOMoveZ", _m_DOMoveZ_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMove", _m_DOLocalMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveX", _m_DOLocalMoveX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveY", _m_DOLocalMoveY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveZ", _m_DOLocalMoveZ_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOJump", _m_DOJump_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalJump", _m_DOLocalJump_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFieldOfView", _m_DOFieldOfView_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DORotate", _m_DORotate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalRotate", _m_DOLocalRotate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLookAt", _m_DOLookAt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScaleV", _m_DOScaleV_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScale", _m_DOScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScaleX", _m_DOScaleX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScaleY", _m_DOScaleY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScaleZ", _m_DOScaleZ_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPath", _m_DOPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalPath", _m_DOLocalPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPunchPosition", _m_DOPunchPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPunchRotation", _m_DOPunchRotation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPunchScale", _m_DOPunchScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOShakePosition", _m_DOShakePosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOShakeRotation", _m_DOShakeRotation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOShakeScale", _m_DOShakeScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableMoveBy", _m_DOBlendableMoveBy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableLocalMoveBy", _m_DOBlendableLocalMoveBy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableRotateBy", _m_DOBlendableRotateBy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableLocalRotateBy", _m_DOBlendableLocalRotateBy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableScaleBy", _m_DOBlendableScaleBy_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOColor", _m_DOColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFade", _m_DOFade_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOText", _m_DOText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBlendableColor", _m_DOBlendableColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFillAmount", _m_DOFillAmount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOAnchorPos", _m_DOAnchorPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOAnchorPosX", _m_DOAnchorPosX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOAnchorPosY", _m_DOAnchorPosY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOAnchorPos3D", _m_DOAnchorPos3D_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOJumpAnchorPos", _m_DOJumpAnchorPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPunchAnchorPos", _m_DOPunchAnchorPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOShakeAnchorPos", _m_DOShakeAnchorPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOSizeDelta", _m_DOSizeDelta_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOValue", _m_DOValue_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameTween does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Sequence_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = GameTween.Sequence(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Kill_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    object _targetOrId = translator.GetObject(L, 1, typeof(object));
                    bool _complete = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = GameTween.Kill( _targetOrId, _complete );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object _targetOrId = translator.GetObject(L, 1, typeof(object));
                    
                        var gen_ret = GameTween.Kill( _targetOrId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.Kill!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Component>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.Component _comp = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    bool _withCallbacks = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = GameTween.DOComplete( _comp, _withCallbacks );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Component>(L, 1)) 
                {
                    UnityEngine.Component _comp = (UnityEngine.Component)translator.GetObject(L, 1, typeof(UnityEngine.Component));
                    
                        var gen_ret = GameTween.DOComplete( _comp );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.DOComplete!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_To_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _callback = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    float _startValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = GameTween.To( _callback, _startValue, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Punch_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _getter = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    UnityEngine.Vector3 _direction;translator.Get(L, 3, out _direction);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 5);
                    float _elasticity = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = GameTween.Punch( _getter, _setter, _direction, _duration, _vibrato, _elasticity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Shake_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _getter = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    float _strength = (float)LuaAPI.lua_tonumber(L, 4);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 5);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _ignoreZAxis = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = GameTween.Shake( _getter, _setter, _duration, _strength, _vibrato, _randomness, _ignoreZAxis );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToAlpha_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _getter = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    float _to = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = GameTween.ToAlpha( _getter, _setter, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToArray_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaFunction _getter = (XLua.LuaFunction)translator.GetObject(L, 1, typeof(XLua.LuaFunction));
                    XLua.LuaFunction _setter = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
                    UnityEngine.Vector3[] _to = (UnityEngine.Vector3[])translator.GetObject(L, 3, typeof(UnityEngine.Vector3[]));
                    float[] _duration = (float[])translator.GetObject(L, 4, typeof(float[]));
                    
                        var gen_ret = GameTween.ToArray( _getter, _setter, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOMove( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOMoveX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOMoveX( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOMoveY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOMoveY( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOMoveZ_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOMoveZ( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOLocalMove( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOLocalMoveX( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOLocalMoveY( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveZ_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOLocalMoveZ( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOJump_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _jumpPower = (float)LuaAPI.lua_tonumber(L, 3);
                    int _numJumps = LuaAPI.xlua_tointeger(L, 4);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOJump( _obj, _endValue, _jumpPower, _numJumps, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalJump_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _jumpPower = (float)LuaAPI.lua_tonumber(L, 3);
                    int _numJumps = LuaAPI.xlua_tointeger(L, 4);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOLocalJump( _obj, _endValue, _jumpPower, _numJumps, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFieldOfView_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOFieldOfView( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DORotate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _mode = LuaAPI.xlua_tointeger(L, 4);
                    
                        var gen_ret = GameTween.DORotate( _obj, _to, _duration, _mode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalRotate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    XRotateMode _mode;translator.Get(L, 4, out _mode);
                    
                        var gen_ret = GameTween.DOLocalRotate( _obj, _to, _duration, _mode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLookAt_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _towards;translator.Get(L, 2, out _towards);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _axisConstraint = LuaAPI.xlua_tointeger(L, 4);
                    UnityEngine.Vector3 _up;translator.Get(L, 5, out _up);
                    
                        var gen_ret = GameTween.DOLookAt( _obj, _towards, _duration, _axisConstraint, _up );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScaleV_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOScaleV( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOScale( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScaleX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOScaleX( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScaleY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOScaleY( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScaleZ_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOScaleZ( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3[] _waypoints = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _pathType = LuaAPI.xlua_tointeger(L, 4);
                    int _pathMode = LuaAPI.xlua_tointeger(L, 5);
                    int _resolution = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.Color _gizmoColor;translator.Get(L, 7, out _gizmoColor);
                    
                        var gen_ret = GameTween.DOPath( _obj, _waypoints, _duration, _pathType, _pathMode, _resolution, _gizmoColor );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3[] _waypoints = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _pathType = LuaAPI.xlua_tointeger(L, 4);
                    int _pathMode = LuaAPI.xlua_tointeger(L, 5);
                    int _resolution = LuaAPI.xlua_tointeger(L, 6);
                    UnityEngine.Color _gizmoColor;translator.Get(L, 7, out _gizmoColor);
                    
                        var gen_ret = GameTween.DOLocalPath( _obj, _waypoints, _duration, _pathType, _pathMode, _resolution, _gizmoColor );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPunchPosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _punch;translator.Get(L, 2, out _punch);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _elasticity = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOPunchPosition( _obj, _punch, _duration, _vibrato, _elasticity, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPunchRotation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _punch;translator.Get(L, 2, out _punch);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _elasticity = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = GameTween.DOPunchRotation( _obj, _punch, _duration, _vibrato, _elasticity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPunchScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _punch;translator.Get(L, 2, out _punch);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _elasticity = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = GameTween.DOPunchScale( _obj, _punch, _duration, _vibrato, _elasticity );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOShakePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _strength = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakePosition( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _strength;translator.Get(L, 3, out _strength);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakePosition( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.DOShakePosition!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOShakeRotation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _strength = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeRotation( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _strength;translator.Get(L, 3, out _strength);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeRotation( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.DOShakeRotation!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOShakeScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _strength = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeScale( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _strength;translator.Get(L, 3, out _strength);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _fadeOut = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeScale( _obj, _duration, _strength, _vibrato, _randomness, _fadeOut );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.DOShakeScale!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableMoveBy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _by;translator.Get(L, 2, out _by);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOBlendableMoveBy( _obj, _by, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableLocalMoveBy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _by;translator.Get(L, 2, out _by);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOBlendableLocalMoveBy( _obj, _by, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableRotateBy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _by;translator.Get(L, 2, out _by);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _mode = LuaAPI.xlua_tointeger(L, 4);
                    
                        var gen_ret = GameTween.DOBlendableRotateBy( _obj, _by, _duration, _mode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableLocalRotateBy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _by;translator.Get(L, 2, out _by);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _mode = LuaAPI.xlua_tointeger(L, 4);
                    
                        var gen_ret = GameTween.DOBlendableLocalRotateBy( _obj, _by, _duration, _mode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableScaleBy_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _by;translator.Get(L, 2, out _by);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOBlendableScaleBy( _obj, _by, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Color _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOColor( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFade_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOFade( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    string _to = LuaAPI.lua_tostring(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _richTextEnabled = LuaAPI.lua_toboolean(L, 4);
                    int _scrambleMode = LuaAPI.xlua_tointeger(L, 5);
                    string _scrambleChars = LuaAPI.lua_tostring(L, 6);
                    
                        var gen_ret = GameTween.DOText( _obj, _to, _duration, _richTextEnabled, _scrambleMode, _scrambleChars );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBlendableColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Color _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOBlendableColor( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFillAmount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = GameTween.DOFillAmount( _obj, _to, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOAnchorPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector2 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOAnchorPos( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOAnchorPosX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOAnchorPosX( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOAnchorPosY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _to = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOAnchorPosY( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOAnchorPos3D_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOAnchorPos3D( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOJumpAnchorPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector2 _endValue;translator.Get(L, 2, out _endValue);
                    float _jumpPower = (float)LuaAPI.lua_tonumber(L, 3);
                    int _numJumps = LuaAPI.xlua_tointeger(L, 4);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOJumpAnchorPos( _obj, _endValue, _jumpPower, _numJumps, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPunchAnchorPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector2 _punch;translator.Get(L, 2, out _punch);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _elasticity = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOPunchAnchorPos( _obj, _punch, _duration, _vibrato, _elasticity, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOShakeAnchorPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _strength = (float)LuaAPI.lua_tonumber(L, 3);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeAnchorPos( _obj, _duration, _strength, _vibrato, _randomness, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _strength;translator.Get(L, 3, out _strength);
                    int _vibrato = LuaAPI.xlua_tointeger(L, 4);
                    float _randomness = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _snapping = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = GameTween.DOShakeAnchorPos( _obj, _duration, _strength, _vibrato, _randomness, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameTween.DOShakeAnchorPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOSizeDelta_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    UnityEngine.Vector2 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOSizeDelta( _obj, _to, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOValue_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _obj = translator.GetObject(L, 1, typeof(object));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snapping = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = GameTween.DOValue( _obj, _endValue, _duration, _snapping );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
