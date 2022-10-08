--[[
Description: App运行界面
Author: xinZhao
Date: 2022-04-02 15:57:55
LastEditTime: 2022-04-02 15:58:10
--]]


local AppSetupView = class('AppSetupView', UIBase)

function AppSetupView:OnLoad()
    UITool:SetSprte(self.ui.CompanyLogoImgEx, 'AppSetupView/logo_'..LuaHelper.GetAppPlatform(), true)

    self:AddEvent(UIEvent.AppSetupView.UpdateTips, function (tips, detail)
        if tips then
            self.ui.TipsTxtEx.text = tips
        end
        if detail then
            self.ui.DetailTxtEx.text = detail
        end
    end)

    self:AddEvent(UIEvent.AppSetupView.UpdateProgress, function (curSize, totalSize, str, isInGame)
        if str then
            self.ui.ProgressValueTxtEx.text = str
            return 
        end

        self.ui.ProgressSldEx.value = curSize / totalSize
        self.ui.ProgressValueTxtEx.text = string.format("%0.2f", curSize / totalSize * 100.0).."%"

        if not isInGame and self.ui.ProgressSldEx.value >= 1 then
            self.ui.AccountGo:SetActive(true)
            self.ui.LoadingGo:SetActive(false)
        end
    end)

    self:AddEvent(UIEvent.AppSetupView.OnLogin, function (email, password)
        self:onSync(email, password)
    end)
end


function AppSetupView:OnShow(callBack)
    self.ui.AccountGo:SetActive(false)
    self.ui.LoadingGo:SetActive(true)
end
 

function AppSetupView:onSync(email, password)
    LuaHelper.ClearUI(false)
end


return AppSetupView