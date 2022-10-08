--[[
Description: 头像公用组件
Author: xinZhao
Date: 2022-05-11 14:14:52
]]

PlayerGrid = class('PlayerGrid')

local function onGetComponent(gameObject, root)
    root = root or 'Container'
    local ui =
    {
        gameObject = gameObject,
        transform = gameObject.transform,
        headIcon = gameObject.transform:Find(root..'/HeadFrame/HeadIcon'):GetComponent(typeof(ImageEx)),
        checkPart = gameObject.transform:Find('CheckPart'),
        addPart = gameObject.transform:Find('AddPart'),
        mainIconButton = gameObject.transform:Find(root):GetComponent(typeof(ButtonEx)),
    }

    return ui
end


function PlayerGrid:onInit(gameObject, root)
    self.ui = onGetComponent(gameObject, root)
    return self
end


function PlayerGrid:onRefresh(headId)
    if IsEmptyStringOrNull(headId) then
        headId = 1
    end

    PlayerData:onSetPlayerHeadIcon(self.ui.headIcon, headId)
end


--[[
    @desc: 注册点击事件
    time:2022-05-17 10:40:24
]]
function PlayerGrid:onAddClickEventHandler(callBack)
    UITool:onAddClickAndClear(self.ui.mainIconButton, function()
        if callBack then
            callBack()
        end
    end)
end


--[[
    @desc: 是否置灰
    time:2022-05-17 10:26:02
]]
function PlayerGrid:onSetCheck(isCheck)
    self.ui.checkPart.gameObject:SetActive(isCheck)
end


--[[
    @desc: 是否显示添加状态
    time:2022-05-26 11:19:43
]]
function PlayerGrid:onSetAdd(isAdd)
    self.ui.addPart.gameObject:SetActive(isAdd)
end

return PlayerGrid