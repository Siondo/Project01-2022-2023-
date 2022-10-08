--[[
Description: 沙盒支付界面
Author: xinZhao
Date: 2022-04-29 10:58:23
LastEditTime: 2022-04-29 10:58:34
--]]


SandBoxPayView = class('SandBoxPayView', UIBase)


function SandBoxPayView:OnLoad()
    self.itemGrids = {}
    for i = 1, 8 do
        self.itemGrids[i] = {
            root = self.ui.ContentRectTf:Find('Items/ItemGrid'..i),
            itemIcon = self.ui.ContentRectTf:Find('Items/ItemGrid'..i..'/Slot/Icon'):GetComponent(typeof(ImageEx)),
            itemCount = self.ui.ContentRectTf:Find('Items/ItemGrid'..i..'/Slot/Count'):GetComponent(typeof(TextEx)),
        }
    end
end


function SandBoxPayView:OnShow(data)
    local cloneData = clone(data)
    local mergeList = cloneData.DisplayContent
    for i = 1, #cloneData.Content do
        mergeList[#mergeList + 1] = cloneData.Content[i]
    end

    for i = 1, #self.itemGrids do
        self.itemGrids[i].root.gameObject:SetActive(#mergeList >= i)
        if #mergeList >= i then
            local config = ItemData:onGetItemConfig(mergeList[i][1])
            local txtDisplay = config.ItemID == ItemType.INFINITEPOWER and UITool:timeString(mergeList[i][2], 2) or UITool:tradeConvert(mergeList[i][2])
            UITool:SetSprte(self.itemGrids[i].itemIcon, 'Common/Icon/'..config.IconFile, true)
            self.itemGrids[i].itemCount.text = txtDisplay
        end
    end

    self.ui.ItemDescribeTxtEx.text = '商品名称\n己所不欲, 勿施于人\n天下没有白吃的午餐'
    self.ui.CastCountTxtEx.text = '免费享用'

    self.data = data
end


function SandBoxPayView:onClick_BtnBuy()
    IAPManager:onPay(self.data.StoreID, function(table)
        --刷新含有倒计时的商品
        if self.data.Type == StoreType.Shop_Slot_1 then
            self.data.endTime = -2
            if self.data.refresh then
                self.data.refresh()
            end
        end

        self:onClose()
    end)
end


return SandBoxPayView
