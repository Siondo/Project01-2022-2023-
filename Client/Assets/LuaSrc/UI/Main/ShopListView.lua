--[[
Description: 商城界面
Author: xinZhao
Date: 2022-04-02 15:57:55
LastEditTime: 2022-04-02 15:58:10
--]]
ShopListView = class("ShopListView", UIBase)

local bundlePath = 'Res/UI/Prefab/Golobal/Item/Shop/'

function ShopListView:OnLoad()
    UITool:SetSprte(self.ui.ShopImgEx, 'ShopView/shop_shop_'..LuaHelper.GetAppPlatform(), true)

    local shopData = ShopData:onGetShopData()
    for index = 1, #shopData do
        LuaHelper.LoadAsset(bundlePath..shopData[index].Type..'.prefab', function(status, asset)
            if status then
                local obj = GameObject.Instantiate(asset.mainAsset, self.ui.ContentRectTf)
                obj.name = 'ITEM-'..index..'-TYPE:'..shopData[index].Type
                ShopData:onAddRefreshCallBack(index, self:onCreateItems(shopData[index], obj))
            end
        end)
    end
end


function ShopListView:onCreateItems(bindData, obj)
    local disPlayItem ,items, btnBuy, buyPrice, limitTime, title, btnVideo, limitCount, endTimeTxtEx = {}, {}
    local limitPartTxt
    if obj.transform:Find('Title') then
        title = obj.transform:Find('Title'):GetComponent(typeof(TextEx))
    end

    if obj.transform:Find('BtnBuy') then
        btnBuy = obj.transform:Find('BtnBuy'):GetComponent(typeof(ButtonEx))
        buyPrice = obj.transform:Find('BtnBuy/BuyPrice'):GetComponent(typeof(TextEx))
    end

    if obj.transform:Find('BtnVideo') then
        btnVideo = obj.transform:Find('BtnVideo'):GetComponent(typeof(ButtonEx))
        limitCount = obj.transform:Find('BtnVideo/RedDot/Text'):GetComponent(typeof(TextEx))
        endTimeTxtEx = obj.transform:Find('BtnVideo/Mask/EndTime'):GetComponent(typeof(TextEx))
    end

    if bindData.Type == StoreType.Shop_Slot_1 then
        limitTime = obj.transform:Find('Root/Limited/LimitTime'):GetComponent(typeof(TextEx))
        --先直接初始化一次
        local time = bindData.endTime - os.time()
        limitTime.text = UITool:timeString(time, 1)

        disPlayItem[#disPlayItem + 1] = {
            root = obj.transform:Find('Root/Items/Item1'),
            itemIcon = obj.transform:Find('Root/Items/Item1'):GetComponent(typeof(ImageEx)),
            itemCount = obj.transform:Find('Root/Items/Item1/Text'):GetComponent(typeof(TextEx)),
        }

    elseif bindData.Type == StoreType.Shop_Slot_2 or bindData.Type == StoreType.Shop_Slot_5 then
        disPlayItem[#disPlayItem + 1] = {
            root = obj.transform:Find('Root/Display/Title'),
            itemIcon = obj.transform:Find('Root/Display/Title/Image'):GetComponent(typeof(ImageEx)),
            itemCount = obj.transform:Find('Root/Display/Title/Text'):GetComponent(typeof(TextEx)),
        }
        for i = 1, 4 do
            items[#items + 1] = {
                root = obj.transform:Find('Root/Items/Item'..i),
                itemIcon = obj.transform:Find('Root/Items/Item'..i..'/Image'):GetComponent(typeof(ImageEx)),
                itemCount = obj.transform:Find('Root/Items/Item'..i..'/Text'):GetComponent(typeof(TextEx)),
            }
        end
        if bindData.Type == StoreType.Shop_Slot_5 then
            limitPartTxt = obj.transform:Find('Root/LimitPart/TextEx'):GetComponent(typeof(TextEx))
        end
    
    elseif bindData.Type == StoreType.Shop_Slot_3 or bindData.Type == StoreType.Shop_Slot_4 then
        items[#items + 1] = {
            root = obj.transform:Find('Cast'),
            itemIcon = obj.transform:Find('Cast/CastIcon'):GetComponent(typeof(ImageEx)),
            itemCount = obj.transform:Find('Cast/CastCount'):GetComponent(typeof(TextEx)),
        }
    end
    
    return function()
        local setItemStyle = function(pool, data, isSetSprite)
            for i = 1, #pool do
                pool[i].root.gameObject:SetActive(#data >= i)
                if #data >= i then
                    local config = ItemData:onGetItemConfig(data[i][1])
                    local textDisplay = config.ItemID == ItemType.INFINITEPOWER and UITool:timeString(data[i][2], 2) or UITool:tradeConvert(data[i][2])
                    if isSetSprite then
                        UITool:SetSprte(pool[i].itemIcon, 'Common/Icon/'..config.IconFile, true)
                    end
                    pool[i].itemCount.text = textDisplay
                end
            end
        end

        --设置标题
        if title then
            title.text = UIText(bindData.Name)
        end
    
        --设置Item样式
        setItemStyle(disPlayItem, bindData.DisplayContent, false)
        setItemStyle(items, bindData.Content, true)

        --购买按钮
        if btnBuy then
            buyPrice.text = 'US$'..bindData.Price
            UITool:onAddClickAndClear(btnBuy, function()
                LuaHelper.ShowUI(UI.SandBoxPayView, bindData)
            end)
        end


        if limitPartTxt then
            limitPartTxt.text = UIText(bindData.LimitWord or '')
        end

        --广告按钮 + 限购次数
        if btnVideo then
            limitCount.text = bindData.limitNum

            --从注册表中获取剩余时间
            local regeditTime = LuaHelper.GetString(bindData.StoreID), bindData.CD
            regeditTime = not IsEmptyStringOrNull(regeditTime) and regeditTime or bindData.CD

            --注册计时器
            local setTimer = function(totallyTime)      
                local timer = Timer.Create(function(loop)
                    regeditTime = regeditTime - 1
                    endTimeTxtEx.text = UITool:timeString(regeditTime)
                    LuaHelper.SetString(bindData.StoreID, loop)

                    if loop == 1 then
                        LuaHelper.SetString(bindData.StoreID, '')
                        endTimeTxtEx.gameObject.transform.parent.gameObject:SetActive(false)
                    end
                end, 1, totallyTime)
                timer:Start()
            end

            --如果计时器未结束, 重新直接开启计时器
            if not IsEmptyStringOrNull(LuaHelper.GetString(bindData.StoreID)) then
                endTimeTxtEx.gameObject.transform.parent.gameObject:SetActive(true)
                endTimeTxtEx.text = UITool:timeString(regeditTime)
                setTimer(regeditTime)
            else
                endTimeTxtEx.gameObject.transform.parent.gameObject:SetActive(false)
            end

            UITool:onAddClickAndClear(btnVideo, function()
                if not IsEmptyStringOrNull(LuaHelper.GetString(bindData.StoreID)) then
                    UITool:onShowTips('倒计时结束后才能继续观看广告')
                    return
                end

                IAPManager:onPay(bindData.StoreID, function()
                    --次数减少
                    bindData.limitNum = bindData.limitNum - 1
                    limitCount.text = bindData.limitNum

                    --倒计时时间初始化
                    regeditTime = bindData.CD
                    endTimeTxtEx.text = UITool:timeString(regeditTime)

                    --开启遮罩
                    endTimeTxtEx.gameObject.transform.parent.gameObject:SetActive(true)
                    setTimer(regeditTime)
                end)
            end)
        end


        --限时礼包+计时器
        obj:SetActive(true)
        if limitTime then
            local clearItem = function()
                obj:SetActive(false)
                if bindData.timer then
                    bindData.timer:Stop()
                end
            end

            if bindData.endTime > 0 then
                if not bindData.timer then
                    bindData.timer = Timer.Create(function()
                        local time = bindData.endTime - os.time()
                        limitTime.text = UITool:timeString(time, 1)
                        if time <= 0 then
                            clearItem()
                        end

                    end, 1, 999)
                    bindData.timer:Start()
                end
            elseif bindData.endTime == -2 then
                clearItem()
            end
        end
    end
end


function ShopListView:OnShow()
    ShopData:onSetAllTimerStatus(true)  --开启倒计时
end


function ShopListView:OnHide()
    ShopData:onSetAllTimerStatus(false) --关闭/清理倒计时
end

return ShopListView