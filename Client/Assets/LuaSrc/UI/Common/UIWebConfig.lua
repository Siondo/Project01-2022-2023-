--短链接
UIWebType =
{
    GetAppInfo          = "/configure/info",        --获取端口数据

    HeartBeat           = "/user/heartbeat",        --心跳
    Register            = '/user/sendMail',         --注册账号
    RemoveAccount       = '/user/removePlayer',     --删除账号
    ForgetPassword      = '/user/resetMailPwd',     --忘记密码
    CheckAccount        = '/user/checkMail',        --检查账户是否存在

    --GMTOOLS
    GM_AddItem          = '/gm/addItem',            --添加道具
}


--长链接
UIProtoType = {
    CommonErrorProto      = {protoID = -1,   receiveId = -1},       --通用错误协议
    Login                 = {protoID = 1001, receiveId = 1002},     --登录
    Fight                 = {protoID = 1101, receiveId = 1102},     --PVE战斗请求
    FightEnd              = {protoID = 1103, receiveId = 1104},     --PVE战斗结算
    RecordList            = {protoID = 1201, receiveId = 1202},     --战斗记录
    CliamRecord           = {protoID = 1203, receiveId = 1204},     --领取战绩奖励
    RankList              = {protoID = 1501, receiveId = 1502},     --排行榜
    Head                  = {protoID = 1301, receiveId = 1302},     --账号头像
    ShopList              = {protoID = 1701, receiveId = 1702},     --商店界面
    SearchFriend          = {protoID = 1801, receiveId = 1802},     --查询好友
    AddFriend             = {protoID = 1803, receiveId = 1804},     --添加好友
    DeleteFriend          = {protoID = 1811, receiveId = 1812},     --删除好友
    GetApplyFriendList    = {protoID = 1805, receiveId = 1806},     --获取好友申请列表
    GetFriendList         = {protoID = 1809, receiveId = 1810},     --获取好友列表
    UpdateApplyList       = {protoID = 1807, receiveId = 1808},     --更新好友列表数据
    GetFriendAllList      = {protoID = 1827, receiveId = 1828},     --获取好友所有数据
    SendFriendPower       = {protoID = 1813, receiveId = 1814},     --赠送体力
    GetFriendCliamList    = {protoID = 1815, receiveId = 1816},     --查询体力赠送列表
    CliamFriendPower      = {protoID = 1817, receiveId = 1818},     --领取体力
    InviteFriendBattle    = {protoID = 1819, receiveId = 1820},     --好友邀请战斗
    DisposeFriendInvite   = {protoID = 1821, receiveId = 1822},     --处理好友的邀请
    PVPMatchStart         = {protoID = 1401, receiveId = 1402},     --请求开始匹配
    PVPMatchQuit          = {protoID = 1407, receiveId = 1408},     --请求退出匹配
    ChangeAccountInfo     = {protoID = 1301, receiveId = 1302},     --改变用户信息
    GetSignInfo           = {protoID = 1901, receiveId = 1902},     --拉取签到数据
    SignDay               = {protoID = 1903, receiveId = 1904},     --签到
    CliamChestRewards     = {protoID = 2001, receiveId = 2002},     --领取宝箱道具
    CheckPVPRoomInfo      = {protoID = 1205, receiveId = 1206},     --查询本局匹配玩家信息

    SandBoxPay            = {protoID = 1703, receiveId = 1704},     --沙盒支付
    BuyItems              = {protoID = 1601, receiveId = 1602},     --购买道具
    UseItems              = {protoID = 1603, receiveId = 1604},     --使用道具

    PVPFightStart         = {protoID = 1403, receiveId = 1404},     --PVP战斗开始请求
    PVPFightEnd           = {protoID = 1405, receiveId = 1406},     --PVP战斗结束请求
    PVPReCoverFight       = {protoID = 1409, receiveId = 1410},     --PVP战斗重连请求
    FriendFightStart      = {protoID = 1823, receiveId = 1824},     --好友战斗开始请求
    FriendFightEnd        = {protoID = 1825, receiveId = 1826},     --好友PK战斗结束请求
}

UIProtoPushType = {
    Push_Event_FriendApply          = {protoId = 1806, cbName = 'onFriendApplyPush'},        --好友申请推送
    Push_Event_FriendList           = {protoId = 1810, cbName = 'onFriendListPush'},         --好友同意推送
    Push_Event_FriendSendPower      = {protoId = 1816, cbName = 'onFriendSendPowerPush'},    --好友赠送体力推送
    Push_Event_SignInfoUpdate       = {protoId = 1902, cbName = 'onSignInfoUpdatePush'},     --签到数据跨天自动更新

    Push_Event_Common               = {protoId = 1, cbName = 'onCommonPush'},                --好友拒绝战斗主动推送
    Push_Event_UserJoinPvP          = {protoId = 2, cbName = 'onUserJoinPvPPush'},           --其他玩家加入匹配推送
    Push_Event_InviteFriendBattle   = {protoId = 3, cbName = 'onInviteFriendBattlePush'},    --好友邀请战斗主动推送
    Push_Event_AgreeFriendInvite    = {protoId = 4, cbName = 'onAgreeFriendInvitePush'},     --好友同意战斗主动推送
    Push_Event_RefreshGameView      = {protoId = 5, cbName = 'onRefreshGameViewPush'},       --界面刷新通用刷新
    Push_Event_SeasonEnd            = {protoId = 6, cbName = 'onSeasonEndPush'},             --赛季结算推送
}