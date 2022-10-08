--[[
    Description: 重连数据类
    Author: xinZhao
    Date: 2022-05-12 14:14:52
]]

ReconnectionData = class('ReconnectionData')

function ReconnectionData:onGetInit()
    return self.isInit
end


function ReconnectionData:onSetUpdateFunctionActive()
    UpdateEvent:AddListener(self.Update, self)
    self.isInit = true
end


function ReconnectionData:onInit()
    self.isActiveUpdateFunc = nil
    self.connectList = {}
    self.isInit = false

    --赛季结算奖励
    self:onAddEventHander(ReconnectionType.SeasonEndStar)
    self:onAddEventHander(ReconnectionType.SeasonEndTrophy)

    --PVP 上次下线未完成的战斗
    if not IsEmptyStringOrNull(MatchData:onGetPVPRoom()) then
        self:onAddEventHander(ReconnectionType.MatchPVPReconnect)
    end

    --好友PK 上次下线未完成的操作- 同意/邀请
    if not LuaHelper.IsShowUI(UI.BattleListView) then
        self:onAddEventHander(ReconnectionType.FriendInviteReconnect)
        self:onAddEventHander(ReconnectionType.FriendAgreeReconnect)
    end
end


--[[
    @desc: 向重连数据中添加一条info
    time:2022-05-12 10:32:33
]]
function ReconnectionData:onAddEventHander(eventName, eventCallBack)
    local isInsert = false
    --PVP/好友主玩法内退出重连
    if eventName == ReconnectionType.MatchPVPReconnect then
        eventCallBack = eventCallBack or function()
            LuaHelper.ShowUI(UI.MatchStopView)
        end
        
    --好友邀请战斗/同意战斗
    elseif eventName == ReconnectionType.FriendInviteReconnect or eventName == ReconnectionType.FriendAgreeReconnect then
        isInsert = #FriendData:onGetFriendBattleMessage(eventName) > 0 and true or false

        eventCallBack = eventCallBack or function()
            local infos = FriendData:onGetFriendBattleMessage(eventName)
            if #infos > 0 then
                FriendData:onOpenFriendInviteView(eventName)
            else
                self:onHandlerEventMessage()
            end
        end

    --赛季奖励
    elseif eventName == ReconnectionType.SeasonEndStar or eventName == ReconnectionType.SeasonEndTrophy then
        isInsert = RankData:onGetSeasonEndInfo(eventName) ~= nil and true or false

        eventCallBack = eventCallBack or function()
            local infos = RankData:onGetSeasonEndInfo(eventName)
            if infos then
                LuaHelper.ShowUI(UI.SeaonEndView, infos)
            else
                self:onHandlerEventMessage()
            end
        end
    end

    if isInsert then
        self.connectList[#self.connectList + 1] = {
            isHandler = false,
            eventName = eventName,
            callBack = eventCallBack,
        } 
    end
end


--[[
    @desc: 执行重连数据
    time:2022-05-12 10:32:55
]]
function ReconnectionData:Update()
    if not MatchManager.isStart and self.connectList and #self.connectList > 0 then
        local data = self.connectList[1]
        if not data.isHandler then
            data.isHandler = true
            data.callBack()
        end
    end
end


--[[
    @desc: 处理执行的数据
    time:2022-05-12 11:04:37
]]
function ReconnectionData:onHandlerEventMessage()
    if self.connectList and #self.connectList > 0 then
        local data = self.connectList[1]
        if data.isHandler then
            local infos = {}
            for i = 2, #self.connectList do
                infos[#infos + 1] = self.connectList[i]
            end
            self.connectList = infos
        end
    end
end


return ReconnectionData