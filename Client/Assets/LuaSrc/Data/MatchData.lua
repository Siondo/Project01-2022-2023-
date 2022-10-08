--[[
    Description: 三消战斗数据类
    Author: xinZhao
    time:2022-05-07 15:27:47
]]

MatchData = class('MatchData')


function MatchData:onInit(roomId)
    self.belongPVPRoom = roomId
    self.pvpMatchPlayers = {}
end


--[[
    @desc: 向PVP匹配列表中插入一个玩家
    time:2022-05-07 15:29:51
    --@data: 玩家数据(uid, headurl, name)
]]
function MatchData:onAddPlayerToPVPMatch(data)
    if FriendData:onCheckListExistElement(self.pvpMatchPlayers, data) then
        return
    end

    self.pvpMatchPlayers[#self.pvpMatchPlayers + 1] = data
end


--[[
    @desc: 向PVP匹配列表中移除一个玩家
    time:2022-05-07 16:18:30
    --@data: 玩家数据(uid, headurl, name)
]]
function MatchData:onRemovePlayerToPVPMatch(data)
    local list = {}
    for i = 1, #self.pvpMatchPlayers do
        if self.pvpMatchPlayers[i].uid ~= data.uid then
            list[#list + 1] = self.pvpMatchPlayers[i]
        end
    end

    self.pvpMatchPlayers = list
end


--[[
    @desc: 获取PVP当前匹配的玩家列表
    time:2022-05-07 15:29:33
]]
function MatchData:onGetPvPMatchPlayer()
    return self.pvpMatchPlayers
end


--[[
    @desc: 设置PVP房间ID
    time:2022-05-07 16:01:07
]]
function MatchData:onSetPVPRoom(roomId)
    self.belongPVPRoom = roomId
end


--[[
    @desc: 获取PVP房间ID
    time:2022-05-07 16:01:16
]]
function MatchData:onGetPVPRoom()
    return self.belongPVPRoom
end


--[[
    @desc: 设置PVP战局玩家数据
    time:2022-05-09 14:28:31
]]
function MatchData:onSetPVPEndPlayerInfos(info, type)
    self.maxUserInfos = type == BattleType.PVP and 5 or 2
    self.playerInfos = {}
    self.playerInfos.completeCount = 0
    for i = 1, self.maxUserInfos do
        local index = #self.playerInfos + 1
        self.playerInfos[index] = {
            state = -1 --1:已经完成  0:正在进行 -1:等待加入
        }

        if #info >= i then
            self.playerInfos[index] = info[i]
            if info[i].state == 1 then
                self.playerInfos.completeCount = self.playerInfos.completeCount + 1
            end
        end
    end
end


--[[
    @desc: 获取PVP战局玩家数据
    time:2022-05-09 14:28:15
]]
function MatchData:onGetPVPEndPlayerInfos()
    return self.playerInfos
end


--[[
    @desc: 清理数据
    time:2022-05-07 16:01:29
]]
function MatchData:onRelease()
    self.belongPVPRoom = nil
    self.pvpMatchPlayers = {}
end

return MatchData