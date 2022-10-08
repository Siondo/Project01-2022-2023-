--[[
Description: 签到界面
Author: xinZhao
Date: 2022-03-31 14:00:54
LastEditTime: 2022-04-01 10:27:50
--]]

SignView = class("SignView", UIBase)

function SignView:OnLoad()
    self:AddEvent(UIEvent.SignView.OnRefresh, function()
        self:onRefreshDay7()
        self:onRefresh_Content()
    end)

    self.day7RewardsList = {}
    for i = 1, 3 do
        self.day7RewardsList[#self.day7RewardsList + 1] = {
            obj = self.ui.ContentRewardsGo.transform:Find('Item'..i),
            icon = self.ui.ContentRewardsGo.transform:Find('Item'..i..'/ItemIcon'):GetComponent(typeof(ImageEx)),
            count = self.ui.ContentRewardsGo.transform:Find('Item'..i..'/ItemCount'):GetComponent(typeof(TextEx)),
        }
    end

    local data = {}
    for i = 1, #SignData:onGetInfo() - 1 do
        data[#data + 1] = SignData:onGetInfo()[i]
    end
    self:onBindData(data, 'Content', false)
end


function SignView:OnShow()
    self:onRefreshDay7()
    self:onRefresh_Content()
end


function SignView:onChanged_Content(index, data, obj)
    local button = obj.transform:Find('Container'):GetComponent(typeof(ButtonEx))
    local frame = button.gameObject:GetComponent(typeof(ImageEx))
    local title = button.gameObject.transform:Find('Title'):GetComponent(typeof(TextEx))
    local icon = button.gameObject.transform:Find('Icon'):GetComponent(typeof(ImageEx))
    local iconCount = button.gameObject.transform:Find('Icon/Count'):GetComponent(typeof(TextEx))
    local checkedPart = button.gameObject.transform:Find('CheckedPart').gameObject
    local cannotCheckPart = button.gameObject.transform:Find('CantCheckPart').gameObject
    local canCheckPart = button.gameObject.transform:Find('CanCheckPart').gameObject

    self:onRefresh(data, frame, title, icon, checkedPart, cannotCheckPart, canCheckPart, button, iconCount)
end


--[[
    @desc: 刷新第七天
    time:2022-05-17 15:07:50
]]
function SignView:onRefreshDay7()
    self:onRefresh(SignData:onGetInfo()[7], self.ui.Day7ImgEx, self.ui.Day7TitleTxtEx, self.ui.IconImgEx, self.ui.CheckedPartGo, self.ui.CantCheckPartGo, self.ui.CanCheckPartGo)
end


--[[
    @desc: 界面刷新
    time:2022-05-17 15:03:49
]]
function SignView:onRefresh(data, frame, title, icon, checkedPart, cannotCheckPart, canCheckPart, button, iconCount)
    local signInfos = SignData:onGetInfo()

    --根据周期计算当前领取的道具ID
    local cycle = signInfos.cycle
    local toleranceNum
    while not toleranceNum do
        if cycle % 3 == 0 then
            toleranceNum = cycle
        else
            cycle = cycle - 1
        end
    end

    local itemIndex = (signInfos.cycle - toleranceNum) + 1
    local currentItem = ItemData:onJsonConvertLuaItem(data.Items[itemIndex])
    title.text = UIText('ui_dailybouns_00003', (signInfos.cycle * 7 + data.SignDays))
    if data.SignDays == 7 then
        self.ui.Day7TitleRewardsTxtEx.text = title.text
    end
    -- log('第%d周期, 第%d组道具可发放', signInfos.cycle, itemIndex)

    local currentSignDay = signInfos.current + 1
    local config = ItemData:onGetItemConfig(currentItem.id)
    --log('INDEX ='..data.SignDays..'  Config = '..config.IconFile)

    --已经领取过的道具展示样式
    if currentSignDay > data.SignDays then
        --第七天单独处理
        if data.SignDays == 7 then
            self.ui.Day7RewardsGo:SetActive(true)
            for i = 1, #self.day7RewardsList do
                local reward = ItemData:onJsonConvertLuaItem(data.Items[i])
                self.day7RewardsList[i].count.text = 'x'..reward.num
                UITool:SetSprte(self.day7RewardsList[i].icon, 'Common/icon/'..ItemData:onGetItemConfig(reward.id).IconFile, true)
            end
        else
            checkedPart:SetActive(true)
            iconCount.gameObject:SetActive(true)
            iconCount.text = 'x'..currentItem.num
            UITool:SetSprte(frame, 'Common/pnl/common_pin_qdon')
            UITool:SetSprte(icon, 'Common/icon/'..config.IconFile, true)
        end

    --未领取
    else
        --第七天单独处理
        if data.SignDays == 7 then
            self.ui.Day7RewardsGo:SetActive(false)
        else
            checkedPart:SetActive(false)
            iconCount.gameObject:SetActive(false)
            UITool:SetSprte(frame, 'Common/pnl/common_pin_qdoff')
            cannotCheckPart:SetActive(currentSignDay < data.SignDays or signInfos.states == false)
        end

        local iconSprite = data.SignDays == 7 and 'icon_dj_7rbx' or 'icon_dj_bx'
        UITool:SetSprte(icon, 'Common/icon/'..iconSprite, true)
    end

    --可领取的
    canCheckPart:SetActive(currentSignDay == data.SignDays and signInfos.states == true)
    if button then
        UITool:onAddClickAndClear(button, function()
            self:onClickSignEventHandler(data, itemIndex)
        end)
    end
end


function SignView:onClick_Day7()
    self:onClickSignEventHandler(SignData:onGetInfo()[7])
end


--[[
    @desc: 签到点击事件注册
    time:2022-05-17 15:04:00
]]
function SignView:onClickSignEventHandler(data, itemIndex)
    local signInfos = SignData:onGetInfo()
    if (signInfos.current + 1) > data.SignDays then
        UITool:onShowTips('该天的道具已经签到领取过了')
        return
    end

    if not signInfos.states then
        UITool:onShowTips('今日已签到, 明天在来吧')
        return
    end

    if (signInfos.current + 1) == data.SignDays then
        NetWork:onRequest(UIProtoType.SignDay.protoID, nil, function(status, tbData)
            if status then
                SignData:onSign(tbData.signDay, tbData.signState)
                local rewardInfos = {}
                if data.SignDays == 7 then
                    for i = 1, #data.Items do
                        rewardInfos[#rewardInfos + 1] = ItemData:onJsonConvertLuaItem(data.Items[i])
                    end
                else
                    rewardInfos[#rewardInfos + 1] = ItemData:onJsonConvertLuaItem(data.Items[itemIndex])
                end

                --发放奖励
                LuaHelper.ShowUI(UI.CommonRewardView, rewardInfos, function()
                    for i = 1, #rewardInfos do
                        ItemData:onAddItem(rewardInfos[i].id, rewardInfos[i].num)
                    end

                    --界面刷新
                    self:onRefreshDay7()
                    self:onRefresh_Content()
                    -- UITool:onShowTips(UIText('ui_tip_00009'))
                end)
            end
        end)
    end
end


function SignView:onClick_BtnTest()
    SignData:onSignTest()
    self:onRefreshDay7()
    self:onRefresh_Content()
end


function SignView:onClick_BtnClose()
    self:onClose()
end

return SignView
