--[[
Description: 战斗列表界面
Author: xinZhao
Date: 2022-04-06 11:05:04
LastEditTime: 2022-04-06 11:05:04
--]]

BattleListView = class("BattleListView", UIBase)


function BattleListView:OnLoad()
    ReconnectionData:onSetUpdateFunctionActive()
end


function BattleListView:OnShow()
    self:onRefreshChest()
    self.ui.StageLevelTxtEx.text = 'LEVEL-'..(PlayerData:getLevelId() - LEVEL_ID_CONST)
end


function BattleListView:onRefreshChest()
    local star = PlayerData:getStarCount()
    for _, config in UITool:PairsBykeys(Config.tbStarChest) do

        star = star > config.StarNumber and config.StarNumber or star
        self.ui.StarCountTxtEx.text = star..'/'..config.StarNumber
        self.ui.StarSliderSldEx.value = (star / config.StarNumber)
    end


    local maxLevel = PlayerData:getLevelCliam()
    local minLevel = maxLevel - 5
    local level = (PlayerData:getLevelId() - LEVEL_ID_CONST)
    local curLevel = level - minLevel
    self.ui.LevelCountTxtEx.text = curLevel..'/'..5
    self.ui.LevelSliderSldEx.value = (curLevel / 5)
end

--90 95 

--

function BattleListView:onClick_BtnStarChest()
    self:onRequetChestRewards(1)
end


function BattleListView:onClick_BtnLevelChest()
    self:onRequetChestRewards(2)
end


function BattleListView:onRequetChestRewards(type)
    NetWork:onRequest(UIProtoType.CliamChestRewards.protoID, {type = type}, function(status, tbData)
        if status then
            for i = 1, #tbData.items do
                local rewards = tbData.items[i]
                ItemData:onAddItem(rewards.itemId, rewards.num)
            end

            if type == 1 then
                PlayerData:setStarCount(0)
            else
                PlayerData:setLevelCliam(tbData.levelChestId)
            end

            UITool:onShowTips(UIText('ui_tip_00031'))
            self:onRefreshChest()
        else
            if tbData == 149 then
                UITool:onShowTips('领取宝箱条件不足')
            end
        end
    end)
end


--[[
    @desc: 检查玩家数据, 根据什么情况进入战斗
    time:2022-04-26 15:30:51
    --@type: 战斗类型
]]
function BattleListView:onCheck(type)
    MatchProtoRequest:onRequest(true, type, {})
end


function BattleListView:onClick_BtnStart()
    self:onCheck(BattleType.PVE)
end


function BattleListView:onClick_BtnBattle()
    self:onCheck(BattleType.PVP)
end


function BattleListView:onClick_BtnSign()
    LuaHelper.ShowUI(UI.SignView)
end


function BattleListView:onClick_BtnQuest()
    LuaHelper.ShowUI(UI.HelpView,  UIText('ui_rules_00001'), UIText('ui_rules_00002'))
end

return BattleListView