
PlayerLogic = {}

--数据
local data = {
    secrect = "37574B9964DE8D1B5D0DDEAF8C64321G",
    userID = "0",               --用户ID
}

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/9 16:59
---@see 是否可以免费获得金币
function PlayerLogic:IsCanFreeGetGold()
    local getNum, maxData, curData = PlayerData:getExchangeGoldNum() + 1
    for _, v in pairs(Config.tbGold) do
        if v.getType == GetGoldType.Diamonds then
            if v.time == getNum then
                curData = v
            end
            if nil == maxData or v.time > maxData.time then
                maxData = v
            end
        end
    end
    curData = curData or maxData
    return curData.cost == 0
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/9 17:03
---@see 是否可以升级基础属性等级
function PlayerLogic:IsCanUpgradeBaseAtt()
    local redDot, config = false, Config.tbQualityLevel[tostring(PlayerData:getBaseAttLv())]
    if config then
        local cost = config.cost
        if cost[1][1] == ItemType.Gold and PlayerData:getGold() >= cost[1][2] then
            redDot = true
        elseif cost[1][1] == ItemType.Diamonds and PlayerData:getDiamonds() >= cost[1][2] then
            redDot = true
        end
    end
    return redDot
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/9 17:15
---@see 是否可以升级分身数量等级
function PlayerLogic:IsCanUpgradeMagic()
    local redDot, config = false, Config.tbSeparation[tostring(PlayerData:getMagicLv())]
    if config then
        local cost = config.cost
        if cost[1][1] == ItemType.Gold and PlayerData:getGold() >= cost[1][2] then
            redDot = true
        elseif cost[1][1] == ItemType.Diamonds and PlayerData:getDiamonds() >= cost[1][2] then
            redDot = true
        end
    end
    return redDot
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/9 17:16
---@see 是否可以升级恢复分身等级
function PlayerLogic:IsCanUpgradeRecovery()
    local redDot, config = false, Config.tbAutoResume[tostring(PlayerData:getRecoveryLv())]
    if config then
        local cost = config.cost
        if cost[1][1] == ItemType.Gold and PlayerData:getGold() >= cost[1][2] then
            redDot = true
        elseif cost[1][1] == ItemType.Diamonds and PlayerData:getDiamonds() >= cost[1][2] then
            redDot = true
        end
    end
    return redDot
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 17:50
---@see 日常任务红点更新
function PlayerLogic:DailyTaskRedDotUpdate()
    local list, data = PlayerData:getDailyTaskData()
    local redDot = false
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and v.state ~= TaskState.YetGet then
            if v.schedule >= data.times then
                redDot = true
                break
            end
        end
    end
    if not redDot then
        local activeIds = PlayerData:getActiveIds()
        local activeValue = PlayerData:getActiveValue()
        local len, has = table.len(Config.tbBrisk)
        for i = 1, len do
            data = Config.tbBrisk[tostring(i)]
            if data and activeValue >= data.briskValue then
                has = false
                for _, v in ipairs(activeIds) do
                    if v == data.id then
                        has = true
                        break
                    end
                end
                if not has then
                    redDot = true
                    break
                end
            end
        end
    end

    RedDot.Notice(RedDotType.DailyTask, redDot)
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:26
---@see 日常任务数据重置
function PlayerLogic:DailyTaskReset()
    local list = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        v.schedule = 0
    end
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/14 9:58
---@see 登录报告
function PlayerLogic:LoginReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.Login then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 17:09
---@see 签到报告
function PlayerLogic:SignReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.Sign then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:07
---@see 得到金币报告
function PlayerLogic:GetGoldReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.GetGold then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:13
---@see 转盘
function PlayerLogic:TurntableReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.Luck then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:13
---@see 升级报告
function PlayerLogic:UpgradeReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.Upgrade then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end


---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:19
---@see 通关报告
function PlayerLogic:WinReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.Win then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end

---@module PlayerLogic.lua
---@author Rubble
---@since 2022/3/10 18:19
---@see 看广告报告
function PlayerLogic:LookAdReport()
    local list, data = PlayerData:getDailyTaskData()
    for _, v in pairs(list) do
        data = Config.tbTask[tostring(v.id)]
        if data and data.taskType == DailyTaskType.LookAd then
            v.schedule = v.schedule + 1
            break
        end
    end
    self:DailyTaskRedDotUpdate()
end