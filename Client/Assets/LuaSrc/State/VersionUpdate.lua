
local VersionUpdate = class("Version", StateMachineBase)

function VersionUpdate:OnEnter()
    self.super:OnEnter()
    LuaHelper.ShowUIImmediate(UI.AppSetupView)
    UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, nil, nil, '正在获取远端数据...')
    NetWork:onInit(function(data)
        APP.host = data.host
        APP.port = data.port
        APP.url = data.url

        local isConnect = false
        UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, nil, nil, '正在与服务器建立连接...')
        NetWork:onConnect(function(result)
            isConnect = result
        end)

        coroutine.start(function ()
            coroutine.step(1)
            while not isConnect do
                coroutine.step(1)
            end

            -- 准备其他库
            Require()
            -- 获取远程版本文件
            if APP.openUpdate then
                UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, UIText(11002), UIText(11003))
                self:RequestVersionInfo()
            else
                StateMachine:OnEnter(StateMachineType.AssetLoadState)
            end
        end)
    end)
end

--请求版本信息
function VersionUpdate:RequestVersionInfo()
    local url = string.format(Const.REMOTE_VERSION_DIRECTORY, APP.url, APP.platformTag, APP.innerVersion)
    LuaHelper.WWWGetText(url, function (error, tbData)
        self:Complete(error, tbData)
    end, 6, 2, 3)
end

--请求版本信息完成
function VersionUpdate:Complete(error, tbData)
    if string.IsNullOrEmpty(error) then
        APP.Update(tbData)
        local jsonData = Json.decode(tbData)
        local cur = string.Split(APP.innerVersion, '.')
        local remote = string.Split(jsonData.version, '.')

        local forceUpdate = not (tonumber(cur[1]) >= tonumber(remote[1]) and tonumber(cur[2]) >= tonumber(remote[2]))
        if forceUpdate then
            LuaHelper.ShowUI(UI.UIVersionUpdate)
        else
            StateMachine:OnEnter(StateMachineType.AssetUpdateState)
        end
    else
        -- 获取版本信息失败
        LuaHelper.ShowUI(UI.UIUpdateFail, UIText(13003), UIText(13001), UIText(12007), function ()
            coroutine.start(function ()
                coroutine.step(2)
                self:RequestVersionInfo()
            end)
        end)
    end
end

return VersionUpdate