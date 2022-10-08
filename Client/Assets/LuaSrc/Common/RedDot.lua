local data = {}

local function Get(name)
    local tbRedDot = data[name]
    if tbRedDot == nil then
        tbRedDot = {}
        tbRedDot.name = name
        tbRedDot.bActive = false
        tbRedDot.tbListen = {}--字典
        tbRedDot.tbNotice = {}--字典
        tbRedDot.nListenCnt = 0--监听列表大小
        data[name] = tbRedDot

        function tbRedDot:AddListen(name)
            if not self.tbListen[name] then
                self.tbListen[name] = true
                self.nListenCnt = self.nListenCnt + 1
            end
        end

        function tbRedDot:RemoveListen(name)
            if self.tbListen[name] then
                self.tbListen[name] = nil
                self.nListenCnt = self.nListenCnt - 1
            end
        end

        function tbRedDot:ClearListen()
            self.tbListen = {}
            self.nListenCnt = 0
        end

        function tbRedDot:AddNotice(name)
            if not self.tbNotice[name] then
                self.tbNotice[name] = true
            end
        end

        function tbRedDot:RemoveNotice(name)
            if self.tbNotice[name] then
                self.tbNotice[name] = nil
            end
        end

        function tbRedDot:ClearNotice()
            self.tbNotice = {}
        end
    end
    return tbRedDot
end

local function For(func)
    for _, v in pairs(data) do
        func(v)
    end
end

local function Remove(name)
    if name == nil then
        data = {}
    else
        data[name] = nil
    end
end

local function UpdateRedDotState(name)
    local tbRedDot = Get(name)
    if tbRedDot.nListenCnt == 0 then
        return tbRedDot.bActive
    else
        for k, v in pairs(tbRedDot.tbListen) do
            if UpdateRedDotState(k) then
                return true
            end
        end
        return false
    end
end

local function NoticeUpdate(name)
    local tbRedDot = Get(name)
    local bActive = UpdateRedDotState(name)
    if bActive ~= tbRedDot.bActive then
        tbRedDot.bActive = bActive
        if IsNotNull(tbRedDot.go) and tbRedDot.event ~= nil then
            tbRedDot.event(tbRedDot.bActive)
        end
        for k, v in pairs(tbRedDot.tbNotice) do
            NoticeUpdate(k)
        end
    end
end

local function Notice(name, bActive)
    local tbRedDot = Get(name)
    if tbRedDot.nListenCnt > 0 then
        logError("<color=red>外部不能直接改变有子红点的红点对象，只能通过子红点的改变影响父红点</color>")
        return
    end
    if bActive ~= tbRedDot.bActive then
        tbRedDot.bActive = bActive
        if IsNotNull(tbRedDot.go) and tbRedDot.event ~= nil then
            tbRedDot.event(tbRedDot.bActive)
        end
        for k, v in pairs(tbRedDot.tbNotice) do
            NoticeUpdate(k)
        end
    end
end

local function AddRedDot(name, go, func, tbListen)
    tbListen = tbListen or {}

    local tbRedDot = Get(name)
    tbRedDot.go = go
    tbRedDot.event = func

    local tbTemp = nil
    for i, v in ipairs(tbListen) do
        tbTemp = Get(v)
        tbTemp:AddNotice(name)
        tbRedDot:AddListen(v)
    end

    tbRedDot.bActive = UpdateRedDotState(name)
    if IsNotNull(tbRedDot.go) and tbRedDot.event ~= nil then
        tbRedDot.event(tbRedDot.bActive)
    end
end

local function RemoveRedDot(name)
    local tbRedDot = Get(name)

    local tbTemp = nil
    for k, v in pairs(tbRedDot.tbListen) do
        tbTemp = Get(k)
        tbTemp:RemoveNotice(name)
    end
    tbRedDot:ClearListen()

    tbRedDot.bActive = false
    if IsNotNull(tbRedDot.go) and tbRedDot.event ~= nil then
        tbRedDot.event(tbRedDot.bActive)
    end

    for k, v in pairs(tbRedDot.tbNotice) do
        tbTemp = Get(k)
        tbTemp:RemoveListen(name)
        NoticeUpdate(k)
    end
    tbRedDot:ClearNotice()

    Remove(name)
end

local function RemoveAllRedDot()
    For(function (tbRedDot)
        tbRedDot.bActive = false
        if IsNotNull(tbRedDot.go) and tbRedDot.event ~= nil then
            tbRedDot.event(_tbRedDot.bActive)
        end
    end)
    Remove()
end

RedDotType = {
    Sign = "Sign",                                                          --签到
    Turntable = "Turntable",                                                --转盘
    FreeGetGold = "FreeGetGold",                                            --免费获取金币红点
    RoleUpgrade = "RoleUpgrade",                                            --角色升级
    BaseAttUpgrade = "BaseAttUpgrade",                                      --基础属性升级
    MagicUpgrade = "MagicUpgrade",                                          --分身升级
    RecoveryUpgrade = "RecoveryUpgrade",                                    --回复升级
    DailyTask = "DailyTask",                                                --日常任务
}

RedDot = {
    Add = function (name, go, func, tbListen)
        try(function ()
            AddRedDot(name, go, func, tbListen)
        end)
    end,
    Remove = function (name)
        try(function ()
            if string.IsNotNullOrEmpty(name) then
                RemoveRedDot(name)
            else
                RemoveAllRedDot()
            end
        end)
    end,
    Notice = function (name, bActive)
        try(function ()
            Notice(name, bActive)
        end)
    end,
    Init = function ()
        try(function ()
            RemoveAllRedDot()
            --开局一个人
            AddRedDot(RedDotType.Sign)
            AddRedDot(RedDotType.Turntable)
            AddRedDot(RedDotType.FreeGetGold)
            AddRedDot(RedDotType.BaseAttUpgrade)
            AddRedDot(RedDotType.MagicUpgrade)
            AddRedDot(RedDotType.RecoveryUpgrade)
            AddRedDot(RedDotType.RoleUpgrade, nil, nil, {RedDotType.BaseAttUpgrade, RedDotType.MagicUpgrade, RedDotType.RecoveryUpgrade})
        end)
    end,
}

