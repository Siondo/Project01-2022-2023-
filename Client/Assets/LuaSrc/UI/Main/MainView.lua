--[[
Description: 主界面
Author: xinZhao
Date: 2022-04-02 14:14:52
LastEditTime: 2022-04-02 14:14:52
--]]

local MainView = class('MainView', UIBase)

function MainView:OnLoad()

    --初始化重连数据
    ReconnectionData:onInit()

    self.mainBar = bind(self.ui.MainTabBarGo, MainTabBar)
    self.mainPowerBar = bind(self.ui.transform:Find('TopContent/StatusBarPower').gameObject, MainStausBar, ItemType.POWER)
    self.mainCoinBar = bind(self.ui.transform:Find('TopContent/StatusBarCoin').gameObject, MainStausBar, ItemType.COIN)

    self:AddEvent(UIEvent.MainView.RefreshStatusBar, function()
        self.mainCoinBar:onRefreshStatusBar()
        self.mainPowerBar:onRefreshStatusBar()
        self:onCheckPowerTimer()
    end)

    self:AddEvent(UIEvent.MainView.JumpStore, function()
        self.mainBar:onClickTabEventHandler(MAINTAB_INDEX.SHOP)
    end)

    --获取道具飞行的目的地组件
    self:AddEvent(UIEvent.MainView.GetTargetComponent, function(itemId, callBack)
        local targetCpn
        if itemId == ItemType.COIN then
            targetCpn = self.mainCoinBar:onGetIconComponent()
        elseif itemId == ItemType.POWER or itemId == ItemType.INFINITEPOWER then
            targetCpn = self.mainPowerBar:onGetIconComponent()
        end
        callBack(targetCpn)
    end)
end


function MainView:OnShow(index)
    self.mainBar:onClickTabEventHandler(index or MAINTAB_INDEX.MAIN)
    self:onCheckPowerTimer()
end


--[[
    @desc: 检测无限体力
    time:2022-05-25 10:28:45
]]
function MainView:onCheckPowerTimer()
    local powerEndTime = ItemData:onGetItemCount(ItemType.INFINITEPOWER)
    if powerEndTime - os.time() > 0 then
        if not self.golobalTimer then
            self.golobalTimer = Timer.Create(function()
                if powerEndTime - os.time() > 0 then
                    self.mainPowerBar:onRefreshStatusBar()
                else
                    self.golobalTimer:Stop()
                    ItemData:onOperateInfinite(ItemType.INFINITEPOWER, 0)
                end
            end, 1, 999)
        end

        self.golobalTimer:Start()
    end
end


function MainView:OnHide()
    if self.golobalTimer then
        self.golobalTimer:Stop()
    end
end


function MainView:onClick_BtnSetting()
    LuaHelper.ShowUI(UI.SettingView)
end

return MainView