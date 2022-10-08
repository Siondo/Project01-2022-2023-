--[[
Description: 通用奖励界面
Author: xinZhao
Date: 2022-05-25 16:06:28
LastEditTime: 2022-05-25 16:06:28
]]

CommonRewardView = class('CommonRewardView', UIBase)

function CommonRewardView:OnLoad()
    self.itemList = {}
    for i = 1, 9 do
        self.itemList[i] = {
            root = self.ui.SlotContentRectTf:Find('Slot'..i).gameObject,
            icon = self.ui.SlotContentRectTf:Find('Slot'..i..'/Icon'):GetComponent(typeof(ImageEx)),
            count = self.ui.SlotContentRectTf:Find('Slot'..i..'/Count'):GetComponent(typeof(TextEx)),
        }
    end 
end


function CommonRewardView:OnShow(rewards, callBack)
    self.callBack = callBack
    if rewards and #rewards > 0 then
        for i = 1, #self.itemList do
            self.itemList[i].root:SetActive(#rewards >= i)
            if #rewards >= i then
                local itemId = rewards[i].itemId or rewards[i].id
                local itemConfig = ItemData:onGetItemConfig(itemId)
                UITool:SetSprte(self.itemList[i].icon, 'Common/icon/'..itemConfig.IconFile, true)

                if itemId == ItemType.INFINITEPOWER then
                    self.itemList[i].count.text = UITool:timeString(rewards[i].num, 2)
                else
                    self.itemList[i].count.text = rewards[i].num
                end
            end
        end
    end
end


function CommonRewardView:OnHide()
    if self.callBack then
        self.callBack()
    end
end

return CommonRewardView