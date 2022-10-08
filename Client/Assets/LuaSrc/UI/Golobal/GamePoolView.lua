--[[
Description: 全局游戏缓存界面
Author: xinZhao
Date: 2022-05-25 14:11:02
LastEditTime: 2022-05-25 14:11:02
--]]

GamePoolView = class('GamePoolView', UIBase)

local uiFlyItemPath = 'Res/UI/Prefab/Golobal/Item/FlyItem.prefab'
local uiTpisItemPath = 'Res/UI/Prefab/Golobal/Item/TipsItem.prefab'
function GamePoolView:OnLoad()
    self:AddEvent(UIEvent.GamePoolView.OnPushHandler, function(type, data)
        self:onPushHandler(type, data)
    end)
end


function GamePoolView:onPushHandler(type, data)
    --道具飞行
    if type == GamePoolType.FLY_ITEM then
        self:onFlyItem(self.ui.FlyItemPartRectTf, data)
    elseif type == GamePoolType.TIPS_ITem then
        self:onShowTipsItem(self.ui.TipsPartRectTf, data)
    end
end


--[[
    @desc: 全局tips提示弹窗
    time:2022-05-26 10:40:36
]]
function GamePoolView:onShowTipsItem(parent, data)
    LuaHelper.LoadAsset(uiTpisItemPath, function(status, asset)
        if status then
            local tipsItem = GameObject.Instantiate(asset.mainAsset, parent).transform
            local canvasGroup = tipsItem:GetComponent("CanvasGroup")
            local tips = tipsItem:Find('Image/Tips'):GetComponent(typeof(TextEx))
            tips.text = data.str
    
            GameTween.DOLocalMoveY(tipsItem, 600, 0.8, false):SetDelay(0.6):SetAutoKill(true)
            GameTween.DOFade(canvasGroup, 0, 1):SetDelay(1):OnComplete(function()
                GameObject.Destroy(tipsItem.gameObject)
            end):SetAutoKill(true)
        end
    end)
end


--[[
    @desc: 道具飞行动画&缓冲池
    time:2022-05-25 14:23:46
    --@itemId: 道具Id
]]
function GamePoolView:onFlyItem(parent, data)
    local doTweenSetup = function(targetCpn)
        LuaHelper.LoadAsset(uiFlyItemPath, function(status, asset)
            if status then
                local delayTime = 0.08
                for i = 1, 5 do
                    UITool:onDelay(delayTime, function()
                        local flyitem = GameObject.Instantiate(asset.mainAsset, parent).transform
                        local flyIcon = flyitem:GetComponent(typeof(ImageEx))

                        if data.itemId ~= 0 then
                            UITool:SetSprte(flyIcon, 'Common/icon/'..Config.tbItems[tostring(data.itemId)].IconFile, true)
                        else
                            UITool:SetSprte(flyIcon, 'MatchMainView/icon_xingxing', true)
                        end
        
                        --坐标转换
                        local uiPos
                        if data.isNeedCovert then
                            uiPos = LuaHelper.ScreenPointToLocalPointInRectangle(MatchManager.uiRectTransform, MatchManager.uiCamera, data.startCpn)
                        else
                            uiPos = LuaHelper.ScreenPointToLocalPointInRectangle(MatchManager.uiRectTransform, MatchManager.uiCamera, data.startCpn.position)
                            uiPos = Vector3(uiPos.x, -uiPos.y, 0)
                        end
                        flyitem.localPosition = Vector3(uiPos.x, uiPos.y, 0)

                        targetCpn = targetCpn or data.endCpn
                        GameTween.DOMove(flyitem, targetCpn.position, 1, false):OnComplete(function()
                            GameObject.Destroy(flyitem.gameObject)
                        end):SetAutoKill(true)
                    end)
                    delayTime = (0.1 * i)
                end
            end
        end)
    end

    if data.itemId ~= 0 then
        UIEvent:OnEvent(UIEvent.MainView.GetTargetComponent, data.itemId, function(targetCpn)
            if targetCpn then
                doTweenSetup(targetCpn)
            end
        end) 
    else
        doTweenSetup()
    end
end


return GamePoolView