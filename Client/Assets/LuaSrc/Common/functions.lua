
function string.Split(s, delim)
    local start, delimLength = 1, string.len (delim)
    local t = {}
    while true do
        local pos = string.find (s, delim, start, true) -- plain find
        if not pos then
            break
        end

        table.insert (t, string.sub (s, start, pos - 1))
        start = pos + delimLength
    end
    table.insert (t, string.sub (s, start))

    return t
end

function table.Remove(list, item)
    for i, v in ipairs(list) do
        if v == item then
            table.remove(list, i)
            break
        end
    end
end

function table.len(tb)
    local len = 0
    for k, v in pairs(tb) do
        len = len + 1
    end
    return len
end

function table.easyCopy(tb)
    local t = {}
    for k, v in pairs(tb) do
        t[k] = v
    end
    return t
end

function math.clamp(value, minValue, maxValue)
    minValue = minValue or 0
    maxValue = maxValue or 1
    if value < minValue then
        return minValue
    end
    if value > maxValue then
        return maxValue
    end
    return value
end

---@module functions.lua
---@author Rubble
---@since 2022/2/24 13:57
---@see 四舍五入
function math.round(value)
    value = value + 0.5
    return math.floor(value)
end

function PathCombine(path1, path2)
    return Path.Combine(path1, path2)
end


function IsNull(obj)
    return nil == obj or obj:Equals(nil)
end

function IsNotNull(obj)
    return not IsNull(obj)
end

---@module functions.lua
---@author Rubble
---@since 2022/3/9 13:43
---@see 异常处理
function try(func, catch)
    local flag, traceMsg = true, nil
    xpcall(func, function (args)
        flag = false
        traceMsg = args
        if catch then
            catch(traceMsg)
        else
            traceMsg = traceMsg.."\n"..debug.traceback()
            logError(traceMsg)
        end
    end)
    return flag, traceMsg
end


--[[
    @desc: 字符串是否为空
    --@str: 字符串
    @return: 真假
]]
function IsEmptyStringOrNull(str)
    if str == '' or str == nil then
        return true
    end

    return false
end