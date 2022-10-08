--[[
Description: 十六进制颜色值转换
Author: xinZhao
Date: 2022-04-25 14:32:54
LastEditTime: 2022-04-25 14:32:54
--]]

UIColor = class('UIColor')

--[[doc: 24位默认RGB色值]]
local channel_default = 255
local rgb_default = {
    r = channel_default,
    g = channel_default,
    b = channel_default
}
 
--[[doc: 32位默认RGB色值]]
local rgba_default = {
    r = rgb_default.r,
    g = rgb_default.g,
    b = rgb_default.b,
    a = channel_default
}

local function to_color_bit(color)
    color = tonumber(color)
    if color and color <= channel_default and channel_default >= 0 then
        return color
    end
    return channel_default
end

local function str_to_hex_num(hex)
    return tonumber(hex, 16) or channel_default
end


--[[
    @desc: 颜色值转换 [十六进制 转 (255,255,255,255)]
    time:2022-04-25 14:35:34
    --@hex: 十六进制颜色代码
    @return: (255,255,255,255)
]]
function UIColor:GetColorv1(hex)
    local len = string.len(hex)
    if len < 7 then
        return rgb_default
    end
 
    if string.sub(hex, 1, 1) ~= '#' then
        return rgb_default
    end
 
    local color = {
        r = to_color_bit(str_to_hex_num(string.sub(hex, 2, 3))),
        g = to_color_bit(str_to_hex_num(string.sub(hex, 4, 5))),
        b = to_color_bit(str_to_hex_num(string.sub(hex, 6, 7))),
        a = channel_default
    }
 
    if len == 9 then
        color.a = to_color_bit(str_to_hex_num(string.sub(hex, 8, 9)))
    end
 
    return color
end


--[[
    @desc: 颜色值转换 [十六进制 转 (0.1,0.1,0.1,0.1)]
    time:2022-04-25 14:34:18
    --@hex: 十六进制颜色代码
    @return: (0.1,0.1,0.1)
]]
function UIColor:GetColorv2(hex)
    local color = UIColor:GetColorv1(hex)
    local color_unit = {
        r = color.r / channel_default,
        g = color.g / channel_default,
        b = color.b / channel_default,
        a = color.a / channel_default,
    }
    return color_unit
end


return UIColor