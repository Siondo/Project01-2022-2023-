--[[
Description: 通用排行榜界面
Author: xinZhao
Date: 2022-04-25 20:53:54
LastEditTime: 2022-04-25 20:54:06
--]]

RankListView = class('RankListView', UIBase)

function RankListView:OnLoad()
    self.topGrids = {}
    for i = 1, 3 do
        local root = self.ui.RabFristGo.transform:Find('Slot'..i) 
        self.topGrids[i] = {
            root = root.gameObject,
            playerGrid = root:Find('PlayerGrid').gameObject,
            playerName = root:Find('PlayerName'):GetComponent(typeof(TextEx)),
            playerScore = root:Find('PlayerScore/Score'):GetComponent(typeof(TextEx)),
            playerScoreImg = root:Find('PlayerScore/ScoreIcon'):GetComponent(typeof(ImageEx))
        }
    end

    self.scrollPool = self.ui.ContentGo:GetComponent(typeof(ScrollPoolVertical))
    self:onClick_BtnStarRank()
end


function RankListView:onClick_BtnStarRank()
    if self.rankType == 1 then return end
    self.rankType = 1
    self:onRefreshTabEvenHandler(self.rankType)
end


function RankListView:onClick_BtnTrophyRank()
    if self.rankType == 2 then return end
    self.rankType = 2
    self:onRefreshTabEvenHandler(self.rankType)
end


function RankListView:onChangeButtonStyle(btnType)
    local doSetSprite = function(index, spriteName, outlineColor, shadowColor)
        local btnImg = index == 1 and self.ui.BtnStarRankImgEx or self.ui.BtnTrophyRankImgEx
        UITool:SetSprte(btnImg, 'FriendListView/'..spriteName)

        local outline, shadow
        if index == 1 then
            outline = self.ui.RankGo.transform:GetComponent(typeof(Outline))
            shadow = self.ui.RankGo.transform:GetComponent(typeof(Shadow))
        else
            outline = self.ui.TrophyRankGo.transform:GetComponent(typeof(Outline))
            shadow = self.ui.TrophyRankGo.transform:GetComponent(typeof(Shadow))
        end

        outline.effectColor = UIColor:GetColorv2(outlineColor)
        shadow.effectColor = UIColor:GetColorv2(shadowColor)
    end

    if btnType == 1 then
        doSetSprite(1, 'common_friend_btn1', '#CA8B2E', '#A36D10')
        doSetSprite(2, 'common_friend_btn2', '#5A53D2', '#3D3AA1')
    elseif btnType == 2 then
        doSetSprite(2, 'common_friend_btn1', '#CA8B2E', '#A36D10')
        doSetSprite(1, 'common_friend_btn2', '#5A53D2', '#3D3AA1')
    end
end


--[[
    @desc: 刷新页面
    time:2022-04-26 10:19:41
    --@rankType: 页签编号
]]
function RankListView:onRefreshTabEvenHandler(rankType)
    self:onChangeButtonStyle(rankType)
    RankData:onPullRankData(rankType, function()
        self:onChanged_TopContent(rankType)
        local data = RankData:onGetOtherRankList(rankType) or {}

        self.ui.NoDataGo:SetActive(#data == 0)
        self.ui.RabFristGo:SetActive(#data > 0)
        self.ui.ScrollViewGo:SetActive(#data > 0)

        self.scrollPool:SetUpdateCallBack(function(index, obj)
            self:onChanged_Content(index, data[index], obj)
        end)
        self.scrollPool:InitPool(#data)
    end)
end


function RankListView:onChanged_TopContent(rankType)
    local data = RankData:onGetTopRankList(rankType) or {}
    local spriteName = rankType == 1 and 'common_icon_star' or 'common_icon_Rank1'

    for i = 1, #self.topGrids do
        self.topGrids[i].root:SetActive(#data >= i)
        if #data >= i then
            self.topGrids[i].playerName.text = data[i].name
            self.topGrids[i].playerScore.text = data[i].score
            UITool:SetSprte(self.topGrids[i].playerScoreImg, 'Common/icon/'..spriteName)

            PlayerGrid:onInit(self.topGrids[i].playerGrid):onRefresh(data[i].head)
        end
    end
end


function RankListView:onChanged_Content(index, data, obj)
    local itemRoot = obj:GetComponent(typeof(ButtonEx))
    local playerGrid = obj.transform:Find('PlayerGrid').gameObject
    local playerRank = obj.transform:Find('PlayerRank'):GetComponent(typeof(TextEx))
    local playerName = obj.transform:Find('PlayerName'):GetComponent(typeof(TextEx))
    local playerScore = obj.transform:Find('PlayerScore/Score'):GetComponent(typeof(TextEx))
    local playerScoreImg = obj.transform:Find('PlayerScore/ScoreIcon'):GetComponent(typeof(ImageEx))

    playerRank.text = data.rank
    playerName.text = data.name
    playerScore.text = data.score
    itemRoot.interactable = tostring(data.uid) ~= PlayerData:getUserID()
    PlayerGrid:onInit(playerGrid):onRefresh(data.head)

    local spriteName = self.rankType == 1 and 'common_icon_star' or 'common_icon_Rank1'
    UITool:SetSprte(playerScoreImg, 'Common/icon/'..spriteName)
end


function RankListView:onClick_BtnIcon()
    LuaHelper.ShowUI(UI.RankRewardListView, RankData:onGetRankRewardData(self.rankType))
end

return RankListView