
local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield

local cmap = {}
setmetatable(cmap, { __mode = "kv" })

function coroutine.start(f, ...)
    local args = {...}
    local c = create(f)

    if running() == nil then
        local flag, errorMsg = resume(c, unpack(args))
        if not flag then
            logTrace(errorMsg)
        end
    else
        local timer = nil

        local action = function()
            cmap[c] = nil
            timer:Stop()
            timer.func = nil

            local flag, errorMsg = resume(c, unpack(args))
            if not flag then
                logTrace(errorMsg)
            end
        end

        timer = FrameTimer.Create(action, 0, 1)
        cmap[c] = timer
        timer:Start()
    end

    return c
end

function coroutine.wait(t, c, ...)
    local args = {...}
    c = c or running()
    local timer = nil

    local action = function()
        cmap[c] = nil
        timer:Stop()
        timer.func = nil

        local flag, errorMsg = resume(c, unpack(args))
        if not flag then
            logTrace(errorMsg)
        end
    end

    timer = CoroutineTimer.Create(action, t or 1, 1)
    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.step(t, c, ...)
    local args = {...}
    c = c or running()
    local timer = nil

    local action = function()
        cmap[c] = nil
        timer:Stop()
        timer.func = nil

        local flag, errorMsg = resume(c, unpack(args))
        if not flag then
            logTrace(errorMsg)
        end
    end

    timer = FrameTimer.Create(action, t or 1, 1)
    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.www(www, c)
    c = c or running()
    local timer = nil

    local action = function()
        if not www.isDone then
            return
        end

        cmap[c] = nil
        timer:Stop()
        timer.func = nil

        local flag, errorMsg = resume(c)
        if not flag then
            logTrace(errorMsg)
        end
    end

    timer = FrameTimer.Create(action, 1, -1)
    cmap[c] = timer
    timer:Start()
    return yield()
end

function coroutine.stop(c)
    local timer = cmap[c]
    if timer ~= nil then
        cmap[c] = nil
        timer:Stop()
    end
end

-- 停止所有协程
function coroutine.stopAll()
    for i, v in pairs(cmap) do
        cmap[i] = nil
        v:Stop()
    end
end