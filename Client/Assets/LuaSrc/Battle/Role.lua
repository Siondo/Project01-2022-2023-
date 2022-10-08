local Life = require("Battle.Life")
local t = table.easyCopy(Life)

---@module Role.lua
---@author Rubble
---@since 2022/3/4 13:34
---@see 初始化角色信息
function t:Create(gameObject, mapNode, id, config, baseConfig, safeArea, boxSize)
    self.gameObject = gameObject
    self.transform = gameObject.transform
    self.mapNode = mapNode
    self.mapNode.data = self
    self.id = id
    self.config = config
    self.baseConfig = baseConfig
    self.safeArea = safeArea
    self.boxSize = boxSize
    self.spineRenderer = GetComponentsInChildren(gameObject, typeof(Renderer))
    self.animator = GetComponentInChildren(gameObject, typeof(Animator))

    self.hp = self.baseConfig.blood
    self.attack = self.baseConfig.attack
    self.defense = self.baseConfig.defense or 0
    self.target = nil --攻击目标
    self.lifeType = LifeType.Role
    self.speed = 500                    --速度
    self.attackCD = 1.2                 --攻击距离
    self.attactTime = 0                 --上一次攻击的时间
    self.radius = 30                    --盒子半径
    self.attackRadius = 63             --攻击半径
    self.stopDistance = 3               --移动停止距离

    self:Idle()
end

return t