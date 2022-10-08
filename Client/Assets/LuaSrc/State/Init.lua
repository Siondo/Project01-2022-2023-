require("Common.Const")

local Init = class("Init", StateMachineBase)

function Init:OnEnter()
    self.super:OnEnter()
    -- 沙盒版本检测
    if APP.innerVersion ~= LuaHelper.GetString(Const.SANDBOX_VERSION) then
        -- 清空文件夹，以便重新解压
        LuaHelper.DeleteFileOrDirectory(LuaHelper.persistentDataPath)
        LuaHelper.SetString(Const.SANDBOX_VERSION, APP.innerVersion)
    end
    -- 获取清单数据
    if APP.openUpdate or APP.abMode or APP.abLua then
        local asset = LuaHelper.AssetBundleLoad(Const.MANIFESTFILE_JSON)
        if string.IsNullOrEmpty(asset.error) then
            LuaHelper.UpdateManifestConfig(asset.text)
            LuaHelper.UnloadAsset(asset, true)
        end
    end
    -- 获取清单映射数据
    if APP.openUpdate or APP.abMode or APP.abLua then
        local asset = LuaHelper.AssetBundleLoad(Const.MANIFESTFILEMAPPING_JSON)
        if string.IsNullOrEmpty(asset.error) then
            LuaHelper.UpdateManifestMappingConfig(asset.text)
            LuaHelper.UnloadAsset(asset, true)
        end
    end
    -- 图集请求监听
    LuaHelper.AtlasRequested()
    -- 准备必须库
    GlobalRequire()
    Require()
    -- 加载全局配置表
    Config.InitGlobal()
    -- 加载多语言
    Config.InitLang()
    -- 加载音效
	AudioManager.Init()
    -- 根据下一步指示，进入指定的状态
    local nextState = APP.nextState or StateMachineType.VersionUpdateState
    StateMachine:OnEnter(nextState)
end

return Init