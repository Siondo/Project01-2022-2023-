--[[
Description: PVE/PVP开始界面
Author: xinZhao
Date: 2022-04-26 14:04:40
LastEditTime: 2022-04-26 14:04:40
--]]

MatchStartView = class('MatchStartView', UIBase)

function MatchStartView:OnLoad()
    self.itemGrids = {}
    for i = 1, 4 do
        local root = self.ui.ItemRootRectTf:Find('Item'..i)
        self.itemGrids[i] = {
            root = root,
            btnRoot = root:GetComponent(typeof(ButtonEx)),
            Icon = root:Find('Icon'):GetComponent(typeof(ImageEx)),
            Check = root:Find('Check'),
            Belong = root:Find('Belong'),
            BelongCount = root:Find('Belong/Text'):GetComponent(typeof(TextEx)),
        }
    end
end


function MatchStartView:OnShow(type)
    local title, config = '', {}
    self.type = type
    self.chooseItem = {}
    if self.type == BattleType.PVE then
        config = Config.tbItemUnlockPVE
        title =  UIText('ui_game_00001', (PlayerData:getLevelId()-LEVEL_ID_CONST))
    elseif self.type == BattleType.PVP or self.type == BattleType.FRIEND then
        config = Config.tbItemUnlockPVP
        title = UIText('ui_game_00004')
    end

    self.ui.TitleTxtEx.text = title
    self:onRefreshItem(config)
end


--[[
    @desc: 刷新Item
    time:2022-04-26 14:28:17
    --@config: 配置表
]]
function MatchStartView:onRefreshItem(config)
    config = UITool:onTableConver(config)
    for i = 1, #self.itemGrids do
        self.itemGrids[i].root.gameObject:SetActive(#config >= i)
        if #config >= i then
            UITool:SetSprte(self.itemGrids[i].Icon, 'Common/icon/'..config[i].Icon, true)
            local count = ItemData:onGetItemCount(config[i].ItemID)
            self.itemGrids[i].BelongCount.text = count
            self.itemGrids[i].Belong.gameObject:SetActive(true)
            self.itemGrids[i].Check.gameObject:SetActive(false)

            UITool:onAddClickAndClear(self.itemGrids[i].btnRoot, function()
                if count <= 0 then
                    local itemConfig = ItemData:onGetItemConfig(config[i].ItemID)
                    LuaHelper.ShowUI(UI.BuyItemView, itemConfig, function()
                        local count = ItemData:onGetItemCount(config[i].ItemID)
                        self.itemGrids[i].BelongCount.text = count
                    end)
                    return
                end

                local status = self.chooseItem[config[i].ItemID] and true or false
                if self.chooseItem[config[i].ItemID] then
                    self.chooseItem[config[i].ItemID] = nil
                else
                    self.chooseItem[config[i].ItemID] = config[i]
                end

                self.itemGrids[i].Belong.gameObject:SetActive(status)
                self.itemGrids[i].Check.gameObject:SetActive(not status)
            end)
        end
    end
end


--[[
    @desc: 开始战斗
    time:2022-04-26 14:14:19
]]
function MatchStartView:onClick_BtnPlay()
    local itemIDs = {}
    for idstr, data in pairs(self.chooseItem) do
        itemIDs[#itemIDs + 1] = {
            id = data.ItemID,
            deductNum = data.DeductNum or 1
        }
    end
    MatchProtoRequest:onRequest(false, self.type, itemIDs)
end


--[[
    @desc: 退出界面
    time:2022-04-26 14:31:33
]]
function MatchStartView:onClick_BtnClose()
    self:onClose()
end

return MatchStartView