
GlobalLogic = {}

function GlobalLogic:Init()
    Net:Register(MSG_ID.Heart_SC, self.ReceiveHeartMsg, self)
    Net:Register(MSG_ID.Login_SC, self.ReceiveLoginMsg, self)
    Net:Register(MSG_ID.Turntable_SC, self.ReceiveTurntableMsg, self)
    Net:Register(MSG_ID.GetGold_SC, self.ReceiveGetGoldMsg, self)
    Net:Register(MSG_ID.GetDailyTaskInfo_SC, self.ReceiveGetDailyTaskInfoMsg, self)
    Net:Register(MSG_ID.GetDailyTaskReward_SC, self.ReceiveGetDailyTaskRewardMsg, self)
    Net:Register(MSG_ID.GetDailyTaskActiveReward_SC, self.ReceiveGetDailyTaskActiveRewardMsg, self)
    Net:Register(MSG_ID.GetRoleInfo_SC, self.ReceiveRoleInfoMsg, self)
    Net:Register(MSG_ID.RoleUp_SC, self.ReceiveRoleUpMsg, self)
    Net:Register(MSG_ID.UnlockHero_SC, self.ReceiveUnlockHeroMsg, self)
    Net:Register(MSG_ID.EquipHero_SC, self.ReceiveEquipHeroMsg, self)
    Net:Register(MSG_ID.SignInfo_SC, self.ReceiveSignInfoMsg, self)
    Net:Register(MSG_ID.Sign_SC, self.ReceiveSignMsg, self)
    Net:Register(MSG_ID.Guide_SC, self.ReceiveGuideMsg, self)
    Net:Register(MSG_ID.RecoveryHero_SC, self.ReceiveRecoveryHeroMsg, self)
    Net:Register(MSG_ID.Tower_Ready_SC, self.ReceiveTowerReadyMsg, self)
    Net:Register(MSG_ID.Tower_Start_SC, self.ReceiveTowerStartMsg, self)
    Net:Register(MSG_ID.Tower_Kill_Monster_SC, self.ReceiveTowerKillMonsterMsg, self)
    Net:Register(MSG_ID.Tower_End_SC, self.ReceiveTowerEndMsg, self)

    --推送消息
    Net:Register(MSG_ID.Push_Money_SC, self.ReceiveChangeMoneyMsg, self)
    Net:Register(MSG_ID.Push_Recovery_SC, self.ReceiveRecoveryPushMsg, self)
    Net:Register(MSG_ID.Push_DailyTask_SC, self.ReceiveDailyTaskPushMsg, self)
    Net:Register(MSG_ID.Push_RedDot_SC, self.ReceiveRedDotPushMsg, self)
    Net:Register(MSG_ID.Push_Across_SC, self.ReceiveAcrossPushMsg, self)
    Net:Register(MSG_ID.Push_UnlockHero_SC, self.ReceiveUnlockHeroPushMsg, self)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 13:52
---@see Money改变
function GlobalLogic:ReceiveChangeMoneyMsg(msg)
    if msg.gold then
        if LuaHelper.IsShowUI(UI.UIGetGold) or LuaHelper.IsShowUI(UI.UITurntable) or
                LuaHelper.IsShowUI(UI.UISign) or LuaHelper.IsShowUI(UI.UIDailyTask) then
        else
            local add = msg.gold - PlayerData:getGold()
            UIEvent:OnEvent(UIEvent.UI.AddGold, add, true)
        end
    end
    if msg.diamond then
        if LuaHelper.IsShowUI(UI.UIGetGold) or LuaHelper.IsShowUI(UI.UITurntable) or
                LuaHelper.IsShowUI(UI.UISign) or LuaHelper.IsShowUI(UI.UIDailyTask) then
        else
            local add = msg.diamond - PlayerData:getDiamonds()
            UIEvent:OnEvent(UIEvent.UI.AddDiamonds, add, true)
        end
    end
    if msg.skinDebris then
        PlayerData:setSkinDebris(msg.skinDebris)
    end
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 15:37
---@see 推送恢复消息
function GlobalLogic:ReceiveRecoveryPushMsg(msg)
    PlayerData:UpdateHeroPb(msg)
    UIEvent:OnEvent(UIEvent.Recovery, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 10:43
---@see 推送日常任务消息
function GlobalLogic:ReceiveDailyTaskPushMsg(msg)
    PlayerData:updateDailyTaskData(msg)
    PlayerLogic:DailyTaskRedDotUpdate()
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 10:38
---@see 推送红点消息
function GlobalLogic:ReceiveRedDotPushMsg(msg)
    --初始化红点更新
    RedDot.Notice(RedDotType.Sign, msg.sign ~= nil)
    RedDot.Notice(RedDotType.Turntable, msg.draw ~= nil)
    RedDot.Notice(RedDotType.DailyTask, msg.task ~= nil)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 11:22
---@see 接收到跨天消息
function GlobalLogic:ReceiveAcrossPushMsg(msg)
    logError("ReceiveAcrossPushMsg")
    UIEvent:OnEvent(UIEvent.PushAcross, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/19 11:46
---@see 接收到被动解锁英雄
function GlobalLogic:ReceiveUnlockHeroPushMsg(msg)
    PlayerData:PushUnlockSkinId(msg.heroId)
    PlayerData:UpdateHeroPb(msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 14:13
---@see 发送心跳消息
function GlobalLogic:SendHeartMsg()
    local msg = nil
    Net:Send(MSG_ID.Heart_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 14:13
---@see 接收到心跳消息
function GlobalLogic:ReceiveHeartMsg(msg)
    PlayerData:setServerTime(msg.time)

    coroutine.start(function ()
        coroutine.wait(10)
        GlobalLogic:SendHeartMsg()
    end)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 15:49
---@see 发送登录消息
function GlobalLogic:SendLoginMsg(uc_token)
    local msg = {}
    msg.uc_token = uc_token or APP.token
    msg.token = string.Empty
    msg.name = string.Empty
    msg.head = string.Empty
    Net:Send(MSG_ID.Login_CS, msg)
    --记录最近登录的Token
    LuaHelper.SetString(Const.LAST_LOGIN_TOKEN, msg.uc_token)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 15:49
---@see 接收登录消息
function GlobalLogic:ReceiveLoginMsg(msg)
    if Net.reConnect then
        Net.reConnect = false
        UIEvent:OnEvent(UIEvent.ReLoginSuccess, msg)
    else
        UIEvent:OnEvent(UIEvent.LoginSuccess, msg)
    end
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 16:18
---@see 发送转盘启动消息
function GlobalLogic:SendTurntableMsg(lookGG)
    local msg = {}
    msg.lookGG = lookGG
    Net:Send(MSG_ID.Turntable_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 16:19
---@see 返回转盘启动消息
function GlobalLogic:ReceiveTurntableMsg(msg)
    UIEvent:OnEvent(UIEvent.StartTurntable, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 17:55
---@see 发送得到金币
function GlobalLogic:SendGetGoldMsg(getType)
    local msg = {}
    msg.getType = getType
    Net:Send(MSG_ID.GetGold_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 17:55
---@see 返回得到金币
function GlobalLogic:ReceiveGetGoldMsg(msg)
    UIEvent:OnEvent(UIEvent.GetGoldSuccess, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:19
---@see 请求得到日常任务信息
function GlobalLogic:SendGetDailyTaskInfoMsg()
    local msg = {}
    msg.taskType = TaskType.DailyTask
    Net:Send(MSG_ID.GetDailyTaskInfo_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:19
---@see 返回日常任务信息
function GlobalLogic:ReceiveGetDailyTaskInfoMsg(msg)
    UIEvent:OnEvent(UIEvent.GetDailyTaskInfo, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:20
---@see 请求得到日常任务奖励
function GlobalLogic:SendGetDailyTaskRewardMsg(taskId)
    local msg = {}
    msg.taskType = TaskType.DailyTask
    msg.taskId = taskId
    Net:Send(MSG_ID.GetDailyTaskReward_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:20
---@see 得到日常任务奖励返回
function GlobalLogic:ReceiveGetDailyTaskRewardMsg(msg)
    UIEvent:OnEvent(UIEvent.GetDailyTaskReward, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:24
---@see 请求得到日常任务活跃奖励
function GlobalLogic:SendGetDailyTaskActiveRewardMsg(awardId)
    local msg = {}
    msg.taskType = TaskType.DailyTask
    msg.awardId = awardId
    Net:Send(MSG_ID.GetDailyTaskActiveReward_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 18:24
---@see 得到日常任务活跃奖励返回
function GlobalLogic:ReceiveGetDailyTaskActiveRewardMsg(msg)
    UIEvent:OnEvent(UIEvent.GetDailyTaskActive, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:33
---@see 请求角色信息
function GlobalLogic:RequestRoleInfoMsg()
    local msg = nil
    Net:Send(MSG_ID.GetRoleInfo_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:34
---@see 返回角色信息
function GlobalLogic:ReceiveRoleInfoMsg(msg)
    UIEvent:OnEvent(UIEvent.GetRoleInfo, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:35
---@see 请求角色升级
function GlobalLogic:RequestRoleUpMsg(heroId, upType)
    local msg = {}
    msg.heroId = heroId
    msg.upType = upType
    Net:Send(MSG_ID.RoleUp_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:35
---@see 返回角色升级
function GlobalLogic:ReceiveRoleUpMsg(msg)
    UIEvent:OnEvent(UIEvent.RoleUp, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:38
---@see 解锁英雄
function GlobalLogic:RequestUnlockHeroMsg(skinId)
    local msg = {}
    msg.skinId = skinId
    Net:Send(MSG_ID.UnlockHero_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:39
---@see 解锁英雄返回
function GlobalLogic:ReceiveUnlockHeroMsg(msg)
    UIEvent:OnEvent(UIEvent.UnlockHero, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:42
---@see 请求上阵英雄
function GlobalLogic:RequestEquipHeroMsg(skinId)
    local msg = {}
    msg.skinId = skinId
    Net:Send(MSG_ID.EquipHero_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/14 19:42
---@see 收到英雄上阵
function GlobalLogic:ReceiveEquipHeroMsg(msg)
    PlayerData:setEquipHero(msg.equipSkin)
    UIEvent:OnEvent(UIEvent.EquipHero, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 11:26
---@see 发送战斗结束
function GlobalLogic:SendBattleEndMsg(chapterId, costResidueValue, isWin, killData, callback, selfParam)
    local msg = {}
    msg.chapterId = chapterId
    msg.costResidueValue = costResidueValue
    msg.isWin = isWin
    msg.killData = killData
    Net:Send(MSG_ID.BattleEnd_CS, msg, callback, selfParam)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 13:55
---@see 请求签到信息
function GlobalLogic:RequestSignInfoMsg()
    local msg = nil
    Net:Send(MSG_ID.SignInfo_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 13:55
---@see 返回签到信息
function GlobalLogic:ReceiveSignInfoMsg(msg)
    UIEvent:OnEvent(UIEvent.SignInfo, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 13:57
---@see 请求签到消息
function GlobalLogic:RequestSignMsg(getType)
    local msg = {}
    msg.getType = getType
    Net:Send(MSG_ID.Sign_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/15 13:57
---@see 接收到签到消息
function GlobalLogic:ReceiveSignMsg(msg)
    UIEvent:OnEvent(UIEvent.SignSuccess, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 10:25
---@see 发送引导消息
function GlobalLogic:RequestGuideMsg(guide_id)
    local msg = {}
    msg.guide_id = guide_id
    Net:Send(MSG_ID.Guide_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 10:26
---@see 返回引导消息
function GlobalLogic:ReceiveGuideMsg(msg)

end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 9:42
---@see 请求恢复英雄
function GlobalLogic:RequestRecoveryHeroMsg(heroId, type)
    local msg = {}
    msg.heroId = heroId
    msg.type = type
    Net:Send(MSG_ID.RecoveryHero_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/18 9:41
---@see 收到主动恢复英雄
function GlobalLogic:ReceiveRecoveryHeroMsg(msg)
    PlayerData:setRecoveryNum(msg.recover_num)
    local heroPb = PlayerData:GetHeroPb(msg.heroId)
    if heroPb then
        heroPb.resume_time_num = msg.resume_time_num
    end
    UIEvent:OnEvent(UIEvent.Recovery, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 10:48
---@see 试炼准备
function GlobalLogic:RequestTowerReadyMsg()
    local msg = nil
    Net:Send(MSG_ID.Tower_Ready_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 10:48
---@see 收到试炼准备
function GlobalLogic:ReceiveTowerReadyMsg(msg)
    UIEvent:OnEvent(UIEvent.TowerReady, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 11:29
---@see 请求试炼开始
function GlobalLogic:RequestTowerStartMsg(heroId, benefitType)
    local msg = {}
    msg.heroId = heroId
    msg.benefitType = benefitType
    Net:Send(MSG_ID.Tower_Start_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 11:31
---@see 返回试炼开始
function GlobalLogic:ReceiveTowerStartMsg(msg)
    UIEvent:OnEvent(UIEvent.TowerStart, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 16:34
---@see 试炼杀死怪物
function GlobalLogic:RequestTowerKillMonsterMsg(monsterId, monsterNum)
    local msg = {}
    msg.monsterId = monsterId
    msg.monsterNum = monsterNum
    Net:Send(MSG_ID.Tower_Kill_Monster_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 16:34
---@see 试炼杀怪返回
function GlobalLogic:ReceiveTowerKillMonsterMsg(msg)
    UIEvent:OnEvent(UIEvent.TowerEmenyDie, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 17:11
---@see 离开试炼
function GlobalLogic:RequestTowerEndMsg()
    local msg = nil
    Net:Send(MSG_ID.Tower_End_CS, msg)
end

---@module GlobalLogic.lua
---@author Rubble
---@since 2022/4/22 17:11
---@see 返回离开试炼
function GlobalLogic:ReceiveTowerEndMsg(msg)
    PlayerData:UpdateHeroPb(msg.heroData)
    UIEvent:OnEvent(UIEvent.TowerEnd, msg)
end