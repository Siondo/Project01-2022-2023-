--[[
Description: 好友列表界面
Author: xinZhao
Date: 2022-04-06 11:33:37
LastEditTime: 2022-04-06 11:33:37
--]]

FriendListView = class('FriendListView', UIBase)

function FriendListView:OnLoad()
    self:onBindData(FriendData:onGetFriendList(), 'Content', false) --绑定数据

    self:AddEvent(UIEvent.FriendListView.RefreshApplyFriendList, function()
        if self.tabIndex == 2 and self.btnType == 'Right' then
            self:onEnterPage(2, 'Right')
        end
    end)

    self:AddEvent(UIEvent.FriendListView.RefreshCliamList, function()
        if self.tabIndex == 1 and self.btnType == 'Right' then
            self:onEnterPage(1, 'Right')
        end
    end)

    self:AddEvent(UIEvent.FriendListView.RefreshFriendList, function()
        if self.tabIndex == 1 and self.btnType == 'Left' then
            self:onEnterPage(1, 'Left')
        end
    end)

    self.tabList = {}
    for i = 1, 2 do
        self.tabList[i] = {}
        self.tabList[i].btnFreeze = self.ui.transform:Find('Board/TabContent/'..i..'/BtnFreeze')
        self.tabList[i].btnClick = self.ui.transform:Find('Board/TabContent/'..i..'/BtnClick'):GetComponent(typeof(ButtonEx))
        UITool:onAddClickAndClear(self.tabList[i].btnClick, function()
            self:onClickTabEventHandler(i)
        end)
    end

    self.sortList = {}
    for i = 1, 3 do
        local rect = self.ui.transform:Find('Board/@FriendContent/@SortContent/Btn'..i)
        self.sortList[i] = {}
        self.sortList[i].icon = rect:Find('Image'):GetComponent(typeof(ImageEx))
        self.sortList[i].btnImage = rect:GetComponent(typeof(ImageEx))
        self.sortList[i].btnClick = rect:GetComponent(typeof(ButtonEx))
        UITool:onAddClickAndClear(self.sortList[i].btnClick, function()
            self:onClickSortEventHandler(i)
        end)
    end

    self.ui.TxtUserInfoTxtEx.text = UIText('ui_addfriends_00012')..': '..PlayerData:getUserID()
end


function FriendListView:OnShow()
    self:onClickTabEventHandler(1)
end


function FriendListView:onClickTabEventHandler(index)
    if self.tabIndex == index then return end

    local doOpenTabPage = function(index, status)
        if index == 1 then
            self.ui.FriendContentRectTf.gameObject:SetActive(status)
        elseif index == 2 then
            self.ui.AddFriendContentRectTf.gameObject:SetActive(status)
        end
    end

    --之前点击的按钮
    if self.tabIndex then
        self.tabList[self.tabIndex].btnFreeze.gameObject:SetActive(false)
        self.tabList[self.tabIndex].btnClick.gameObject:SetActive(true)
        doOpenTabPage(self.tabIndex, false)
    end

    --之后点击的按钮
    self.tabList[index].btnFreeze.gameObject:SetActive(true)
    self.tabList[index].btnClick.gameObject:SetActive(false)
    self:onEnterPage(index, 'Left') --点击页签默认进入导航栏的左边页签
    self.tabIndex = index
    doOpenTabPage(self.tabIndex, true)

    --两个页签上的按钮标题切换
    if self.tabIndex == 1 then
        self.ui.LeftTextTxtEx.text = UIText('ui_friends_00003')
        self.ui.RightTextTxtEx.text = UIText('ui_friends_00004')
    elseif self.tabIndex == 2 then
        self.ui.LeftTextTxtEx.text = UIText('ui_friends_00002') 
        self.ui.RightTextTxtEx.text = UIText('ui_addfriends_00019')
    end
end


function FriendListView:onEnterPage(tab, btnType)
    -- if self.tabIndex == tab and self.btnType == btnType then
    --     return
    -- end

    self.btnType = btnType
    self:onChangeButtonStyle(btnType)

    local listInfo, offsetSize
    if tab == 1 and btnType == 'Left' then
        --log('好友列表')
        offsetSize = -88
        listInfo = FriendData:onGetFriendList()
        self.ui.TxtTipTxtEx.text  = UIText('ui_addfriends_00002')..'\n'..UIText('ui_addfriends_00003')--'You don’t have any friends Go to add friends and play with them'
        self.ui.BtnAdd1RectTf.gameObject:SetActive(true)
        self.ui.BtnAdd3RectTf.gameObject:SetActive(false)
        self.ui.SortContentRectTf.gameObject:SetActive(true)
        self.ui.FriendContentRectTf.gameObject:SetActive(true)
        self.ui.AddFriendContentRectTf.gameObject:SetActive(false)

    elseif tab == 1 and btnType == 'Right' then
        --log('被赠送体力列表')
        offsetSize = 0
        listInfo = FriendData:onGetCliamList()
        self.ui.TxtTipTxtEx.text  = UIText('ui_addfriends_00005')--'You Don’t have any lives! Add Friends and get free lives from your friends'
        self.ui.SortContentRectTf.gameObject:SetActive(false)
        self.ui.FriendContentRectTf.gameObject:SetActive(true)
        self.ui.AddFriendContentRectTf.gameObject:SetActive(false)

    elseif tab == 2 and btnType == 'Left' then
        --log('添加/邀请界面')
        self.ui.FriendContentRectTf.gameObject:SetActive(false)
        self.ui.AddFriendContentRectTf.gameObject:SetActive(true)
        self.ui.SortContentRectTf.gameObject:SetActive(false)
        self.ui.NoFriendContentRectTf.gameObject:SetActive(false)

    elseif tab == 2 and btnType == 'Right' then
        --log('申请者列表')
        offsetSize = 0
        listInfo = FriendData:onGetApplyList()
        self.ui.TxtTipTxtEx.text  = UIText('ui_addfriends_00020')..'\n'..UIText('ui_addfriends_00021')--'You Don’t have any friend application go to invite friends to stars a challenge!'
        self.ui.BtnAdd1RectTf.gameObject:SetActive(false)
        self.ui.BtnAdd3RectTf.gameObject:SetActive(true)
        self.ui.SortContentRectTf.gameObject:SetActive(false)
        self.ui.FriendContentRectTf.gameObject:SetActive(true)
        self.ui.AddFriendContentRectTf.gameObject:SetActive(false)
    end

    if listInfo then
        self.ui.NoFriendContentRectTf.gameObject:SetActive(#listInfo == 0)

        --刷新列表
        if self.ui.ScrollViewRectTf.offsetMax then
            self.ui.ScrollViewRectTf.offsetMax = Vector2(self.ui.ScrollViewRectTf.offsetMax.x, offsetSize)
        end
        self:onBindData(listInfo, 'Content')
        self:onRefresh_Content()
    end
end


function FriendListView:onChangeButtonStyle(btnType)
    local doSetSprite = function(index, spriteName, outlineColor, shadowColor)
        local btnImg = index == 1 and self.ui.BtnLeftImgEx or self.ui.BtnRightImgEx
        UITool:SetSprte(btnImg, 'FriendListView/'..spriteName)

        local outline, shadow
        if index == 1 then
            outline = self.ui.LeftTextGo.transform:GetComponent(typeof(Outline))
            shadow = self.ui.LeftTextGo.transform:GetComponent(typeof(Shadow))
        else
            outline = self.ui.RightTextGo.transform:GetComponent(typeof(Outline))
            shadow = self.ui.RightTextGo.transform:GetComponent(typeof(Shadow))
        end

        outline.effectColor = UIColor:GetColorv2(outlineColor)
        shadow.effectColor = UIColor:GetColorv2(shadowColor)
    end

    if btnType == 'Left' then
        doSetSprite(1, 'common_friend_btn1', '#CA8B2E', '#A36D10')
        doSetSprite(2, 'common_friend_btn2', '#5A53D2', '#3D3AA1')
    elseif btnType == 'Right' then
        doSetSprite(2, 'common_friend_btn1', '#CA8B2E', '#A36D10')
        doSetSprite(1, 'common_friend_btn2', '#5A53D2', '#3D3AA1')
    end
end


function FriendListView:onChanged_Content(index, data, obj)
    local playerGrid = obj.transform:Find('Inner/PlayerGrid').gameObject
    local playerIndex = obj.transform:Find('Inner/Index'):GetComponent(typeof(TextEx))
    local playerName = obj.transform:Find('Inner/PlayerName'):GetComponent(typeof(TextEx))
    local friendRoot = obj.transform:Find('Inner/FriendRoot')
    local powerRoot = obj.transform:Find('Inner/PowerRoot')

    playerIndex.text = index
    playerName.text = data.info.name
    friendRoot.gameObject:SetActive(data.root == 'FriendRoot')
    powerRoot.gameObject:SetActive(data.root == 'PowerRoot')
    PlayerGrid:onInit(playerGrid):onRefresh(data.head)

    local btnInner = obj.transform:Find('Inner'):GetComponent(typeof(ButtonEx))
    UITool:onAddClickAndClear(btnInner, function()
        NetWork:onRequest(UIProtoType.SearchFriend.protoID, {friendId = data.info.uid}, function(status, tbData)
            if status then
                LuaHelper.ShowUI(UI.PlayerInfoView, data.info, tbData)
            end
        end)
    end)

    if data.root == 'FriendRoot' then
        local playerUID = friendRoot:Find('PlayerUID'):GetComponent(typeof(TextEx))
        local playerStar = friendRoot:Find('Items/Item1/Count'):GetComponent(typeof(TextEx))
        local playerPower = friendRoot:Find('Items/Item2/Count'):GetComponent(typeof(TextEx))
        local playerLevel = friendRoot:Find('Items/Item3/Count'):GetComponent(typeof(TextEx))
        local imgLeft = friendRoot:Find('BtnLeft'):GetComponent(typeof(ImageEx))
        local imgRight = friendRoot:Find('BtnRight'):GetComponent(typeof(ImageEx))
        local setupPower = function()
            local spriteName = data.iconName[1]
            if self.tabIndex == 1 then
                spriteName = data.info.giveStrength == 1 and 'friend_life3' or data.iconName[1]
            end
            UITool:SetSprte(imgLeft, 'FriendListView/'..spriteName, true)
            UITool:SetSprte(imgRight, 'FriendListView/'..data.iconName[2], true)
        end

        setupPower()
        playerUID.text = 'ID: <color=#A99DE9>'..data.info.uid..'</color>'
        playerStar.text = data.info.starsNum
        playerPower.text = data.info.cupNum
        playerLevel.text = data.info.level

        local btnLeft = friendRoot:Find('BtnLeft'):GetComponent(typeof(ButtonEx))
        UITool:onAddClickAndClear(btnLeft, function()
            --好友列表
            if self.tabIndex == 1 then
                NetWork:onRequest(UIProtoType.SendFriendPower.protoID, {friendId = data.info.uid}, function(status, errorID)
                    if status then
                        FriendData:onSendPower(data.info)
                        setupPower()
                        UITool:onShowTips(UIText('ui_tip_00001'))
                    else
                        if errorID == 142 then
                            UITool:onShowTips(UIText('ui_tip_00002'))
                        end
                    end
                end)

            --申请好友列表
            elseif self.tabIndex == 2 then
                NetWork:onRequest(UIProtoType.UpdateApplyList.protoID, {friendId = data.info.uid, applyState = 1}, function(status)
                    if status then
                        local element = FriendData:onDeleteElement(FriendData:onGetApplyList(), data.info, 1)
                        FriendData:onAddFriendList(element)
                        self:onEnterPage(2, 'Left')
                        UITool:onShowTips(UIText('ui_tip_00003'))
                    end
                end)
            end
        end)

        local btnRight = friendRoot:Find('BtnRight'):GetComponent(typeof(ButtonEx))
        UITool:onAddClickAndClear(btnRight, function()
            --好友列表界面
            if self.tabIndex == 1 then
                if ItemData:onCheckItemIsEnough(ItemType.POWER, 1) then
                    NetWork:onRequest(UIProtoType.InviteFriendBattle.protoID, {friendId = data.info.uid}, function(status)
                        if status then
                            UITool:onShowTips(UIText('ui_tip_00004'))
                        end
                    end)
                end

            --申请好友界面
            elseif self.tabIndex == 2 then
                NetWork:onRequest(UIProtoType.UpdateApplyList.protoID, {friendId = data.info.uid, applyState = 2}, function(status)
                    if status then
                        FriendData:onDeleteElement(FriendData:onGetApplyList(), data.info, 1)
                        self:onEnterPage(2, 'Right')
                        UITool:onShowTips(UIText('ui_tip_00005'))
                    end
                end)
            end
        end)

    --领取体力界面
    elseif data.root == 'PowerRoot' then
        local btnRight = powerRoot:Find('BtnLove'):GetComponent(typeof(ButtonEx))
        UITool:onAddClickAndClear(btnRight, function()
            NetWork:onRequest(UIProtoType.CliamFriendPower.protoID, {friendId = data.info.uid}, function(status, tbData)
                if status then
                    FriendData:onDeleteElement(FriendData:onGetCliamList(), data.info, 2)
                    self:onEnterPage(1, 'Right')
                    UITool:onShowTips(UIText('ui_tip_00006'))
                end
            end)
        end)
    end
end


--[[
    @desc: 列表排序
    time:2022-05-05 11:48:05
    --@sortIndex: 分类下标
]]
function FriendListView:onClickSortEventHandler(sortIndex)
    self.preSortIndex = self.preSortIndex or 1
    if self.preSortIndex == sortIndex then return end
    if self.preSortIndex then
        self.sortList[self.preSortIndex].btnImage.enabled = false
        UITool:SetSprte(self.sortList[self.preSortIndex].icon, 'FriendListView/friend_'..self.preSortIndex..'_1')
    end

    self.sortList[sortIndex].btnImage.enabled = true
    UITool:SetSprte(self.sortList[sortIndex].icon, 'FriendListView/friend_'..sortIndex..'_2')
    self.preSortIndex = sortIndex
    FriendData:onSortFriendList(sortIndex)
    self:onEnterPage(1, 'Left')
end


function FriendListView:onClick_BtnLeft()
    self:onEnterPage(self.tabIndex, 'Left')
end


function FriendListView:onClick_BtnRight()
    self:onEnterPage(self.tabIndex, 'Right')
end


--[[
    @desc: 添加好友入口1 - 没有好友时
    time:2022-05-05 13:57:33
]]
function FriendListView:onClick_BtnAdd1()
    LuaHelper.ShowUI(UI.FriendAddView)
end


--[[
    @desc: 添加好友入口2 - 搜索好友界面按钮
    time:2022-05-05 13:57:33
]]
function FriendListView:onClick_BtnAdd2()
    LuaHelper.ShowUI(UI.FriendAddView)
end

--[[
    @desc: 好友邀请入口1 - 没有申请列表时
    time:2022-05-05 13:57:33
]]
function FriendListView:onClick_BtnAdd3()
    LuaHelper.ShowUI(UI.FriendSharpView)
end


--[[
    @desc: 好友邀请入口2 - 搜索好友界面按钮
    time:2022-05-05 13:57:33
]]
function FriendListView:onClick_BtnInvite()
    LuaHelper.ShowUI(UI.FriendSharpView)
end


--[[
    @desc: 复制UID
    time:2022-05-06 17:58:22
]]
function FriendListView:onClick_BtnCopy()
    UnityEngine.GUIUtility.systemCopyBuffer = string.Split(self.ui.TxtUserInfoTxtEx.text,':')[2]
    UITool:onShowTips(UIText('ui_tip_00007'))
end

return FriendListView