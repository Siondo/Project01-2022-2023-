--[[
Description: 匹配游戏管理器
Author: xinZhao
Date: 2022-03-31 17:22:54
LastEditTime: 2022-04-01 10:28:58
--]]
MatchManager = class('MatchManager')

local NORMAL_STAR_COUNT = 5
local COLLECTION_STAR_COUNT = 100

local matchGameBundlePath = 'Res/Model/MatchGame.prefab'
local matchGameElementPath = 'Res/Model/'

function MatchManager:onInit()
    UpdateEvent:AddListener(self.OnUpdate, self)
    LuaHelper.LoadAsset(matchGameBundlePath, function(status, asset)
        if status then
            self.matchRoot = GameObject.Instantiate(asset.mainAsset).transform
            self.matchRoot.name = 'MatchGame'
            self.matchMap = self.matchRoot:Find('MatchMap')
            self.matchCamera = self.matchRoot:Find('MatchCamera'):GetComponent(typeof(Camera))
            self.uiCamera = GameObject.Find('UIManager/UICamera'):GetComponent(typeof(Camera))
            self.uiRectTransform = GameObject.Find('UIManager/UIRoot'):GetComponent(typeof(RectTransform))

            self.valvaDoor = self.matchRoot:Find('MatchMap/Content/ValvaDoor').gameObject
            self.blockWallUp = self.matchRoot:Find('MatchMap/Content/BlockWallTop').gameObject
            self.blockWallDown = self.matchRoot:Find('MatchMap/Content/BlockWallDown').gameObject
            log('匹配游戏初始化完成')
        end
    end)
end


--[[
    @desc: 开启三消游戏
    time:2022-04-12 17:01:16
    --@level: 目标关卡
]]
function MatchManager:onStartMatchGame(endTime, useItemId, levelID, roomId)
    LuaHelper.ClearUI(false)
    LuaHelper.ShowUI(UI.AppSetupView, function()
        AudioManager:onPlayGameBGM(BgmType.Map)

        self.valvaDoor:SetActive(false)
        self.blockWallUp:SetActive(true)
        self.blockWallDown:SetActive(true)

        self.isStart = true             --游戏是否开始

        --玩法字段
        self.isResurgence = false       --是否通过金币复活
        self.starCount = 0              --星星获得数量
        self.starStatus = 0             --非任务元素点击错误次数 (-1:错误点击不发奖励 [0-2]:普通点击无额外奖励 [3+]:连续点击奖励翻倍)
        self.pveEndTime = endTime       --PVE结束时间
        self.uiElementCompleteCount = 0 --任务元素完成个数
        self.levelID = levelID          --关卡ID
        self.roomId = roomId            --房间ID

        --加载字段
        self.elementLoadSize = 0  --地图生成元素总个数
        self.elementAllSize = 0   --地图元素总个数
        self.uiElement = {}       --UI界面收集元素
        self.mapElement = {}      --地图生成元素
        self.useMagentItem = {isUsed = false, eid = 0}  --是否使用过磁铁
        self.usePVPItem = {}                            --PVP使用过的道具
        self:onHandlerUsedItems(useItemId)


        local config
        if MatchProtoRequest.BattleType == BattleType.PVE then
            self.settlementPart = 'Part2'
            config = Config.tbLevelTable[tostring(self.levelID)]
        elseif MatchProtoRequest.BattleType == BattleType.PVP or MatchProtoRequest.BattleType == BattleType.FRIEND then
            self.settlementPart = 'Part1'
            config = Config.tbPVPLevel[tostring(self.levelID)]
        end
        
        if config then
            self.timeRemaining = self.pveEndTime - os.time()  --剩余时间
            self.timeMax = config.LevelTime                   --总时间
            self:onFormattedElementConfig(config.OtherElementList, false)
            self:onFormattedElementConfig(config.TaskElementList, true)
            self.isLoading = true
            self.elementAllSize = self.elementLoadSize
            self:onCreateMapElement()
        else
            logError('服务器传递关卡ID:'..levelID..'在配置表 (tbLevelTable/tbPVPLevel) 中查询不到结构')
        end
    end)
    UIEvent:OnEvent(UIEvent.AppSetupView.UpdateTips, '正在加载地图资源, 请稍等...', '')
end


--[[
    @desc: 处理使用过的道具
    time:2022-05-11 14:00:33
    --@useItemId: 战斗前选择的道具
]]
function MatchManager:onHandlerUsedItems(useItemId)
    for i = 1, #useItemId do
        if MatchProtoRequest.BattleType == BattleType.PVE then
            if ItemType.MAGENT == useItemId[i].id then
                self.useMagentItem.isUsed = true
            end
        elseif MatchProtoRequest.BattleType == BattleType.PVP or MatchProtoRequest.BattleType == BattleType.FRIEND then
            self.usePVPItem[useItemId[i].id] = {
                existCount = useItemId[i].num
            }
        end

        ItemData:onSubItem(useItemId[i].id, useItemId[i].num)
    end
end


--[[
    @desc: 格式化元素表
    time:2022-04-12 16:55:46
]]
function MatchManager:onFormattedElementConfig(table, isTaskElement, callBack)
    for i = 1, #table do
        local eid = table[i][1]
        local num = table[i][2]
        if self.mapElement[eid] then
            logError('地图元素中出现了相同的元素ID(%d), 联系策划', eid)
            return
        end

        --地图元素
        self.mapElement[eid] = { 
            num = num,
            config = Config.tbElement[tostring(eid)],
            isTaskElement = isTaskElement,
        }
        self.elementLoadSize = self.elementLoadSize + num

        --任务元素
        if isTaskElement then
            if self.useMagentItem.isUsed then
                self.useMagentItem.eid = eid
                self.useMagentItem.isUsed = false
            end

            self.uiElement[#self.uiElement + 1] = {
                eid = eid,
                exist = 0,
                maxNum = num,
            }
        end
    end
end


--[[
    @desc: 生成地图元素
    time:2022-04-12 17:25:27
]]
function MatchManager:onCreateMapElement()
    for eid, data in pairs(self.mapElement) do
        if data.config and data.config.BundleName then
            local elementBundlePath = matchGameElementPath..data.config.BundleName..'.prefab'
            LuaHelper.LoadAsset(elementBundlePath, function(status, asset)
                if status then
                    coroutine.start(function ()
                        for i = 1, data.num do      
                            local isCreated = false         
                            
                            local model = GameObject.Instantiate(asset.mainAsset, self.matchMap).transform
                            local rigidBody = model:GetComponent(typeof(typeof(Rigidbody)))
                            rigidBody.isKinematic = true
                            model.transform.localScale = Vector3(data.config.ScaleX, data.config.ScaleY, data.config.ScaleZ)
                            model.transform.localPosition = Vector3(0, 4.582, 1.44)
    
                            if not self.mapElement[eid].models then
                                self.mapElement[eid].models = {}
                            end
                            self.mapElement[eid].models[model.name..i] = model.gameObject
                            self.mapElement[eid].orgScale = model.transform.localScale
                            model.name = eid..'-Model:'..model.name..i
    
                            --物理引擎配置表化
                            rigidBody.isKinematic = false
                            rigidBody.drag = data.config.Drag or 0.25
                            rigidBody.mass = data.config.Mass or 1
                            rigidBody.angularDrag = data.config.AngularDrag or 0.05
                            local bornForce = Vector3(0, data.config.BornForce or -5500, 0)
                            rigidBody:AddForce(bornForce)
                            self.elementLoadSize = self.elementLoadSize - 1

                            local curSize = (self.elementAllSize - self.elementLoadSize)
                            UIEvent:OnEvent(UIEvent.AppSetupView.UpdateProgress, curSize, self.elementAllSize, nil, true)
                            UITool:onDelay(0.2, function()
                                isCreated = true
                            end)

                            while not isCreated do
                                coroutine.step(1)
                            end
                        end
                    end)
                else
                    logError(elementBundlePath..' 未在该文件夹中 找到该模型')
                end
            end, true)
        else
            logError('无法在Config.tbElement内找到ID(%d)元素', eid)
        end
    end
end


--[[
    @desc: 每帧执行(包含: 元素加载完成后打开主玩法界面 ...等等等)
    time:2022-04-14 18:19:05
]]
function MatchManager:OnUpdate()
    if self.isLoading and self.elementLoadSize <= 0 then
        self.isLoading = false

        UITool:onDelay(0.25, function()
            if self.levelID == (LEVEL_ID_CONST + 1) then
                AudioManager.PlaySound(AudioManager.Audio.Battle_Start_1)
            else
                AudioManager.PlaySound(AudioManager.Audio.Battle_Start)
            end
    
            LuaHelper.ShowUI(UI.MatchMainView)
            self:onStartManagerTimer()
    
    
            --如果使用过磁铁 开始游戏1.2s 自动收集物品
            UITool:onDelay(1.2, function()
                if self.useMagentItem.eid ~= 0 then
                    if self.mapElement[self.useMagentItem.eid] then
                        for modelId, _ in pairs(self.mapElement[self.useMagentItem.eid].models) do
                            MatchItem:onCollectElement(self.useMagentItem.eid, modelId)
                            self.useMagentItem.eid = 0
                            return
                        end
                    end
                end
            end)
        end)
    end
end


--[[
    @desc: 开启倒计时
    time:2022-04-21 15:19:58
]]
function MatchManager:onStartManagerTimer()
    self:onStopManagerTimer()
    self.timer = Timer.Create(function()
        self.timeRemaining = self.timeRemaining - 1

        --倒计时结束, 暂停Timer
        if self.timeRemaining <= 0 then
            self:onStopManagerTimer()
            if MatchProtoRequest.BattleType == BattleType.PVE then
                LuaHelper.ShowUI(UI.MatchTimeUpView)
            elseif MatchProtoRequest.BattleType == BattleType.PVP or MatchProtoRequest.BattleType == BattleType.FRIEND then
                self:onEnterSettlementView(false)
            end
        end
        UIEvent:OnEvent(UIEvent.MatchMainView.SetGameOverCountDown, UITool:timeString(self.timeRemaining))
    end, 1, 999)
    self.timer:Start()
end


function MatchManager:onEnterSettlementView(isWin)
    LuaHelper.ShowUI(UI.MatchSettlementView, {
        type = self.settlementPart,
        starCount = self.starCount,
        remainingTime = self.timeRemaining,
        maxTime = self.timeMax,
        isResurgence = self.isResurgence,
        win = isWin,
    })
end

-----------------------------------------------------------------
---------------------以下是管理器外部接口--------------------------
-----------------------------------------------------------------

--[[
    @desc: 计算本次点击获得星星数量
    time:2022-04-21 15:51:24
    --@isUIElement: 是否是任务元素
]]
function MatchManager:onCalculateStarCount(isUIElement, isCompleteCollection)
    if isUIElement then
        if self.starStatus == -1 then
            self.starStatus = 0
        end
        self.starStatus = self.starStatus + 1
    else
        self.starStatus = -1
    end

    --普通点击无额外奖励
    if self.starStatus >= 0 and self.starStatus <= 2 then
        --logWarning('普通点击')
        self.starCount = self.starCount + NORMAL_STAR_COUNT
        --完成收集奖励再额外奖励
        if isCompleteCollection then
            --logWarning('普通点击收集完毕, 再送额外奖励')
            self.starCount = self.starCount + COLLECTION_STAR_COUNT
            self.uiElementCompleteCount = self.uiElementCompleteCount + 1
        end

    --连续点击, 奖励翻倍
    elseif self.starStatus > 2 then
        --log('连续点击')
        self.starCount = self.starCount + (NORMAL_STAR_COUNT * 2)
        --完成收集再额外奖励
        if isCompleteCollection then
            --log('连续点击收集完毕 再送额外奖励')
            self.starCount = self.starCount + math.floor(COLLECTION_STAR_COUNT * 1.2)
            self.uiElementCompleteCount = self.uiElementCompleteCount + 1
        end

    elseif self.starStatus == -1 then
        --logError('错误点击')
    end

    --收集完毕暂停倒计时, 等待动画播放完毕进入结算界面
    if self.uiElementCompleteCount >= #self.uiElement then
        --log('所有元素收集完毕, 进入结算界面')
        self:onEnterSettlementView(true)
        self:onStopManagerTimer()
    end

    return self.starCount
end


--[[
    @desc: 增加倒计时时间
    time:2022-04-22 09:47:42
    --@timeCount: 时间(s)
]]
function MatchManager:onAddManagerTime(timeCount)
    self.pveEndTime = os.time() + timeCount
end


--[[
    @desc: 开关倒计时
    time:2022-04-26 15:08:02
]]
function MatchManager:onSetTimerStatus(status)
    if self.timer then
        if status then
            self.timer:Start()
        else
            self.timer:Stop()
        end
    end
end


--[[
    @desc: 关闭倒计时
    time:2022-04-21 15:20:06
]]
function MatchManager:onStopManagerTimer()
    if self.timer then
        self.timer:Stop()
        self.timer = nil
    end
end


--[[
    @desc: 获取元素飞行的UI下标
    time:2022-04-14 18:02:29
    --@eid: 元素Id
]]
function MatchManager:onGetUISlotIndex(eid)
    for i = 1, #self.uiElement do
        if self.uiElement[i].eid == eid then
            return i
        end
    end
end


--[[
    @desc: UI元素递增1
    time:2022-04-18 18:12:37
    --@index: 元素表中的对应元素下表
]]
function MatchManager:onAddUIElement(index)
    self.uiElement[index].exist = self.uiElement[index].exist + 1
end

-----------------------------------------------------------------
---------------------以下是处理管理器销毁逻辑----------------------
-----------------------------------------------------------------


--[[
    @desc: 关闭三消游戏
    time:2022-04-12 17:49:48
]]
function MatchManager:onOffMatchGame(isClose)
    for eid, data in pairs(self.mapElement) do
        for _, obj in pairs(data.models) do
            GameObject.Destroy(obj.gameObject)
        end
    end

    if isClose then
        --log('执行关闭')
        self.isStart = false
        LuaHelper.ClearUI(false)
        LuaHelper.ShowUI(UI.MainView, MAINTAB_INDEX.MAIN)
        self:onStopManagerTimer()
        AudioManager:onPlayGameBGM(BgmType.Gmae)
    else
        --log('执行刷新')
        NetWork:onRequest(UIProtoType.Fight.protoID, {chapterId = PlayerData:getLevelId(), useItem = {}}, function(reslut, data)
            if reslut then
                MatchManager:onStartMatchGame(data.endTime, {}, PlayerData:getLevelId())
            end
        end) 
    end
end


-----------------------------------------------------------------
---------------------以下是使用道具的外部接口----------------------
-----------------------------------------------------------------


--[[
    @desc: 取随机数
    time:2022-04-27 10:55:31
    --@min: 最小
	--@max: 最大
]]
local function onRandom(min, max)
    math.randomseed(os.clock() * 1000000)
    return math.random(min, max)
end


--[[
    @desc: 查询剩余的任务元素个数
    time:2022-04-27 10:54:45
]]
local function onCheckRemainElementCount()
    local count = 0
    for i = 1, #MatchManager.uiElement do
        local uiElement = MatchManager.uiElement[i]
        count = count + (uiElement.maxNum - uiElement.exist)
    end
    return count
end


--[[
    @desc: 查询剩余的任务元素模型个数
    time:2022-04-27 10:54:59
    --@models: 模型组
]]
local function onCheckRemainModelCount(models)
    local count = 0
    for _, obj in pairs(models) do
        count = count + 1
    end
    return count
end


--[[
    @desc: 重置所有地图元素位置
    time:2022-04-27 10:55:16
]]
function MatchManager:onResetAllElement()
    self.valvaDoor:SetActive(true)
    self.blockWallUp:SetActive(false)
    self.blockWallDown:SetActive(false)

    for eid, data in pairs(self.mapElement) do
        for _, obj in pairs(data.models) do
            local rigidBody = obj:GetComponent(typeof(typeof(Rigidbody)))
            local position = obj.transform.localPosition
            local convertForce = data.config.ConvertForce or 800
            if position.y < 0.18 then
                rigidBody:AddForce(Vector3(0, convertForce, onRandom(1, 10) * 8))
            else
                rigidBody:AddForce(Vector3(0, convertForce - 600, onRandom(-10, -1) * 8))
            end
        end
    end
end


--[[
    @desc: 显示任务元素
    time:2022-04-26 18:48:49
    --@count: 显示个数
]]
function MatchManager:onShowTaskElement(count, findObj, times)
    times = times or 1
    if times >= 60 then
        return
    end

    if count <= 0 then return end
    local remainCount = onCheckRemainElementCount()
    if remainCount < count then
        UITool:onShowTips(UIText('ui_tip_00010'))
        return
    end

    findObj = findObj or {}
    local randomElement = onRandom(1, #self.uiElement)
    local uiElement, elementId = self.uiElement[randomElement], nil
    if uiElement.exist >= uiElement.maxNum then
        self:onShowTaskElement(count, findObj)
        return
    end

    elementId = uiElement.eid
    --log('eid = '..elementId)

    for eid, data in pairs(self.mapElement) do
        if eid == elementId then
            local remainModelCount = onCheckRemainModelCount(data.models)
            local randomModel, index = onRandom(1, remainModelCount), 1

            for _, obj in pairs(data.models) do
                if index == randomModel then
                    local exist = false
                    for i = 1, #findObj do
                        if findObj[i].name == obj.name then
                            exist = true
                            times = times + 1
                            self:onShowTaskElement(count, findObj)
                            return
                        end
                    end

                    local oldScale = data.orgScale.x  --obj.transform.localScale.x
                    GameTween.DOScale(obj.transform, (oldScale * 2), 0.5):SetDelay(0.2):SetAutoKill(true)
                    count = count - 1
                    findObj[#findObj + 1] = obj
                    times = times + 1
                    self:onShowTaskElement(count, findObj)
                    return
                end
                index = index + 1
            end
        end
    end
end


--[[
    @desc: 时间暂停
    time:2022-04-27 11:01:16
    --@time: 停止时间
]]
function MatchManager:onStopCountdownByTime(time)
    self:onSetTimerStatus(false)
    local timer = Timer.Create(function(loop)
        --log('time = '..loop)
        if loop == 1 then
            self:onSetTimerStatus(true)
        end
    end, 1, time)
    timer:Start()
end


return MatchManager