
local Event = class("Event")

function Event:Ctor()
    self.list = {}
    self.calling = false
    self.clear = false
    self.addList = {}
    self.removeList = {}
end

-- 避免了重复事件的监听
function Event:AddListener(event, useObj)
    local data = {
        event = event,
        useObj = useObj,
    }
    if self.calling then
        --添加到待添加列表
        table.insert(self.addList, data)
    else
        local pos = self:GetEventIndex(event)
        if pos > -1 then
            logError("Add event repeatedly!")
        else
            --直接添加到列表
            table.insert(self.list, data)
        end
    end
end

function Event:RemoveListener(event, useObj)
    if self.calling then
        local data = {
            event = event,
            useObj = useObj,
        }
        --添加到待移除列表
        table.insert(self.removeList, data)
    else
        --直接移除
        local pos = self:GetEventIndex(event, useObj)
        if pos > -1 then
            table.remove(self.list, pos)
        end
    end
end

function Event:GetEventIndex(event, useObj)
    for i, v in ipairs(self.list) do
        if event == v.event or useObj == v.useObj then
            return i, v
        end
    end
    return -1
end

function Event:Count()
    return #self.list
end

function Event:Clear()
    if self.calling then
        self.clear = true
    else
        self.list = {}
    end
end

function Event:Call(...)
    if #self.list > 0 then
        self.calling = true
        local args = {...}

        --执行事件
        for _, v in ipairs(self.list) do
            xpcall(function ()
                if v.useObj ~= nil then
                    v.event(v.useObj, unpack(args))
                else
                    v.event(unpack(args))
                end
            end, logTrace)
        end

        --移除事件
        if #self.removeList then
            for _, v in ipairs(self.removeList) do
                local pos = self:GetEventIndex(v.event, v.useObj)
                if pos > -1 then
                    table.remove(self.list, pos)
                end
            end
            self.removeList = {}
        end

        --添加事件
        if #self.addList then
            for _, v in ipairs(self.addList) do
                local pos = self:GetEventIndex(v.event)
                if pos > -1 then
                    logError("Add event repeatedly!")
                else
                    --直接添加到列表
                    table.insert(self.list, v)
                end
            end
            self.addList = {}
        end

        --清理事件
        if self.clear then
            self.list = {}
            self.clear = false
        end
        self.calling = false
    end
end

function Event.CreateEvent()
    return Event.New()
end

-- 通用事件
EventListener = Event.CreateEvent()
-- 更新事件
UpdateEvent = EventListener.CreateEvent()
LateUpdateEvent = EventListener.CreateEvent()
FixedUpdateEvent = EventListener.CreateEvent()
CoroutineUpdateEvent = EventListener.CreateEvent()