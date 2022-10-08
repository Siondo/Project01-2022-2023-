--[[
Description: 网络协议
Author: xinZhao
Date: 2022-04-24 14:38:04
LastEditTime: 2022-04-24 14:38:04
--]]

NetWork = class('NetWork')

local ProtoWaitLockTime = 1

local function NetWorkLog(type, msg)
    if type == MsgType.Normal then
        log("<color=#FFCC22>[协议]</color> %s", msg)
    elseif type == MsgType.Send then
        log("<color=#008800>[请求]</color> %s", msg)
    elseif type == MsgType.Receive then
        log("<color=#0066FF>[接受]</color> %s", msg)
    elseif type == MsgType.ReceiveError then
        log("<color=#FF0000>[接受]</color> %s", msg)
    end
end


--[[
    @desc: 网络初始化
    time:2022-04-24 15:01:58
]]
function NetWork:onInit(callBack)
    self.isConnect = false

    local setup = function()
        HttpTool.SendRequest(UIWebType.GetAppInfo, {}, function(result, tbData)
            if result then
                if self.isObtainPort == -1 then
                    self.isObtainPort = 1
                    self.requestData = self.tbData
                    if self.obtainTimer then
                        self.obtainTimer:Stop()
                        self.obtainTimer = nil
                    end
            
                    self:onRegistEventModule()
                    self.netInstance = CS.NetMessage.instance
                    NetWorkLog(MsgType.Normal, string.format("成功获取到服务器端口数据 host:[%s] port:[%d] url:[%s]", tbData.host, tbData.port, tbData.url))
                    callBack(tbData)
                    UpdateEvent:AddListener(self.Update, self)

                    --网络延迟锁屏, 请求开始的一秒
                    self.netTimer = Timer.Create(function(loop)
                        if not self.netWorkIO then
                            local isAcitve = LuaHelper.IsShowUI(UI.ScreenLockView)
                            if not isAcitve then
                                LuaHelper.ShowUIImmediate(UI.ScreenLockView)
                            end
                        end
                    end, ProtoWaitLockTime, 1)
                end
            else
                NetWorkLog(MsgType.Normal, "与服务器端获取数据失败")
            end
        end)
    end

    if not self.obtainTimer then
        --无限重新请求远端Port地址
        self.obtainTimer = Timer.Create(function()
            if self.isObtainPort == -1 then
                UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, nil, nil, '获取失败, 正在尝试重新获取远端数据...')
                setup()
            end
        end, 3, 999)

        --首次请求
        setup()
        self.isObtainPort = -1
        self.obtainTimer:Start()
    end
end


--[[
    @desc: 连接服务器
    time:2022-04-24 14:38:36
]]
function NetWork:onConnect(callBack)
    if not self.timer then
        self.timer = Timer.Create(function(count)
            local str = count == 1 and '与服务器建立连接失败' or '与服务器建立连接失败, 正在尝试第 '..(5-count)..' 次重新连接'
            UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, nil, nil, str)
            self:onConnect(callBack, count)
        end, 5, 4)
    end

    self.netInstance:Connect(APP.host, APP.port, function (result)
        if result then
            NetWorkLog(MsgType.Normal, string.format("成功与服务器 %s:%d 建立连接", APP.host, APP.port))
            callBack(result)
            self.timer:Stop()
            self.timer = nil
        else
            self.timer:Start()
        end
    end)

    self.netInstance:ListenRecieve(function (protoType, data)
        self:onReceive(protoType, data)
    end)
end


--[[
    @desc: 断开连接
    time:2022-04-24 15:53:05
]]
function NetWork:DisConnect()
    self.netInstance:DisConnect()
end


--[[
    @desc: 连接状态
    time:2022-04-24 15:53:36
]]
function NetWork:Connected()
    return self.netInstance.connected
end


--[[
    @desc: 注册协议模块
    time:2022-04-24 16:51:42
]]
function NetWork:onRegistEventModule()
    --请求协议
    self.receiveProtoList = {}
    for name, data in pairs(UIProtoType) do
        self.receiveProtoList[data.protoID] = {
            protoName = name,
            protoID = data.protoID,
            ReceiveID = data.receiveId
        }
    end

    self.requestQueue = {}

    --推送协议
    self.pushProtoList = {}
    for name, data in pairs(UIProtoPushType) do
        self.pushProtoList[data.protoId] = {
            protoName = name,
            protoID = data.protoId,
            protoCbName = data.cbName
        }
    end
end


--[[
    @desc: 协议请求
    time:2022-04-24 16:32:10
    --@protoType: 协议类型
	--@parmas: 参数
	--@callBack: 回调
]]
function NetWork:onRequest(protoType, parmas, callBack)
    parmas = parmas or {}
    parmas.client = 1
    local data = parmas and Json.encode(parmas) or '{}'
    local protoName = 'Request_Event_'..self.receiveProtoList[protoType].protoName or 'NULL'
    NetWorkLog(MsgType.Send, '['..protoType..']协议('..protoName..') 传出数据流: '..data)

    --记录回调
    self.receiveProtoList[protoType].callBack = callBack
    self.requestQueue[#self.requestQueue + 1] = {
        protoType = protoType,
        data = data,
        config = self.receiveProtoList[protoType],
        isRequest = false,
    }
end


--[[
    @desc: 队列执行协议
    time:2022-05-05 17:40:18
]]
function NetWork:Update()
    if self.requestQueue and #self.requestQueue >= 1 then
        if self.requestQueue[1].isRequest == false then
            self.requestQueue[1].isRequest = true
            self.netInstance:Send(self.requestQueue[1].protoType, self.requestQueue[1].data)
            self.netTimer:Start()--开启锁屏
        end
    end
end


--[[
    @desc: 停止锁屏计时器
    time:2022-05-06 14:08:51
]]
function NetWork:onStopNetScreenLockTimer()
    self.netWorkIO = nil
    LuaHelper.HideUI(UI.ScreenLockView)
    self.netTimer:Stop()
end


--[[
    @desc: 协议返回
    time:2022-04-24 16:32:32
    --@protoType: 协议类型
	--@data: 传入数据流
	--@callBack: 回调
]]
function NetWork:onReceive(protoType, data)
    try(function ()
        if string.sub(data, 1, 1) ~= "{" then
            NetWorkLog(MsgType.Normal, "Json 格式错误: "..protoType..", data: "..data)
            return
        end

        local status = protoType ~= -1 and true or false
        local jsonData = Json.decode(data)
        log('protoId: %d  json: %s', protoType, data)

        --推送协议
        if self.pushProtoList[protoType] then
            local isPass = true
            if jsonData.client and jsonData.client == 1 then
                isPass = false
            end

            if isPass then
                local cbName = self.pushProtoList[protoType].protoCbName
                if cbName then
                    NetWorkLog(MsgType.Normal, '推送协议(PUSH_EVENT_'..self.pushProtoList[protoType].protoName..') '..data)
                    NetWork[cbName](self, jsonData)
                    return
                end
                NetWorkLog(MsgType.Normal, '推送协议(PUSH_EVENT_'..self.pushProtoList[protoType].protoName..') 未绑定推送方法')
                return
            end
        end

        self.netWorkIO = true
        --请求协议
        local reData = protoType == -1 and self.receiveProtoList[jsonData.code] or self.receiveProtoList[protoType - 1]
        if not reData then
            NetWorkLog(MsgType.ReceiveError, '协议接受编号(receiveId): '..protoType..' 在网络消息注册中心中找不到, 请确保服务器返回的是 receiveId')
            return
        end

        if status then
            NetWorkLog(MsgType.Receive, '['..protoType..']协议(Receive_Event_'..reData.protoName..') 传入数据流: '..data)
            if reData.callBack then
                reData.callBack(status, jsonData)
            end
        else
            NetWorkLog(MsgType.ReceiveError, '['..protoType..']协议(Receive_Event_'..reData.protoName..') 传入数据流: '..data)
            if reData.callBack then
                reData.callBack(status, jsonData.errCode)
            end
        end

        reData.callBack = nil --清理回调
        local newList = {}
        for i = 1, #self.requestQueue do
            if reData.protoName ~= self.requestQueue[i].config.protoName then
                newList[#newList + 1] = self.requestQueue[i]
            end
        end
        self.requestQueue = newList
        self:onStopNetScreenLockTimer()
    end)
end


--以下服务器主动推送方法注册:--------------------
-----------------------------------------------

--[[
    @desc: 通用推送
    time:2022-05-23 15:20:46
]]
function NetWork:onCommonPush(data)
    ItemData.items[ItemType.COIN] = data.gold                 --金币
    ItemData.items[ItemType.POWER] = data.strength            --体力
    ItemData:onOperateInfinite(data.infiniteStrengthEndTime)  --无限体力

    UIEvent:OnEvent(UIEvent.MainView.RefreshStatusBar)
end


--[[
    @desc: 好友申请主动推送
    time:2022-05-06 11:12:32
]]
function NetWork:onFriendApplyPush(data)
    for i = 1, #data.data do
        FriendData:onAddApplyList(data.data[i])
    end
    UIEvent:OnEvent(UIEvent.FriendListView.RefreshApplyFriendList)
end


--[[
    @desc: 好友同意/删除主动推送
    time:2022-05-06 11:12:46
]]
function NetWork:onFriendListPush(data)
    local list = {}
    for i = 1, #data.data do
        list[#list+1] = {
            info = data.data[i],
            root = 'FriendRoot',
            iconName = {
                'friend_life1',
                'friend_fight'
            }
        }
    end
    FriendData.friendList = list
    UIEvent:OnEvent(UIEvent.FriendListView.RefreshFriendList)
end


--[[
    @desc: 好友赠送体力主动推送
    time:2022-05-06 11:12:46
]]
function NetWork:onFriendSendPowerPush(data)
    for i = 1, #data.data do
        FriendData:onAddCliamList(data.data[i])
    end
    UIEvent:OnEvent(UIEvent.FriendListView.RefreshCliamList)
end


--[[
    @desc: 邀请好友对战主动推送 [邀请]
    time:2022-05-06 19:26:55
]]
function NetWork:onInviteFriendBattlePush(data)
    FriendData:onAddFriendInviteMessage(data.data)

    --在线的情况下, 推送的消息进行线上叠加
    if ReconnectionData:onGetInit() then
        ReconnectionData:onAddEventHander(ReconnectionType.FriendInviteReconnect)
    end
end


--[[
    @desc: 同意好友对战主动推送 [同意]
    time:2022-05-11 19:16:13
]]
function NetWork:onAgreeFriendInvitePush(data)
    FriendData:onAddFriendAgreeMessage(data.data)

    --在线的情况下, 推送的消息进行线上叠加
    if ReconnectionData:onGetInit() then
        ReconnectionData:onAddEventHander(ReconnectionType.FriendAgreeReconnect)
    end
end


--[[
    @desc: 赛季结算数据推送
    time:2022-05-24 15:56:22
]]
function NetWork:onSeasonEndPush(data)
    RankData:onSetSeasonEndInfo(data)

    --在线的情况下, 推送的消息进行线上叠加
    if ReconnectionData:onGetInit() then
        if data.rankType == 1 then
            ReconnectionData:onAddEventHander(ReconnectionType.SeasonEndStar)
        else
            ReconnectionData:onAddEventHander(ReconnectionType.SeasonEndTrophy)
        end
    end
end


--[[
    @desc: 其他玩家加入/退出匹配推送
    time:2022-05-07 15:58:04
]]
function NetWork:onUserJoinPvPPush(data)
    if data.state == 1 then
        --加入匹配
        MatchData:onAddPlayerToPVPMatch(data)
    else
        --退出匹配
        MatchData:onRemovePlayerToPVPMatch(data)
    end
    UIEvent:OnEvent(UIEvent.PVPMatchView.OnAddPVPPlayer)
end


--[[
    @desc: 签到数据跨天推送更新
    time:2022-05-17 15:33:26
]]
function NetWork:onSignInfoUpdatePush(data)
    SignData:onSetInfo(data)
    if LuaHelper.IsShowUI(UI.SignView) then
        UIEvent:OnEvent(UIEvent.SignView.OnRefresh)
    end
end


--[[
    @desc: 通用刷新推送
    time:2022-05-18 14:56:16
]]
function NetWork:onRefreshGameViewPush(data)
    --PVP/好友对战 只要有玩家结束战斗都会进行推送
    if data.type == 1 or data.type == 2 then
        RecordData:onPullRecordData(true)
    end

    --PVE结束
    FriendData:onPullAllFrendData()
end

return NetWork