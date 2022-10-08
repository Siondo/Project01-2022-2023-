--[[
Description: 内购支付
Author: xinZhao
Date: 2022-04-29 10:20:34
LastEditTime: 2022-04-29 10:20:34
--]]

IAPManager = class('IAPManager')


--[[
    @desc: 沙盒支付
    time:2022-04-29 15:16:32
    --@storeId: 商品ID
	--@callBack: 回调
]]
function IAPManager:onPay(storeId, callBack)
    NetWork:onRequest(UIProtoType.SandBoxPay.protoID, {shopId = storeId}, function(status, table)
        if status then
            --解析道具
            local config = Config.tbStore[tostring(storeId)]
            local cloneData = clone(config)
            local mergeList = {}
            if cloneData.DisplayContent[1] then
                mergeList[#mergeList + 1] = ItemData:onJsonConvertLuaItem(cloneData.DisplayContent[1])
            end

            for i = 1, #cloneData.Content do
                mergeList[#mergeList + 1] = ItemData:onJsonConvertLuaItem(cloneData.Content[i])
            end

            callBack(table)
            LuaHelper.ShowUI(UI.CommonRewardView, mergeList, function()
                --发送道具
                for i = 1, #mergeList do
                    ItemData:onAddItem(mergeList[i].id, mergeList[i].num)
                end

                UITool:onShowTips(UIText('ui_tip_00020'))
            end)
        end
    end)
end


return IAPManager