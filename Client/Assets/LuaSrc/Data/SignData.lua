--[[
Description: 签到数据类
Author: xinZhao
Date: 2022-05-17 14:00:54
--]]
SignData = class('SignData')

function SignData:onPullSignData()
    self.signInfos = {}
    NetWork:onRequest(UIProtoType.GetSignInfo.protoID, nil, function(status, tbData)
        if status then
            self:onSetInfo(tbData)
            for i, config in UITool:PairsBykeys(Config.tbSignReward) do
                self.signInfos[config.SignDays] = config
            end
        end
    end)
end


--[[
    @desc: 设置签到数据
    time:2022-05-17 15:34:19
]]
function SignData:onSetInfo(tbData)
    if self.signInfos then
        self.signInfos.current = tbData.signDay
        self.signInfos.cycle = tbData.signResetNum
        self:onSetSignState(tbData.signState)
    end
end


--[[
    @desc: 获取签到数据
    time:2022-05-17 15:34:30
]]
function SignData:onGetInfo()
    return self.signInfos
end


--[[
    @desc: 设置签到状态
    time:2022-05-17 15:34:43
    --@state: false(不能签到)/true(可以签到)
]]
function SignData:onSetSignState(state)
    self.signInfos.states = state
end


--[[
    @desc: 手动点击签到数据更新
    time:2022-05-17 15:35:08
]]
function SignData:onSign(signDay, signState)
    self:onSetSignState(signState)
    self.signInfos.current = signDay
end


function SignData:onSignTest()
    if self.signInfos.current < 7 then
        self.signInfos.current = self.signInfos.current + 1
    else
        self.signInfos.cycle = self.signInfos.cycle + 1
        self.signInfos.current = 0
    end
end

return SignData