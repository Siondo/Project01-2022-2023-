--[[
    Description: PVP匹配界面
    Author: xinZhao
    time:2022-05-07 14:57:53
]]

PVPMatchView = class('PVPMatchView', UIBase)

local WAIT_TIME = 5

function PVPMatchView:OnLoad()
    WAIT_TIME = Config.tbCommon['StartTime'].value

    self.playerGridCpn = bind(self.ui.PlayerGridGo, PlayerGrid)
    self.otherPlayerGridCpn = {}
    for i = 1, 4 do
        local playerGrid = self.ui.ContentRectTf:Find('PlayerGrid'..i)
        self.otherPlayerGridCpn[i] = bind(playerGrid, PlayerGrid)
    end

    self:AddEvent(UIEvent.PVPMatchView.OnAddPVPPlayer, function()
        self:onRefresh()
    end)

    self.pvpTimer = Timer.Create(function(loop)
        self.matchTime = self.matchTime + 1

        if self.matchTime <= WAIT_TIME then
            self.ui.TimerTxtEx.text = UIText('pvp_tips_00002', self.randomTimeCount, self.matchTime)

            if self.matchTime == WAIT_TIME then
                self.pvpTimer:Stop()
                self:onEnterPVP()
            end
        end
    end, 1, 999)
end


function PVPMatchView:OnShow(itemIds)
    self.itemIds = itemIds
    self:onRefresh()
end


function PVPMatchView:onRefresh()
    local otherPlayerData = MatchData:onGetPvPMatchPlayer()
    for i = 1, #self.otherPlayerGridCpn do
        self.otherPlayerGridCpn[i]:onSetAdd(#otherPlayerData <= i)
        if #otherPlayerData >= i then
            self.otherPlayerGridCpn[i]:onRefresh(otherPlayerData[i].head)
        end
    end

    local totallyPlayerCount = (#otherPlayerData + 1)
    self.ui.PlayerCountTxtEx.text = totallyPlayerCount..'/5 '..UIText('pvp_tips_00001')

    self.matchTime = 0
    self.randomTimeCount = math.random(10, 30)
    self.pvpTimer:Start()
    self.ui.TimerTxtEx.text = UIText('pvp_tips_00002', self.randomTimeCount, self.matchTime)
end


function PVPMatchView:onEnterPVP()
    local parmas = {
        useItem = self.itemIds
    }
    NetWork:onRequest(UIProtoType.PVPFightStart.protoID, parmas, function(reslut, data)
        if reslut then
            MatchManager:onStartMatchGame(data.endTime, data.useItems, data.chapterId)
        end
    end)
end


function PVPMatchView:onClick_BtnQuit()
    NetWork:onRequest(UIProtoType.PVPMatchQuit.protoID, nil, function(status)
        if status then
            self.pvpTimer:Stop()
            MatchData:onRelease()
            LuaHelper.ClearUI(false)
            LuaHelper.ShowUI(UI.MainView, MAINTAB_INDEX.MAIN)
        end
    end)
end

return PVPMatchView