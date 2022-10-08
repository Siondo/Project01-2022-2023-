Const = {
    MAX_LOADER = 4,                                         --最大同时加载资源数
    MAX_UPDATE = 6,                                         --最大同时下载更新资源数
    SANDBOX_VERSION = "SandboxVersion",                     --沙盒版本
    MANIFESTFILE = "manifest",                              --清单文件名
    MANIFESTFILE_JSON = "manifest.json",                    --清单文件名(带Json)
    MANIFESTFILEMAPPING = "manifestmapping",                --清单映射文件名
    MANIFESTFILEMAPPING_JSON = "manifestmapping.json",      --清单映射文件名(带Json)
    REMOTE_VERSION_DIRECTORY = "%s/%s/v%s.json",            --远程版本配置文件路径
    REMOTE_DIRECTORY = "%s/%s/%s/%s",                       --远程目录
    UPGRADE_APK = "upgrade.apk",                            --升级APK文件名
    ACCOUNT = "Account",                                    --预设帐号
    TARGETFRAMERATE = 60,                                   --目标帧率
}

MsgType = {
    Send = 1,
    Normal = 2,
    Receive = 3,
    ReceiveError = 4
}

LEVEL_ID_CONST = 3000000

--精灵图片加载路径
SPRITE_BUNDLE_PATH = 'Res/UI/Texture/'

--主界面页签类型
MAINTAB_INDEX =
{
    SHOP = 1,
    RECORD= 2,
    MAIN = 3,
    RANK = 4,
    FRIEND = 5,
}

ItemType =
{
    Magnifier            = 200001,       --放大镜
    Light                = 200004,       --灯泡
    Freezing             = 200005,       --雪花
    Reset                = 200008,       --重置
    POWER                = 200006,       --体力
    COIN                 = 200007,       --金币
    MAGENT               = 200002,       --磁铁
    INFINITEPOWER        = 200009,       --无限体力
}

BattleType =
{
    PVE    = 1,   --PVE
    PVP    = 2,   --PVP
    FRIEND = 3,   --好友对战
}


StoreType =
{
    Shop_Slot_1 = 'Shop_Slot_1',
    Shop_Slot_2 = 'Shop_Slot_2',
    Shop_Slot_3 = 'Shop_Slot_3',
    Shop_Slot_4 = 'Shop_Slot_4',
    Shop_Slot_5 = 'Shop_Slot_5',
}


ReconnectionType = {
    SeasonEndStar          = 'SeasonEndStar',           --赛季星星数结算推送
    SeasonEndTrophy        = 'SeasonEndTrophy',         --赛季奖杯数结算推送
    MatchPVPReconnect      = 'MatchPVPReconnect',       --PVP断线重连
    FriendInviteReconnect  = 'FriendInviteReconnect',   --好友邀请战斗
    FriendAgreeReconnect   = 'FriendAgreeReconnect',    --好友同意战斗
}

BgmType = {
    Gmae = 'game',
    Map = 'map'
}


GamePoolType = {
    FLY_ITEM = 'FLY_ITEM',
    TIPS_ITem = 'TIPS_ITem'
}