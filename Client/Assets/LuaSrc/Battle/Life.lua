local t = {}

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:34
---@see 动作状态
local State = {
    None = "none",      --无状态
    Idle = "idle",      --待机
    Move = "yidong",    --移动到目标
    Attack = "attack",  --攻击目标
    Hit = "hit",        --受击
    Die = "die",        --死亡
}

local DirStep = 12
local DirAngle = -360 / DirStep

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:47
---@see 更新行为
function t:Update(tbLife)
    --死亡后不需要更新行为
    if self:HasDie() then
        return
    end
    --处于攻击中不需要更新行为
    local time = GetRealtimeSinceStartup()
    if self.attactTime > 0 and time < self.attactTime then
        return
    end

    --没有目标或目标死亡则找寻新的目标
    if nil == self.target or self.target:HasDie() then
        self.target = self:FindAttTarget(tbLife)
    else
        local toTargetDistance = self:Distance(self.target, self)
        if toTargetDistance > self.attackRadius + self.target.radius then
            self.target = self:FindAttTarget(tbLife)
        end
    end

    local state = State.Idle
    if self.target then
        --移动范围内可能碰撞到的对象
        local tb = {}
        local deltaTime = GetDeltaTime()
        local maxDistance, distance, dir = self.speed * deltaTime + self.radius
        for _, life in pairs(tbLife) do
            if life ~= self then
                distance = self:Distance(life, self)
                if distance < maxDistance + life.radius then
                    table.insert(tb, life)
                end
            end
        end

        distance, dir = self:Distance(self.target, self)
        local moveDistance, aheadDir = self.speed * deltaTime
        --前进被阻挡
        if self:IsObstructAhead(dir, moveDistance, tb) then
            --在攻击范围则展开攻击
            if self:Distance(self.target, self) < self.attackRadius + self.target.radius then
                self:Attack()
                return
            else --不在攻击范围，设法移动到目标
                moveDistance = moveDistance * 0.6
                for i = 2, DirStep do
                    dir = Quaternion.AngleAxis(DirAngle, Vector3.forward) * dir
                    if not self:IsObstructAhead(dir, moveDistance, tb) then
                        aheadDir = dir
                        break
                    end
                end
            end
        else --可以继续前进，则确定前进方向
            aheadDir = dir
        end
        --找到了前进方向则前进
        if aheadDir then
            aheadDir = aheadDir.normalized * moveDistance
            self.transform.localPosition = self.transform.localPosition + aheadDir
            state = State.Move

            if aheadDir.magnitude < moveDistance * 0.6 then
                logError("抖动了")
            end
        end

        --更新朝向
        self:UpdateDir()
    end

    self:Play(state)
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 17:24
---@see 是否阻挡前进
function t:IsObstructAhead(dir, distance, tbLife)
    local circleCenter, rectCenter, v, h, u = nil
    for _, life in ipairs(tbLife) do
        circleCenter = life.transform.localPosition
        v = circleCenter - (self.transform.localPosition + dir.normalized * distance)
        if v.magnitude < self.radius + life.radius then
            return true
        end

        rectCenter = self.transform.localPosition + dir.normalized * distance * 0.5
        v = Vector3(math.abs(circleCenter.x - rectCenter.x), math.abs(circleCenter.y - rectCenter.y))
        h = Vector3(self.radius, distance * 0.5)
        u = v - h
        u.x = math.max(u.x, 0)
        u.y = math.max(u.y, 0)
        if u.sqrMagnitude < life.radius * life.radius then
            return true
        end
    end
    return false
end

---@module Life.lua
---@author Rubble
---@since 2022/3/21 11:33
---@see 更新朝向
function t:UpdateDir()
    local localScale = self.transform.localScale
    if self.target.transform.localPosition.x > self.transform.localPosition.x then
        localScale.x = -1
    else
        localScale.x = 1
    end
    self.transform.localScale = localScale
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:34
---@see 是否死亡
function t:HasDie()
    return self.hp <= 0
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:39
---@see 计算距离
function t:Distance(lifeA, lifeB)
    local dir = lifeA.transform.localPosition - lifeB.transform.localPosition
    return dir.magnitude, dir
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:38
---@see 找寻攻击目标
function t:FindAttTarget(tbLife)
    local minDis, distance, target = math.maxinteger
    for _, v in ipairs(tbLife) do
        if v.lifeType ~= self.lifeType then
            distance = self:Distance(v, self)
            if distance < minDis then
                minDis = distance
                target = v
            end
        end
    end
    return target
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:37
---@see 进入待机状态
function t:Idle()
    self:Play(State.Idle)
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:37
---@see 攻击
function t:Attack()
    local time = GetRealtimeSinceStartup()
    if time >= self.attactTime then
        self:Play(State.Attack)

        local hurt = self.attack - self.target.defense
        hurt = math.max(hurt, 0)
        self.target.hp = self.target.hp - hurt
        self.attactTime = time + self.attackCD

        if self.target:HasDie() then
            self.target = nil
        end
    end
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:37
---@see 死亡
function t:Die()
    if not self:IsState(State.Hit) and not self:IsState(State.Die) then
        self:Play(State.Hit)
    end
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:36
---@see 是否是指定动作
function t:IsState(stateName)
    local clip = self.animator:GetCurrentAnimatorStateInfo(0)
    return clip:IsName(stateName)
end

---@module Life.lua
---@author Rubble
---@since 2022/3/19 13:36
---@see 播放动作
function t:Play(state)
    if not self:IsState(state) then
        self.animator:Play(state, 0, 0)
    end
end

return t