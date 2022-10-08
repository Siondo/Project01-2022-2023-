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
    public class LuaHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 87, 5, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowReporter", _m_ShowReporter_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAppPlatform", _m_GetAppPlatform_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDebugModeStatus", _m_GetDebugModeStatus_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetString", _m_GetString_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInt", _m_GetInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFloat", _m_GetFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBool", _m_GetBool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetString", _m_SetString_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetInt", _m_SetInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetFloat", _m_SetFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetBool", _m_SetBool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteKey", _m_DeleteKey_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteAll", _m_DeleteAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Save", _m_Save_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DeleteFileOrDirectory", _m_DeleteFileOrDirectory_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFileNameWithoutExtension", _m_GetFileNameWithoutExtension_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateManifestConfig", _m_UpdateManifestConfig_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StringToManifestConfig", _m_StringToManifestConfig_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateManifestMappingConfig", _m_UpdateManifestMappingConfig_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMD5", _m_GetMD5_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Combine", _m_Combine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WriteAllBytes", _m_WriteAllBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowUIImmediate", _m_ShowUIImmediate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowUI", _m_ShowUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HideUI", _m_HideUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUIBase", _m_GetUIBase_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsShowUI", _m_IsShowUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearUI", _m_ClearUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveWindowStackData", _m_RemoveWindowStackData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HideDisplayLogo", _m_HideDisplayLogo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RefreshAllText", _m_RefreshAllText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Quit", _m_Quit_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReStartGame", _m_ReStartGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendMail", _m_SendMail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenURL", _m_OpenURL_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsNull", _m_IsNull_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsNotNull", _m_IsNotNull_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Instantiate", _m_Instantiate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "EntityIdentity", _m_EntityIdentity_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestroyImmediate", _m_DestroyImmediate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DontDestroyOnLoad", _m_DontDestroyOnLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AtlasRequested", _m_AtlasRequested_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenKill", _m_DoTweenKill_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveY", _m_DOLocalMoveY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ToFloat", _m_ToFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEventTrigger", _m_AddEventTrigger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveEventTrigger", _m_RemoveEventTrigger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveAllEventTrigger", _m_RemoveAllEventTrigger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeLayer", _m_ChangeLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToLocalPointInRectangle", _m_ScreenPointToLocalPointInRectangle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TextFormat", _m_TextFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckGuiRaycastObjects", _m_CheckGuiRaycastObjects_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WWWGetText", _m_WWWGetText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WWWGetByte", _m_WWWGetByte_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WWWPostText", _m_WWWPostText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetWWWForm", _m_GetWWWForm_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WWWPostByte", _m_WWWPostByte_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUrlSprite", _m_GetUrlSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AssetBundleLoadFromMemory", _m_AssetBundleLoadFromMemory_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AssetBundleLoad", _m_AssetBundleLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AssetBundleAsyncLoad", _m_AssetBundleAsyncLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAsset", _m_LoadAsset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FileAssetLoad", _m_FileAssetLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FileAssetAsyncLoad", _m_FileAssetAsyncLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadAsset", _m_UnloadAsset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadAssets", _m_UnloadAssets_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadFromPool", _m_LoadFromPool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadToPool", _m_UnloadToPool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAssetFromPool", _m_LoadAssetFromPool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadObjectFromPool", _m_LoadObjectFromPool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitShader", _m_InitShader_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitMaterial", _m_InitMaterial_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetShader", _m_GetShader_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetGray", _m_SetGray_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetGrayInChildren", _m_SetGrayInChildren_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSpineOutline", _m_SetSpineOutline_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCanvasSize", _m_GetCanvasSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToWorldPoint", _m_ScreenPointToWorldPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToWorldPointByCamera", _m_ScreenPointToWorldPointByCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenPointToUIPoint", _m_ScreenPointToUIPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldToScreenPoint", _m_WorldToScreenPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldPosToUiLocalPointIn3DScene", _m_WorldPosToUiLocalPointIn3DScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RectangleContainsScreenPoint", _m_RectangleContainsScreenPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetParticleSortingOrder", _m_SetParticleSortingOrder_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSpineSortingOrder", _m_SetSpineSortingOrder_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUISortingOrder", _m_SetUISortingOrder_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isEditor", _g_get_isEditor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "persistentDataPath", _g_get_persistentDataPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "targetFrameRate", _g_get_targetFrameRate);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "uiCamera", _g_get_uiCamera);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "uiRoot", _g_get_uiRoot);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "targetFrameRate", _s_set_targetFrameRate);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new LuaHelper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowReporter_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.ShowReporter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAppPlatform_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = LuaHelper.GetAppPlatform(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDebugModeStatus_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = LuaHelper.GetDebugModeStatus(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetString_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    string _defaultValue = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = LuaHelper.GetString( _key, _defaultValue );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    int _defaultValue = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = LuaHelper.GetInt( _key, _defaultValue );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    float _defaultValue = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        var gen_ret = LuaHelper.GetFloat( _key, _defaultValue );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBool_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    bool _defaultValue = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = LuaHelper.GetBool( _key, _defaultValue );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetString_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    string _value = LuaAPI.lua_tostring(L, 2);
                    
                    LuaHelper.SetString( _key, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    int _value = LuaAPI.xlua_tointeger(L, 2);
                    
                    LuaHelper.SetInt( _key, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    float _value = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    LuaHelper.SetFloat( _key, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBool_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    bool _value = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.SetBool( _key, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteKey_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.DeleteKey( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.DeleteAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Save_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.Save(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteFileOrDirectory_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.DeleteFileOrDirectory( _path );
                    
                    
                    
                    return 0;
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
                    
                        var gen_ret = LuaHelper.GetFileNameWithoutExtension( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateManifestConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.UpdateManifestConfig( _text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StringToManifestConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.StringToManifestConfig( _text );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateManifestMappingConfig_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.UpdateManifestMappingConfig( _text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMD5_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.GetMD5( _path );
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
                    
                        var gen_ret = LuaHelper.Combine( _path1, _path2 );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteAllBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
                    
                    LuaHelper.WriteAllBytes( _path, _bytes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowUIImmediate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    object[] _args = translator.GetParams<object>(L, 2);
                    
                    LuaHelper.ShowUIImmediate( _name, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    object[] _args = translator.GetParams<object>(L, 2);
                    
                    LuaHelper.ShowUI( _name, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HideUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.HideUI( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIBase_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.GetUIBase( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsShowUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.IsShowUI( _name );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _isDestory = LuaAPI.lua_toboolean(L, 1);
                    
                    LuaHelper.ClearUI( _isDestory );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveWindowStackData_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.RemoveWindowStackData( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HideDisplayLogo_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.HideDisplayLogo(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshAllText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<System.Action>(L, 1)) 
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 1);
                    
                    LuaHelper.RefreshAllText( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    LuaHelper.RefreshAllText(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.RefreshAllText!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Quit_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.Quit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReStartGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.ReStartGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendMail_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _email = LuaAPI.lua_tostring(L, 1);
                    string _title = LuaAPI.lua_tostring(L, 2);
                    
                    LuaHelper.SendMail( _email, _title );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenURL_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.OpenURL( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsNull_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    
                        var gen_ret = LuaHelper.IsNull( _obj );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsNotNull_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    
                        var gen_ret = LuaHelper.IsNotNull( _obj );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Instantiate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _name = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    bool _worldPositionStays = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = LuaHelper.Instantiate( _go, _name, _parent, _worldPositionStays );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityIdentity_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _name = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    bool _worldPositionStays = LuaAPI.lua_toboolean(L, 4);
                    
                    LuaHelper.EntityIdentity( _go, _name, _parent, _worldPositionStays );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyImmediate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 1, typeof(UnityEngine.Object));
                    
                    LuaHelper.DestroyImmediate( _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DontDestroyOnLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    LuaHelper.DontDestroyOnLoad( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AtlasRequested_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaHelper.AtlasRequested(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenKill_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    string _doTweenId = LuaAPI.lua_tostring(L, 1);
                    bool _complete = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.DoTweenKill( _doTweenId, _complete );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _doTweenId = LuaAPI.lua_tostring(L, 1);
                    
                    LuaHelper.DoTweenKill( _doTweenId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.DoTweenKill!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveY_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& translator.Assignable<UnityEngine.Transform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 5);
                    string _doTweenId = LuaAPI.lua_tostring(L, 6);
                    
                    LuaHelper.DOLocalMoveY( _target, _endValue, _delay, _duration, _complete, _doTweenId );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Transform>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 5);
                    
                    LuaHelper.DOLocalMoveY( _target, _endValue, _delay, _duration, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.DOLocalMoveY!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _start = (float)LuaAPI.lua_tonumber(L, 1);
                    float _end = (float)LuaAPI.lua_tonumber(L, 2);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 3);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action<float> _callBack = translator.GetDelegate<System.Action<float>>(L, 5);
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 6);
                    
                    LuaHelper.ToFloat( _start, _end, _delay, _duration, _callBack, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventTrigger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.EventSystems.EventTriggerType _eventTriggerType;translator.Get(L, 2, out _eventTriggerType);
                    UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData> _e = translator.GetDelegate<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>(L, 3);
                    
                    LuaHelper.AddEventTrigger( _go, _eventTriggerType, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEventTrigger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.EventSystems.EventTriggerType _eventTriggerType;translator.Get(L, 2, out _eventTriggerType);
                    
                    LuaHelper.RemoveEventTrigger( _go, _eventTriggerType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllEventTrigger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    LuaHelper.RemoveAllEventTrigger( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _layerName = LuaAPI.lua_tostring(L, 2);
                    
                    LuaHelper.ChangeLayer( _target, _layerName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToLocalPointInRectangle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _targetPos;translator.Get(L, 3, out _targetPos);
                    
                        var gen_ret = LuaHelper.ScreenPointToLocalPointInRectangle( _rect, _camera, _targetPos );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TextFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _str = LuaAPI.lua_tostring(L, 1);
                    object[] _args = translator.GetParams<object>(L, 2);
                    
                        var gen_ret = LuaHelper.TextFormat( _str, _args );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckGuiRaycastObjects_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = LuaHelper.CheckGuiRaycastObjects(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WWWGetText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    int _requestCount = LuaAPI.xlua_tointeger(L, 5);
                    
                    LuaHelper.WWWGetText( _url, _complete, _timeout, _interval, _requestCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    LuaHelper.WWWGetText( _url, _complete, _timeout, _interval );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    
                    LuaHelper.WWWGetText( _url, _complete, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    
                    LuaHelper.WWWGetText( _url, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.WWWGetText!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WWWGetByte_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    int _requestCount = LuaAPI.xlua_tointeger(L, 5);
                    
                    LuaHelper.WWWGetByte( _url, _complete, _timeout, _interval, _requestCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    LuaHelper.WWWGetByte( _url, _complete, _timeout, _interval );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    
                    LuaHelper.WWWGetByte( _url, _complete, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, object>>(L, 2)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 2);
                    
                    LuaHelper.WWWGetByte( _url, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.WWWGetByte!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WWWPostText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 5);
                    int _requestCount = LuaAPI.xlua_tointeger(L, 6);
                    
                    LuaHelper.WWWPostText( _url, _form, _complete, _timeout, _interval, _requestCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    LuaHelper.WWWPostText( _url, _form, _complete, _timeout, _interval );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    
                    LuaHelper.WWWPostText( _url, _form, _complete, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    
                    LuaHelper.WWWPostText( _url, _form, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.WWWPostText!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWWWForm_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LuaHelper.GetWWWForm(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WWWPostByte_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 5);
                    int _requestCount = LuaAPI.xlua_tointeger(L, 6);
                    
                    LuaHelper.WWWPostByte( _url, _form, _complete, _timeout, _interval, _requestCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    LuaHelper.WWWPostByte( _url, _form, _complete, _timeout, _interval );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    
                    LuaHelper.WWWPostByte( _url, _form, _complete, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.WWWForm>(L, 2)&& translator.Assignable<System.Action<string, object>>(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.WWWForm _form = (UnityEngine.WWWForm)translator.GetObject(L, 2, typeof(UnityEngine.WWWForm));
                    System.Action<string, object> _complete = translator.GetDelegate<System.Action<string, object>>(L, 3);
                    
                    LuaHelper.WWWPostByte( _url, _form, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.WWWPostByte!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUrlSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, UnityEngine.Sprite>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, UnityEngine.Sprite> _complete = translator.GetDelegate<System.Action<string, UnityEngine.Sprite>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    int _requestCount = LuaAPI.xlua_tointeger(L, 5);
                    
                    LuaHelper.GetUrlSprite( _url, _complete, _timeout, _interval, _requestCount );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, UnityEngine.Sprite>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, UnityEngine.Sprite> _complete = translator.GetDelegate<System.Action<string, UnityEngine.Sprite>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    LuaHelper.GetUrlSprite( _url, _complete, _timeout, _interval );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, UnityEngine.Sprite>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, UnityEngine.Sprite> _complete = translator.GetDelegate<System.Action<string, UnityEngine.Sprite>>(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    
                    LuaHelper.GetUrlSprite( _url, _complete, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string, UnityEngine.Sprite>>(L, 2)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    System.Action<string, UnityEngine.Sprite> _complete = translator.GetDelegate<System.Action<string, UnityEngine.Sprite>>(L, 2);
                    
                    LuaHelper.GetUrlSprite( _url, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.GetUrlSprite!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssetBundleLoadFromMemory_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    byte[] _data = LuaAPI.lua_tobytes(L, 1);
                    string _name = LuaAPI.lua_tostring(L, 2);
                    System.Action<UnityEngine.Object> _action = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 3);
                    
                    LuaHelper.AssetBundleLoadFromMemory( _data, _name, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssetBundleLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.AssetBundleLoad( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssetBundleAsyncLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    
                        var gen_ret = LuaHelper.AssetBundleAsyncLoad( _path, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    bool _async = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = LuaHelper.LoadAsset( _name, _complete, _async );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    
                        var gen_ret = LuaHelper.LoadAsset( _name, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.LoadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FileAssetLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.FileAssetLoad( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FileAssetAsyncLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    
                        var gen_ret = LuaHelper.FileAssetAsyncLoad( _path, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAsset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Framework.UnityAsset.AsyncAsset _asset = (Framework.UnityAsset.AsyncAsset)translator.GetObject(L, 1, typeof(Framework.UnityAsset.AsyncAsset));
                    bool _unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.UnloadAsset( _asset, _unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAssets_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool _unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 1);
                    
                    LuaHelper.UnloadAssets( _unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 0) 
                {
                    
                    LuaHelper.UnloadAssets(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.UnloadAssets!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFromPool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.GameObject> _complete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 2);
                    bool _async = LuaAPI.lua_toboolean(L, 3);
                    
                    LuaHelper.LoadFromPool( _name, _complete, _async );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.GameObject>>(L, 2)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.GameObject> _complete = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 2);
                    
                    LuaHelper.LoadFromPool( _name, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.LoadFromPool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadToPool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    LuaHelper.UnloadToPool( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetFromPool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    bool _async = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = LuaHelper.LoadAssetFromPool( _name, _complete, _async );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<bool, Framework.UnityAsset.AsyncAsset> _complete = translator.GetDelegate<System.Action<bool, Framework.UnityAsset.AsyncAsset>>(L, 2);
                    
                        var gen_ret = LuaHelper.LoadAssetFromPool( _name, _complete );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.LoadAssetFromPool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadObjectFromPool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.Object> _complete = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 2);
                    bool _async = LuaAPI.lua_toboolean(L, 3);
                    
                    LuaHelper.LoadObjectFromPool( _name, _complete, _async );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<UnityEngine.Object>>(L, 2)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.Object> _complete = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 2);
                    
                    LuaHelper.LoadObjectFromPool( _name, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.LoadObjectFromPool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitShader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object[] _shaders = (UnityEngine.Object[])translator.GetObject(L, 1, typeof(UnityEngine.Object[]));
                    
                    LuaHelper.InitShader( _shaders );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitMaterial_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Object[] _materials = (UnityEngine.Object[])translator.GetObject(L, 1, typeof(UnityEngine.Object[]));
                    
                    LuaHelper.InitMaterial( _materials );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetShader_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaHelper.GetShader( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGray_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Graphic>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.UI.Graphic _graphic = (UnityEngine.UI.Graphic)translator.GetObject(L, 1, typeof(UnityEngine.UI.Graphic));
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    float _power = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    LuaHelper.SetGray( _graphic, _enable, _power );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.UI.Graphic>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.UI.Graphic _graphic = (UnityEngine.UI.Graphic)translator.GetObject(L, 1, typeof(UnityEngine.UI.Graphic));
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.SetGray( _graphic, _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.SetGray!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGrayInChildren_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    float _power = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    LuaHelper.SetGrayInChildren( _go, _enable, _power );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.SetGrayInChildren( _go, _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.SetGrayInChildren!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSpineOutline_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool _outline = LuaAPI.lua_toboolean(L, 2);
                    
                    LuaHelper.SetSpineOutline( _go, _outline );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCanvasSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LuaHelper.GetCanvasSize(  );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToWorldPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _point;translator.Get(L, 2, out _point);
                    
                        var gen_ret = LuaHelper.ScreenPointToWorldPoint( _rect, _point );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToWorldPointByCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _point;translator.Get(L, 2, out _point);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                        var gen_ret = LuaHelper.ScreenPointToWorldPointByCamera( _rect, _point, _camera );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenPointToUIPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _point;translator.Get(L, 2, out _point);
                    
                        var gen_ret = LuaHelper.ScreenPointToUIPoint( _rect, _point );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToScreenPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 1, out _pos);
                    
                        var gen_ret = LuaHelper.WorldToScreenPoint( _pos );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldPosToUiLocalPointIn3DScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera3D = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _worldPos3D;translator.Get(L, 2, out _worldPos3D);
                    UnityEngine.RectTransform _uiRectTransform = (UnityEngine.RectTransform)translator.GetObject(L, 3, typeof(UnityEngine.RectTransform));
                    
                        var gen_ret = LuaHelper.WorldPosToUiLocalPointIn3DScene( _camera3D, _worldPos3D, _uiRectTransform );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RectangleContainsScreenPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& translator.Assignable<UnityEngine.Camera>(L, 3)) 
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _screenPoint;translator.Get(L, 2, out _screenPoint);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                        var gen_ret = LuaHelper.RectangleContainsScreenPoint( _rect, _screenPoint, _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)) 
                {
                    UnityEngine.RectTransform _rect = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _screenPoint;translator.Get(L, 2, out _screenPoint);
                    
                        var gen_ret = LuaHelper.RectangleContainsScreenPoint( _rect, _screenPoint );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaHelper.RectangleContainsScreenPoint!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetParticleSortingOrder_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sortingOrder = LuaAPI.xlua_tointeger(L, 2);
                    
                    LuaHelper.SetParticleSortingOrder( _go, _sortingOrder );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSpineSortingOrder_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sortingOrder = LuaAPI.xlua_tointeger(L, 2);
                    
                    LuaHelper.SetSpineSortingOrder( _go, _sortingOrder );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUISortingOrder_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sortingOrder = LuaAPI.xlua_tointeger(L, 2);
                    
                    LuaHelper.SetUISortingOrder( _go, _sortingOrder );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isEditor(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, LuaHelper.isEditor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_persistentDataPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, LuaHelper.persistentDataPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_targetFrameRate(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, LuaHelper.targetFrameRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uiCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, LuaHelper.uiCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_uiRoot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, LuaHelper.uiRoot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_targetFrameRate(RealStatePtr L)
        {
		    try {
                
			    LuaHelper.targetFrameRate = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
