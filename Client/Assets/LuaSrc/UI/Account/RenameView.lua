--[[
Description: 改名界面
Author: xinZhao
Date: 2022-05-11 14:14:52
LastEditTime: 2022-04-02 14:14:52
--]]

local RenameView = class('RenameView', UIBase)

function RenameView:OnShow()
    self.inputUID = ''
    self.ui.InputFieldInputEx.text = self.inputUID
end

function RenameView:onInputChanged_InputField(word)
    self.inputUID = word
end

function RenameView:onClick_BtnComfirm()
    if IsEmptyStringOrNull(self.inputUID) then
        UITool:onShowTips(UIText('ui_tip_00014'))
        return
    end
    
    if #self.inputUID > 15 then
        UITool:onShowTips(UIText('ui_tip_00015'))
        return
    end

    local parmas = {
        infos = {
            name = self.inputUID
        }
    }
    NetWork:onRequest(UIProtoType.ChangeAccountInfo.protoID, parmas, function(status, tbData)
        if status then
            UITool:onShowTips(UIText('ui_tip_00016'))
            PlayerData:setUserName(self.inputUID)
            self:onClose()
        end
    end)
end


function RenameView:onClick_BtnClose()
    self:onClose()
end

return RenameView