
local AssetLoad = class("AssetLoad", StateMachineBase)

function AssetLoad:OnEnter()
    self.super:OnEnter()
    UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, UIText(11016), UIText(11017)) --消息分发
    coroutine.start(function ()
        --coroutine.step(2)
        coroutine.wait(2.2)
        -- 准备其他库
        Require()
        -- 准备配置
        self.loadList = Config.Init()
        --table.insert(self.loadList, 1, {name = Const.GuideItemRes})
        if APP.abMode and false then
            table.insert(self.loadList, 1, {name = "res/shader.unity3d", func = function(tbData)
                if tbData then
                    LuaHelper.InitShader(tbData:LoadAllAssets())
                end
            end})
        end
        self.assetLoading, self.currentSize, self.size, self.asyncList = true, 0, 0, {}
        local temp = string.Empty
        for _, v in ipairs(self.loadList) do
            temp = APP.manifestMapping:Get(v.name)
            temp = APP.manifest:Get(temp)
            if string.IsNotNullOrEmpty(temp) then
                v.size = temp.size
            else
                v.size = 1
            end
            self.size = self.size + v.size
        end
    end)
end

function AssetLoad:OnUpdate()
    if self.assetLoading then
        -- 推入到加载
        if #self.asyncList < Const.MAX_LOADER and #self.loadList > 0 then
            local data = table.remove(self.loadList, 1)
            local async = LuaHelper.LoadAsset(data.name, function (result, tbData)
                if result then
                    self.currentSize = self.currentSize + data.size
                    if data.func then
                        data.func(tbData)
                    end
                else
                    table.insert(self.loadList, #self.loadList + 1, data)
                    logError(tbData.error.."\n"..data.name)
                end
                table.Remove(self.asyncList, tbData)
            end, true)
            async.userData = data.size
            if APP.abMode then
                table.insert(self.asyncList, #self.asyncList + 1, async)
            end
        end
        --计算下载完成总量
        self.currentRealSize = self.currentSize
        for _, v in ipairs(self.asyncList) do
            self.currentRealSize = self.currentRealSize + v.userData * v.progress
        end
        --文件加载失败后，避免进度条回滚
        if self.tempCurrentRealSize == nil or self.tempCurrentRealSize < self.currentRealSize then
            self.tempCurrentRealSize = self.currentRealSize
        end
        UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, self.tempCurrentRealSize, self.size)
        if #self.asyncList == 0 and #self.loadList == 0 and Config.IsInitComplete() then
            self.assetLoading = false
            self:LoadComplete()
        end
    end
end

function AssetLoad:LoadComplete()
    StateMachine:OnEnter(StateMachineType.StartState)
end

return AssetLoad