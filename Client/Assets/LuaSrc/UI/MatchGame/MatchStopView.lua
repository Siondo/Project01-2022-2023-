--[[
Description: 三消暂停界面
Author: xinZhao
Date: 2022-04-21 16:55:04
LastEditTime: 2022-04-21 16:55:04
--]]

MatchStopView = class('MatchStopView', UIBase)

function MatchStopView:OnShow()
    self.isEnter = nil
end


function MatchStopView:onClick_BtnHome()
    self.isEnter = false
    self:onClose()
end


function MatchStopView:onClick_BtnReStart()
    self.isEnter = true
    NetWork:onRequest(UIProtoType.PVPReCoverFight.protoID, nil, function(reslut, data)
        if reslut then
            MatchProtoRequest.BattleType = BattleType.PVP
            MatchManager:onStartMatchGame(data.endTime, data.useItems, data.chapterId)
            ReconnectionData:onHandlerEventMessage()
        end
    end)
end


function MatchStopView:OnHide()
    if not self.isEnter then
        local parmas = {
            starNum = 0,
            isWin = 0,
        }
    
        NetWork:onRequest(UIProtoType.PVPFightEnd.protoID, parmas, function(reslut, data)
            if reslut then
                MatchData:onSetPVPRoom(nil)
                ReconnectionData:onHandlerEventMessage()
                self:onClose()
            end
        end)
    end
end

return MatchStopView
