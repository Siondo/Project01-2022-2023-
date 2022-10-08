--[[
Description: 匹配游戏道具组件
Author: xinZhao
Date: 2022-03-31 14:00:54
LastEditTime: 2022-04-01 10:27:50
--]]
MatchItem = class('MatchItem')

local matchGameFlyItemPath = 'Res/UI/Prefab/Golobal/Item/MatchFlyItem.prefab'
local onGetElementEid = function(gameObject)
    local eid, modelId
    local splitStr = string.Split(gameObject.name, ':')
    if splitStr then
        eid = tonumber(string.Split(splitStr[1], '-')[1])
        modelId = splitStr[2]
    end

    return eid, modelId
end

--[[
    @desc: 初始化
    --@gameObject: 道具组件
    @return: 匹配游戏摄像机组件
]]
function MatchItem:onInit(gameObject)
    return MatchManager.matchCamera
end


--[[
    @desc: 当选择道具
]]
function MatchItem:onMouseDown()
end


--[[
    @desc: 当拖拽道具
]]
function MatchItem:onMouseDrag(gameObject)
    --log('拖拽道具')
end


--[[
    @desc: 当点击道具
]]
function MatchItem:onMouseUp(gameObject)
    if LuaHelper.CheckGuiRaycastObjects() then
        return
    end

    local eid, modelId = onGetElementEid(gameObject)
    MatchItem:onCollectElement(eid, modelId, gameObject)
end


function MatchItem:onCollectElement(eid, modelId, clickModel)
    if MatchManager.mapElement[eid] and MatchManager.mapElement[eid].isTaskElement then
        UIEvent:OnEvent(UIEvent.MatchMainView.GetUIComponent, function(uiComponent)
            LuaHelper.LoadAsset(matchGameFlyItemPath, function(status, asset)
                if status then
                    --生成元素
                    local flyitem = GameObject.Instantiate(asset.mainAsset, uiComponent.parent).transform
                    local flyIcon = flyitem:GetComponent(typeof(ImageEx))
                    UITool:SetSprte(flyIcon, Config.tbElement[tostring(eid)].IconName, true)
                    local bornPos = LuaHelper.ScreenPointToLocalPointInRectangle(MatchManager.uiRectTransform, MatchManager.uiCamera, Input.mousePosition)
                    flyitem.localPosition = Vector3(bornPos.x, bornPos.y, 0)
                    AudioManager.PlaySound(AudioManager.Audio.Task_Click)
    
                    --删除元素
                    if MatchManager.mapElement[eid].models and MatchManager.mapElement[eid].models[modelId] then
                        GameObject.Destroy(MatchManager.mapElement[eid].models[modelId])
                        MatchManager.mapElement[eid].models[modelId] = nil
                    end
    
                    --元素飞行动画
                    local index = MatchManager:onGetUISlotIndex(eid)
                    UIEvent:OnEvent(UIEvent.MatchMainView.SetUIStarCount, index) --变更星星数量
                    GameTween.DOLocalMove(flyitem, uiComponent.mapItemPos[index].localPosition, 1, false):SetEase(15):SetAutoKill(true)
                    GameTween.DOScale(flyitem, 0, 1):SetDelay(0.35):SetEase(28):OnComplete(function()
                        GameObject.Destroy(flyitem.gameObject)
                        --UIEvent:OnEvent(UIEvent.MatchMainView.SetUIElementCount, index) --元素数量增加
                    end):SetAutoKill(true)
                end
            end)
        end)
    else
        if MatchManager.mapElement[eid] then
            local angle = MatchManager.mapElement[eid].config.Angle
            local angleTime = MatchManager.mapElement[eid].config.AngleTime or 0.36
            local xyz = string.Split(angle, ',')
            local x, y, z = UITool:onInterceptionValue(xyz[1], 2, #xyz[1]), xyz[2], UITool:onInterceptionValue(xyz[3], 1, #xyz[3] - 1)
            local v3 = Vector3(tonumber(x), tonumber(y), tonumber(z))
            GameTween.DOLocalRotate(clickModel.transform, v3, angleTime, 0):SetAutoKill(true)
        end
        MatchManager:onCalculateStarCount(false)
    end
end


return MatchItem
