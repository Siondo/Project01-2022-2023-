--[[
Description: 匹配道具自定义组件
Author: xinZhao
Date: 2022-04-22 17:11:24
LastEditTime: 2022-04-22 17:11:25
--]]

MatchItemBar = class('MatchItemBar')

function MatchItemBar:onGetComponent()
    self.iconImg = self.transform:Find('Icon'):GetComponent(typeof(ImageEx))
    self.countRoot = self.transform:Find('CountRoot')
    self.countTxt = self.countRoot:Find('Count'):GetComponent(typeof(TextEx))
    self.unLockTxt = self.transform:Find('UnLockLv'):GetComponent(typeof(TextEx))
    self.maskRoot = self.transform:Find('Mask')

    local btn = self.transform:GetComponent(typeof(ButtonEx))
    UITool:onAddClickAndClear(btn, function()
        local doUsetItem = function()
            self:onRefreshItemBar()

            --重置-打乱所有元素位置
            if self.config.bindData.ItemID == ItemType.Reset then
                AudioManager.PlaySound(AudioManager.Audio.Refresh_Use)
                MatchManager:onResetAllElement()

            --灯泡-显示1个任务元素位置
            elseif self.config.bindData.ItemID == ItemType.Light then
                MatchManager:onShowTaskElement(1)

            --放大镜-显示3个任务元素位置
            elseif self.config.bindData.ItemID == ItemType.Magnifier then
                AudioManager.PlaySound(AudioManager.Audio.Magnifier_Use)
                MatchManager:onShowTaskElement(3)

            --雪花-关卡倒计时停止十秒
            elseif self.config.bindData.ItemID == ItemType.Freezing then
                MatchManager:onStopCountdownByTime(10)
            end 
        end

        --PVE使用道具
        if MatchProtoRequest.BattleType == BattleType.PVE and ItemData:onCheckItemIsEnough(self.config.bindData.ItemID, 1) then
            if PlayerData:getLevelId() < self.config.bindData.LevelID then
                UITool:onShowTips(UIText('ui_tip_00024'))
                return
            end
            
            local parmas = {
                itemData = {
                    {itemId = self.config.bindData.ItemID, count = 1},
                }
            }    
            
            NetWork:onRequest(UIProtoType.UseItems.protoID, parmas, function(status, tbData)
                if status then
                    --道具减少
                    ItemData:onSubItem(self.config.bindData.ItemID, 1)
                    doUsetItem()
                end
            end)

        --PVP使用道具
        elseif MatchProtoRequest.BattleType == BattleType.PVP or MatchProtoRequest.BattleType == BattleType.FRIEND then
            local existCount = MatchManager.usePVPItem[self.config.bindData.ItemID] and MatchManager.usePVPItem[self.config.bindData.ItemID].existCount or 0
            if existCount > 0 then
                MatchManager.usePVPItem[self.config.bindData.ItemID].existCount = MatchManager.usePVPItem[self.config.bindData.ItemID].existCount - 1
                self:onRefreshItemBar()
                doUsetItem()
            else
                local config = ItemData:onGetItemConfig(self.config.bindData.ItemID)
                UITool:onShowTips(UIText('ui_tip_00008', UIText(config.Name)))
            end
        end
    end)
end


function MatchItemBar:onInit(gameObject, data)
    self.config = ItemData:onGetItemConfig(data.ItemID)
    self.config.bindData = data
    self.gameObject = gameObject
    self.transform = gameObject.transform

    self:onGetComponent()
end


function MatchItemBar:onRefreshItemBar()
    UITool:SetSprte(self.iconImg, 'MatchMainView/'..self.config.IconFile)

    local itemCount = 0

    if MatchProtoRequest.BattleType == BattleType.PVE then
        local levelCount = PlayerData:getLevelId()
        itemCount = ItemData:onGetItemCount(self.config.bindData.ItemID)

        self.countRoot.gameObject:SetActive(levelCount >= self.config.bindData.LevelID)
        self.maskRoot.gameObject:SetActive(levelCount < self.config.bindData.LevelID)
        self.unLockTxt.text = 'Lvl.'..(self.config.bindData.LevelID - LEVEL_ID_CONST)
        self.unLockTxt.gameObject:SetActive(levelCount < self.config.bindData.LevelID)

    elseif MatchProtoRequest.BattleType == BattleType.PVP or MatchProtoRequest.BattleType == BattleType.FRIEND then
        itemCount = MatchManager.usePVPItem[self.config.bindData.ItemID] and MatchManager.usePVPItem[self.config.bindData.ItemID].existCount or 0
    
        self.countRoot.gameObject:SetActive(itemCount >= 1)
        self.maskRoot.gameObject:SetActive(itemCount < 1)
        self.unLockTxt.text = ''
        self.unLockTxt.gameObject:SetActive(itemCount < 1)
    end

    self.countTxt.text = itemCount
end

return MatchItemBar
