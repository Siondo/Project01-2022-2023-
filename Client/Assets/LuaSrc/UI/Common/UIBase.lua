UIBase = {}

-- 构造
function UIBase:Ctor()
    log(self.class.className)
end

--[[
    @desc: 界面被呼出, 只加载一次
    主要用于脚本自动绑定事件, 和初始化工作
]]
function UIBase:OnAwake()
    self:onAutoBindEventHandler()
end

---@module UIBase.txt
---@author Rubble
---@since 2020/7/9 11:45
---@see 只调用一次，用于初始化
function UIBase:OnLoad()
end

-- 显示
function UIBase:OnShow(...)
end

-- 更新
function UIBase:OnUpdate(...)
end

-- 隐藏
function UIBase:OnHide()
end

-- 销毁
function UIBase:OnUnload()
end

--是否显示
function UIBase:IsShow()
    return nil ~= self.class and IsNotNull(self.class.ui.gameObject) and self.class.ui.gameObject.activeSelf
end


--[[
    @desc: 绑定滑动列表数据
    --@data: 数组 {xxx, xxx, xxx}
    --@isInitRefresh: 是否在初始化完成后进行自动刷新
]]
function UIBase:onBindData(data, funcName, isInitRefresh)
    self.bindData = self.bindData or {}
    self.bindData[funcName] = data
    self.bindData[funcName].isInitRefresh = isInitRefresh == nil and true or isInitRefresh
end


--[[
    @desc: 自动绑定点击事件
    time:2022-04-02 17:35:51
]]
function UIBase:onAutoBindEventHandler()
    for cpnName, cpn in pairs(self.ui) do

        --点击事件绑定
        local isExist, startPos, endPos = UITool:onContainsValue(cpnName, 'BtnEx')
        if isExist and endPos == string.len(cpnName) then
            local buttonName = 'onClick_'..UITool:onInterceptionValue(cpnName, 1, (startPos - 1))
            self:AddClickAndClear(cpn, function()
                if self[buttonName] then
                    self[buttonName](self, cpn)
                end
            end)
        end

        --读写事件绑定
        local isExist, startPos, endPos = UITool:onContainsValue(cpnName, 'InputEx')
        if isExist and endPos == string.len(cpnName) then
            local inputName = 'onInputChanged_'..UITool:onInterceptionValue(cpnName, 1, (startPos - 1))
            cpn.onValueChanged:RemoveAllListeners()
            cpn.onValueChanged:AddListener(function(word)
                if self[inputName] then
                    self[inputName](self, word)
                end
            end)
        end

        --滑动列表绑定
        local scroll, isExist, startPos, endPos
        if UITool:onContainsValue(cpnName, '&GridRectTf') then
            isExist, startPos, endPos = UITool:onContainsValue(cpnName, '&GridRectTf')
            scroll = self.ui[cpnName]:GetComponent(typeof(ScrollPoolGrid))
        elseif UITool:onContainsValue(cpnName, '&HRectTf') then
            isExist, startPos, endPos = UITool:onContainsValue(cpnName, '&HRectTf')
            scroll = self.ui[cpnName]:GetComponent(typeof(ScrollPoolHorizontal))
        elseif UITool:onContainsValue(cpnName, '&VRectTf') then
            isExist, startPos, endPos = UITool:onContainsValue(cpnName, '&VRectTf')
            scroll = self.ui[cpnName]:GetComponent(typeof(ScrollPoolVertical))
        end

        if scroll then
            local funcEndName = UITool:onInterceptionValue(cpnName, 1, (startPos - 1))
            if self.bindData[funcEndName] then
                self['onRefresh_'..funcEndName] = function()
                    scroll:SetUpdateCallBack(function(index, obj)
                        if self['onChanged_'..funcEndName] then
                            self['onChanged_'..funcEndName](self, index, self.bindData[funcEndName][index], obj)
                        end
                    end)
                    scroll:InitPool(#self.bindData[funcEndName])
                end

                if self.bindData[funcEndName].isInitRefresh then
                    self['onRefresh_'..funcEndName]()
                end
            else
                logError('检测到你使用了 ScroolPool组件, 需要在界面 OnLoad 的时候调用 onBindData 绑定数据')
            end
        end
    end
end

--移除点击事件
---@param button UnityEngine.UI.Button
function UIBase:RemoveAllListeners(button)
    button.onClick:RemoveAllListeners()
end

--移除并添加点击事件
---@param button UnityEngine.UI.Button
---@param action function
function UIBase:AddClickAndClear(button, event, useObj)
    UITool:onAddClickAndClear(button, event, useObj)
end

---@module UIBase.lua
---@author Rubble
---@since 2022/2/23 14:23
---@see 移除并添加Toggle事件
function UIBase:AddToggleAndClear(toggle, event, useObj)
    toggle.onValueChanged:RemoveAllListeners()
    toggle:OnAddListener()
    toggle.onValueChanged:AddListener(function(isOn)
        if useObj then
            event(useObj, isOn)
        else
            event(isOn)
        end
    end)
end

---@module UIBase.lua
---@author Rubble
---@since 2022/2/24 15:51
---@see 移除并添加触发事件
function UIBase:AddEventTriggerAndClear(go, eventType, event, useObj)
    LuaHelper.RemoveEventTrigger(go, eventType)
    LuaHelper.AddEventTrigger(go, eventType, function (baseEventData)
        if useObj then
            event(useObj, baseEventData)
        else
            event(baseEventData)
        end
    end)
end

---@module UIBase.lua
---@author Rubble
---@since 2022/2/24 15:51
---@see 移除触发事件
function UIBase:RemoveEventTrigger(go, eventType)
    LuaHelper.RemoveEventTrigger(go, eventType)
end

---@module UIBase.lua
---@author Rubble
---@since 2022/2/24 15:52
---@see 移除所有的触发事件
function UIBase:RemoveAllEventTrigger(go)
    LuaHelper.RemoveAllEventTrigger(go)
end

---@param eventName string
---@param event function
---@param useObj table
function UIBase:AddEvent(eventName, event, useObj)
    UIEvent:AddEvent(eventName, event, useObj)
end

---@param eventName string
---@param event function
---@param useObj table
function UIBase:RemoveEvent(eventName, event, useObj)
    UIEvent:RemoveEvent(eventName, event, useObj)
end

---@param eventName string
function UIBase:OnEvent(eventName, ...)
    UIEvent:OnEvent(eventName, ...)
end


function UIBase:onClose()
    LuaHelper.HideUI(UI[self.class.className])
end