require("Battle.BattleMap")
local Role = require("Battle.Role")
local Monster = require("Battle.Monster")

local t = {}

local randomSeed = 0       --随机种子
local birthAreaHeight = 4  --出生区域高度
local birthAreaMargin = 1  --出生区域边距(宽度使用)
local mapBoxSize = 50      --地图格子大小
local mapBoxWidth = 14     --地图宽度格子数
local mapBoxHeight = 30    --地图高度格子数
local mapSafeHeight = 6    --安全高度

function t:GetInfo()
    return mapBoxWidth, mapBoxHeight, mapBoxSize
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 10:46
---@see 初始化战斗
function t:Init(battleArea, friendsArea, enemyArea, sortingOrder)
    --战斗场地
    self.battleArea = battleArea
    self.friendsArea = friendsArea
    self.enemyArea = enemyArea
    --起始层级
    self.sortingOrder = sortingOrder
    --战斗区域大小
    self.areaWidth = self.battleArea.rect.width
    self.areaHeight = self.battleArea.rect.height
    --计算地图格子大小，数量
    mapBoxSize = self.areaWidth / mapBoxWidth
    mapBoxHeight = math.ceil(self.areaHeight / mapBoxSize)

    --初始化地图
    BattleMap:CreateMap(mapBoxWidth, mapBoxHeight)
    log("mapBoxWidth == "..mapBoxWidth..", mapBoxHeight == "..mapBoxHeight..", mapBoxSize == "..mapBoxSize)
    log("battleArea.rect.width == "..self.battleArea.rect.width..", battleArea.rect.height == "..self.battleArea.rect.height)

    --存储战斗的生命对象
    self.tbLife = {}

    --存储可使用的随机角色位置
    self.tbRolePos = {}
    for y = 1, birthAreaHeight do
        for x = birthAreaMargin + 1, mapBoxWidth - birthAreaMargin do
            table.insert(self.tbRolePos, (y - 1) * mapBoxWidth + x)
        end
    end
    --存储可使用的随机怪物位置
    self.tbMonsterPos = {}
    for y = mapBoxHeight - birthAreaHeight + 1, mapBoxHeight do
        for x = birthAreaMargin + 1, mapBoxWidth - birthAreaMargin do
            table.insert(self.tbMonsterPos, (y - 1) * mapBoxWidth + x)
        end
    end
    --角色和怪物的数量
    self.roleNumber = 0
    self.monsterNumber = 0
    --初始战斗结果
    self.result = BattleResult.Unknown
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 18:19
---@see 得到随机角色位置
function t:GetRandomRolePosition()
    local posIndex = 0
    if self.roleNumber == 1 then
        posIndex = math.ceil(#self.tbRolePos / 2)
    else
        math.randomseed(randomSeed..tostring(socket.gettime()):reverse():sub(1, 6))
        posIndex = math.random(1, #self.tbRolePos)
    end
    posIndex = table.remove(self.tbRolePos, posIndex)
    randomSeed = posIndex

    local x = posIndex % mapBoxWidth
    local y = math.ceil(posIndex / mapBoxWidth)

    x = x - 0.5
    y = y - 0.5

    x = x * mapBoxSize - self.battleArea.rect.width * 0.5
    y = y * mapBoxSize - self.battleArea.rect.height * 0.5
    return Vector3(x, y, 0)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/4 10:47
---@see 得到随机怪物位置
function t:GetRandomMonsterPosition()
    local posIndex = 0
    if self.monsterNumber == 1 then
        posIndex = math.ceil(#self.tbMonsterPos / 2)
    else
        math.randomseed(randomSeed..tostring(socket.gettime()):reverse():sub(1, 6))
        posIndex = math.random(1, #self.tbMonsterPos)
    end
    posIndex = table.remove(self.tbMonsterPos, posIndex)
    randomSeed = posIndex

    local x = posIndex % mapBoxWidth
    local y = math.ceil(posIndex / mapBoxWidth)

    x = x - 0.5
    y = y - 0.5

    x = x * mapBoxSize - self.battleArea.rect.width * 0.5
    y = y * mapBoxSize - self.battleArea.rect.height * 0.5
    return Vector3(x, y, 0)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 10:50
---@see 角色进入战场
function t:AddRole(go, config)
    self.roleNumber = self.roleNumber + 1
    LuaHelper.ChangeLayer(go, Layers.UI)
    local localPosition = self:GetRandomRolePosition()
    go.transform.localPosition = localPosition

    local baseConfig = Config.tbQualityLevel[tostring(PlayerData:getBaseAttLv())]
    local role = class("Role", Role)
    role:Create(go, self:GetMapNode(localPosition), #self.tbLife, config, baseConfig, mapSafeHeight, mapBoxSize)
    table.insert(self.tbLife, role)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 11:03
---@see 角色全部进入战场完成
function t:AddRoleComplete()
    self:UpdateOrderInLayer()
    UIEvent:OnEvent(UIEvent.UIBattle.LifeNumber, self.roleNumber, self.monsterNumber)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 11:03
---@see 怪物进入战场
function t:AddMonster(go, config, doorPosition)
    self.monsterNumber = self.monsterNumber + 1
    LuaHelper.ChangeLayer(go, Layers.UI)
    local localPosition = self:GetRandomMonsterPosition()
    go.transform.localScale = Vector3.zero
    go.transform.localPosition = doorPosition
    go.transform:DOScale(Vector3.one, 1)
    go.transform:DOLocalMove(localPosition, 1)

    local monster = class("Monster", Monster)
    monster:Create(go, self:GetMapNode(localPosition), #self.tbLife, config, mapBoxHeight - mapSafeHeight + 1, mapBoxSize)
    table.insert(self.tbLife, monster)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 11:04
---@see 怪物全部进入战场完成
function t:AddMonsterComplete()
    self:UpdateOrderInLayer()
    UIEvent:OnEvent(UIEvent.UIBattle.LifeNumber, self.roleNumber, self.monsterNumber)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 16:55
---@see Boss进入战场
function t:AddBoss(go, config, doorPosition)
    self.monsterNumber = self.monsterNumber + 1
    LuaHelper.ChangeLayer(go, Layers.UI)
    local localPosition = self:GetRandomMonsterPosition()
    go.transform.localScale = Vector3.zero
    go.transform.localPosition = doorPosition
    go.transform:DOScale(Vector3.one, 1)
    go.transform:DOLocalMove(localPosition, 1)

    local monster = class("Monster", Monster)
    monster:Create(go, self:GetMapNode(localPosition), #self.tbLife, config, mapBoxHeight - mapSafeHeight + 1, mapBoxSize)
    table.insert(self.tbLife, monster)

end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 16:56
---@see Boss全部进入战场完成
function t:AddBossComplete()
    self:UpdateOrderInLayer()
    UIEvent:OnEvent(UIEvent.UIBattle.LifeNumber, self.roleNumber, self.monsterNumber)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 11:15
---@see 更新渲染顺序
function t:UpdateOrderInLayer()
    table.sort(self.tbLife, function (dataA, dataB)
        if dataA.transform.localPosition.y ~= dataB.transform.localPosition.y then
            return dataA.transform.localPosition.y > dataB.transform.localPosition.y
        else
            return dataA.id < dataB.id
        end
    end)
    for sortingOrder, life in ipairs(self.tbLife) do
        for i = 0, life.spineRenderer.Length - 1 do
            life.spineRenderer[i].sortingOrder = self.sortingOrder + sortingOrder
        end
    end
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/19 15:39
---@see 准备战斗
function t:ReadyBattle()
    coroutine.start(function ()
        while true do
            --战斗结束鉴定
            if self.result ~= BattleResult.Unknown then
                break
            end

            --更新渲染顺序
            self:UpdateOrderInLayer()

            --等待下一帧
            coroutine.step(1)
        end
    end)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 11:02
---@see 开始战斗
function t:StartBattle()
    coroutine.start(function ()
        while true do
            --战斗结束鉴定
            if self.roleNumber == 0 or self.monsterNumber == 0 then
                coroutine.wait(1)
                self:BattleEnd()
                break
            end
            --战斗
            for _, life in ipairs(self.tbLife) do
                life:Update(self.tbLife)
            end
            --死亡
            self:CheckDie()

            --等待下一帧
            coroutine.step(1)
        end
    end)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/4 17:52
---@see 检测死亡
function t:CheckDie()
    local n, tbDie, lifeType, hasDie = #self.tbLife, {}
    for i = n, 1, -1 do
        if self.tbLife[i]:HasDie() then
            lifeType = self.tbLife[i].lifeType
            if lifeType == LifeType.Monster then
                self.monsterNumber = self.monsterNumber - 1
            elseif lifeType == LifeType.Role then
                self.roleNumber = self.roleNumber - 1
            end

            self.tbLife[i]:Die()
            table.insert(tbDie, self.tbLife[i])
            table.remove(self.tbLife, i)
            hasDie = true
        end
    end
    local timer = Timer.Create(function ()
        for _, life in ipairs(tbDie) do
            LuaHelper.UnloadToPool(life.gameObject)
        end
    end, 1.33, 1)
    timer:Start()

    if hasDie then
        UIEvent:OnEvent(UIEvent.UIBattle.LifeNumber, self.roleNumber, self.monsterNumber)
    end
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/5 17:28
---@see 战斗结束
function t:BattleEnd()
    for _, life in ipairs(self.tbLife) do
        life:Idle()
    end

    self.result = BattleResult.Win
    if self.monsterNumber > 0 then
        self.result = BattleResult.Fail
    end
    UIEvent:OnEvent(UIEvent.UIBattle.BattleEnd, self.result, self.roleNumber)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/3 16:47
---@see 退出战场
function t:ExitBattle()
    for _, life in ipairs(self.tbLife) do
        LuaHelper.UnloadToPool(life.gameObject)
    end
    self.tbLife = {}
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/4 11:08
---@see 得到当前位置所在的地图块
function t:GetMapNode(localPosition)
    local x = localPosition.x + self.battleArea.rect.width * 0.5
    local y = localPosition.y + self.battleArea.rect.height * 0.5
    x = math.ceil(x / mapBoxSize)
    y = math.ceil(y / mapBoxSize)

    return BattleMap:FindNode(x, y)
end

---@module Battle.lua
---@author Rubble
---@since 2022/3/4 11:20
---@see 根据地图节点得到位置
function t:GetPositionByMapNode(x, y)
    x = x - 0.5
    y = y - 0.5

    x = x * mapBoxSize - self.battleArea.rect.width * 0.5
    y = y * mapBoxSize - self.battleArea.rect.height * 0.5
    return Vector3(x, y, 0)
end

Battle = t