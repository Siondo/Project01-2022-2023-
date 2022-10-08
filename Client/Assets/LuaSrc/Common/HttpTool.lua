HttpTool = {}

local WebType = {
    GetAppInfo = "/configure/info",
}

---@module HttpTool.lua
---@author Rubble
---@since 2022/1/26 16:16
---@see POST发送
local function PostSend(url, requestHeader, form, callback, interval, requestCount, questTimes)
    local delayTime = 0
    questTimes = questTimes or 0
    -- LuaHelper.ShowUI(UI.ScreenLockView) --锁屏处理

    interval = interval or 1
    requestCount = requestCount or 6

    requestCount = requestCount - 1
    local c = coroutine.start(function ()
        -- local tipStr = questTimes == 0 and '网络协议请求中...' or '正在尝试第'..questTimes..'次重新请求...'
        -- UIEvent:OnEvent(UIEvent.ScreenLockView.ScreenLockTips, tipStr)
    
        -- questTimes = questTimes + 1
        -- if questTimes == 6 then
        --     UIEvent:OnEvent(UIEvent.ScreenLockView.ScreenLockTips, '网络协议请求失败, 请检查网络状态')
        -- end
    
        local webRequest = UnityWebRequest.Post(url, form)
        webRequest.downloadHandler = DownloadHandlerBuffer()
        for k, v in pairs(requestHeader) do
            webRequest:SetRequestHeader(k, v)
        end
        webRequest:SendWebRequest()
        while (true) do
            if webRequest.isDone then break end
            coroutine.step()
        end
        if string.IsNullOrEmpty(webRequest.error) then
            requestCount = 0
            callback(true, webRequest.downloadHandler.text)
            -- LuaHelper.HideUI(UI.ScreenLockView)
        elseif requestCount <= 0 then
            callback(false, webRequest.error)
            -- LuaHelper.HideUI(UI.ScreenLockView)
        end

        webRequest:Dispose()
        webRequest = nil

        if requestCount > 0 then
            coroutine.wait(interval)
            PostSend(url, requestHeader, form, callback, interval, requestCount, questTimes)
        end
    end)
end

---@module HttpTool.lua
---@author Rubble
---@since 2022/1/26 17:23
---@see 得到签名
local function GetSignature(post, random32, noncestime)
    local str = Util.GetStringBuilder(post, PlayerData:getUserID(), random32, noncestime, PlayerData:getSecrect())
    str = Util.GetTextMD5(str)
    str = string.upper(str)
    return str
end


---@module HttpTool.lua
---@author Rubble
---@since 2022/2/10 14:55
---@see 得到表单数据
local function GetFormData(dict)
    local post = string.Empty
    local form = WWWForm()
    local parmasToStr = ''
    if nil ~= dict then
        local list = {}
        for k, v in pairs(dict) do
            form:AddField(k, v)
            table.insert(list, k)
        end
        table.sort(list)
        for i, v in ipairs(list) do
            post = post..tostring(dict[v])
            parmasToStr = parmasToStr..v..' = "'..tostring(dict[v])..'"'
            parmasToStr = i == #list and parmasToStr or parmasToStr..', '
        end
    end

    return post, form, parmasToStr
end


---@module HttpTool.lua
---@author Rubble
---@since 2022/1/26 16:24
---@see POST
local function Post(methodName, dict, callback, isGmTools)
    local post, form, parmasToStr = GetFormData(dict)
    local address = APP.address
    if string.IsNullOrEmpty(address) then
        address = APP.url
    end
    local url = string.format("%s%s", address, methodName)

    local random32 = Util.Get32Random()
    local noncestime = tostring(Util.Now())
    local requestHeader = {}
    requestHeader["platform"] = Util.GetPlatform()
    requestHeader["version"] = APP.innerVersion
    requestHeader["uid"] = isGmTools and -99 or PlayerData:getUserID()
    requestHeader["deviceId"] = APP.deviceId
    requestHeader["channel"] = ""
    requestHeader["deviceBrand"] = APP.deviceBrand
    requestHeader["nonce"] = random32
    requestHeader["noncestime"] = noncestime
    requestHeader["signature"] = GetSignature(post, random32, noncestime)

    local _callback = function(result, tbData)
        local jsonData = Json.decode(tbData)
        if result and 200 == jsonData.status then
            log('<color=#0066FF>[接受]</color> 协议名称:(%s) 返回数据流: %s', methodName, tbData)
            callback(result, jsonData.data, jsonData.status)
        else
            log('<color=#FF0000>[接受]</color> 协议名称:(%s) 请求失败状态码:(%d) 错误信息: %s', methodName, jsonData.status, jsonData.errorMsg)
            callback(false, jsonData.errorMsg, jsonData.status)
        end
    end

    log("<color=#008800>[请求]</color> 协议名称:(%s) 传入数据流: {%s}", methodName, parmasToStr)

    PostSend(url, requestHeader, form, _callback)
end


--[[
    @desc: 请求服务器端协议数据
    time:2022-04-13 17:02:27
    --@webType: 网络协议类型
	--@parmas:  网络协议数据 (没有参数的协议 传入nil或者{}都可以)
	--@callBack: 回调
]]
function HttpTool.SendRequest(uiWebType, parmas, callBack, isGmTools)
    Post(uiWebType, parmas or {}, callBack, isGmTools)
end