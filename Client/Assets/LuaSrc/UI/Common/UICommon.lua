
UI = {
    AppSetupView             = "AppSetup/AppSetupView",
    MainView                 = "Main/MainView",
    BattleListView           = "Main/BattleListView",
    FriendListView           = "Main/FriendListView",
    FriendAddView            = 'Main/FriendAddView',
    FriendSharpView          = 'Main/FriendSharpView',
    RecordListView           = "Main/RecordListView",
    RankListView             = "Main/RankListView",
    ShopListView             = "Main/ShopListView",
    MatchMainView            = "Match/MatchMainView",
    MatchStopView            = "Match/MatchStopView",
    MatchSettlementView      = "Match/MatchSettlementView",
    MatchEndPlayerInfoView   = 'Match/MatchEndPlayerInfoView',
    MatchTimeUpView          = "Match/MatchTimeUpView",
    MatchStartView           = 'Match/MatchStartView',
    MatchResumeView          = 'Match/MatchResumeView',
    PVPMatchView             = 'Match/PVPMatchView',
    SignView                 = "Sign/SignView",
    SettingView              = "Setting/SettingView",
    AccountView              = "Account/AccountView",
    RenameView               = 'Account/RenameView',
    ReheadView               = 'Account/ReheadView',
    RegisterView             = 'Account/RegisterView',
    LoginView                = 'Account/LoginView',
    ForgetView               = 'Account/ForgetView',

    --通用界面这里添加
    SecondaryView            = "Golobal/SecondaryView",
    ScreenLockView           = "Golobal/ScreenLockView",
    BuyItemView              = 'Golobal/BuyItemView',
    SandBoxPayView           = 'Golobal/SandBoxPayView',              --沙盒支付
    PlayerInfoView           = 'Golobal/PlayerInfoView',              --玩家详情
    PlayerInviteBattleView   = 'Golobal/PlayerInviteBattleView',      --玩家邀请战斗界面
    RankRewardListView       = 'Golobal/RankRewardListView',          --排行榜奖励
    SeaonEndView             = 'Golobal/SeaonEndView',                --赛季结束
    HelpView                 = 'Golobal/HelpView',                    --全局通用帮助弹窗
    GamePoolView             = 'Golobal/GamePoolView',                --全局通用界面外部预制体生成界面
    CommonRewardView         = 'Golobal/CommonRewardView',            --全局通用奖励展示界面
}

UIEvent.AppSetupView = {
    UpdateTips = "UpdateTips",
    UpdateProgress = "UpdateProgress",
    OnLogin = 'OnLogin',
}

UIEvent.MainView = {
    JumpStore = 'JumpStore',
    RefreshStatusBar = 'RefreshStatusBar',
    GetTargetComponent = 'GetTargetComponent'
}


UIEvent.MatchMainView = {
    GetUIComponent = "GetUIComponent",
    SetUIElementCount = 'SetUIElementCount',
    SetGameOverCountDown = 'SetGameOverCountDown',
    SetUIStarCount = 'SetUIStarCount',
}

UIEvent.ScreenLockView = {
    ScreenLockTips = "ScreenLockTips"
}

UIEvent.RecordListView = {
    AddElement = 'AddElement'
}

UIEvent.ShopListView = {
    AddElement = 'AddElement'
}

UIEvent.FriendListView = {
    RefreshApplyFriendList = 'RefreshApplyFriendList',
    RefreshFriendList = 'RefreshFriendList',
    RefreshCliamList = 'RefreshCliamList'
}

UIEvent.PlayerInviteBattleView = {
    OnAddMessage = 'OnAddMessage',
    OnGetCurrentDataInfo = 'OnGetCurrentDataInfo'
}

UIEvent.PVPMatchView = {
    OnAddPVPPlayer = 'OnAddPVPPlayer'
}

UIEvent.AccountView = {
    OnRefresh = 'OnRefresh'
}

UIEvent.SignView = {
    OnRefresh = 'OnRefresh'
}

UIEvent.GamePoolView = {
    OnPushHandler = 'OnPushHandler'
}