--[[
Description: 锁屏界面
Author: xinZhao
Date: 2022-04-12 14:28:20
LastEditTime: 2022-04-12 14:28:20
--]]

ScreenLockView = class('ScreenLockView', UIBase)

function ScreenLockView:OnLoad()
    self:AddEvent(UIEvent.ScreenLockView.ScreenLockTips, function (tips)
        self.ui.TipsTxtEx.text = tips
    end)
end

function ScreenLockView:OnShow()
end

return ScreenLockView
