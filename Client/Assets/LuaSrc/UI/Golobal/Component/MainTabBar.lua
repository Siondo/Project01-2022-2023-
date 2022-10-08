--[[
Description: 主界面页签自定义组件
Author: xinZhao
Date: 2022-04-02 15:09:57
LastEditTime: 2022-04-02 15:09:57
--]]

MainTabBar = class('MainTabBar')

function MainTabBar:onGetComponent()
    self.tabList = {}
    self.normalRoot = self.transform:Find('Normal')
    self.pressedRoot = self.transform:Find('Pressed')
    for i = 1, 5 do
        self.tabList[i] = {}
        self.tabList[i].frame = self.normalRoot:Find('Frame'..i)
        self.tabList[i].button = self.tabList[i].frame:Find('BtnTab'):GetComponent(typeof(Button))
        self.tabList[i].image = self.tabList[i].frame:Find('BtnTab'):GetComponent(typeof(ImageEx))
        self.tabList[i].iconTrans = self.tabList[i].frame:Find('BtnTab/Icon')
        self.tabList[i].pageText = self.tabList[i].frame:Find('BtnTab/Text'):GetComponent(typeof(TextEx))

        UITool:onAddClickAndClear(self.tabList[i].button, function()
            self:onClickTabEventHandler(i)
        end, AudioManager.Audio.Switch_TAB)
    end
end


function MainTabBar:onInit(gameObject)
    self.prePressedIndex = nil
    self.gameObject = gameObject
    self.transform = gameObject.transform

    self:onGetComponent()
end


--[[
    @desc: 当点击某个页签事件
    time:2022-04-02 16:06:04
    --@index: 页签Index
]]
function MainTabBar:onClickTabEventHandler(index)
    if not index then return end

    --处理上一个点击的Tab
    if self.prePressedIndex then
        local preTabData = self.tabList[self.prePressedIndex]
        preTabData.button.gameObject.transform:SetParent(preTabData.frame)
        self:onRefreshTab(self.prePressedIndex, false)
        self:onHideView(self.prePressedIndex)
    end

    --处理当前点击的Tab
    self.prePressedIndex = index
    self.tabList[index].button.gameObject.transform:SetParent(self.pressedRoot)
    self:onRefreshTab(index, true)
    self:onEnterView(index)
end


--[[
    @desc: 刷新某个页签显示状态
    --@index: 页签Index
	--@isPressed: 是否是按下
]]
function MainTabBar:onRefreshTab(index, isPressed)
    local spriteBundle = isPressed and 'Common/pnl/common_pin_actionbar_2' or 'Common/pnl/common_pin_actionbar_1'
    local iconTransPos = isPressed and Vector3(0, 10, 0) or Vector3(0, 0, 0)

    self.tabList[index].button.interactable = not isPressed
    self.tabList[index].iconTrans.localPosition = iconTransPos
    self.tabList[index].pageText.gameObject:SetActive(isPressed)
    UITool:SetSprte(self.tabList[index].image, spriteBundle, true)
end


--[[
    @desc: 隐藏上一次打开的界面
    --@index: 页签Index
]]
function MainTabBar:onHideView(index)
    if index == MAINTAB_INDEX.MAIN then
        LuaHelper.HideUI(UI.BattleListView)
    elseif index == MAINTAB_INDEX.FRIEND then
        LuaHelper.HideUI(UI.FriendListView)
    elseif index == MAINTAB_INDEX.SHOP then
        LuaHelper.HideUI(UI.ShopListView)
    elseif index == MAINTAB_INDEX.RECORD then
        LuaHelper.HideUI(UI.RecordListView)
    elseif index == MAINTAB_INDEX.RANK then
        LuaHelper.HideUI(UI.RankListView)
    end
end


--[[
    @desc: 开启这一次点击的界面
    --@index: 页签Index
]]
function MainTabBar:onEnterView(index)
    --主界面
    if index == MAINTAB_INDEX.MAIN then
        LuaHelper.ShowUI(UI.BattleListView)

    --好友界面
    elseif index == MAINTAB_INDEX.FRIEND then
        LuaHelper.ShowUI(UI.FriendListView)

    --商店界面
    elseif index == MAINTAB_INDEX.SHOP then
        LuaHelper.ShowUI(UI.ShopListView)

    --战绩界面
    elseif index == MAINTAB_INDEX.RECORD then
        LuaHelper.ShowUI(UI.RecordListView)

    --排行榜界面
    elseif index == MAINTAB_INDEX.RANK then
        LuaHelper.ShowUI(UI.RankListView)
    end
end

return MainTabBar