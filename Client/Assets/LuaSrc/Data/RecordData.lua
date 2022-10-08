--[[
Description: 战绩数据类
Author: xinZhao
Date: 2022-04-25 10:37:26
LastEditTime: 2022-04-25 10:37:26
--]]

RecordData = class('RecordData')

local FontColor = {
    "#814811",
    "#55558E",
    "#642B24",
    "#45355C",
}

local ShadowColor = {
    "#A07044",
    "#8666A0",
    "#743839",
    "#4F3B6D",
}

--[[
    @desc: 拉取战绩数据
    time:2022-04-25 10:38:06
]]
function RecordData:onPullRecordData(isInnerPull)
    self.recordList = self.recordList or {}   --服务器数据
    self.unDoneList = self.unDoneList or {}   --未完成的 (未领取/进行中)
    self.completList = self.completList or {} --完成的

    NetWork:onRequest(UIProtoType.RecordList.protoID, nil, function(result, tbData)
        if result then
            local curTime = os.time()
            local serverData = tbData.recordList or {}
            local startIndex, isNeedRefreshNewItem = #self.recordList + 1, false
            if isInnerPull and #tbData.recordList > #self.recordList then --如果服务器的数据比本地的大, 说明有新增需要进行新增并刷新
                isNeedRefreshNewItem = true
                startIndex = (#self.recordList + 1)
            end

            for i = startIndex, #serverData do
                local config = serverData[i].roomType == 1 and Config.tbPVPAward or Config.tbFriendsAward
                local isCollect = serverData[i].isCollect == 1 and true or false

                local rank = serverData[i].rank == 0 and 1 or serverData[i].rank
                self.recordList[#self.recordList + 1] = {
                    roomId = serverData[i].roomId,
                    roomType = serverData[i].roomType,
                    rank = rank,
                    score = serverData[i].score,
                    endTime = serverData[i].endTime,
                    isCollect = isCollect,
                    rewards = config[tostring(rank)].Reward,
                }
            end
        

            for i = startIndex, #self.recordList do
                local data = self.recordList[i]
                self:onAddData(data)
                if isNeedRefreshNewItem then
                    self:onAddElement(1, self.unDoneList[#self.unDoneList], false)
                end
            end

            self.unDoneList.length = #self.unDoneList
            self.completList.length = #self.completList
        end
    end)
end


function RecordData:onAddData(data)
    local formatData = {
        roomId = data.roomId,
        roomType = data.roomType,
        rank = data.rank,
        score = data.score,
        endTime = data.endTime,
        endTimeByFormat = data.endTime > 0 and os.date("%d/%m/%Y", data.endTime) or UIText('ui_results_00006'),
        rewards = data.rewards or {},
        isCollect = data.isCollect,

        --美术样式
        rankStyle = data.rank <= 3 and 'Common/record_'..data.rank or nil,
        bannerStyle = data.rank <= 3 and 'RecordView/record_botten'..data.rank or 'RecordView/record_botten4',
        titleStyle = data.rank <= 3 and 'RecordView/record_title'..data.rank or 'RecordView/record_title4',
        fontColor = data.rank <= 3 and FontColor[data.rank] or FontColor[4],
        shadowColor = data.rank <= 3 and ShadowColor[data.rank] or ShadowColor[4],
    }

    --未完成的
    if data.endTime == 0 or data.isCollect == false then
        self.unDoneList[#self.unDoneList + 1] = formatData
    --完成的
    else
        self.completList[#self.completList + 1] = formatData
    end
end


--[[
    @desc: 刷新所有元素
    time:2022-04-25 16:29:06
]]
function RecordData:onRefreshAll()
    for i = 1, #self.unDoneList do
        if self.unDoneList[i].refreshCallBack then
            self.unDoneList[i].refreshCallBack()
        end
    end

    for i = 1, #self.completList do
        if self.completList[i].refreshCallBack then
            self.completList[i].refreshCallBack()
        end
    end
end


--[[
    @desc: 获取未完成的数据
    time:2022-04-25 11:29:40
]]
function RecordData:onGetUnDoneList()
    return self.unDoneList
end


--[[
    @desc: 获取未完成的数据
    time:2022-04-25 11:29:40
]]
function RecordData:onGetCompletList()
    return self.completList
end


--[[
    @desc: 根据类型获取对应表
    time:2022-04-25 15:49:33
    --@type: 类型(1: 未完成 2:已完成的)
]]
function RecordData:onGetPool(type)
    return type == 1 and self.unDoneList or self.completList
end


--[[
    @desc: 添加刷新方法
    time:2022-04-25 11:52:49
    --@type: 类型
	--@index: 表内下标
	--@itemObj: UnityEngine.GameObject
]]
function RecordData:onAddElementRefreshCallBack(type, index, itemObj, refreshCallBack)
    local list = self:onGetPool(type)
    if not list[index] then return end

    list[index].itemGameObject = itemObj
    list[index].refreshCallBack = refreshCallBack
    list[index].refreshCallBack()
end


--[[
    @desc: 领取道具
    time:2022-04-25 15:51:26
    --@type: 类型
	--@index: 表内下表 
]]
function RecordData:onClaimReward(type, index)
    local list = self:onGetPool(type)
    if not list[index] then return end

    NetWork:onRequest(UIProtoType.CliamRecord.protoID, {roomId = list[index].roomId}, function(result, tbData)
        if result then
            list[index].isCollect = true
            list[index].itemGameObject.name = list[index].itemGameObject.name..'[Abandon]'
            list[index].refreshCallBack()
            list[index].itemGameObject = nil
            list[index].refreshCallBack = nil
            list.length = list.length - 1 --未完成领取的战绩-1
        
            --向已经完成的战绩列表中添加一个新的元素
            self:onAddElement(2, list[index], true)
        else
            if tbData == 203 then
                UITool:onShowTips('当前关卡还有其他玩家未完成, 无法领取道具')
                return
            end
        end
    end)
end


--[[
    @desc: 向某个池子中添加一个元素
    time:2022-04-25 18:13:17
    --@type: 类型
	--@data: 数据
    --@isClaim: 是否是领取(领取是从另外一个表取数据到当前的表, 需要新开辟存储空间)
]]
function RecordData:onAddElement(type, data, isClaim)
    local list = self:onGetPool(type)

    local index = isClaim and #list + 1 or #list 
    list[index] = table.easyCopy(data)
    list.length = list.length + 1 --完成领取的战绩+1
    UIEvent:OnEvent(UIEvent.RecordListView.AddElement, type, index, list[index])
end


return RecordData