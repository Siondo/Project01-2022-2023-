--[[
Description: 赛季结算界面
Author: xinZhao
Date: 2022-05-24 15:44:05
LastEditTime: 2022-05-24 15:44:05
]]

SeaonEndView = class('SeaonEndView', UIBase)

function SeaonEndView:OnLoad()
    self.itemList = {}
    for i = 1, 3 do
        self.itemList[i] = {
            root = self.ui.SlotContentRectTf:Find('Slot'..i).gameObject,
            icon = self.ui.SlotContentRectTf:Find('Slot'..i..'/Icon'):GetComponent(typeof(ImageEx)),
            count = self.ui.SlotContentRectTf:Find('Slot'..i..'/Count'):GetComponent(typeof(TextEx)),
        }
    end 
end


function SeaonEndView:OnShow(data)
    self.rewards = data.items or {}
    local str = data.rankType == 1 and UIText('ui_starreward_00001') or UIText('ui_trophyreward_00001')
    self.ui.TitleTxtEx.text = str

    self.partIndex = 1
    self.ui.Part1Go:SetActive(true)
    self.ui.Part2Go:SetActive(false)
end


function SeaonEndView:onClick_BtnMask()
    if self.partIndex == 1 then
        self.partIndex = 2
        self.ui.Part1Go:SetActive(false)
        self.ui.Part2Go:SetActive(true)
        self:onRefresh()
    else
        self:onClose()
        ReconnectionData:onHandlerEventMessage()
    end
end


function SeaonEndView:onRefresh()
    local rewards = self.rewards
    if rewards and #rewards > 0 then
        for i = 1, #self.itemList do
            self.itemList[i].root:SetActive(#rewards >= i)
            if #rewards >= i then
                local itemId = rewards[i].itemId or rewards[i].id
                local itemConfig = ItemData:onGetItemConfig(itemId)
                UITool:SetSprte(self.itemList[i].icon, 'Common/icon/'..itemConfig.IconFile, true)
                self.itemList[i].count.text = rewards[i].num
                ItemData:onAddItem(itemId, rewards[i].num)
            end
        end
    end
end


return SeaonEndView