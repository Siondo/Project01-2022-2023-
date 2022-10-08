--[[
Description: App运行界面
Author: xinZhao
Date: 2022-04-02 15:57:55
LastEditTime: 2022-04-02 15:58:10
--]]


local AppSetupView = class('AppSetupView', UIBase)

function AppSetupView:OnLoad()
    UITool:SetSprte(self.ui.CompanyLogoImgEx, 'AppSetupView/logo_'..LuaHelper.GetAppPlatform(), true)

    self:AddEvent(UIEvent.AppSetupView.UpdateTips, function (tips, detail)
        if tips then
            self.ui.TipsTxtEx.text = tips
        end
        if detail then
            self.ui.DetailTxtEx.text = detail
        end
    end)

    self:AddEvent(UIEvent.AppSetupView.UpdateProgress, function (curSize, totalSize, str, isInGame)
        if str then
            self.ui.ProgressValueTxtEx.text = str
            return 
        end

        self.ui.ProgressSldEx.value = curSize / totalSize
        self.ui.ProgressValueTxtEx.text = string.format("%0.2f", curSize / totalSize * 100.0).."%"

        if not isInGame and self.ui.ProgressSldEx.value >= 1 then
            self.ui.AccountGo:SetActive(true)
            self.ui.LoadingGo:SetActive(false)
        end
    end)

    self:AddEvent(UIEvent.AppSetupView.OnLogin, function (email, password)
        self:onSync(email, password)
    end)
end


function AppSetupView:OnShow(callBack)
    self.ui.AccountGo:SetActive(false)
    self.ui.LoadingGo:SetActive(true)

    if not callBack then
        self:onRefreshGameController()
    else
        callBack()
    end
end


--[[
    @desc: 刷新登录页面
]]
function AppSetupView:onRefreshGameController()
    local email = LuaHelper.GetString('email')
    local password = LuaHelper.GetString('password')
    local uid = LuaHelper.GetString('uid')

    if LuaHelper.GetDebugModeStatus() then
        self.ui.BtnBeHeroGo:SetActive(not IsEmptyStringOrNull(uid))
        self.ui.BtnDeleteGo:SetActive(true)
    else
        self.ui.BtnBeHeroGo:SetActive(false)
        self.ui.BtnDeleteGo:SetActive(false)
    end
end


--[[
    @desc: 游客登录 / 本地记录的账户登录
    time:2022-04-22 15:03:16
]]
function AppSetupView:onClick_BtnStart()
    local email = LuaHelper.GetString('email')
    local password = LuaHelper.GetString('password')
    self:onSync(email, password)
end


--[[
    @desc: 注册游戏
    time:2022-04-22 15:03:25
]]
function AppSetupView:onClick_BtnRegister()
    LuaHelper.ShowUI(UI.RegisterView)
end


--[[
    @desc: 登录游戏
    time:2022-05-20 10:45:14
]]
function AppSetupView:onClick_BtnSign()
    LuaHelper.ShowUI(UI.LoginView)
end


function AppSetupView:onSync(email, password)
    local parmas = {
        name = string.Empty,
        head = string.Empty,
        token = email or '',
        uc_token = APP.token,
        password = password or ''
    }

    NetWork:onRequest(UIProtoType.Login.protoID, parmas, function(result, tbData)
        if result then
            --sync数据处理
            LuaHelper.SetString('email', email)
            LuaHelper.SetString('password', password)
            LuaHelper.SetString('uid', tbData.uid)

            PlayerData:setUserID(tbData.uid)
            PlayerData:setLevelId(tbData.chapter_id)
            PlayerData:setUserName(tbData.user_name)
            PlayerData:setUserHeadAddress(tbData.user_icon)
            PlayerData:setStarCount(tbData.star)
            PlayerData:setLevelCliam(tbData.levelChestId)
            ItemData:onInit(tbData.items)
            ItemData:onOperateInfinite(tbData.infiniteStrengthEndTime)

            --其他主动拉取数据
            MatchData:onInit(tbData.roomId)
            ShopData:onPullShopData()
            SignData:onPullSignData()
            RecordData:onPullRecordData(false)
            FriendData:onPullAllFrendData()

            --进入游戏
            AudioManager:onPlayGameBGM(BgmType.Gmae)
            LuaHelper.ClearUI(false)
            LuaHelper.ShowUI(UI.MainView, MAINTAB_INDEX.MAIN)
            NetWork.isConnect = true
        else
            if tbData == 2 then
                UITool:onShowTips(UIText('ui_tip_00017'))
            elseif tbData == 7 then
                UITool:onShowTips(UIText('ui_tip_00018'))
            elseif tbData == 8 then
                UITool:onShowTips(UIText('ui_tip_00019'))
            end
        end
    end)
end


--[[
    @desc: 一键变强
    time:2022-04-22 15:02:46
]]
function AppSetupView:onClick_BtnBeHero()
    local uid = LuaHelper.GetString('uid')
    if not IsEmptyStringOrNull(uid) then
        local items = {}
        for i, v in pairs(Config.tbItems) do
            items[#items + 1] = {id = v.ItemID, num = 90}
        end
        local parmas = {
            items = Json.encode(items),
            uid = tonumber(uid)
        }

        HttpTool.SendRequest(UIWebType.GM_AddItem, parmas, function(result, tbData)
        end, true)
    end
end


--[[
    @desc: 删除账号
    time:2022-04-22 15:02:56
]]
function AppSetupView:onClick_BtnDelete()
    local uid = LuaHelper.GetString('uid')
    if not IsEmptyStringOrNull(uid) then
        LuaHelper.SetString('uid', '')
        LuaHelper.SetString('email', '')
        LuaHelper.SetString('password', '')

        HttpTool.SendRequest(UIWebType.RemoveAccount, {playerId = tonumber(uid)}, function(result, tbData)
            if result then
                self:onRefreshGameController()
            end
        end)
    end
end

return AppSetupView