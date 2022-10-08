--[[
Description: 忘记密码界面
Author: xinZhao
Date: 2022-05-20 10:58:50
]]

ForgetView = class('ForgetView', UIBase)

function ForgetView:OnShow()
    self.ui.EmailInputInputEx.text = ''
end


function ForgetView:onInputChanged_EmailInput(emailStr)
    self.emailStr = emailStr
end


function ForgetView:onClick_BtnSubmit()
    local parmas = {
        mail = self.emailStr,
    }

    HttpTool.SendRequest(UIWebType.ForgetPassword, parmas, function(status, tbData)
        if status then
            log('123')
        end
    end)
end

return ForgetView