--[[
Description: PVP战绩界面
Author: xinZhao
Date: 2022-04-02 14:14:52
LastEditTime: 2022-04-02 14:14:52
--]]
RecordListView = class("RecordListView", UIBase)

function RecordListView:OnLoad()
    self:AddEvent(UIEvent.RecordListView.AddElement, function(type, index, data)
        local parent = type == 1 and self.ui.UnDoneRootRectTf or self.ui.CompletRootRectTf
        LuaHelper.LoadAsset('Res/UI/Prefab/Golobal/Item/RecordItem.prefab', function(status, asset)
            if status then
                local item = GameObject.Instantiate(asset.mainAsset, parent)
                item.name = 'ITEM-'..index
        
                --创建单个Item/并添加刷新方法
                RecordData:onAddElementRefreshCallBack(type, index, item, self:onCreateItem(type, index, data, item))

                --显示title
                if self['RecordTitleComplete'] and self['RecordTitleUndone'] then
                    local completePool, unDonePool = RecordData:onGetPool(2), RecordData:onGetPool(1)
                    self['RecordTitleUndone']:SetActive(unDonePool.length > 0)
                    self['RecordTitleComplete']:SetActive(completePool.length > 0)

                    self.ui.NoDataGo:SetActive(unDonePool.length <= 0 and completePool.length <= 0)
                end
            end
        end)
    end)

    self:onCreateZone(RecordData:onGetUnDoneList(), self.ui.UnDoneRootRectTf, 'RecordTitleUndone')
    self:onCreateZone(RecordData:onGetCompletList(), self.ui.CompletRootRectTf, 'RecordTitleComplete')
    local completePool, unDonePool = RecordData:onGetCompletList(), RecordData:onGetUnDoneList()
    self.ui.NoDataGo:SetActive(unDonePool.length <= 0 and completePool.length <= 0)
end


function RecordListView:onCreateZone(list, parent, bundleName)
    local type = bundleName == 'RecordTitleUndone' and 1 or 2
    LuaHelper.LoadAsset('Res/UI/Prefab/Golobal/Item/'..bundleName..'.prefab', function(status, asset)
        if status then
            local title = GameObject.Instantiate(asset.mainAsset, parent)
            title.name = bundleName
            title.gameObject:SetActive(#list > 0)
            self[bundleName] = title.gameObject
        end
    end)

    if list and #list > 0 then
        for index = 1, #list do
            UIEvent:OnEvent(UIEvent.RecordListView.AddElement, type, index, list[index])
        end
    end
end


function RecordListView:OnShow()
    RecordData:onRefreshAll()
end


function RecordListView:onCreateItem(type, index, data, obj)
    local bannerImgEx = obj:GetComponent(typeof(ImageEx))
    local btnItem = obj:GetComponent(typeof(ButtonEx))
    local rankImgEx = obj.transform:Find('Rank/TopRank'):GetComponent(typeof(ImageEx))
    local rankTxtEx = obj.transform:Find('Rank/OtherRank'):GetComponent(typeof(TextEx))
    local titleImgEx = obj.transform:Find('Title'):GetComponent(typeof(ImageEx))
    local scoreTitleTxtEx = obj.transform:Find('Score/ScoreTitle'):GetComponent(typeof(TextEx))
    local scoreNumTxtEx = obj.transform:Find('Score/ScoreNum'):GetComponent(typeof(TextEx))
    local scoreNumOutline = scoreNumTxtEx:GetComponent(typeof(Outline))
    local endTimeTxtEx = obj.transform:Find('EndTime'):GetComponent(typeof(TextEx))
    local codeList = {
        obj = {
            obj.transform:Find('Status/CODE-1'),
            obj.transform:Find('Status/CODE-2'),
            obj.transform:Find('Status/CODE-3'),
        },
        rewardGo = {
            root = {
                obj.transform:Find('Status/CODE-1/Reward1').gameObject,
                obj.transform:Find('Status/CODE-1/Reward2').gameObject
            },
            count = {
                obj.transform:Find('Status/CODE-1/Reward1/Count'):GetComponent(typeof(TextEx)),
                obj.transform:Find('Status/CODE-1/Reward2/Count'):GetComponent(typeof(TextEx))
            },
            icon = {
                obj.transform:Find('Status/CODE-1/Reward1/Icon'):GetComponent(typeof(ImageEx)),
                obj.transform:Find('Status/CODE-1/Reward2/Icon'):GetComponent(typeof(ImageEx))
            }
        },
        btnReward = obj.transform:Find('Status/CODE-1/BtnReward'):GetComponent(typeof(ButtonEx))
    }

    return function()
        obj:SetActive(true)
        if type == 1 then
            if data.endTime > 0 and data.isCollect == true then
                obj:SetActive(false)
                return
            end
        end

        --模板样式
        UITool:SetSprte(bannerImgEx, data.bannerStyle)

        --排行样式
        rankImgEx.gameObject:SetActive(data.rankStyle ~= nil)
        rankTxtEx.gameObject:SetActive(data.rankStyle == nil)
        if data.rankStyle then
            UITool:SetSprte(rankImgEx, data.rankStyle)
        else
            rankTxtEx.text = data.rank
        end

        --标题样式
        UITool:SetSprte(titleImgEx, data.titleStyle)

        --积分样式
        scoreNumTxtEx.text = data.score
        local fontColor = UIColor:GetColorv2(data.fontColor)
        scoreTitleTxtEx.color = fontColor
        scoreNumOutline.effectColor = fontColor
        
        --战斗结束时间
        endTimeTxtEx.text = '<color='..data.fontColor..'>'..UIText('ui_results_00005')..'</color>  <color='..data.shadowColor..'>'..data.endTimeByFormat..'</color>'

        --奖励相关
        codeList.obj[1].gameObject:SetActive(false)
        codeList.obj[2].gameObject:SetActive(false)
        if data.endTime == 0 then
            codeList.obj[2].gameObject:SetActive(true) --未完成的开CODE-2
        elseif #data.rewards > 0 then
            codeList.obj[1].gameObject:SetActive(true) --有奖励开CODE-1

            for i = 1, 2 do
                codeList.rewardGo.root[i]:SetActive(false)
                if #data.rewards >= i then
                    codeList.rewardGo.root[i]:SetActive(true)
                    UITool:SetSprte(codeList.rewardGo.icon[i], 'Common/icon/'..Config.tbItems[tostring(data.rewards[i][1])].IconFile)
                    codeList.rewardGo.count[i].text = data.rewards[i][2]
                end
            end
            codeList.btnReward.gameObject:SetActive(not data.isCollect) --奖励未领取开启BtnReward按钮
        end

        UITool:onAddClickAndClear(codeList.btnReward, function()
            RecordData:onClaimReward(type, index)
        end)

        UITool:onAddClickAndClear(btnItem, function()
            NetWork:onRequest(UIProtoType.CheckPVPRoomInfo.protoID, {roomId = data.roomId}, function(reslut, tbData)
                if reslut then
                    MatchProtoRequest.BattleType = BattleType.PVP
                    MatchData:onSetPVPEndPlayerInfos(tbData.data, BattleType.PVP)
                    LuaHelper.ClearUI(false)
                    LuaHelper.ShowUI(UI.MatchEndPlayerInfoView, 2)
                end
            end)
        end)
    end
end


function RecordListView:OnHide()
end


return RecordListView