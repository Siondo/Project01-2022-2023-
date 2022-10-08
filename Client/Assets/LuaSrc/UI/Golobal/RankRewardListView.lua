--[[
Description: 赛季奖励界面
Author: xinZhao
Date: 2022-04-06 11:33:37
LastEditTime: 2022-04-06 11:33:37
]]

RankRewardListView = class('RankRewardListView', UIBase)

function RankRewardListView:OnLoad()
    self.itemList = {}
    for i = 1, 3 do
        self.itemList[i] = {
            root = self.ui.SlotContentRectTf:Find('Slot'..i).gameObject,
            icon = self.ui.SlotContentRectTf:Find('Slot'..i..'/Icon'):GetComponent(typeof(ImageEx)),
            count = self.ui.SlotContentRectTf:Find('Slot'..i..'/Count'):GetComponent(typeof(TextEx)),
        }
    end
end


function RankRewardListView:OnShow(bindData)
    bindData.endTime = bindData.endTime > 0 and bindData.endTime or 0
    local timeInfo = string.Split(os.date('%d-%H', bindData.endTime), '-')
    self.ui.DescotentTxtEx.text = UIText('ui_rakinglist_00005', timeInfo[1], timeInfo[2])
    self:onRefresh(bindData.period, bindData.myRank)
end


function RankRewardListView:onRefresh(period, myRank)
    myRank = myRank == 0 and 201 or myRank
    for i, config in UITool:PairsBykeys(Config.tbStarRankingRewardClient) do
        local rank = string.Split(config.StarRanking, '-')
        if myRank >= tonumber(rank[1]) and myRank <= tonumber(rank[2]) then
            local rewards
            local doSetup = function(startPos, endPos)
                local list = {}
                for i = startPos, endPos do
                    if config.Items[i][1] ~= 0 then
                        list[#list + 1] = ItemData:onJsonConvertLuaItem(config.Items[i])
                    end
                end
                return list
            end

            -- 等差数列
            -- 3n-2
            -- 2+(n-1)*3 => 3n-1
            -- 3+(n-1)*3 => 3+3n-3
            if (period + 2) % 3 == 0 then
                rewards = doSetup(1, 2)
                log('1 4 7 10...')
            elseif (period + 1) % 3 == 0 then
                rewards = doSetup(3, 4)
                log('2 5 8 11...')
            elseif period % 3 == 0 then
                rewards = doSetup(5, 7)
                log('3 6 9 12...')
            end

            if rewards and #rewards > 0 then
                for i = 1, #self.itemList do
                    self.itemList[i].root:SetActive(#rewards >= i)
                    if #rewards >= i then
                        local itemConfig = ItemData:onGetItemConfig(rewards[i].id)
                        UITool:SetSprte(self.itemList[i].icon, 'Common/icon/'..itemConfig.IconFile, true)
                        self.itemList[i].count.text = rewards[i].num
                    end
                end
            end
            return
        end
    end
end


function RankRewardListView:onClick_BtnClose()
    self:onClose()
end


function RankRewardListView:onClick_BtnSure()
    self:onClose()
end

return RankRewardListView