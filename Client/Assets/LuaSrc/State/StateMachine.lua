StateMachineBase = {}
function StateMachineBase:OnEnter()
    log("Switch To "..StateMachine:GetCurrentState().className.." State!")
end
function StateMachineBase:OnUpdate() end
function StateMachineBase:OnExit() end

StateMachine = { states = {} }

function StateMachine:OnEnter(state)
    if nil ~= self.current then
        self.current:OnExit()
    end
    self.current = self.states[state]
    if nil == self.current then
        self.current = require(state)
    end
    self.current:OnEnter()
end

function StateMachine:GetCurrentState()
    return self.current
end

function StateMachine:OnUpdate()
    self.current:OnUpdate()
end

function StateMachine:OnExit()
    self.current:OnExit()
end