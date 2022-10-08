AudioManager = {}

local MAX_BGM_VOLUME = 0.35
local MAX_AUDIO_VOLUME = 0.8

AudioManager.Bgm = {
    BGM_Login  = 'Log_in',     --登录BGM
    BGM_1      = 'Homebgm',    --游戏内BGM1
    BGM_2      = 'Homebgam3',  --游戏内BGM2
    BGM_MAP_1  = 'Gamebgm1',   --战斗内BGM1
    BGM_MAP_2  = 'Gamebgm2',   --战斗内BGM2
}

AudioManager.Audio = {
    Click          = 'button',
    Switch_TAB     = 'switch_tab',     --主界面页签切换
    Battle_Win     = 'winning',        --战斗胜利
    Battle_Lose    = 'Gameover',       --战斗失败
    Refresh_Use    = 'Refresh_Use',    --重置道具
    Recive_All     = 'Task_get_res',   --收集完所有道具
    Battle_Start   = 'GameStart',      --战斗开始(除开第一关)
    Battle_Start_1 = 'Level1',         --战斗开始(第一关)
    Magnifier_Use  = 'magnifier_use',  --使用放大镜
    Task_Click     = 'Task_Click',     --点击任务元素
    Get_Stars      = 'Get_Stars',      --获得星星
}

---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 16:49
---@see 初始化音效
function AudioManager.Init()
    AudioManager.audioListener = LuaHelper.Instantiate(nil, "Audio Listener", nil)
    LuaHelper.DontDestroyOnLoad(AudioManager.audioListener)

    AudioManager.BgmClip = AudioManager.CreateBgm()
    AudioManager.AudioClip = AudioManager.CreateAudio()
    AudioManager:onGetAudioValve()
    AudioManager.PlayMusic(AudioManager.Bgm.BGM_Login)
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 17:14
---@see 创建一个背景声音源
function AudioManager.CreateBgm()
    local audio = AudioManager.audioListener:AddComponent(typeof(AudioSource))
    audio.minDistance = 480
    audio.enabled = true
    audio.playOnAwake = false
    audio.loop = true
    audio.volume = AudioManager.musicValve and MAX_BGM_VOLUME or 0

    local sound = {}
    sound.audio = audio

    return sound
end

---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 17:27
---@see 创建一个音效声音源
function AudioManager.CreateAudio()
    local audio = AudioManager.audioListener:AddComponent(typeof(AudioSource))
    audio.minDistance = 480
    audio.enabled = true
    audio.playOnAwake = false
    audio.loop = false
    audio.volume = AudioManager.musicValve and MAX_AUDIO_VOLUME or 0

    local sound = {}
    sound.audio = audio

    return sound
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 16:43
---@see 播放音乐
function AudioManager.PlayMusic(name)
    local bundlePath = 'Res/Audio/Bgm/'..name..'.mp3'

    local volume = AudioManager.musicValve and MAX_BGM_VOLUME or 0
    if volume then
        LuaHelper.LoadObjectFromPool(bundlePath, function (clipAudio)
            if clipAudio then
                AudioManager.BgmClip.audio.clip = clipAudio
                AudioManager.BgmClip.audio.volume = volume
                AudioManager.BgmClip.audio.loop = true
                AudioManager.BgmClip.audio:Play()
                AudioManager.BgmClip.name = name
            end
        end)
    end
end

---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 16:43
---@see 停止音乐
function AudioManager.StopMusic()
    AudioManager.BgmClip.audio:Stop()
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/4/22 19:33
---@see 暂停音乐
function AudioManager.PauseMusic()
    AudioManager.BgmClip.audio:Pause()
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/4/22 19:32
---@see 重启音乐
function AudioManager.UnPauseMusic()
    AudioManager.BgmClip.audio.volume = MAX_BGM_VOLUME
    --AudioManager.BgmClip.audio:UnPause()
    AudioManager.BgmClip.audio:Play()
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 17:56
---@see 播放音效
function AudioManager.PlaySound(name)
    local bundlePath = 'Res/Audio/Audio/'..name..'.mp3'

    local volume = AudioManager.audioValve and MAX_AUDIO_VOLUME or 0
    if volume > 0 then
        LuaHelper.LoadObjectFromPool(bundlePath, function (clipAudio)
            if clipAudio then
                AudioManager.AudioClip.audio.clip = clipAudio
                AudioManager.AudioClip.audio.volume = volume
                AudioManager.AudioClip.audio.loop = false
                AudioManager.AudioClip.audio:Play()
                AudioManager.AudioClip.name = name
            end
        end)
    end
end


---@module AudioManager.lua
---@author Rubble
---@since 2022/3/28 17:55
---@see 停止音效播放
function AudioManager.StopSound()
    AudioManager.AudioClip.audio:Stop()
end


--[[
    @desc: 获取音效阀门开关
    time:2022-05-20 11:44:18
]]
function AudioManager:onGetAudioValve()
    self.musicValve = LuaHelper.GetString('music')
    if self.musicValve == '0' then
        self.musicValve = false
    else
        self.musicValve = true
    end

    self.audioValve = LuaHelper.GetString('audio')
    if self.audioValve == '0' then
        self.audioValve = false
    else
        self.audioValve = true
    end
end


--[[
    @desc: 设置BGM阀门开关
]]
function AudioManager:onSetMusicValve(valve)
    local musicValue = valve and '1' or '0'
    LuaHelper.SetString('music', musicValue)
    self.musicValve = valve

    if not self.musicValve then
        AudioManager.StopMusic()
    else
        AudioManager.UnPauseMusic()
    end
end


--[[
    @desc: 设置音效阀门开关
]]
function AudioManager:onSetAudioValve(valve)
    local audioValve = valve and '1' or '0'
    LuaHelper.SetString('audio', audioValve)
    self.audioValve = valve

    if not self.musicValve then
        AudioManager.StopSound()
    end
end


function AudioManager:onPlayGameBGM(type)
    math.randomseed(os.clock() * 1000000)
    local randomCount = math.random(1, 2)
    if type == BgmType.Gmae then
        AudioManager.PlayMusic(AudioManager.Bgm['BGM_'..randomCount])
    elseif type == BgmType.Map then
        AudioManager.PlayMusic(AudioManager.Bgm['BGM_MAP_'..randomCount])
    end
end


--[[
    @desc: 设置音量
    time:2022-05-26 11:34:23
]]
function AudioManager:onSetAudioVolume()
    MAX_BGM_VOLUME = Config.tbCommon['BGM'].value
    MAX_AUDIO_VOLUME = Config.tbCommon['Audio'].value
end