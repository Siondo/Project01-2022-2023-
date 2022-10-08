
local t = class("PosterUIPanel", UIBase)

local TIME = 0.5
local COUNT = 2
local OFFSET = 30
local AUTOTIME = 5

local Dir = {
    Left = 1,
    Right = 2,
}

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 10:21
---@see 初始化
function t:Init(count, auto, updateFunc, clickFunc, preFunc, aftFunc)
    self.count = count
    self.auto = auto
    self.updateFunc = updateFunc
    self.clickFunc = clickFunc
    self.preFunc = preFunc
    self.aftFunc = aftFunc
    self.dir = Dir.Right
    --初始化Content大小
    local viewPortRect = self.ui.ViewportRectTf.rect
    self.itemWidth =  self.ui.ItemRectTf.rect.width
    self.loopCount = math.round(viewPortRect.width / self.itemWidth) + COUNT
    local sizeDelta = self.ui.BgContainerRectTf.sizeDelta
    sizeDelta.x = self.itemWidth * self.count
    self.ui.BgContainerRectTf.sizeDelta = sizeDelta
    local v3 = self.ui.BgContainerRectTf.anchoredPosition
    v3.x = 0
    self.ui.BgContainerRectTf.anchoredPosition = v3
    --数据
    self.index = 1
    self.min = 1
    self.max = self.loopCount
    --初始化Content内容
    self.ui.ItemGo:SetActive(false)
    local go = self.ui.ItemGo
    self.itemGo = self.itemGo or {}
    for i = #self.itemGo + 1, self.loopCount do
        go = LuaHelper.Instantiate(self.ui.ItemGo, i, self.ui.BgContainerRectTf)
        table.insert(self.itemGo, go)
    end
    local len = #self.itemGo
    for i = len, 1, -1 do
        go = self.itemGo[i]

        if i > self.loopCount then
            table.remove(self.itemGo, i)
            LuaHelper.DestroyImmediate(go)
        else
            go.transform.localPosition = Vector3((self.itemWidth + self.ui.Const.OffsetX) * (i - 1), 0)
            go.name = i
            go:SetActive(i <= self.count)
        end
    end
    for i = 1, #self.itemGo do
        if i <= self.count and self.updateFunc then
            go = self.itemGo[i]
            self.updateFunc(go, i)
        end
    end
    --初始化Point
    if self.ui.PointContainerGo.activeSelf then
        local go, image = self.ui.BackgroundGo
        self.pointUI = self.pointUI or {}
        for i = #self.pointUI + 1, self.count do
            go = LuaHelper.Instantiate(self.ui.BackgroundGo, nil, self.ui.PointContainerRectTf)
            image = GetComponent(go, "ImageEx")
            table.insert(self.pointUI, {go = go, image = image})
        end
        local len, point = #self.pointUI
        for i = len, 1, -1 do
            point = self.pointUI[i]
            if i > self.count then
                table.remove(self.pointUI, i)
                LuaHelper.DestroyImmediate(point.go)
            else
                point.go.name = i
                point.go:SetActive(true)
                point.image.sprite = i == self.index and self.ui.CheckmarkImgEx.sprite or self.ui.BackgroundImgEx.sprite
            end
        end
    end
    --滑动事件
    self:AddEventTriggerAndClear(self.ui.ViewportGo, EventTriggerType.PointerDown, self.OnPointerDown, self)
    self:AddEventTriggerAndClear(self.ui.ViewportGo, EventTriggerType.PointerUp, self.OnPointerUp, self)
    if self.aftFunc and self.count > 0 then
        self.aftFunc(self.ui.BgContainerRectTf:Find(self.index).gameObject, self.index)
    end
end

function t:OnPointerDown(baseEventData)
    self.position = baseEventData.position
    self:AutoShowEnable(false)
end

function t:OnPointerUp(baseEventData)
    local x = baseEventData.position.x - self.position.x
    if x > OFFSET then
        if self:hasLast() then
            self:Last()
        else
            self:AutoShowEnable(true)
        end
    elseif x < -OFFSET then
        if self:hasNext() then
            self:Next()
        else
            self:AutoShowEnable(true)
        end
    else
        if self.clickFunc then
            self.clickFunc(self.index)
        end
        self:AutoShowEnable(true)
    end
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 16:24
---@see 开启自动显示
function t:AutoShowEnable(isEnable)
    if not self.auto then
        return
    end
    if isEnable then
        if self.timer then
            self.timer:Stop()
            self.timer = nil
        end
        self.timer = Timer.Create(self.AutoShow, AUTOTIME, 1, self)
        self.timer:Start()
    else
        if self.timer then
            self.timer:Stop()
            self.timer = nil
        end
    end
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 16:31
---@see 自动显示
function t:AutoShow(loop)
    if Dir.Right == self.dir and self:hasNext() then
        self:Next()
    elseif Dir.Left == self.dir and not self:hasLast() then
        self.dir = Dir.Right
        self:Next()
    else
        self.dir = Dir.Left
        self:Last()
    end
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 14:02
---@see 是否有上一个
function t:hasLast()
    return self.index > 1
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 13:59
---@see 上一个
function t:Last()
    if self.index > 1 then
        if self.pointUI then
            self.pointUI[self.index].image.sprite = self.ui.BackgroundImgEx.sprite
        end
        self.index = self.index - 1
        if self.pointUI then
            self.pointUI[self.index].image.sprite = self.ui.CheckmarkImgEx.sprite
        end

        if self.index <= self.min and self.min > 1 then
            self.min = self.min - 1
            self.max = self.max - 1

            local go = self.itemGo[#self.itemGo]
            table.remove(self.itemGo, #self.itemGo)
            table.insert(self.itemGo, 1, go)
            go.transform.localPosition = Vector3((self.itemWidth + self.ui.Const.OffsetX) * (self.min - 1), 0)
            go.name = self.min
            if self.updateFunc then
                self.updateFunc(go, self.min)
            end
        end

        if self.preFunc then
            self.preFunc(self.ui.BgContainerRectTf:Find(self.index).gameObject, self.index)
        end
        if self.tween then
            self.tween:Kill()
            self.tween = nil
        end
        local x = -(self.index - 1) * (self.itemWidth + self.ui.Const.OffsetX)
        self.tween = self.ui.BgContainerRectTf:DOAnchorPos3DX(x, TIME)
        self.tween.onComplete = function ()
            self.tween = nil
            if self.aftFunc then
                self.aftFunc(self.ui.BgContainerRectTf:Find(self.index).gameObject, self.index)
            end
        end

        self:AutoShowEnable(true)
    end
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 14:01
---@see 是否有下一个
function t:hasNext()
    return self.index < self.count
end

---@module PosterUIPanelH.lua
---@author Rubble
---@since 2022/2/24 13:59
---@see 下一个
function t:Next()
    if self.index < self.count then
        if self.pointUI then
            self.pointUI[self.index].image.sprite = self.ui.BackgroundImgEx.sprite
        end
        self.index = self.index + 1
        if self.pointUI then
            self.pointUI[self.index].image.sprite = self.ui.CheckmarkImgEx.sprite
        end

        if self.index >= self.max and self.max < self.count then
            self.min = self.min + 1
            self.max = self.max + 1

            local go = self.itemGo[1]
            table.remove(self.itemGo, 1)
            table.insert(self.itemGo, go)
            go.transform.localPosition = Vector3((self.itemWidth + self.ui.Const.OffsetX) * (self.max - 1), 0)
            go.name = self.max
            if self.updateFunc then
                self.updateFunc(go, self.max)
            end
        end

        if self.preFunc then
            self.preFunc(self.ui.BgContainerRectTf:Find(self.index).gameObject, self.index)
        end
        if self.tween then
            self.tween:Kill()
            self.tween = nil
        end
        local x = -(self.index - 1) * (self.itemWidth + self.ui.Const.OffsetX)
        self.tween = self.ui.BgContainerRectTf:DOAnchorPos3DX(x, TIME)
        self.tween.onComplete = function ()
            self.tween = nil
            if self.aftFunc then
                self.aftFunc(self.ui.BgContainerRectTf:Find(self.index).gameObject, self.index)
            end
        end

        self:AutoShowEnable(true)
    end
end

return t