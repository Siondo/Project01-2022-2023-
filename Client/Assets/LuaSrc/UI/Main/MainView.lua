local MainView = class('MainView', UIBase)


function MainView:OnLoad()
    log('self.ui.BtnLoginGo = ', self.ui.BtnLoginGo.name)
end

function MainView:OnShow()
end

function MainView:onClick_BtnLogin()
    log('我被点击了')
    LuaHelper.ShowUI(UI.MainTipView)
end

return MainView