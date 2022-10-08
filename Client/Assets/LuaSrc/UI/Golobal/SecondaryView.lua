
local SecondaryView = class("SecondaryView", UIBase)

function SecondaryView:OnShow(...)
    local args = {...}
    local type = args[1]
    self.ui.CloseBtnGo:SetActive(false)
    self.ui.LeftBtnGo:SetActive(true)
    self.ui.TipsTextTxtEx.text = args[2]
    if 0 == type then
        self.ui.RightTextTxtEx.text = args[3]
        local rightFunc, closeFunc = args[4], args[5]
        self.ui.LeftBtnGo:SetActive(false)
        self:AddClickAndClear(self.ui.RightBtnBtnEx, function () LuaHelper.HideUI(UI.SecondaryView) rightFunc() end)
        if closeFunc then
            self.ui.CloseBtnGo:SetActive(true)
            self:AddClickAndClear(self.ui.CloseBtnBtnEx, function () closeFunc() end)
        end
    elseif 1 == type or -1 == type then
        local leftFunc, rightFunc, closeFunc = args[4], args[6], args[7]
        if 1 == type then
            self.ui.LeftTextTxtEx.text = args[3] or UIText(12002)
            self.ui.RightTextTxtEx.text = args[5] or UIText(12003)
            self.ui.RightBtnRectTf:SetAsLastSibling()
        else
            self.ui.RightTextTxtEx.text = args[3] or UIText(12003)
            self.ui.LeftTextTxtEx.text = args[5] or UIText(12002)
            rightFunc, leftFunc = leftFunc, rightFunc
            self.ui.LeftBtnRectTf:SetAsLastSibling()
        end

        self:AddClickAndClear(self.ui.LeftBtnBtnEx, function () leftFunc() end)
        self:AddClickAndClear(self.ui.RightBtnBtnEx, function ()
            LuaHelper.HideUI(UI.SecondaryView)
            rightFunc()
        end)
        if closeFunc then
            self.ui.CloseBtnGo:SetActive(true)
            self:AddClickAndClear(self.ui.CloseBtnBtnEx, function () LuaHelper.HideUI(UI.SecondaryView) closeFunc() end)
        end
    end

end

return SecondaryView