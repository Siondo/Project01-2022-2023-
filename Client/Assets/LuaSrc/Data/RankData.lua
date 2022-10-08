--[[
Description: 排行榜数据类
Author: xinZhao
Date: 2022-04-26 10:08:16
LastEditTime: 2022-04-26 10:08:16
--]]

RankData = class('RankData')

function RankData:onPullRankData(rankType, callBack)
    self.rankList = self.rankList or {star = {}, trophy = {}}

    NetWork:onRequest(UIProtoType.RankList.protoID, {rankType = rankType}, function(result, tbData)
        if result then
            local list = {top = {}, other = {}}
            for i = 1, #tbData.ranks do
                local innertb = i <= 3 and list.top or list.other
                innertb[#innertb + 1] = tbData.ranks[i]
            end

            if tbData.rankType == 1 then
                self.rankList.star = list
                self.rankList.star.rewardInfo = {
                    endTime = tbData.time,
                    period = tbData.period + 1,
                    myRank = tbData.myRank
                }
            else
                self.rankList.trophy = list
                self.rankList.trophy.rewardInfo = {
                    endTime = tbData.time,
                    period = tbData.period + 1,
                    myRank = tbData.myRank
                }
            end

            if callBack then
                callBack()
            end
        end
    end)
end


function RankData:onSetSeasonEndInfo(data)
    self.seasonEndInfo = {}
    if data and data.rankType == 1 then
        self.seasonEndInfo[1] = data
    elseif data and data.rankType == 2 then
        self.seasonEndInfo[2] = data
    end
end


function RankData:onGetSeasonEndInfo(type)
    local listInfo = self.seasonEndInfo or {}
    if type == ReconnectionType.SeasonEndStar then
        return listInfo[1] or nil
    elseif type == ReconnectionType.SeasonEndTrophy then
        return listInfo[2] or nil
    end
end


--[[
    @desc: 获取其他数据
    time:2022-04-26 11:24:27
    --@rankType: 类型
]]
function RankData:onGetOtherRankList(rankType)
    return rankType == 1 and self.rankList.star.other or self.rankList.trophy.other
end


--[[
    @desc: 获取头部数据
    time:2022-04-26 11:24:16
    --@rankType: 类型
]]
function RankData:onGetTopRankList(rankType)
    return rankType == 1 and self.rankList.star.top or self.rankList.trophy.top
end


--[[
    @desc: 获取排名和赛季期数
    time:2022-05-24 15:54:57
    --@rankType: 类型
]]
function RankData:onGetRankRewardData(rankType)
    return rankType == 1 and self.rankList.star.rewardInfo or self.rankList.trophy.rewardInfo
end

return RankData