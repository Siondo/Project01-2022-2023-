BattleMap = {}

function BattleMap:CreateMap(width, height)
    self.map = {}
    self.width = width
    self.height = height

    for y = 1, height do
        local row  = {}
        for x = 1, width do
            local point = {}
            point.x = x
            point.y = y
            point.g = 0
            point.h = 0
            point.f = point.g + point.h
            point.data = nil
            function point:CalcF()
                self.f = self.g + self.h
            end

            row[x] = point
        end
        self.map[y] = row
    end
end

---@module Role.lua
---@author Rubble
---@since 2022/3/1 18:46
---@see 查找结点
function BattleMap:FindNode(x, y)
    x = math.floor(x)
    y = math.floor(y)
    local row  = self.map[y]
    if row and row[x] then
        return row[x]
    end
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 15:38
---@see 查找路径
function BattleMap:Find(startNode, endNode)
    --代价清理
    local tempNode = nil
    for y = 1, self.height do
        for x = 1, self.width do
            tempNode = self.map[y][x]

            tempNode.g = 0
            tempNode.h = 0
            tempNode.f = tempNode.g + tempNode.h
            tempNode.parent = nil
        end
    end
    self.openList = {}
    self.closeList = {}

    startNode.g = 0
    startNode.h = self:CalcH(startNode, endNode)
    startNode:CalcF()
    table.insert(self.openList, startNode)
    local node, nodeList = nil
    while #self.openList > 0 do
        node = self:PopMinNodeFromOpen()
        self:PushNodeToClose(node)

        nodeList = self:FindNearbyNode(node)
        for _, v in ipairs(nodeList) do
            local g = self:CalcG(node, v)
            if self:HasNode(self.openList, v) then
                if g < v.g then
                    v.parent = node
                    v.g = g
                    v:CalcF()
                end
            else
                v.parent = node
                v.g = g
                v.h = self:CalcH(endNode, v)
                v:CalcF()
                self:PushNodeToOpen(v)
            end
        end

        if self:HasNode(self.openList, endNode) then
            return self:Get(endNode)
        end
    end
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 15:34
---@see 得到找寻到的节点列表
function BattleMap:Get(node)
    local tb = {}
    while node ~= nil do
        table.insert(tb, 1, node)
        node = node.parent
    end
    return tb
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 14:36
---@see 计算起点节点到中间节点的代价
function BattleMap:CalcG(startNode, node)
    local g = self:Distance(startNode, node)
    return g + startNode.g
end

---@module AStar.lua
---@author Rubble
---@since 2022/3/2 16:42
---@see 距离
function BattleMap:Distance(nodeA, nodeB)
    local x = math.abs(nodeA.x - nodeB.x)
    local y = math.abs(nodeA.y - nodeB.y)
    return math.sqrt(x * x + y * y)
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 14:36
---@see 计算中间节点到结束节点的代价
function BattleMap:CalcH(endNode, node)
    return self:Distance(endNode, node)
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 11:19
---@see 得到最小节点
function BattleMap:PopMinNodeFromOpen()
    if #self.openList == 0 then
        return nil
    end
    table.sort(self.openList, function (nodeA, nodeB)
        if nodeA.f ~= nodeB.f then
            return nodeA.f < nodeB.f
        else
            return false
        end
    end)
    return table.remove(self.openList, 1)
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 14:46
---@see 推入一个节点到开启列表
function BattleMap:PushNodeToOpen(node)
    table.insert(self.openList, node)
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 11:23
---@see 推入一个节点到关闭列表
function BattleMap:PushNodeToClose(node)
    self.closeList[node] = true
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 14:23
---@see 找寻附近的节点
function BattleMap:FindNearbyNode(node)
    local tb = {}
    local nearbyNode = nil
    --[[
    for x = node.x - 1, node.x + 1 do
        for y = node.y - 1, node.y + 1 do
            if not (x == node.x and y == node.y) then
                nearbyNode = self:GetNeedCheckNode(x, y)
                if nearbyNode ~= nil then
                    table.insert(tb, nearbyNode)
                end
            end
        end
    end
    --]]
    nearbyNode = self:GetNeedCheckNode(node.x, node.y + 1)
    if nearbyNode ~= nil then
        table.insert(tb, nearbyNode)
    end
    nearbyNode = self:GetNeedCheckNode(node.x, node.y - 1)
    if nearbyNode ~= nil then
        table.insert(tb, nearbyNode)
    end
    nearbyNode = self:GetNeedCheckNode(node.x - 1, node.y)
    if nearbyNode ~= nil then
        table.insert(tb, nearbyNode)
    end
    nearbyNode = self:GetNeedCheckNode(node.x + 1, node.y)
    if nearbyNode ~= nil then
        table.insert(tb, nearbyNode)
    end

    return tb
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 11:28
---@see 得到需要检测的节点
function BattleMap:GetNeedCheckNode(x, y)
    local node = self:FindNode(x, y)
    if nil == node then
        return node
    end

    if self.closeList[node] then
        return nil
    end

    if node.data == nil then
        return node
    end
end

---@module Role.lua
---@author Rubble
---@since 2022/3/2 11:32
---@see 列表里是否有这个节点
function BattleMap:HasNode(list, node)
    for _, v in ipairs(list) do
        if v == node then
            return true
        end
    end
    return false
end