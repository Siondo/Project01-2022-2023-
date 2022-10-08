--[[
Description: 添加好友界面
Author: xinZhao
Date: 2022-05-05 11:33:37
LastEditTime: 2022-04-06 11:33:37
--]]

FriendAddView = class('FriendAddView', UIBase)

function FriendAddView:OnLoad()
    self.inputUID = ''
end


function FriendAddView:onInputChanged_InputField(word)
    self.inputUID = word
end


function FriendAddView:onClick_BtnAdd()
    if not IsEmptyStringOrNull(self.inputUID) then
        local parmas = {
            friendId = tonumber(self.inputUID)
        }

        NetWork:onRequest(UIProtoType.SearchFriend.protoID, parmas, function(status, data)
            if status then
                NetWork:onRequest(UIProtoType.AddFriend.protoID, parmas, function(status, data)
                    if status then
                        UITool:onShowTips(UIText('ui_tip_00026'))
                        self:onClose()
                    end
                end)
            else
                if data == 5 then
                    UITool:onShowTips(UIText('ui_tip_00027'))
                elseif data == 134 then
                    UITool:onShowTips(UIText('ui_tip_00028'))
                end
            end
        end)

    else
        UITool:onShowTips(UIText('ui_tip_00029'))
    end
end


function FriendAddView:onClick_BtnClose()
    self:onClose()
end

return FriendAddView