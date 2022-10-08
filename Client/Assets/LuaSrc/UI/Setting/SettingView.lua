--[[
    author:{gq}
    time:2022-04-21 14:26:46
]]
local SettingView = class('SettingView', UIBase)

function SettingView:OnLoad()
end

function SettingView:OnShow()
    self.ui.TitleTxtEx.text="Setting"
    self.ui.SettingContentGo:SetActive(true)
    self.ui.HelpContentGo:SetActive(false)
    self.ui.ContactContentGo:SetActive(false)

    self:onRefreshAudioHandler('music')
    self:onRefreshAudioHandler('audio')
end


function SettingView:onClick_HelpBtn()
    self.ui.TitleTxtEx.text = UIText('ui_help_00001')
    self.ui.SettingContentGo:SetActive(false)
    self.ui.HelpContentGo:SetActive(true)
    self.ui.ContactContentGo:SetActive(false)
end

--[[setting]]
function SettingView:onClick_TermsBtn()
    --打开条款和隐私，跳转至条款和隐私网页
end

function SettingView:onClick_AccountBtn()
    LuaHelper.ShowUI(UI.AccountView)
end

function SettingView:onClick_FacebooktBtn()
    --脸书
end

function SettingView:onClick_AppletBtn()
    --苹果
end

function SettingView:onClick_AppletBtn()
    --谷歌
end

--[[Help and Support]]
function SettingView:onClick_GamePlayBtn()
    UnityEngine.Application.OpenURL("http://jiujiukyi.com/GamePlayGuide.html");
end

function SettingView:onClick_EventsBtn()
    UnityEngine.Application.OpenURL("http://jiujiukyi.com/GamePlayGuide.html");
end

function SettingView:onClick_CoinsBtn()
    UnityEngine.Application.OpenURL("http://jiujiukyi.com/GamePlayGuide.html");
end

function SettingView:onClick_AccountManageBtn()
    UnityEngine.Application.OpenURL("http://jiujiukyi.com/GamePlayGuide.html");
end

function SettingView:onClick_ContactBtn()
    self.ui.TitleTxtEx.text = UIText('ui_contact_00001')
    self.ui.SettingContentGo:SetActive(false)
    self.ui.HelpContentGo:SetActive(false)
    self.ui.ContactContentGo:SetActive(true)
end

--[[Contact us]]
function SettingView:onClick_InGameBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00002'))
end

function SettingView:onClick_AdvertiseBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00003'))
end

function SettingView:onClick_GameStuckBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00004'))
end

function SettingView:onClick_AccountSignBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00005'))
end

function SettingView:onClick_OtherBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00006'))
end

function SettingView:onClick_FeedbackBtn()
    LuaHelper.SendMail(Config.tbCommon['Email'].value1, UIText('ui_contact_00007'))
end

function SettingView:onClick_BtnClose()
    LuaHelper.HideUI(UI.SettingView)
end

------------------------------------------------------------------
------------------------------------------------------------------
-------------------------------------------------BGM/音效开关部分--
function SettingView:onClick_BtnMusic()
    AudioManager:onSetMusicValve(not AudioManager.musicValve)
    self:onRefreshAudioHandler('music')
end


function SettingView:onClick_BtnAudio()
    AudioManager:onSetAudioValve(not AudioManager.audioValve)
    self:onRefreshAudioHandler('audio')
end


function SettingView:onRefreshAudioHandler(type)
    if type == 'music' then
        self.ui.MusicOffGo:SetActive(not AudioManager.musicValve)
        self.ui.MusicOnGo:SetActive(AudioManager.musicValve)
    else
        self.ui.AudioOffGo:SetActive(not AudioManager.audioValve)
        self.ui.AudioOnGo:SetActive(AudioManager.audioValve)
    end
end
------------------------------------------------------------------
------------------------------------------------------------------
------------------------------------------------------------------

return SettingView