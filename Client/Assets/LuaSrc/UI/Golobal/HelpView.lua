--[[
Description: 通用帮助界面
Author: xinZhao
Date: 2022-05-24 17:16:52
LastEditTime: 2022-05-24 17:16:52
]]

HelpView = class('HelpView', UIBase)

function HelpView:OnShow(title, content)
    self.ui.TitleTxtEx.text = title
    self.ui.DescTxtEx.text = content
end


function HelpView:onClick_BtnClose()
    self:onClose()
end

return HelpView