--[[
Description: 账户界面
Author: xinZhao
Date: 2022-05-11 14:14:52
LastEditTime: 2022-04-02 14:14:52
--]]
local AccountView = class('AccountView', UIBase)


function AccountView:OnLoad()
    self.playerGridCpn = bind(self.ui.PlayerGridGo, PlayerGrid, '@Container')

    --刷新
    self:AddEvent(UIEvent.AccountView.OnRefresh, function()
        self:onRefreshView()
    end)
end


function AccountView:OnShow()
    self:onRefreshView()
end


function AccountView:onRefreshView()
    self.ui.PlayerNameTxtEx.text = PlayerData:getUserName()
    self.playerGridCpn:onRefresh(PlayerData:getUserHeadAddress())
    self.ui.TxtUserInfoTxtEx.text = UIText('ui_addfriends_00012')..': '..PlayerData:getUserID()
end


--[[
    @desc: 复制UID
    time:2022-05-06 17:58:22
]]
function AccountView:onClick_BtnCopy()
    UnityEngine.GUIUtility.systemCopyBuffer = string.Split(self.ui.TxtUserInfoTxtEx.text,':')[2]
    UITool:onShowTips(UIText('ui_tip_00007'))
end


--[[
    @desc: 进入更改头像界面
    time:2022-05-11 17:04:43
]]
function AccountView:onClick_Container()
    LuaHelper.ShowUI(UI.ReheadView)
end


--[[
    @desc: 进入更改名称界面
    time:2022-05-11 15:44:50
]]
function AccountView:onClick_BtnModify()
    LuaHelper.ShowUI(UI.RenameView)
end


function AccountView:onClick_BtnClose()
    self:onClose()
end


function AccountView:onClick_BtnRegister()
    LuaHelper.ShowUI(UI.RegisterView)
end

function AccountView:onClick_BtnSign()
    LuaHelper.ShowUI(UI.LoginView)
end

return AccountView