local MainTipView = class('MainTipView', UIBase)

function MainTipView:OnLoad()
    --代码
end

function MainTipView:onClick_BtnClose()
    self:onClose()
end


function MainTipView:OnUpdate()
    log('123')
end

return MainTipView