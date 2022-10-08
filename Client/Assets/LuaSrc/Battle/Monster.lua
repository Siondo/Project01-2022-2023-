local Life = require("Battle.Life")
local t = table.easyCopy(Life)

---@module Monster.lua
---@author Rubble
---@since 2022/3/4 13:59
---@see 初始化怪物信息
function t:Create(gameObject, mapNode, id, config, safeArea, boxSize)
    self.gameObject = gameObject
    self.transform = gameObject.transform
    self.mapNode = mapNode
    self.mapNode.data = self
    self.id = id
    self.config = config
    self.safeArea = safeArea
    self.boxSize = boxSize
    self.spineRenderer = GetComponentsInChildren(gameObject, typeof(Renderer))
    self.animator = GetComponentInChildren(gameObject, typeof(Animator))

    self.hp = self.config.blood
    self.attack = self.config.attack
    self.defense = self.config.defense
    self.target = nil --攻击目标
    self.lifeType = LifeType.Monster
    self.speed = 500                    --速度
    self.attackCD = 1.2                 --攻击距离
    self.attactTime = 0                 --上一次攻击的时间
    self.radius = 30                    --盒子半径
    self.attackRadius = 50              --攻击半径
    self.stopDistance = 3               --移动停止距离

    self:Idle()
end

return t