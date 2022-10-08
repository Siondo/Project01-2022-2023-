--[[
Description: 道具数据类
Author: xinZhao
Date: 2022-04-22 14:24:10
LastEditTime: 2022-04-22 14:24:11
--]]

ItemData = class('ItemData')

function ItemData:onInit(items)
    self.items = {}
    if items then
        for i = 1, #items do
            local itemId = items[i].itemId
            local count = items[i].itemNum
            self.items[itemId] = count
        end
    else
        logError('未收到道具数据, 联系服务器')
    end
end


--[[
    @desc: 获取某个道具配置表数据
    time:2022-04-22 15:19:54
    --@itemId: 道具Id
]]
function ItemData:onGetItemConfig(itemId)
    return Config.tbItems[tostring(itemId)] or {}
end


function ItemData:onAddItem(itemId, count)
    if itemId == ItemType.COIN or itemId == ItemType.POWER or itemId == ItemType.INFINITEPOWER then
        UITool:onGlobalFlyItem(itemId)

        if itemId == ItemType.COIN then
            AudioManager.PlaySound(AudioManager.Audio.Get_Stars)
        end
        return
    end

    if self.items[itemId] then
        self.items[itemId] = self.items[itemId] + count
    else
        self.items[itemId] = count
    end

    if itemId == ItemType.COIN or itemId == ItemType.POWER then
        UIEvent:OnEvent(UIEvent.MainView.RefreshStatusBar)
    end
end


function ItemData:onSubItem(itemId, count)
    if itemId == ItemType.COIN or itemId == ItemType.POWER then
        return
    end

    if self.items[itemId] then
        self.items[itemId] = self.items[itemId] - count

        if itemId == ItemType.COIN or itemId == ItemType.POWER then
            UIEvent:OnEvent(UIEvent.MainView.RefreshStatusBar)
        end
    end
end


--[[
    @desc: 添加/扣除无限体力
    time:2022-05-05 09:37:53
]]
function ItemData:onOperateInfinite(time)
    self.items[ItemType.INFINITEPOWER] = {}
    self.items[ItemType.INFINITEPOWER] = time --无限体力截止时间
    UIEvent:OnEvent(UIEvent.MainView.RefreshStatusBar)
end


--[[
    @desc: 获取某个道具数据
    time:2022-04-22 15:17:06
    --@itemId: 道具Id
]]
function ItemData:onGetItemCount(itemId)
    if self.items[itemId] then
        return self.items[itemId]
    end

    return 0
end


--[[
    @desc: 检查道具是否足够
    time:2022-05-05 10:05:17
]]
function ItemData:onCheckItemIsEnough(itemId, cointCount)
    local coin = self:onGetItemCount(itemId)
    if coin < cointCount then
        UITool:onShowTips(UIText('ui_tip_00008', UIText(self:onGetItemConfig(itemId).Name)))
        return false
    end

    return true
end


--[[
    @desc: 道具配置表Json转换Lua表 [只限道具]
    time:2022-05-17 15:35:41
]]
function ItemData:onJsonConvertLuaItem(config)
    return {
        id = config[1],
        num = config[2]
    }
end


return ItemData