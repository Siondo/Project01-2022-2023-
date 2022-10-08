--[[
Description: 注册界面
Author: xinZhao
Date: 2022-05-20 10:15:47
]]

RegisterView = class('RegisterView', UIBase)

function RegisterView:OnShow()
    self.isPasswordHide = true
    self.ui.EmailInputInputEx.text = ''
    self.ui.PasswordInputInputEx.text = ''
    UITool:SetSprte(self.ui.BtnSOHPasswordImgEx, 'AccountView/Registration_off', true)
end


function RegisterView:onInputChanged_EmailInput(emailStr)
    self.emailStr = emailStr
end


function RegisterView:onInputChanged_PasswordInput(passwordStr)
    self.passwrodStr = passwordStr
end


function RegisterView:onClick_BtnSOHPassword()
    --0:显示 7:隐藏
    local spriteName = self.isPasswordHide and 'Registration_on' or 'Registration_off'
    local code = self.isPasswordHide and 0 or 7
    self.ui.PasswordInputInputEx.contentType = code
    UITool:SetSprte(self.ui.BtnSOHPasswordImgEx, 'AccountView/'..spriteName, true)
    self.ui.PasswordInputInputEx:UpdateInputField()
    self.isPasswordHide = not self.isPasswordHide
end


function RegisterView:onClick_BtnRegister()
    if #self.passwrodStr < 6 or #self.passwrodStr > 16 then
        UITool:onShowTips(UIText('ui_tip_00011'))
        return
    end

    local parmas = {
        mail = self.emailStr,
        pwd = self.passwrodStr
    }
    HttpTool.SendRequest(UIWebType.Register, parmas, function(status, tbData)
        if status then
            UITool:onShowTips(UIText('ui_tip_00012'))
            LuaHelper.ShowUI(UI.LoginView)
            self:onClose()
        else
            if tbData == -4 then
                UITool:onShowTips(UIText('ui_tip_00013'))
            end
        end
    end)
end


function RegisterView:onClick_BtnSign()
    LuaHelper.ShowUI(UI.LoginView)
end

return RegisterView