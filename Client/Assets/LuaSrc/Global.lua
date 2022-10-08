---
--- Created by Rubble.
--- DateTime: 2021/6/5 16:00
--- Description: 全局文件

socket                  = require("socket")
APP 			        = CS.App
UnityEngine             = CS.UnityEngine
Debug 			        = UnityEngine.Debug
LuaHelper               = CS.LuaHelper
Util                    = CS.Framework.Util
PathUtil                = CS.Framework.PathUtil
LuaInstance             = CS.Lua
AudioSource             = UnityEngine.AudioSource
RectTransform           = UnityEngine.RectTransform
Renderer                = UnityEngine.Renderer
UnityWebRequest         = UnityEngine.Networking.UnityWebRequest
DownloadHandlerBuffer   = UnityEngine.Networking.DownloadHandlerBuffer
WWWForm                 = UnityEngine.WWWForm
SDK                     = CS.SDKManager
DGRotateMode            = CS.DG.Tweening.RotateMode
DOTween                 = CS.DG.Tweening.DOTween
--Tweener                 = CS.DG.Tweening.Tweener
Vector2                 = UnityEngine.Vector2
Vector3                 = UnityEngine.Vector3
Quaternion              = UnityEngine.Quaternion
Color                   = UnityEngine.Color
Color32                 = UnityEngine.Color32
EventTriggerType        = UnityEngine.EventSystems.EventTriggerType
Animator                = UnityEngine.Animator
Space                   = UnityEngine.Space
GameObject              = UnityEngine.GameObject
Camera                  = UnityEngine.Camera
Rigidbody               = UnityEngine.Rigidbody
Input                   = UnityEngine.Input
Button                  = UnityEngine.UI.Button
LayoutElement           = UnityEngine.UI.LayoutElement
ButtonEx                = CS.Framework.ButtonEx
ImageEx                 = CS.Framework.ImageEx
TextEx                  = CS.Framework.TextEx
GameTween               = CS.GameTween
ScrollPool              = CS.ScrollPool
ScrollPoolGrid          = CS.ScrollPoolGrid
ScrollPoolVertical      = CS.ScrollPoolVertical
ScrollPoolHorizontal    = CS.ScrollPoolHorizontal
Outline                 = UnityEngine.UI.Outline
Shadow                  = UnityEngine.UI.Shadow
Rigidbody               = UnityEngine.Rigidbody
InputField              = CS.Framework.InputFieldEx
Screen                  = UnityEngine.Screen


--参数解包
unpack = table.unpack

--字符串空字符串
string.Empty = ""

--字符串是否为空
function string.IsNullOrEmpty(str)
    return nil == str or string.Empty == str
end

--字符串是否不为空
function string.IsNotNullOrEmpty(str)
    return not string.IsNullOrEmpty(str)
end

--字符串首字母大写
function string.firstToUpper(str)
    return str:gsub("^%l", string.upper)
end

--普通日志
function log(format, ...)
    Debug.Log(debug.traceback(string.format("LUA: "..format, ...)))
end

--警告日志
function logWarning(format, ...)
    Debug.LogWarning(debug.traceback(string.format("LUA: "..format, ...)))
end

--错误日志
function logError(format, ...)
    Debug.LogError(debug.traceback(string.format("LUA: "..format, ...)))
end

--xpcall异常
function logTrace(error)
    if string.IsNotNullOrEmpty(error) then
        logError(error)
    end
end

local Time = {
    --运行到现在的时间
    realtimeSinceStartup = 0.0,
    --运行到现在的帧数
    frameCount = 0,
    --当前的无压缩增量时间
    unscaledDeltaTime = 0.0,
    --当前增量时间
    deltaTime = 0.0,
}

--设置当前已运行的时间
function SetRealtimeSinceStartup(realtimeSinceStartup)
    Time.realtimeSinceStartup = realtimeSinceStartup
end

--得到当前已运行的时间
function GetRealtimeSinceStartup()
    return Time.realtimeSinceStartup
end

--设置当前帧数
function SetFrameCount(frameCount)
    Time.frameCount = frameCount
end

--得到当前帧数
function GetFrameCount()
    return Time.frameCount
end

--设置当前无压缩的增量时间
function SetUnscaledDeltaTime(unscaledDeltaTime)
    Time.unscaledDeltaTime = unscaledDeltaTime
end

--得到当前无压缩的增量时间
function GetUnscaledDeltaTime()
    return Time.unscaledDeltaTime
end

--设置当前增量时间
function SetDeltaTime(deltaTime)
    Time.deltaTime = deltaTime
end

--得到当前增量时间
function GetDeltaTime()
    return Time.deltaTime
end

-- 克隆类
local function Clone(c, selfClass)
    local tb = {}
    local class = selfClass or tb
    for k, v in pairs(c) do
        if k == "super" then
            tb[k] = Clone(v, class)
            setmetatable(tb, { __index = tb[k]})
        else
            tb[k] = v
        end
    end

    tb.class = class
    return tb
end

--新New一个类
function New(c)
    local tb = Clone(c)
    if nil ~= tb.Ctor then
        tb:Ctor()
    end
    return tb
end

--新new一个类
function new(c)
    return New(c)
end

--绑定一个类
function bind(component, class, ...)
    local table = new(class)
    table:onInit(component, ...)
    return table
end

--类或方法继承
function class(className, super)
    super = super or {}
    local superType = type(super)
    local cls = { className = className }
    -- 基类限定
    if superType ~= "function" and superType ~= "table" then
        superType = nil
        super = nil
    end

    -- 复制基类方法
    if superType == "function" then
        setmetatable(cls, { __index = super })
        cls.super = super
    elseif superType == "table" then
        cls.super = Clone(super, cls)
        setmetatable(cls, { __index = cls.super })
        if nil ~= cls.Ctor then
            cls:Ctor()
        end
    end

    --新New一个类
    function cls.New(className)
        local instance = New(cls)
        if nil ~= className then
            instance.className = className
        end
        return instance
    end

    --新new一个类
    function cls.new(className)
        return cls.New(className)
    end

    return cls
end

function clone(object)
    local lookup_table = {}
    local function copyObj( object )
        if type( object ) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs( object ) do
            new_table[copyObj( key )] = copyObj( value )
        end
        return setmetatable( new_table, getmetatable( object ) )
    end
    return copyObj( object )
end

--全局导入
function GlobalRequire()
    --禁止加入其他模块
    if not _G.Json then
        _G.Json = require "Common.Json"
        require("Common.functions")
        require("Common.Config")
        require("Common.Event")
        require("Common.EventManager")
        require("Common.Timer")
        require("Common.coroutine")

        require("UI.Common.UICommon")
        require("UI.Common.UIBase")
        require("Data.PlayerData")

        require("Common.Util")
        require("Common.AudioManager")
        require("Common.HttpTool")
        require("Logic.PlayerLogic")
    end
end

--准备导入
function Require()
    if not _G.RequireComplete then
        _G.RequireComplete = true

        require("Common.NetWork")
        require("Common.RedDot")
        require("Data.PlayerData")
        require("Data.ItemData")
        require("Data.RankData")
        require("Data.RecordData")
        require("Data.ShopData")
        require("Data.FriendData")
        require("Data.MatchData")
        require("Data.SignData")
        require("Data.ReconnectionData")
        require("UI.Common.UIWebConfig")
        require("UI.Common.UITool")
        require("UI.Common.UIColor")
        require("UI.Common.IAPManager")
        require("UI.Golobal.Component.MainTabBar")
        require("UI.Golobal.Component.MainStausBar")
        require("UI.Golobal.Component.MatchItemBar")
        require("UI.Golobal.Component.PlayerGrid")
        require("MatchGame.base.MatchManager")
        require("MatchGame.MatchProtoRequest")
        require("MatchGame.MatchItem")
    end
end