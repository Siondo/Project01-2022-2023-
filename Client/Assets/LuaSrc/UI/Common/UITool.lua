--[[
Description: UI全局工具
Author: xinZhao
Date: 2022-04-02 15:57:55
LastEditTime: 2022-04-02 15:58:10
--]]

UITool = class('UITool')

--[[
    @desc: 设置图片
    --@image: 图片组件(ImageEx)
	--@spriteName: 图片位置
	--@isSetNativeSize: 是否重置尺寸
]]
function UITool:SetSprte(image, spriteName, isSetNativeSize)
    if not image then return end
    image:SetSprite(SPRITE_BUNDLE_PATH..spriteName..'.png', isSetNativeSize or false)
end


--[[
    @desc: 判断一个字符串中是否包含另一个字符串
    --@originString: 元字符串
	--@otherString:  对比字符串
]]
function UITool:onContainsValue(originString, otherString)
    local startPos, endPos = string.find(originString, otherString)
    if startPos and endPos then
        return true, startPos, endPos
    end

    return false
end


--[[
    @desc: 裁剪字符串
    --@originString: 元字符串
	--@starPos: 开始位置
	--@endPos:  结束位置
]]
function UITool:onInterceptionValue(originString, starPos, endPos)
    return string.sub(originString, starPos, endPos)
end


--[[
    @desc: 保留小数点后指定个数
    time:2022-05-17 17:35:46
    --@nNum: 原始浮点数
	--@n: 个数
]]
function UITool:GetPreciseDecimal(nNum, n)
    if type(nNum) ~= "number" then
        return nNum;
    end
    n = n or 0;
    n = math.floor(n)
    if n < 0 then
        n = 0;
    end
    local nDecimal = 10 ^ n
    local nTemp = math.floor(nNum * nDecimal);
    local nRet = nTemp / nDecimal;
    return nRet;
end


--[[
    @desc: 时间戳格式化
    time:2022-04-21 15:16:38
    --@time: 剩余时间戳
    @return: 格式化的时间str
]]
function UITool:timeString(time, type)
    local originTime = time

    local hours = math.floor(time / 3600)
    local minutes = math.floor((time % 3600) / 60)
    local seconds = math.floor(time % 60)
    if(hours < 10) then hours = "0"..hours end
    if(minutes < 10) then  minutes = "0"..minutes end
    if(seconds < 10) then seconds = "0"..seconds end 

    local time
    if type == 1 then
        time = ""..hours..":"..minutes..":"..seconds  --00:00:00
    elseif type == 2 then
        if math.floor(originTime / 3600) >= 1 then
            time = hours.."h" --xxH
        else
            time = minutes.."m" --xxH
        end
        
    else
        time = minutes..":"..seconds --00:00
    end
    return time
end


--[[
    @desc: 有序化哈希表
    time:2022-04-21 15:16:38
    --@time:
    @return: 有序化表
]]
function UITool:PairsBykeys(tabletemp)
    local a = {}
    for n in pairs(tabletemp) do
        a[#a+1] = n
    end
    table.sort(a)
    local i = 0
    return function ()
    i = i + 1
    return a[i],tabletemp[a[i]]
    end
end


--[[
    @desc: 无序转有序
    time:2022-04-28 09:45:42
    --@table: 数组
]]
function UITool:onTableConver(table)
    local tb = {}
    for id, data in self:PairsBykeys(table) do
        local index = #tb + 1
        tb[index] = data
        tb[index].id = id
    end
    return tb
end


function UITool:ConfigNum(num)
    local t1, t2 = math.modf(num)
    if t2 == 0 then
        return t1
    end
    return num
end


--[[
    @desc: 货币转换
    time:2022-04-29 11:34:05
    @return: 1k,1m...
]]
function UITool:tradeConvert(num)
    if type(num) == "string" then
        return num
    end
    if type(num) ~= "number" then
        logError("数字转字符串错误")
        return num
    end

    local str = ""
    if num > 9999 and num <= 1000000 then
        return tostring(self:ConfigNum(math.floor(num * 10 / 10000) * 0.1)).."k"
    elseif num > 1000000 and num <= 9999999999 then
        return tostring(self:ConfigNum(math.floor(num * 100 / 100000000) * 0.01)).."m"
    elseif num > 9999999999 then
        return tostring(self:ConfigNum(math.floor(num * 10 / 100000000) * 0.1)).."m"
    else
        return self:ConfigNum(num)
    end
end


--[[
    @desc: 延迟执行
    time:2022-05-11 11:46:05
    --@time: 延迟时间
	--@callBack: 执行回调
]]
function UITool:onDelay(time, callBack)
    if time > 0 then
        local delayTimer = Timer.Create(function()
            callBack()
        end, time, 1)
        delayTimer:Start()
    else
        callBack()
    end
end


--[[
    @desc: 全局点击事件绑定方法
    time:2022-05-25 10:35:09
    --@button: 按钮组件
	--@event:  事件回调
	--@audioName: 点击音效名称
	--@useObj: 参数
]]
function UITool:onAddClickAndClear(button, event, audioName, useObj)
    button.onClick:RemoveAllListeners()
    button.onClick:AddListener(function()
        audioName = audioName or AudioManager.Audio.Click
        AudioManager.PlaySound(audioName)
        if useObj then
            event(useObj, button.gameObject)
        else
            event(button.gameObject)
        end
    end)
end


--[[
    @desc: 全局tips提示文本
    time:2022-05-26 10:57:06
]]
function UITool:onShowTips(langStr)
    if not LuaHelper.IsShowUI(UI.GamePoolView) then
        LuaHelper.ShowUIImmediate(UI.GamePoolView)
    end

    local parmas = {
        str = langStr
    }
    
    UIEvent:OnEvent(UIEvent.GamePoolView.OnPushHandler, GamePoolType.TIPS_ITem, parmas)
end


--[[
    @desc: 全局道具飞行动画
    time:2022-05-25 10:36:27
    --@itemId: 道具id
]]
function UITool:onGlobalFlyItem(itemId, startCpn, endCpn)
    if not LuaHelper.IsShowUI(UI.GamePoolView) then
        LuaHelper.ShowUIImmediate(UI.GamePoolView)
    end

    local parmas = {
        itemId = itemId or 0,
        endCpn = endCpn,
    }

    if not startCpn then
        parmas.isNeedCovert = true
        parmas.startCpn = Input.mousePosition
    else
        parmas.isNeedCovert = false
        parmas.startCpn = startCpn
    end

    UIEvent:OnEvent(UIEvent.GamePoolView.OnPushHandler, GamePoolType.FLY_ITEM, parmas)
end


--[[
    @desc: 上一个Spine动画播放完毕后 接着播放的动画
    time:2022-05-27 14:45:23
    --@spine: spine动画组件
	--@aniName: 下个动画名称
	--@isLoop: 是否循环
	--@delayTime: 延迟时间
]]
function UITool:onAddSpineAnimation(spine, aniName, isLoop, delayTime)
    spine.AnimationState:AddAnimation(0, aniName, isLoop, delayTime)
end


--[[
    @desc: 播放Spine动画
    time:2022-05-27 14:48:21
    --@spine: spine动画组件
	--@aniName: 动画名称
	--@isLoop: 是否循环
	--@nextAniName: 紧接着播放的动画名称
	--@nextAniIsLoop: 紧接着播放的动画是否循环
]]
function UITool:onPlaySpineAnimation(spine, aniName, isLoop, nextAniName, nextAniIsLoop)
    spine.AnimationState:SetAnimation(0, aniName, isLoop)
    if nextAniName then
        self:onAddSpineAnimation(spine, nextAniName, nextAniIsLoop, 0)
    end
end

return UITool