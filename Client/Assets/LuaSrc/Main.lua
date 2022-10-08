require("Global")

--状态机类型
StateMachineType = {
	InitState = "State.Init",
	VersionUpdateState = "State.VersionUpdate",
	AssetUpdateState = "State.AssetUpdate",
	AssetLoadState = "State.AssetLoad",
    StartState = "State.Start",
}

main = {}
local breakSocketHandle, debugXpCall

-- 开始
function Start()
	--编辑器模式下开启LuaDebug调试
    if LuaHelper.GetDebugModeStatus() then
		if UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor then
			log('<<<<<<* 开启断点模式 *>>>>>>>')
			breakSocketHandle, debugXpCall = require("LuaDebug")("localhost", 7003)
		end
	end

	APP.manifestMapping:TryAdd("manifest.json", "manifest")
	APP.manifestMapping:TryAdd("manifestmapping.json", "manifestmapping")
	APP.manifestMapping:TryAdd("res/lua/state/statemachine.bytes", "res/lua/state/statemachine.unity3d")
	APP.manifestMapping:TryAdd("res/lua/state/init.bytes", "res/lua/state/init.unity3d")
	APP.manifestMapping:TryAdd("res/lua/state/assetload.bytes", "res/lua/state/assetload.unity3d")
	APP.manifestMapping:TryAdd("res/lua/common/const.bytes", "res/lua/common/const.unity3d")
	nextState = StateMachineType.InitState

	-- 引入状态机
	require("State.StateMachine")
	StateMachine:OnEnter(StateMachineType.InitState)
end

--物理Update
function FixedUpdate()
	FixedUpdateEvent:Call()
end

--逻辑Update
function Update(realtimeSinceStartup, frameCount, deltaTime, unscaledDeltaTime)
	if breakSocketHandle then breakSocketHandle() end

	SetRealtimeSinceStartup(realtimeSinceStartup)
	SetFrameCount(frameCount)
	SetDeltaTime(deltaTime)
	SetUnscaledDeltaTime(unscaledDeltaTime)

	UpdateEvent:Call()
	CoroutineUpdateEvent:Call()

	StateMachine:OnUpdate()
end

--延迟更新
function LateUpdate()
	LateUpdateEvent:Call()
end

-- 销毁
function Destroy()
	StateMachine:OnExit()
end

--得到脚本
function GetScript(modname)
    return require(modname)
end

function GetLanguage(id)
	return UIText(id)
end

---@module Main.lua
---@author Rubble
---@since 2022/3/22 9:56
---@see 播放音效
function PlaySound(name)
	--AudioManager.PlaySound(name)
end

return main