
Net = {}
Net.csNet = CS.NetMessage.instance
Net.msgFunc = {}
Net.msgCallback = {}

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 10:21
---@see 连接状态
function Net:Connected()
    return Net.csNet.connected
end

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 10:20
---@see 连接
function Net:Connect(host, port, callback)
    Net.csNet:Connect(host, port, function (result)
        if result then
            log("Connect Server Success!")
            --self:ListenRecieve()
            self:SetExceptionAction()
            callback(result)
        else
            logError("Connect Server Fail!")
            Net:Connect(host, port, callback)
        end
    end)
end

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 10:20
---@see 断开连接
function Net:DisConnect()
    Net.csNet:DisConnect()
end

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 10:22
---@see 注册消息
function Net:Register(msgCode, callback, param)
    local tb = Net.msgFunc[msgCode]
    if tb then
        logError("消息重复注册: "..msgCode)
    else
        Net.msgFunc[msgCode] = { msgCode = msgCode, callback = callback, param = param}
    end
end

---@module NetMessage.lua
---@author Rubble
---@since 2022/4/14 12:44
---@see 监听接收到的消息
function Net:ListenRecieve()
    Net.csNet:ListenRecieve(function (msgCode, data)
        Net:Receive(msgCode, data)
    end)
end

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 10:35
---@see 接收到消息
function Net:Receive(msgCode, data)
    log("SC："..msgCode.."\n"..data)
    try(function ()
        if string.sub(data, 1, 1) ~= "{" then
            logError("Json 格式错误: "..msgCode..", data: "..data)
            return
        end
        local jsonData = Json.decode(data)

        local tb = Net.msgCallback[msgCode]
        if tb then
            Net.msgCallback[msgCode] = nil
            if tb.param then
                tb.callback(tb.param, jsonData)
            else
                tb.callback(jsonData)
            end
        else
            tb = Net.msgFunc[msgCode]
            if tb then
                if tb.param then
                    tb.callback(tb.param, jsonData)
                else
                    tb.callback(jsonData)
                end
            else
                logError("收到未注册的消息: "..msgCode..", data: "..data)
            end
        end
    end)
end

---@module NetManager.lua
---@author Rubble
---@since 2022/4/14 11:34
---@see 发送消息
function Net:Send(msgCode, jsonData, callback, selfParam)
    local data = nil
    if jsonData then
        data = Json.encode(jsonData)
    else
        data = "{}"
    end
    log("CS:"..msgCode.."\n"..data)
    Net.csNet:Send(msgCode, data)

    if callback then
        msgCode = msgCode + 1
        local tb = Net.msgCallback[msgCode]
        if tb then
            logError("消息重复监听: "..msgCode)
        end
        Net.msgCallback[msgCode] = { msgCode = msgCode, callback = callback, param = selfParam}
    end
end

---@module NetMessage.lua
---@author Rubble
---@since 2022/4/18 16:00
---@see 设置异常事件
function Net:SetExceptionAction()
    Net.csNet:SetExceptionAction(function ()
        Net.reConnect = true
        LuaHelper.ShowUI(UI.UINetReConnect)
        Net:Connect(APP.host, APP.port, function (result)
            if result then
                coroutine.start(function ()
                    coroutine.step(1)

                    local token = LuaHelper.GetString(Const.LAST_LOGIN_TOKEN)
                    GlobalLogic:SendLoginMsg(token)
                    LuaHelper.HideUI(UI.UINetReConnect)
                end)
            end
        end)
    end)
end

--消息ID
--客户端发送_CS
--接收服务器消息_SC
MSG_ID = {
    --推送
    Push_Money_SC = 2,                                   --推送Money
    Push_Recovery_SC = 4,                                --恢复
    Push_DailyTask_SC = 5,                               --日常任务
    Push_RedDot_SC = 6,                                  --红点
    Push_Across_SC = 7,                                  --跨天
    Push_UnlockHero_SC = 8,                              --解锁新英雄

    --心跳
    Heart_CS = 999,
    Heart_SC = 1000,

    --登录
    Login_CS = 1001,
    Login_SC = 1002,

    --转盘
    Turntable_CS = 1101,
    Turntable_SC = 1102,

    --得到金币
    GetGold_CS = 1201,
    GetGold_SC = 1202,

    --任务
    GetDailyTaskInfo_CS = 1301,
    GetDailyTaskInfo_SC = 1302,
    GetDailyTaskReward_CS = 1303,
    GetDailyTaskReward_SC = 1304,
    GetDailyTaskActiveReward_CS = 1305,
    GetDailyTaskActiveReward_SC = 1306,

    --角色
    GetRoleInfo_CS = 1401,
    GetRoleInfo_SC = 1402,
    RoleUp_CS = 1403,
    RoleUp_SC = 1404,
    UnlockHero_CS = 1405,
    UnlockHero_SC = 1406,
    EquipHero_CS = 1407,
    EquipHero_SC = 1408,

    --战斗
    BattleEnd_CS = 1501,
    BattleEnd_SC = 1502,

    --签到
    SignInfo_CS = 1601,
    SignInfo_SC = 1602,
    Sign_CS = 1603,
    Sign_SC = 1604,

    --引导
    Guide_CS = 1801,
    Guide_SC = 1802,

    --恢复英雄
    RecoveryHero_CS = 1901,
    RecoveryHero_SC = 1902,

    --试炼
    Tower_Start_CS = 2001,
    Tower_Start_SC = 2002,
    Tower_Kill_Monster_CS = 2003,
    Tower_Kill_Monster_SC = 2004,
    Tower_End_CS = 2005,
    Tower_End_SC = 2006,
    Tower_Ready_CS = 2007,
    Tower_Ready_SC = 2008,
}