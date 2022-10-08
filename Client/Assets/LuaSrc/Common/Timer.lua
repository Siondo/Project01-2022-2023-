
Timer = class("Timer")

-- unscaled false 采用deltaTime计时，true 采用 unscaledDeltaTime计时
function Timer.Create(func, duration, loop, unscaled, useObj)
    local timer = Timer.New()
    timer:Reset(func, duration, loop, unscaled, useObj)
    return timer
end

function Timer:Start()
    if not self.running then
        self.running = true
        UpdateEvent:AddListener(function () self:Update() end, self)
    end
end


function Timer:Reset(func, duration, loop, unscaled, useObj)
    self.func		= func
    self.duration 	= duration
    self.loop		= math.floor(loop or 1)
    self.unscaled	= unscaled == true
    self.useObj	    = useObj

    self.time		= duration
    self.running    = false
end

function Timer:Stop()
    self.running = false
    UpdateEvent:RemoveListener(nil, self)
end

function Timer:Update()
    if not self.running then
        return
    end

    local delta = self.unscaled and GetUnscaledDeltaTime() or GetDeltaTime()
    self.time = self.time - delta

    if self.time <= 0 then

        --根据指定次数结束Timer
        if self.loop ~= 999 then
            if self.useObj ~= nil then
                self.func(self.useObj, self.loop)
            else
                self.func(self.loop)
            end
    
            if self.loop > 0 then
                self.loop = self.loop - 1
            end

        --永不结束Timer, 除非手动调用
        else 
            self.func()
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.time = self.time + self.duration
        end
    end
end

--给协同使用的帧计数timer
FrameTimer = class("FrameTimer")

function FrameTimer.Create(func, duration, loop, useObj)
    local timer = FrameTimer.New()
    timer:Reset(func, duration, loop, useObj)
    return timer
end

function FrameTimer:Reset(func, duration, loop, useObj)
    self.func = func
    self.duration = duration
    self.loop = math.floor(loop or 1)
    self.useObj = useObj

    self.frameCount = GetFrameCount() + duration
    self.running = false
end

function FrameTimer:Start()
    if not self.running then
        self.running = true
        CoroutineUpdateEvent:AddListener(function () self:Update() end, self)
    end
end

function FrameTimer:Stop()
    self.running = false
    CoroutineUpdateEvent:RemoveListener(nil, self)
end

function FrameTimer:Update()
    if not self.running then
        return
    end

    if GetFrameCount() >= self.frameCount then
        if self.useObj ~= nil then
            self.func(self.useObj, self.loop)
        else
            self.func(self.loop)
        end

        if self.loop > 0 then
            self.loop = self.loop - 1
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.frameCount = GetFrameCount() + self.duration
        end
    end
end

CoroutineTimer = class("CoroutineTimer")

function CoroutineTimer.Create(func, duration, loop, useObj)
    local timer = CoroutineTimer.New()
    timer:Reset(func, duration, loop, useObj)
    return timer
end

function CoroutineTimer:Start()
    if not self.running then
        self.running = true
        CoroutineUpdateEvent:AddListener(function () self:Update() end, self)
    end
end

function CoroutineTimer:Reset(func, duration, loop, useObj)
    self.func		= func
    self.duration 	= duration
    self.loop		= math.floor(loop or 1)
    self.useObj     = useObj

    self.time		= duration
    self.running	= false
end

function CoroutineTimer:Stop()
    self.running = false
    CoroutineUpdateEvent:RemoveListener(nil, self)
end

function CoroutineTimer:Update()
    if not self.running then
        return
    end

    self.time = self.time - GetDeltaTime()
    if self.time <= 0 then
        if self.useObj ~= nil then
            self.func(self.useObj, self.loop)
        else
            self.func(self.loop)
        end

        if self.loop > 0 then
            self.loop = self.loop - 1
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.time = self.time + self.duration
        end
    end
end