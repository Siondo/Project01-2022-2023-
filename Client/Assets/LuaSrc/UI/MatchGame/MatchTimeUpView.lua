--[[
Description: 倒计时结束结算界面
Author: xinZhao
Date: 2022-04-21 20:37:38
LastEditTime: 2022-04-21 20:37:38
--]]

MatchTimeUpView = class('MatchTimeUpView', UIBase)

function MatchTimeUpView:onClick_BtnGiveUp()
    self.limitHide = true
    MatchManager:onOffMatchGame(true)
end

function MatchTimeUpView:onClick_BtnPlayOn()
    local count = ItemData:onGetItemCount(ItemType.COIN)
    if count >= 100 then
        local parmas = {
            itemData = {
                {itemId = ItemType.COIN, count = 100},
            }
        }    
        
        NetWork:onRequest(UIProtoType.UseItems.protoID, parmas, function(status, tbData)
            if status then
                --道具减少
                ItemData:onSubItem(self.config.bindData.ItemID, 1)
                self.limitHide = true
                MatchManager:onAddManagerTime(60)
                MatchManager:onStartManagerTimer()
                LuaHelper.HideUI(UI.MatchTimeUpView)
        
                UIEvent:OnEvent(UIEvent.MatchMainView.SetGameOverCountDown, UITool:timeString(60))
            end
        end)
    else
        UITool:onShowTips(UIText('ui_tip_00032'))
    end
end


function MatchTimeUpView:OnHide()
    if self.limitHide then self.limitHide = nil return end
    MatchManager:onOffMatchGame(true)
end

return MatchTimeUpView