--[[
Description: 消除游戏主界面
Author: xinZhao
Date: 2022-03-30 16:23:37
LastEditTime: 2022-04-01 14:04:23
--]]
local MatchMainView = class('MatchMainView', UIBase)

function MatchMainView:OnLoad()
    self.cpnList = {
        parent = self.ui.ElementFlyPositionRectTf,
        uiItem = {
            self.ui.Item1RectTf,
            self.ui.Item2RectTf,
            self.ui.Item3RectTf,
            self.ui.Item4RectTf,
        },
        uiText = {
            self.ui.SlotCount1TxtEx,
            self.ui.SlotCount2TxtEx,
            self.ui.SlotCount3TxtEx,
            self.ui.SlotCount4TxtEx,
        },
        uiImage = {
            self.ui.Icon1ImgEx,
            self.ui.Icon2ImgEx,
            self.ui.Icon3ImgEx,
            self.ui.Icon4ImgEx,
        },
        mapItemPos = {
            self.ui.Position1RectTf,
            self.ui.Position2RectTf,
            self.ui.Position3RectTf,
            self.ui.Position4RectTf,
        },
        effect = {
            self.ui.vsf_Finish1Go,
            self.ui.vsf_Finish2Go,
            self.ui.vsf_Finish3Go,
            self.ui.vsf_Finish4Go,
        },
        effectClick = {
            self.ui.vsf_Collect1Go,
            self.ui.vsf_Collect2Go,
            self.ui.vsf_Collect3Go,
            self.ui.vsf_Collect4Go,
        }
    }

    --绑定自定义道具格子组件
    self.matchItemBar = {}
    local index = 0
    for levelId, data in UITool:PairsBykeys(Config.tbItemUnlock) do
        index = index + 1
        if data.type == 1 then
            local obj = self.ui.transform:Find('DownContent/MatchItemBar'..index).gameObject
            self.matchItemBar[data.ItemID] = bind(obj, MatchItemBar, data, levelId)
        end
    end
    
    --注册倒计时监听事件
    self:AddEvent(UIEvent.MatchMainView.SetGameOverCountDown, function(time)
        self:onSetGameData(nil, time)
    end)

    --注册获取组件监听事件
    self:AddEvent(UIEvent.MatchMainView.GetUIComponent, function(callBack)
        callBack(self.cpnList)
    end)

    --注册道具点击后UI元素增加的监听事件
    self:AddEvent(UIEvent.MatchMainView.SetUIElementCount, function(index)
        self:onSetUIElementCount(index)
    end)

    --注册道具点击后UI星星数变更监听事件
    self:AddEvent(UIEvent.MatchMainView.SetUIStarCount, function(index)
        MatchManager:onAddUIElement(index)
        self:onSetUIElementCount(index, true)
        local existCount = MatchManager.uiElement[index].exist
        local maxCount = MatchManager.uiElement[index].maxNum
        local curStarCount = MatchManager:onCalculateStarCount(true, existCount == maxCount)
        self:onSetGameData(curStarCount, nil)
    end)
end

function MatchMainView:OnShow()
    --初始化游戏数据
    self:onSetGameData(MatchManager.starCount, UITool:timeString(MatchManager.pveEndTime - os.time()))
    local length = #MatchManager.uiElement
    for i = 1, #self.cpnList.uiItem do
        self.cpnList.uiItem[i].gameObject:SetActive(length >= i)
        self.cpnList.mapItemPos[i].gameObject:SetActive(length >= i)
        if length >= i then
            local imgSprite = Config.tbElement[tostring(MatchManager.uiElement[i].eid)].IconName
            UITool:SetSprte(self.cpnList.uiImage[i], imgSprite, true)
            self:onSetUIElementCount(i, nil)
        end
    end

    --初始化道具
    for _, itemBar in pairs(self.matchItemBar) do
        itemBar:onRefreshItemBar()
    end
end


--[[
    @desc: 设置UI元素数量
    time:2022-04-18 18:04:08
]]
function MatchMainView:onSetUIElementCount(index, isPlay)
    local existCount = MatchManager.uiElement[index].exist
    local maxCount = MatchManager.uiElement[index].maxNum
    
    if existCount == maxCount then
        --全部领取
        UITool:onDelay(0.35, function()
            self.cpnList.effect[index]:SetActive(true)
            UITool:onDelay(1, function()
                self.cpnList.effect[index]:SetActive(false)
            end)
            AudioManager.PlaySound(AudioManager.Audio.Recive_All)
        end)
    else
        if isPlay then
            --单个领取
            UITool:onDelay(0.35, function()
                self.cpnList.effectClick[index]:SetActive(true)
                UITool:onDelay(0.25, function()
                    self.cpnList.effectClick[index]:SetActive(false)
                end)
            end)
        end
    end
    self.cpnList.uiText[index].text = existCount..'/'..maxCount
end


--[[
    @desc: 设置游戏相关数据 (关卡/星星数/倒计时)
    time:2022-04-21 15:28:26
    --@time: 结束时间
]]
function MatchMainView:onSetGameData(starCount, time)
    if starCount then
        self.ui.StarCountTxtEx.text = starCount
    end

    if time then
        self.ui.TimeLeftTxtEx.text = time
    end

    self.ui.LevelTxtEx.text = 'LEVEL '..(PlayerData:getLevelId() - LEVEL_ID_CONST)
end


function MatchMainView:onClick_BtnStop()
    LuaHelper.ShowUI(UI.MatchResumeView)
end


function MatchMainView:OnHide()
end

return MatchMainView