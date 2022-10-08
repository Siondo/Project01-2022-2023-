--[[
Description: 切换头像界面
Author: xinZhao
Date: 2022-05-11 14:14:52
]]

local ReheadView = class('ReheadView', UIBase)

function ReheadView:OnLoad()
    local config, headInfos = Config.tbHead, {}
    for _, data in UITool:PairsBykeys(config) do
        headInfos[#headInfos + 1] = {
            isCheck = false,
            config = data,
        }
    end

    self:onBindData(headInfos, 'Content', false)
end


function ReheadView:OnShow()
    self.headID = PlayerData:getUserHeadAddress()
    self:onRefresh_Content() --刷新缓冲池
end


function ReheadView:onChanged_Content(index, data, obj)
    local checkIcon = obj.transform:Find('BtnIsCheck')
    local playerGrid = PlayerGrid:onInit(obj)

    playerGrid:onRefresh(index)
    playerGrid:onSetCheck(self.headID == data.config.ID)
    playerGrid:onAddClickEventHandler(function()
        self.headID = data.config.ID
        self:onRefresh_Content()
    end)
end


function ReheadView:onClick_BtnComfirm()
    local parmas = {
        infos = {
            head = tostring(self.headID)
        }
    }
    NetWork:onRequest(UIProtoType.ChangeAccountInfo.protoID, parmas, function(status, tbData)
        if status then
            PlayerData:setUserHeadAddress(self.headID)
            self:onClose()
        end
    end)
end


function ReheadView:onClick_BtnClose()
    self:onClose()
end

return ReheadView