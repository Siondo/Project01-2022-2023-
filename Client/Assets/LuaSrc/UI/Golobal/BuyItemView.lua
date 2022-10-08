--[[
Description: 全局购买弹窗(指定个数)
Author: xinZhao
Date: 2022-04-28 14:56:54
LastEditTime: 2022-04-28 14:56:54
--]]

BuyItemView = class('BuyItemView', UIBase)

function BuyItemView:OnShow(config, refreshCallBack)
    self.refreshCallBack = refreshCallBack


    self.ui.ItemNameTxtEx.text = UIText(config.Name)
    self.ui.ItemDescribeTxtEx.text = UIText(config.ItemDescription)

    local num = config.ItemID == ItemType.POWER and (5 - ItemData:onGetItemCount(ItemType.POWER)) or config.ItemNumber
    self.ui.ItemBuyCountTxtEx.text = 'x'..num
    self.ui.CastCountTxtEx.text = config.ItemPrice
    UITool:SetSprte(self.ui.ItemIconImgEx, 'Common/Icon/'..config.IconFile, true)

    self.buyItem = {
        itemId = config.ItemID,
        buyCount = 1,
    }
end


function BuyItemView:onClick_BtnBuy()
    local config = ItemData:onGetItemConfig(self.buyItem.itemId)
    if ItemData:onCheckItemIsEnough(ItemType.COIN, config.ItemPrice) then
        NetWork:onRequest(UIProtoType.BuyItems.protoID, {itemData = {self.buyItem}}, function(result, tbData)
            if result then
                ItemData:onAddItem(config.ItemID, config.ItemNumber)
                UITool:onShowTips(UIText('ui_tip_00020'))
                self:onClose()
                if self.refreshCallBack then
                    self.refreshCallBack()
                end
            end
        end)
    end
end


function BuyItemView:onClick_BtnQuit()
    self:onClose()
end

return BuyItemView