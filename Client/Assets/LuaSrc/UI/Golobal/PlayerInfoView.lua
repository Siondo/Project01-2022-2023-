--[[
Description: 玩家详情界面
Author: xinZhao
Date: 2022-05-06 11:33:37
LastEditTime: 2022-04-06 11:33:37
--]]

PlayerInfoView = class('PlayerInfoView', UIBase)

local dataName = {
    'starsNum',
    'level',
    'cupNum',
    'starRank',
    'cupRank',
    'winRate'
}

function PlayerInfoView:OnLoad()
    self.AttributeItems = {}
    for i = 1, 6 do
        self.AttributeItems[#self.AttributeItems + 1] = {
            dataName = dataName[i],
            Name = self.ui.ContentRectTf:Find('AttributeItem'..i..'/Name'):GetComponent(typeof(TextEx)),
            Count = self.ui.ContentRectTf:Find('AttributeItem'..i..'/CountBoard/Count'):GetComponent(typeof(TextEx)),
        }
    end
end

function PlayerInfoView:OnShow(data, serverData)
    self.bindData = data
    self.serverData = serverData
    PlayerGrid:onInit(self.ui.PlayerGridGo):onRefresh(data.head)

    self.ui.NameTxtTxtEx.text = self.bindData.name
    self.ui.PlayerUIDTxtEx.text = 'ID: <color=#A99DE9>'..self.bindData.uid..'</color>'
    for i = 1, #self.AttributeItems do
        local dataName = self.AttributeItems[i].dataName
        local count = serverData[dataName]
        if dataName == 'starRank' or dataName == 'cupRank' then
            if count == -1 or count == 0 or count > 200 then
                count = '200+'
            end
        elseif dataName == 'winRate' then
            local rate = count * 100
            count = rate..'%'
        end
        self.AttributeItems[i].Count.text = count
    end
end

function PlayerInfoView:onClick_BtnDelete()
    NetWork:onRequest(UIProtoType.DeleteFriend.protoID, {friendId = self.bindData.uid}, function(status, tbData)
        if status then
            FriendData:onDeleteElement(FriendData:onGetFriendList(), self.bindData, 3)
            UIEvent:OnEvent(UIEvent.FriendListView.RefreshFriendList)
            UITool:onShowTips(UIText('ui_tip_00021'))
            self:onClose()
        end
    end)
end


function PlayerInfoView:onClick_BtnClose()
    self:onClose()
end

return PlayerInfoView