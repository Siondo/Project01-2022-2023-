--[[
    Description: PVP结算查询界面
    Author: xinZhao
    time:2022-05-09 14:09:22
]]

MatchEndPlayerInfoView = class('MatchEndPlayerInfoView', UIBase)

function MatchEndPlayerInfoView:OnLoad()
    self.infos = MatchData:onGetPVPEndPlayerInfos()
    self:onBindData(self.infos, 'Content', false)
end


function MatchEndPlayerInfoView:OnShow(type)
    self.type = type
    self:OnLoad()

    local title = self.infos.completeCount >= MatchData.maxUserInfos and UIText('ui_game_00020') or UIText('ui_game_00022')
    self.ui.TitleTxtEx.text = title
    self.ui.PlayerUIDTxtEx.text = 'ID: '..PlayerData:getUserID()
    self:onRefresh_Content()
end


function MatchEndPlayerInfoView:onChanged_Content(index, data, obj)
    local itemRoot = obj:GetComponent(typeof(ButtonEx))
    local imgPlayerRank = obj.transform:Find('ImgPlayerRank'):GetComponent(typeof(ImageEx))
    local txtPlayerRank = obj.transform:Find('TxtPlayerRank'):GetComponent(typeof(TextEx))
    local playerGrid = obj.transform:Find('PlayerGrid')
    local playerName = obj.transform:Find('PlayerName'):GetComponent(typeof(TextEx))
    local playerScore = obj.transform:Find('PlayerScore'):GetComponent(typeof(TextEx))
    local playingPart = obj.transform:Find('PlayingPart')
    local waitingPart = obj.transform:Find('WaitingPart')
    local endPart = obj.transform:Find('EndPart')
    local rewardCpn = {}
    for i = 1, 2 do
        rewardCpn[#rewardCpn + 1] = {
            root = endPart:Find('Content/'..i),
            icon = endPart:Find('Content/'..i..'/Icon'):GetComponent(typeof(ImageEx)),
            count = endPart:Find('Content/'..i..'/Score'):GetComponent(typeof(TextEx)),
        }
    end

    local uid = data.uid ~= nil and tostring(data.uid) or ''
    itemRoot.interactable = uid ~= PlayerData:getUserID()

    playerScore.gameObject:SetActive(data.state == 1)   --玩家已经打完
    playingPart.gameObject:SetActive(data.state == 0)   --玩家正在对局中
    waitingPart.gameObject:SetActive(data.state == -1)  --等待其他玩家加入

    --只要不是等待其他玩家加入的其他状态 都要显示头像和名称
    playerGrid.gameObject:SetActive(data.state ~= -1)
    playerName.gameObject:SetActive(data.state ~= -1)

    local cpn = PlayerGrid:onInit(playerGrid.gameObject)
    cpn:onRefresh(data.head)

    --排名
    txtPlayerRank.text = index
    if index <= 3 then
        UITool:SetSprte(imgPlayerRank, 'Common/record_'..index, true)
    end
    imgPlayerRank.gameObject:SetActive(index <= 3)
    txtPlayerRank.gameObject:SetActive(index >= 4)

    --名称+积分
    playerScore.text = data.score or ''
    playerName.text = data.name or ''

    --全部玩家都完成后显示 + 奖励
    endPart.gameObject:SetActive(self.infos.completeCount >= MatchData.maxUserInfos)
    local config = MatchProtoRequest.BattleType == BattleType.PVP and Config.tbPVPAward or Config.tbFriendsAward
    if config[tostring(index)] then
        obj.gameObject:SetActive(true)
        local rewardData = config[tostring(index)].Reward
        for i = 1, 2 do
            rewardCpn[i].root.gameObject:SetActive(false)
            if #rewardData >= i then
                rewardCpn[i].root.gameObject:SetActive(true)
                UITool:SetSprte(rewardCpn[i].icon, 'Common/icon/'..Config.tbItems[tostring(rewardData[i][1])].IconFile)
                rewardCpn[i].count.text = rewardData[i][2]
            end
        end
    else
        obj.gameObject:SetActive(false)
    end
end


function MatchEndPlayerInfoView:onClick_BtnClose()
    --结算界面返回
    if self.type == 1 then
        MatchManager:onOffMatchGame(true)
    --战绩界面返回
    elseif self.type == 2 then
        LuaHelper.ClearUI(false)
        LuaHelper.ShowUI(UI.MainView, MAINTAB_INDEX.RECORD)
    end
end


return MatchEndPlayerInfoView