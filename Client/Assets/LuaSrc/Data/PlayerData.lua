
PlayerData = {}

--数据
local data = {
    secrect = "37574B9964DE8D1B5D0DDEAF8C64321G",
    userID = "0",               --用户ID
    name = "",                  --用户名
    headAddress = "",           --头像地址
    headSprite = nil,           --头像精灵
    levelId = 1,                --当前的关卡Id
    gold = 0,                   --当前金币数
    diamonds = 0,               --当前钻石数
    signDays = 0,               --已签到天数
    canSign = false,            --能否签到
    exchangeGoldNum = 0,        --钻石兑换金币次数
    lookAdExchangeGoldNum = 0,  --看广告换金币次数
    freeTurntableNum = 0,       --剩余免费转盘次数
    lookAdTurntableNum = 0,     --剩余看广告转盘次数
    baseAttLv = 1,              --基础属性等级
    magicLv = 1,                --幻化分身
    recoveryLv = 1,             --自动回复
    skillLv = 1,                --特殊技能
    skins = {},                 --拥有的皮肤列表
    equipSkin = nil,            --装备中皮肤
    skinDebris = 0,             --皮肤碎片数量
    magicNum = 0,               --当前真实分身数量
    resetTime = 0,              --跨天重置倒计时
    dailyTaskData = {},         --日常任务数据
    activeValue = 0,            --日常活跃
    activeIds = {},             --已领取的日常活跃ID
    languageName = nil,                 --语言对应字段名字编号
}

--用户Secrect
function PlayerData:getSecrect()
    return data.secrect
end

--用户ID
function PlayerData:getUserID()
    return data.userID
end
function PlayerData:setUserID(userID)
    data.userID = tostring(userID)
end

--用户名
function PlayerData:getUserName()
    return data.name
end

function PlayerData:setUserName(name)
    data.name = name
    UIEvent:OnEvent(UIEvent.AccountView.OnRefresh)
end

--用户头像地址
function PlayerData:getUserHeadAddress()
    return data.headAddress
end


function PlayerData:onSetPlayerHeadIcon(img, headID)
    if type(headID) == 'number' or #headID <= 3 then
        UITool:SetSprte(img, 'Common/icon/head/friend_head'..headID, true)
    else
        UITool:SetSprte(img, 'Common/icon/head/friend_head'..1, true)
    end
end

function PlayerData:setUserHeadAddress(headAddress)
    if type(headAddress) == 'number' or #headAddress <= 3 then
        data.headAddress = tonumber(headAddress)
    else
        data.headAddress = headAddress
    end
    UIEvent:OnEvent(UIEvent.AccountView.OnRefresh)
end

--用户头像精灵
function PlayerData:getUserHeadSprite()
    return data.headSprite
end
function PlayerData:setUserHeadSprite(headSprite)
    data.headSprite = headSprite
end

--得到服务器时间
function PlayerData:getServerTime()
    return math.floor(data.serverTime*0.001 + (GetRealtimeSinceStartup() - data.recordTime))
end
function PlayerData:getServerTimeMilli()
    return data.serverTime + (GetRealtimeSinceStartup() - data.recordTime) * 1000
end
function PlayerData:setServerTime(serverTime)
    data.recordTime = GetRealtimeSinceStartup()
    data.serverTime = serverTime
end

--得到当前地图
function PlayerData:getMapId()
    local tbData = Config.tbLevel[tostring(PlayerData:getLevelId())]
    return tbData and tbData.mapid or 1
end

--当前关卡Id
function PlayerData:getLevelId()
    return data.levelId
end

--设置关卡Id
function PlayerData:setLevelId(levelId)
    data.levelId = levelId
end

--金币
function PlayerData:getGold()
    return data.gold
end
function PlayerData:setGold(gold)
    data.gold = gold
end

--星星数
function PlayerData:getStarCount()
    return data.stars
end

function PlayerData:setStarCount(stars)
    data.stars = stars
end

function PlayerData:addStarCount(num)
    data.stars = data.stars + num
end


--已经领取的关卡宝箱
function PlayerData:getLevelCliam()
    return data.levelCliam
end

function PlayerData:setLevelCliam(levelCliam)
    data.levelCliam = levelCliam
end


--已签到天数
function PlayerData:getSignDay()
    return data.signDays
end
function PlayerData:setSignDay(signDays)
    data.signDays = signDays
end

--能否签到
function PlayerData:getCanSign()
    return data.canSign
end
function PlayerData:setCanSign(canSign)
    data.canSign = canSign
end

--钻石兑换金币次数
function PlayerData:getExchangeGoldNum()
    return data.exchangeGoldNum
end
function PlayerData:setExchangeGoldNum(exchangeGoldNum)
    data.exchangeGoldNum = exchangeGoldNum
end

--看广告换金币次数
function PlayerData:getLookAdExchangeGoldNum()
    return data.lookAdExchangeGoldNum
end
function PlayerData:setLookAdExchangeGoldNum(lookAdExchangeGoldNum)
    data.lookAdExchangeGoldNum = lookAdExchangeGoldNum
end

--跨天重置倒计时
function PlayerData:getResetTime()
    return data.resetTime
end
function PlayerData:setResetTime(resetTime)
    data.resetTime = resetTime
end

--剩余免费转盘次数
function PlayerData:getFreeTurntableNum()
    return data.freeTurntableNum
end
function PlayerData:setFreeTurntableNum(freeTurntableNum)
    data.freeTurntableNum = freeTurntableNum
end

--剩余看广告转盘次数
function PlayerData:getLookAdTurntableNum()
    return data.lookAdTurntableNum
end
function PlayerData:setLookAdTurntableNum(lookAdTurntableNum)
    data.lookAdTurntableNum = lookAdTurntableNum
end

--基础属性等级
function PlayerData:getBaseAttLv()
    return data.baseAttLv
end
function PlayerData:setBaseAttLv(baseAttLv)
    data.baseAttLv = baseAttLv
end

--幻化分身
function PlayerData:getMagicLv()
    return data.magicLv
end
function PlayerData:setMagicLv(magicLv)
    data.magicLv = magicLv
end

--自动回复
function PlayerData:getRecoveryLv()
    return data.recoveryLv
end
function PlayerData:setRecoveryLv(recoveryLv)
    data.recoveryLv = recoveryLv
end

--特殊技能
function PlayerData:getSkillLv()
    return data.skillLv
end
function PlayerData:setSkillLv(skillLv)
    data.skillLv = skillLv
end

--拥有的皮肤列表
function PlayerData:getSkins()
    return data.skins
end
function PlayerData:setSkins(skins)
    data.skins = skins
end

--是否拥有皮肤
function PlayerData:hasSkin(skinId)
    for _, v in ipairs(data.skins) do
        if skinId == v then
            return true
        end
    end
    return false
end

--装备中皮肤
function PlayerData:getEquipSkin()
    return data.equipSkin
end
function PlayerData:setEquipSkin(equipSkin)
    data.equipSkin = equipSkin
end

--是否装备了这个皮肤
function PlayerData:hasEquipSkin(skinId)
    return data.equipSkin == skinId
end

--皮肤碎片数量
function PlayerData:getSkinDebris()
    return data.skinDebris
end
function PlayerData:setSkinDebris(skinDebris)
    data.skinDebris = skinDebris
end

--当前真实分身数量
function PlayerData:getMagicNum()
    return data.magicNum
end
function PlayerData:setMagicNum(magicNum)
    data.magicNum = magicNum
end

--日常任务数据
function PlayerData:getDailyTaskData()
    return data.dailyTaskData
end
function PlayerData:setDailyTaskData(dailyTaskData)
    data.dailyTaskData = dailyTaskData
end

--日常活跃
function PlayerData:getActiveValue()
    return data.activeValue
end
function PlayerData:setActiveValue(activeValue)
    data.activeValue = activeValue
end

--已领取的日常活跃ID
function PlayerData:getActiveIds()
    return data.activeIds
end
function PlayerData:setActiveIds(activeIds)
    data.activeIds = activeIds
end

--当前使用的语言编号
function PlayerData:getLanguageName()
    if nil == data.languageName then
        data.languageName = LuaHelper.GetString("lang_name", GetGlobal("DefaultLanguage"))
    end
    return data.languageName
end
function PlayerData:setLanguageName(languageName)
    data.languageName = languageName
    LuaHelper.SetString("lang_name", data.languageName)
end