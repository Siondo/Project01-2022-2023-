--[[
Description: 主玩法战斗请求共用类
Author: xinZhao
Date: 2022-04-28 10:49:36
LastEditTime: 2022-04-28 10:49:36
--]]

MatchProtoRequest = class('MatchProtoRequest')


--[[
    @desc: PVE/PVP战斗开始请求
    time:2022-05-12 14:23:58
]]
function MatchProtoRequest:onRequest(isOpenStartView, type, chooseItems)
    --构造选择的道具
    local itemIds = {}
    local currentStage = (PlayerData:getLevelId() - LEVEL_ID_CONST)
    for i = 1, #chooseItems do
        itemIds[#itemIds + 1] = chooseItems[i].id
    end

    --PVE
    if type == BattleType.PVE then
        self.BattleType = BattleType.PVE
        if isOpenStartView and currentStage > 15 then
            LuaHelper.ShowUI(UI.MatchStartView, type)
            return
        else
            local parmas = {
                chapterId = PlayerData:getLevelId(),
                useItem = itemIds
            }
            NetWork:onRequest(UIProtoType.Fight.protoID, parmas, function(reslut, data)
                if reslut then
                    MatchManager:onStartMatchGame(data.endTime, data.useItems, PlayerData:getLevelId())
                else
                    if data == 6 then
                        UITool:onShowTips(UIText('ui_tip_00030'))
                    end
                end
            end)
        end

    --PVP/好友PK
    elseif type == BattleType.PVP or type == BattleType.FRIEND then
        self.BattleType = type
        if isOpenStartView and currentStage > 3 then
            LuaHelper.ShowUI(UI.MatchStartView, type)
            return
        else
            if type == BattleType.PVP then
                NetWork:onRequest(UIProtoType.PVPMatchStart.protoID, nil, function(status, tbData)
                    if status then
                        MatchData:onSetPVPRoom(tbData.roomId)
                        for i = 1, #tbData.users do
                            MatchData:onAddPlayerToPVPMatch(tbData.users[i])
                        end
    
                        LuaHelper.ClearUI(false)
                        LuaHelper.ShowUI(UI.PVPMatchView, itemIds) --进入PVP
                    else
                        if tbData == 6 then
                            UITool:onShowTips(UIText('ui_tip_00030'))
                        end
                    end
                end)

            elseif type == BattleType.FRIEND then
                --获取数据
                UIEvent:OnEvent(UIEvent.PlayerInviteBattleView.OnGetCurrentDataInfo, function(viewData)
                    local doSetup = function(roomId)
                        local parmas = {
                            roomId = roomId,
                            useItem = itemIds
                        }

                        --进入战斗
                        NetWork:onRequest(UIProtoType.FriendFightStart.protoID, parmas, function(status, tbData)
                            if status then
                                ReconnectionData:onHandlerEventMessage()
                                MatchManager:onStartMatchGame(tbData.endTime, tbData.useItems, tbData.chapterId, parmas.roomId)
                            end
                        end)
                    end

                    if viewData.roomId then
                        --邀请方直接战斗
                        doSetup(viewData.roomId)
                    else
                        --被邀请方通知服务器请求同意
                        NetWork:onRequest(UIProtoType.DisposeFriendInvite.protoID, {objId = viewData.uid, type = 1}, function(status, tbData)
                            if status then
                                --被邀请方进入战斗
                                doSetup(tbData.roomId)
                            end
                        end)
                    end
                end)
            end

        end
    end
end

return MatchProtoRequest