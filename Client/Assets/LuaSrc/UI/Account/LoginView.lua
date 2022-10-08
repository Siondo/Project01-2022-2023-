--[[
Description: 登录界面
Author: xinZhao
Date: 2022-05-20 10:42:16
]]

LoginView = class('LoginView', UIBase)

function LoginView:OnShow()
    self.isPasswordHide = true
    self.ui.EmailInputInputEx.text = ''
    self.ui.PasswordInputInputEx.text = ''
    UITool:SetSprte(self.ui.BtnSOHPasswordImgEx, 'AccountView/Registration_off', true)
end


function LoginView:onInputChanged_EmailInput(emailStr)
    self.emailStr = emailStr
end


function LoginView:onInputChanged_PasswordInput(passwordStr)
    self.passwrodStr = passwordStr
end


function LoginView:onClick_BtnSOHPassword()
    --0:显示 7:隐藏
    local spriteName = self.isPasswordHide and 'Registration_on' or 'Registration_off'
    local code = self.isPasswordHide and 0 or 7
    self.ui.PasswordInputInputEx.contentType = code
    UITool:SetSprte(self.ui.BtnSOHPasswordImgEx, 'AccountView/'..spriteName, true)
    self.ui.PasswordInputInputEx:UpdateInputField()
    self.isPasswordHide = not self.isPasswordHide
end


function LoginView:onClick_BtnLogin()
    if #self.passwrodStr < 6 or #self.passwrodStr > 16 then
        UITool:onShowTips(UIText('ui_tip_00011'))
        return
    end

    --在线切换账户
    if NetWork.isConnect then
        local parmas = {
            mail = self.emailStr,
            pwd = self.passwrodStr
        }
        HttpTool.SendRequest(UIWebType.CheckAccount, parmas, function(status, tbData)
            if status then
                if tbData.state == -3 then
                    UITool:onShowTips(UIText('ui_tip_00017'))
                    return
                end

                if tbData.state == -2 then
                    UITool:onShowTips(UIText('ui_tip_00018'))
                    return
                end

                if tbData.state == -1 then
                    UITool:onShowTips(UIText('ui_tip_00019'))
                    return
                end

                LuaHelper.SetString('email', self.emailStr)
                LuaHelper.SetString('password', self.passwrodStr)
                LuaHelper.SetString('uid', tbData.uid)
                LuaHelper.ReStartGame()
            end
        end)

    --不在线登录账户
    else
        UIEvent:OnEvent(UIEvent.AppSetupView.OnLogin, self.emailStr, self.passwrodStr)
    end
end


function LoginView:onClick_Forget()
    LuaHelper.ShowUI(UI.ForgetView)
end

return LoginView