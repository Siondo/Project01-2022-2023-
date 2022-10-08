
function GetRef(go)
    if IsNotNull(go) then
        return go:GetComponent("UIReference")
    end
end

function GetRefTable(go)
    if IsNotNull(go) then
        return go:GetComponent("UIReference"):GetReferenceTable()
    end
end

function GetLuaScript(go)
    if IsNotNull(go) then
        return go:GetComponent("UIToLua"):GetScriptTable()
    end
end

function GetComponent(go, name)
    if IsNotNull(go) and string.IsNotNullOrEmpty(name) then
        return go:GetComponent(name)
    end
end

---@module Util.lua
---@author Rubble
---@since 2022/3/3 10:59
---@see 得到组件包括子对象
function GetComponentsInChildren(go, type)
    if IsNotNull(go) and nil ~= type then
        return go:GetComponentsInChildren(type)
    end
end

---@module Util.lua
---@author Rubble
---@since 2022/3/4 14:45
---@see 得到组件包括子对象中找寻
function GetComponentInChildren(go, type)
    if IsNotNull(go) and nil ~= type then
        return go:GetComponentInChildren(type)
    end
end

function GetRectTransform(go)
    if IsNotNull(go) then
        return go:GetComponent(typeof(RectTransform))
    end
end

function AddComponent(go, name)
    if IsNotNull(go) and string.IsNotNullOrEmpty(name) then
        return go:AddComponent(name)
    end
end

function TryAddComponent(go, type)
    local component = nil
    if IsNotNull(go) and nil ~= type then
        component = go:GetComponent(type)
        if IsNull(component) then
            component = go:AddComponent(type)
        end
        return component
    end
end

function SetHeadSprite(image)
    local sprite = PlayerData:getUserHeadSprite()
    if IsNotNull(sprite) then
        image.sprite = sprite
    else
        LuaHelper.GetUrlSprite(PlayerData:getUserHeadAddress(), function (result, sprite)
            if string.IsNullOrEmpty(result) then
                PlayerData:setUserHeadSprite(sprite)
                image.sprite = sprite
            end
        end)
    end
end

function GetGlobal(key)
    local v = Config.tbGlobal[key]
    return v and v.value or string.Empty
end

local curGUID = 0
--得到默认的GUID
function DefaultGUID()
    return 0
end

--得到一个新的GUID
function NewGUID()
    curGUID = curGUID + 1
    return curGUID
end

---@module Util.lua
---@author Rubble
---@since 2022/4/22 15:07
---@see 得到时间表
function GetTimeTable(time)
    local m = math.floor(time / 60)
    local s = time % 60
    return m, s
end

---@module Util.lua
---@author Rubble
---@since 2022/4/22 15:04
---@see 得到时间字符串 0分0秒
function GetTimeString(time)
    return UIText(100222, GetTimeTable(time))
end