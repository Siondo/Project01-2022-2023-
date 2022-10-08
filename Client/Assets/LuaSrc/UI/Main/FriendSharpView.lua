--[[
Description: 好友分享界面
Author: xinZhao
Date: 2022-05-05 11:33:37
LastEditTime: 2022-04-06 11:33:37
--]]

FriendSharpView = class('FriendSharpView', UIBase)

function FriendSharpView:onClick_BtnSharp()
    UnityEngine.GUIUtility.systemCopyBuffer = self.ui.InputFieldInputEx.text
    UITool:onShowTips(UIText('ui_tip_00007'))
end

function FriendSharpView:onClick_BtnChange()
    UnityEngine.GUIUtility.systemCopyBuffer = self.ui.InputFieldInputEx.text
    UITool:onShowTips(UIText('ui_tip_00007'))
end

function FriendSharpView:onClick_BtnClose()
    self:onClose()
end

return FriendSharpView
