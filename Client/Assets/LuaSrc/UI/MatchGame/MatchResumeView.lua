--[[
Description: 主玩法游戏暂停界面
Author: xinZhao
Date: 2022-04-26 15:02:45
LastEditTime: 2022-04-26 15:02:45
--]]

MatchResumeView = class('MatchResumeView', UIBase)

function MatchResumeView:OnShow()
    --暂停计时器
    MatchManager:onSetTimerStatus(false)

    self:onRefreshAudioHandler('music')
    self:onRefreshAudioHandler('audio')
end


function MatchResumeView:onClick_BtnClose()
    self:onClose()
end


--[[
    @desc: 返回主玩法
    time:2022-04-26 15:06:26
]]
function MatchResumeView:onClick_BtnResume()
    self:onClose()
end


--[[
    @desc: 返回主界面
    time:2022-04-26 15:06:01
]]
function MatchResumeView:onClick_BtnQuit()
    if MatchProtoRequest.BattleType == BattleType.PVE then
        MatchManager:onOffMatchGame(true)
        UITool:onShowTips('您现在退出游戏，体力将被扣除')

    elseif MatchProtoRequest.BattleType == BattleType.PVP then
        local parmas = {
            starNum = 0,
            isWin = 0,
        }
    
        NetWork:onRequest(UIProtoType.PVPFightEnd.protoID, parmas, function(reslut, data)
            if reslut then
                MatchData:onSetPVPRoom(nil)
                MatchManager:onOffMatchGame(true)
            end
        end)
    elseif MatchProtoRequest.BattleType == BattleType.FRIEND then
        NetWork:onRequest(UIProtoType.FriendFightEnd.protoID, {
            starNum = 0,
            isWin = 0,
            roomId = MatchManager.roomId or ''
        }, function(status)
            if status then
                MatchManager:onOffMatchGame(true)
            end
        end)
    end
end


------------------------------------------------------------------
------------------------------------------------------------------
-------------------------------------------------BGM/音效开关部分--
function MatchResumeView:onClick_BtnMusic()
    AudioManager:onSetMusicValve(not AudioManager.musicValve)
    self:onRefreshAudioHandler('music')
end


function MatchResumeView:onClick_BtnAudio()
    AudioManager:onSetAudioValve(not AudioManager.audioValve)
    self:onRefreshAudioHandler('audio')
end


function MatchResumeView:onRefreshAudioHandler(type)
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


function MatchResumeView:OnHide()
    MatchManager:onSetTimerStatus(true)
end

return MatchResumeView
