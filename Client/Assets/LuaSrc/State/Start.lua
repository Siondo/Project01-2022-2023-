
local Start = class("Start", StateMachineBase)

function Start:OnEnter()
    self.super:OnEnter()
    self:Require()
    LuaHelper.targetFrameRate = Const.TARGETFRAMERATE

    -- AudioManager:onSetAudioVolume()
    -- MatchManager:onInit()
    LuaHelper.HideDisplayLogo()
end

--准备导入
function Start:Require()
    require("Common.HttpTool")
    require("Common.RedDot")
    require("Common.NetMessage")
    require("Logic.GlobalLogic")
end

return Start