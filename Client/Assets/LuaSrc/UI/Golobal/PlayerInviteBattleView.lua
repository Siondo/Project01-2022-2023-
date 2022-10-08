--[[
Description: 玩家邀请战斗界面
Author: xinZhao
Date: 2022-06-05 14:24:10
LastEditTime: 2022-04-22 14:24:11
--]]

PlayerInviteBattleView = class('PlayerInviteBattleView', UIBase)


function PlayerInviteBattleView:OnLoad()
    self:AddEvent(UIEvent.PlayerInviteBattleView.OnAddMessage, function()
        FriendData:onDeleteElement(FriendData:onGetFriendBattleMessage(self.type), self.data, 4, self.type)
        self:onRefreshRedDot()
    end)

    self:AddEvent(UIEvent.PlayerInviteBattleView.OnGetCurrentDataInfo, function(cb)
        cb(self.data)
    end)
end


function PlayerInviteBattleView:OnShow(type)
    self.type = type
    self:onRefreshData()
end


function PlayerInviteBattleView:onRefreshData()
    self.data = clone(FriendData:onGetFriendBattleMessage(self.type)[1])
    if not self.data then
        ReconnectionData:onHandlerEventMessage()
        self:onClose() 
        return 
    end

    FriendData:onDeleteElement(FriendData:onGetFriendBattleMessage(self.type), self.data, 4, self.type)
    self:onRefreshRedDot()

    local str1 = self.type == ReconnectionType.FriendInviteReconnect and ' <color=#A99DE9>'..UIText('ui_tips_00001')..'</color>' or ' <color=#A99DE9>'..UIText('ui_tips_00002')..'</color>'
    local str2 = self.type == ReconnectionType.FriendInviteReconnect and UIText('ui_tips_00002') or UIText('ui_tips_00003')
    self.ui.BattleDescentTxtEx.text = self.data.name..str1
    self.ui.PlayerNameTxtEx.text = self.data.name
    self.ui.PlayContentTxtEx.text = str2

    self.ui.NoPayGo:SetActive(self.type == ReconnectionType.FriendInviteReconnect)
    self.ui.PayedGo:SetActive(self.type == ReconnectionType.FriendAgreeReconnect)
end


function PlayerInviteBattleView:onRefreshRedDot()
    local count = #FriendData:onGetFriendBattleMessage(self.type)
    local str = count > 99 and '99+' or count
    self.ui.RedDotCountTxtEx.text = str
    self.ui.RedDotGo:SetActive(count >= 1)
end



--[[
    @desc: 同意按钮
    time:2022-05-12 14:33:56
]]
function PlayerInviteBattleView:onClick_BtnPlay()
    --if ItemData:onCheckItemIsEnough(ItemType.POWER, 1) then
        --邀请方/被邀请方进战斗
        MatchManager.isStart = true
        MatchProtoRequest:onRequest(true, BattleType.FRIEND, {})
    --end
end



--[[
    @desc: 拒绝按钮
    time:2022-05-12 14:34:04
]]
function PlayerInviteBattleView:onClick_BtnClose()
    --被邀请方拒绝邀请
    if self.type == ReconnectionType.FriendInviteReconnect then
        ReconnectionData:onHandlerEventMessage()
        NetWork:onRequest(UIProtoType.DisposeFriendInvite.protoID, {objId = self.data.uid, type = 0})

    --邀请方拒绝进入战斗
    elseif self.type == ReconnectionType.FriendAgreeReconnect then    
        NetWork:onRequest(UIProtoType.FriendFightEnd.protoID, {
            starNum = 0,
            isWin = 0,
            roomId = self.data.roomId
        }, function(status)
            if status then
                ReconnectionData:onHandlerEventMessage()
            end
        end)
    end

    self:onRefreshData()
end

return PlayerInviteBattleView