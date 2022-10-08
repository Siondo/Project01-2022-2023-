--[[
    author:{gq}
    time:2022-04-18 12:26:15
]]

ShopData = class('ShopData')

function ShopData:onPullShopData()
    self.shopData = {}
    NetWork:onRequest(UIProtoType.ShopList.protoID, nil, function(stauts, data)
        if stauts then
            table.sort(data.storeList, function(a, b)
                return a.StoreID < b.StoreID
            end)

            for i = 1, #data.storeList do
                local config = data.storeList[i]
                if config.Type == StoreType.Shop_Slot_1 and config.endTime == -2 then
                    log('限时礼包到期 (StoreID: '..config.StoreID..')')
                else
                    config.Content = Json.decode(config.Content)
                    config.DisplayContent = Json.decode(config.DisplayContent)
                    self.shopData[#self.shopData + 1] = config
                end
            end
        end
    end)
end


--[[
    @desc: 获取商店数据
    time:2022-04-29 11:50:38
]]
function ShopData:onGetShopData()
    return self.shopData or {}
end


--[[
    @desc: 向每条数据内添加刷新方法
    time:2022-04-29 11:49:58
    --@index: 数组ID
	--@refreshCallBack: 刷新方法
]]
function ShopData:onAddRefreshCallBack(index, refreshCallBack)
    if not self.shopData[index] then return end
    
    self.shopData[index].refresh = refreshCallBack
    refreshCallBack()
end


--[[
    @desc: 暂停/清理 所有商品上带有的倒计时 (**关闭界面的时候, 需要手动调用不能使倒计时全局调用**)
    time:2022-04-29 14:29:21
]]
function ShopData:onSetAllTimerStatus(isStart)
    for i = 1, #self.shopData do
        if self.shopData[i].timer then
            if isStart then
                self.shopData[i].timer:Start()
            else
                self.shopData[i].timer:Stop()
                if self.shopData[i].endTime <= 0 then
                    self.shopData[i].timer = nil
                end
            end
        end
    end
end


return ShopData