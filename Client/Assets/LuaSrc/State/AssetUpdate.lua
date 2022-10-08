
local AssetUpdate = class("AssetUpdate", StateMachineBase)

function AssetUpdate:OnEnter()
    self.super:OnEnter()
    UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, UIText(11005), UIText(11006))
    -- 获取远程更新清单文件
    self.url = string.format(Const.REMOTE_DIRECTORY, APP.url, APP.platformTag, APP.assetDir, Const.MANIFESTFILE)
    LuaHelper.WWWGetByte(self.url, function (error, tbData)
        self:GetManifestFileComplete(error, tbData)
    end, 6, 2, 3)
end

--请求版本信息完成
function AssetUpdate:GetManifestFileComplete(error, tbData)
    if string.IsNullOrEmpty(error) then
        self.remoteManifestBytes = tbData
        local assetName = LuaHelper.GetFileNameWithoutExtension(self.url)
        LuaHelper.AssetBundleLoadFromMemory(tbData, assetName, function (obj)
            self.remoteManifest = LuaHelper.StringToManifestConfig(obj:ToString())
        end)
        self:StartAssetUpdate()
    else
        -- 获取清单文件失败
        LuaHelper.ShowUI(UI.SecondaryView, 0, UIText(14001), UIText(12005), function ()
            LuaHelper.Quit()
        end)
    end
end

function AssetUpdate:OnUpdate()
    if self.assetUpdating then
        -- 更新其它文件
        if #self.asyncList < Const.MAX_UPDATE and #self.updateList > 0 then
            local data = table.remove(self.updateList, 1)
            local url = string.format(Const.REMOTE_DIRECTORY, APP.url, APP.platformTag, APP.assetDir, data.name)
            local async = LuaHelper.FileAssetAsyncLoad(url, function (result, tbData)
                if result then
                    LuaHelper.WriteAllBytes(LuaHelper.Combine(LuaHelper.persistentDataPath, data.name), tbData.bytes)
                    self.currentSize = self.currentSize + tbData.userData
                else
                    table.insert(self.updateList, #self.updateList + 1, data)
                    logError(tbData.error.."\n"..data.name)
                end
                table.Remove(self.asyncList, tbData)
            end)
            async.userData = data.size / 1024.0
            table.insert(self.asyncList, #self.asyncList + 1, async)
        end
        -- 计算下载速度
        self.currentRealSize = self.currentSize
        for _, v in ipairs(self.asyncList) do
            self.currentRealSize = self.currentRealSize + v.userData * v.progress
        end
        if GetRealtimeSinceStartup() >= self.time + 1.0 then
            self.speed = self.currentRealSize - self.lastRealSize
            self.lastRealSize = self.currentRealSize
            self.time = GetRealtimeSinceStartup()

            if self.speed == 0 then
                self.speed = 0.0001
            end
            local remainingTime = math.ceil((self.size - self.currentRealSize) / self.speed)
            if #self.asyncList == 0 and #self.updateList == 0 then--所有文件下载完成后，剩余时间为0
                remainingTime = 0
            end
            local tips = UIText(11009)
            --下载速度大于0.6MB,以MB为单位，否则以KB为单位
            if self.speed > 0.6 then
                tips = tips..UIText(11010, self.size, self.speed, self.remoteManifest.assetVersion)
            else
                tips = tips..UIText(11011, self.size, math.floor(self.speed * 1024.0), self.remoteManifest.assetVersion)
            end
            --剩余时间
            if remainingTime < 60 then
                tips = tips..UIText(11012, remainingTime)
            elseif remainingTime < 3600 then
                tips = tips..UIText(11013, math.floor(remainingTime / 60), remainingTime % 60)
            elseif remainingTime < 86400 then
                remainingTime = math.floor(remainingTime / 60)
                tips = tips..UIText(11014, math.floor(remainingTime / 60), remainingTime % 60)
            else
                tips = tips..UIText(11015, math.floor(remainingTime / 3600))
            end
            UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, nil, tips)
        end

        --文件下载失败后，避免进度条回滚
        if self.tempCurrentRealSize == nil or self.tempCurrentRealSize < self.currentRealSize then
            self.tempCurrentRealSize = self.currentRealSize
        end
        UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, self.tempCurrentRealSize, self.size)
        if #self.asyncList == 0 and #self.updateList == 0 then
            LuaHelper.WriteAllBytes(LuaHelper.Combine(LuaHelper.persistentDataPath, Const.MANIFESTFILE), self.remoteManifestBytes)
            self.assetUpdating = false
            self:AssetUpdateComplete()
        end
    end
end

function AssetUpdate:StartAssetUpdate()
    local hasAssetUpdate = APP.manifest.assetVersion ~= self.remoteManifest.assetVersion
    if hasAssetUpdate then
        self.size, self.currentSize = 0, 0
        local needUpdate = false
        self.updateList = {}
        for _, v in pairs(self.remoteManifest.data.Values) do
            needUpdate = true
            if APP.manifest:Contains(v.name) then
                if APP.manifest:Get(v.name).MD5 == v.MD5 then
                    needUpdate = false
                elseif v.MD5 == LuaHelper.GetMD5(LuaHelper.Combine(LuaHelper.persistentDataPath, v.name)) then
                    needUpdate = false
                end
            end
            if needUpdate then
                self.size = self.size + v.size / 1024.0
                table.insert(self.updateList, v)
            end
        end
        -- 记录清单文件字节
        if self.size == 0 then
            LuaHelper.WriteAllBytes(LuaHelper.Combine(LuaHelper.persistentDataPath, Const.MANIFESTFILE), self.remoteManifestBytes)
            self:AssetUpdateComplete()
        else
            local SureUpdate = function()
                UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, UIText(11008), UIText(11009))
                -- 更新准备
                LuaHelper.UnloadAssets(false)
                self.assetUpdating = true
                self.lastRealSize, self.speed, self.asyncList = 0, 0, {}
                APP.manifest.data:Clear()
                APP.manifestMapping.data:Clear()
                self.time = GetRealtimeSinceStartup()
            end
            if false and APP.dataNetwork then
                LuaHelper.ShowUI(UI.SecondaryView, 0, UIText(14002, self.size), UIText(12004), function ()
                    LuaHelper.HideUI(UI.SecondaryView)
                    SureUpdate()
                end, function ()
                    LuaHelper.Quit()
                end)
            else
                SureUpdate()
            end
        end
    else
        -- 如果没有资源更新，就直接认为更新完成
        self:Complete()
    end
end

function AssetUpdate:AssetUpdateComplete()
    LuaHelper.ClearUI()
    LuaHelper.UnloadAssets()
    APP.nextState = StateMachineType.AssetLoadState
    LuaInstance.instance:OnStopAndDestroy()
    LuaInstance.instance:OnStart()
end

function AssetUpdate:Complete()
    StateMachine:OnEnter(StateMachineType.AssetLoadState)
end

return AssetUpdate