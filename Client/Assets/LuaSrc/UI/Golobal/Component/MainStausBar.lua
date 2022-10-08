--[[
Description: 主界面状态栏自定义组件
Author: xinZhao
Date: 2022-04-22 15:58:40
LastEditTime: 2022-04-22 15:58:40
--]]


MainStausBar = class('MainStausBar')

function MainStausBar:onGetComponent()
    self.iconImg = self.transform:Find('Icon'):GetComponent(typeof(ImageEx))
    self.countTxt = self.transform:Find('DeepBoard/Count'):GetComponent(typeof(TextEx))

    self.btnAdd = self.transform:Find('BtnAdd'):GetComponent(typeof(ButtonEx))
    UITool:onAddClickAndClear(self.btnAdd, function()
        if self.config.ItemID == ItemType.POWER then 
            if (ItemData:onGetItemCount(ItemType.INFINITEPOWER) - os.time()) > 0 then
                UITool:onShowTips(UIText('ui_tip_00022'))
            else
                if ItemData:onGetItemCount(ItemType.POWER) >= 5 then
                    UITool:onShowTips(UIText('ui_tip_00023'))
                    return
                end

                LuaHelper.ShowUI(UI.BuyItemView, ItemData:onGetItemConfig(self.config.ItemID))
            end

        elseif self.config.ItemID == ItemType.COIN then
            UIEvent:OnEvent(UIEvent.MainView.JumpStore)
        end
    end)
end


function MainStausBar:onInit(gameObject, itemId)
    self.config = ItemData:onGetItemConfig(itemId)
    self.gameObject = gameObject
    self.transform = gameObject.transform

    self:onGetComponent()
    self:onRefreshStatusBar()
end


function MainStausBar:onRefreshStatusBar()
    local INFINITEPOWERTime = ItemData:onGetItemCount(ItemType.INFINITEPOWER) - os.time()
    if self.config.ItemID == ItemType.POWER and INFINITEPOWERTime > 0 then
        local config = ItemData:onGetItemConfig(ItemType.INFINITEPOWER)
        UITool:SetSprte(self.iconImg, 'Common/icon/'..config.IconFile)
        self.countTxt.text = UITool:timeString(INFINITEPOWERTime, 1)
    else
        local count = ItemData:onGetItemCount(self.config.ItemID)
        local converCount = UITool:tradeConvert(count)
        if self.config.ItemID == ItemType.POWER then
            converCount = count >= 5 and UIText('ui_home_00001') or count
        end

        UITool:SetSprte(self.iconImg, 'Common/icon/'..self.config.IconFile)
        self.countTxt.text = UITool:tradeConvert(converCount)
    end
end


function MainStausBar:onGetIconComponent()
    return self.iconImg.transform
end

return MainStausBar