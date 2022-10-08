--[[
Description: 好友数据类
Author: xinZhao
Date: 2022-05-05 14:24:10
LastEditTime: 2022-04-22 14:24:11
--]]

FriendData = class('FriendData')


function FriendData:onPullAllFrendData()
    self.friendList = {}
    self.cliamList = {}
    self.applyList = {}
    NetWork:onRequest(UIProtoType.GetFriendAllList.protoID, nil, function(status, tbData)
        if status then
            if tbData.friends.data then
                self:onSetAllFriendData('friend', tbData.friends.data)
            end
            if tbData.getStrengts.data then
                self:onSetAllFriendData('friendCliam', tbData.getStrengts.data)
            end
            if tbData.applys.data then
                self:onSetAllFriendData('friendApply', tbData.applys.data)
            end
        end
    end)
end


function FriendData:onSetAllFriendData(type, data)
    if type == 'friend' then
        --拉取好友列表
        for i = 1, #data do
            self:onAddFriendList(data[i])
        end
    elseif type == 'friendCliam' then
        --拉取体力赠送列表
        for i = 1, #data do
            self:onAddCliamList(data[i])
        end
    elseif type == 'friendApply' then
        --拉取申请列表
        for i = 1, #data do
            self:onAddApplyList(data[i])
        end
    end
end


--[[
    @desc: 查询表内是否包含同一种元素
    time:2022-05-06 10:14:18
    --@table: 元表
	--@data: 数据
]]
function FriendData:onCheckListExistElement(table, data)
    for i = 1, #table do
        local uid = table[i].info and table[i].info.uid or table[i].uid
        if uid == data.uid then
            return table[i].info
        end
    end

    return false
end


--[[
    @desc: 像好友列表插入一个元素
    time:2022-05-06 10:07:55
]]
function FriendData:onAddFriendList(data)
    if self:onCheckListExistElement(self.friendList, data) then
        return
    end

    self.friendList[#self.friendList + 1] = {
        info = data,
        root = 'FriendRoot',
        iconName = {
            'friend_life1',
            'friend_fight'
        }
    }
end


function FriendData:onGetFriendList()
    return self.friendList
end


--[[
    @desc: 像体力赠送列表插入一个元素
    time:2022-05-06 14:25:41
]]
function FriendData:onAddCliamList(data)
    if self:onCheckListExistElement(self.cliamList, data) then
        return
    end

    self.cliamList[#self.cliamList + 1] = {
        info = data,
        root = 'PowerRoot',
    }
end


function FriendData:onGetCliamList()
    return self.cliamList
end


--[[
    @desc: 向申请列表插入一个元素
    time:2022-05-06 10:07:42
]]
function FriendData:onAddApplyList(data)
    if self:onCheckListExistElement(self.applyList, data) then
        return
    end

    self.applyList[#self.applyList + 1] = {
        info = data,
        root = 'FriendRoot',
        iconName = {
            'friend_yes',
            'friend_no'
        }
    }
end


function FriendData:onGetApplyList()
    return self.applyList
end


--[[
    @desc: 删除表中的某个元素 并返回删除的元素
    time:2022-05-06 10:35:47
    --@table: 元表
	--@data: 数据
]]
function FriendData:onDeleteElement(table, data, type, otherType)
    local list = {}
    local deleteElement, isFind = nil, false
    for i = 1, #table do
        local uid = table[i].info and table[i].info.uid or table[i].uid
        if type == 4 then
            if uid ~= data.uid or isFind then
                list[#list + 1] = table[i]
            else
                isFind = true
            end
        else
            if uid ~= data.uid then
                list[#list + 1] = table[i]
            else
                deleteElement = table[i].info
            end
        end
    end

    if type == 1 then
        self.applyList = list
    elseif type == 2 then
        self.cliamList = list
    elseif type == 3 then
        self.friendList = list
    elseif type == 4 then
        if otherType == ReconnectionType.FriendInviteReconnect then
            self.inviteBattleList = list
        elseif otherType == ReconnectionType.FriendAgreeReconnect then
            self.agreeBattleList = list
        end
    end
    return deleteElement
end


--[[
    @desc: 赠送体力
    time:2022-05-06 14:39:02
]]
function FriendData:onSendPower(data)
    for i = 1, #self.friendList do
        if self.friendList[i].info.uid == data.uid then
            self.friendList[i].info.giveStrength = 1
            return
        end
    end
end


--[[
    @desc: 好友列表排序
    time:2022-05-06 16:09:44
]]
function FriendData:onSortFriendList(type)
    table.sort(self.friendList, function(a, b)
        if type == 1 then
            return a.info.starsNum > b.info.starsNum
        elseif type == 2 then
            return a.info.cupNum > b.info.cupNum
        elseif type == 3 then
            return a.info.level > b.info.level
        end
    end)
end


--------------------------------------------------
--------------------------------------------------
--------------------------以下处理好友PK数据逻辑----

--[[
    @desc: 添加邀请战斗数据到列表
    time:2022-05-12 10:05:32
]]
function FriendData:onAddFriendInviteMessage(data)
    self.inviteBattleList = data
end


--[[
    @desc: 添加同意战斗数据到列表
    time:2022-05-12 11:59:04
]]
function FriendData:onAddFriendAgreeMessage(data)
    self.agreeBattleList = data
end


--[[
    @desc: 获取邀请/同意数据列表
    time:2022-05-12 10:05:59
]]
function FriendData:onGetFriendBattleMessage(type)
    if type == ReconnectionType.FriendInviteReconnect then
        return self.inviteBattleList or {}
    elseif type == ReconnectionType.FriendAgreeReconnect then
        return self.agreeBattleList or {}
    end
end


--[[
    @desc: 如果有邀请记录进入邀请战斗界面
    time:2022-05-11 18:30:03
]]
function FriendData:onOpenFriendInviteView(type)
    LuaHelper.ShowUI(UI.PlayerInviteBattleView, type)
    UIEvent:OnEvent(UIEvent.PlayerInviteBattleView.OnAddMessage)
end

return FriendData