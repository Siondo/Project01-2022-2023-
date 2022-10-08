--[[
Description: 结算界面
Author: xinZhao
Date: 2022-04-21 18:12:16
LastEditTime: 2022-04-21 18:12:16
--]]

MatchSettlementView = class('MatchSettlementView', UIBase)


function MatchSettlementView:OnLoad()
    self.boxSpine = self.ui.BoxSpineGo.transform:GetComponent("SkeletonGraphic")
end


function MatchSettlementView:OnShow(data)
    --重置数据
    self.playerInfos = {}
    -- self.playerInfos[#self.playerInfos + 1] = {
    --     uid = PlayerData:getUserID(),
    --     name = PlayerData:getUserName(),
    --     head = PlayerData:getUserHeadAddress(),
    -- }

    --根据剩余时间计算额外的星星数
    self.win = data.win
    if data.win then
        AudioManager.PlaySound(AudioManager.Audio.Battle_Win)
    else
        AudioManager.PlaySound(AudioManager.Audio.Battle_Lose)
    end

    self.starCount = data.starCount
    self.timeStarCount, self.value = self:onCalculateTimeStarCount(data)
    self.formatRemainingTime = UITool:timeString(data.remainingTime)
    self.totallyStarCount = self.starCount + self.timeStarCount
    self:onRequestFightEnd()

    self.ui.yanhuaGo:SetActive(false)
    self.ui.BoxSpineGo:SetActive(false)
    self.ui.BoxIconGo:SetActive(true)
    self.ui[data.type..'Go']:SetActive(true)
    if data.type == 'Part2' then
        self.ui.FillCountTxtEx.text = '0%'
        self.ui.TitleTxtEx.text = UIText('ui_game_00007')
        self.ui['Part1Go']:SetActive(false)
        self.ui['DownRootGo']:SetActive(true)
        self:onRefresh(self.ui.StarCountP2TxtEx, self.ui.TimeCountP2TxtEx, self.ui.TimeLeftP2TxtEx, self.ui.AllCountP2TxtEx, self.ui.FillHandlerImgEx, self.ui.FillCountTxtEx)
    elseif data.type == 'Part1' then
        self.ui.TitleTxtEx.text = UIText('ui_game_00015')
        self.ui['Part2Go']:SetActive(false)
        self.ui['DownRootGo']:SetActive(false)
        self:onRefresh(self.ui.StarCountP1TxtEx, self.ui.TimeCountP1TxtEx, self.ui.TimeLeftp1TxtEx, self.ui.AllCountP1TxtEx, self.ui.FillHandlerImgEx, self.ui.FillCountTxtEx)
    end
end


--[[
    @desc: 战斗结束请求
    time:2022-04-21 19:38:43
]]
function MatchSettlementView:onRequestFightEnd()
    local parmas = {
        starNum = self.totallyStarCount,
        isWin = self.win and 1 or 0,
    }

    if MatchProtoRequest.BattleType == BattleType.PVE then
        PlayerData:addStarCount(self.totallyStarCount)
        NetWork:onRequest(UIProtoType.FightEnd.protoID, parmas, function(reslut, data)
        end)
    
        if self.win then
            local nextLevel = PlayerData:getLevelId() + 1
            PlayerData:setLevelId(nextLevel)
        end
        
    elseif MatchProtoRequest.BattleType == BattleType.PVP then
        NetWork:onRequest(UIProtoType.PVPFightEnd.protoID, parmas, function(reslut, tbData)
            if reslut then
                MatchData:onSetPVPEndPlayerInfos(tbData.data, BattleType.PVP)
            end
        end)

    elseif MatchProtoRequest.BattleType == BattleType.FRIEND then
        NetWork:onRequest(UIProtoType.FriendFightEnd.protoID, {
            starNum = self.totallyStarCount,
            isWin = self.win and 1 or 0,
            roomId = MatchManager.roomId or ''
        }, function(reslut, tbData)
            if reslut then
                MatchData:onSetPVPEndPlayerInfos(tbData.data, BattleType.FRIEND)
            end
        end)
    end
end


--[[
    @desc: 界面刷新
    time:2022-04-21 19:12:13
    --@starCount: 星星数量组件
	--@timeCount: 时间数量组件
	--@timeLeft: 时间剩余组件
	--@allCount: 全部星星数量组件
	--@slider: 滑动条(ImageEx)
	--@sliderCount:  滑动条百分比
]]
function MatchSettlementView:onRefresh(starCount, timeCount, timeLeft, allCount, slider, sliderCount)
    starCount.text = self.starCount
    timeCount.text = self.timeStarCount
    timeLeft.text = self.formatRemainingTime
    allCount.text = self.totallyStarCount
    slider.fillAmount = 0

    self.ui.yanhuaGo:SetActive(true)
    UITool:onDelay(1.2, function()
        local delayTime = 0
        if self.win then
            delayTime = 0.2
            self.ui.BoxSpineGo:SetActive(true)
            UITool:onPlaySpineAnimation(self.boxSpine, 'daiji', true)
        end

        if MatchProtoRequest.BattleType == BattleType.PVE then
            UITool:onDelay(delayTime, function()
          
                UITool:onGlobalFlyItem(nil, self.ui.StartPosRectTf, self.ui.BoxIconRectTf)
                AudioManager.PlaySound(AudioManager.Audio.Get_Stars)
            
                UITool:onDelay(0.8, function()
                    if slider and sliderCount then
                        local star = PlayerData:getStarCount()
                        for _, config in UITool:PairsBykeys(Config.tbStarChest) do
                            local value = (star / config.StarNumber) > 1 and '100%' or math.floor((star / config.StarNumber) * 100)..'%'
                            sliderCount.text = value
                            GameTween.DOFillAmount(slider, (star / config.StarNumber), 0.5):OnComplete(function()
        
                            end):SetAutoKill(true)
                        end
                    end
                end)
            end)
        end
    end) 
end


--[[
    @desc: 根据剩余时间计算额外的星星数
    time:2022-04-21 19:05:49
    --@data: 管理器数据
]]
function MatchSettlementView:onCalculateTimeStarCount(data)
    local timeStarCount = 0
    local value = data.remainingTime / data.maxTime
    if value >= 0.6 and data.isResurgence == false then
        timeStarCount = data.remainingTime * 10

    elseif value >= 0.3 and data.isResurgence == false then
        timeStarCount = data.remainingTime * 8

    elseif value < 0.3 then
        timeStarCount = data.remainingTime * 6
    end
    return timeStarCount, value
end


function MatchSettlementView:onClick_BtnNextLevel()
    self.limitHide = true
    MatchManager:onOffMatchGame(false)
end


--[[
    @desc: 返回主界面
    time:2022-04-22 10:46:40
]]
function MatchSettlementView:onQuitHome()
    self.limitHide = true
    MatchManager:onOffMatchGame(true)
end


--[[
    @desc: PVP查看其他玩家详情界面
    time:2022-05-09 14:16:26
]]
function MatchSettlementView:onClick_BtnConfirm()
    self.limitHide = true
    self:onClose()
    LuaHelper.ShowUI(UI.MatchEndPlayerInfoView, 1)
end


function MatchSettlementView:onClick_BtnHome()
    self:onQuitHome()
end


function MatchSettlementView:OnHide()
    if self.limitHide then self.limitHide = nil return end
    self:onQuitHome()
end

return MatchSettlementView