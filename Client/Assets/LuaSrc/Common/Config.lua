
Config = {}
local langList = {
	'tbLang',
	'tbLangEN',
	'tbLangTC'
}

-- 初始化Lang
function Config.InitLang()
	Config.tbLang = {}

	local langType = LuaHelper.GetAppPlatform()
	LuaHelper.LoadAsset("res/Conf/lang.json", function (bResult, tbData)
		local jsonData = Json.decode(tbData.text)
		Config.tbLang = jsonData["lang"]
		LuaHelper.UnloadAsset(tbData, true)
	end, false)

	LuaHelper.LoadAsset("res/Conf/langType_"..langType..".json", function (bResult, tbData)
		local jsonData = Json.decode(tbData.text)
		Config.tbLangTC = jsonData["langType"]
		LuaHelper.UnloadAsset(tbData, true)
	end, false)
end

--初始化全局配置包
function Config.InitGlobal()
	Config.tbLang = {}
	LuaHelper.LoadAsset("res/Conf/global.json", function (bResult, tbData)
		if bResult then
			local jsonData = Json.decode(tbData.text)
			Config.tbGlobal = jsonData["global"]
			LuaHelper.UnloadAsset(tbData, true)
		end
	end, false)
end


function Config:onGetLangText(id, index)
	while index <= 3 do
		local config = Config[langList[index]]
		index = index + 1
		if config then
			local text = config[tostring(id)]
			if text then
				return text.value
			end
		end
	end
end

--文本
function UIText(id, args1, args2, args3, args4, args5, args6)
	local text = Config:onGetLangText(id, 1)
	-- if text then
	-- 	text = text[PlayerData:getLanguageName()]
	-- end
	if args1 and text then
		return string.format(string.gsub(text, "/n", "\n"), args1, args2, args3, args4, args5, args6)
	end
	return text or id
end

function Config.IsInitComplete()
	return Config.unCompleted and Config.unCompleted == 0
end

-- 初始化
function Config.Init()
	local list = Config.GetConfigList()
	local tb = {
		{
			name = "res/Conf/language.json",
			func = function (tbData)
				local tbLang = Config.tbLang
				local jsonData = Json.decode(tbData.text)
				Config.tbLang = jsonData["lang"]
				for _, v in pairs(tbLang) do
					if Config.tbLang[tostring(v.id)] == nil then
						Config.tbLang[tostring(v.id)] = v
					end
				end
				Config.unCompleted = Config.unCompleted - 1
				LuaHelper.UnloadAsset(tbData, true)
			end
		}
	}
	for _, v in ipairs(list) do
		table.insert(tb, {
			name = v.name,
			func = function (tbData)
				local jsonData = Json.decode(tbData.text)
				for _, sheet in ipairs(v.sheet) do
					Config["tb"..string.firstToUpper(sheet)] = jsonData[sheet]
				end
				Config.unCompleted = Config.unCompleted - 1
				LuaHelper.UnloadAsset(tbData, true)
			end
		})
	end
	Config.unCompleted = #tb
	return tb
end

--要加载的配置列表
function Config.GetConfigList()
	local list = {
		{
			name = "res/Conf/global.json",
			sheet = {"global"}
		},
		{
			name = "res/Conf/SignReward.json",
			sheet = {"SignReward"}
		},
		{
			name = "res/Conf/Element.json",
			sheet = {"Element"}
		},
		{
			name = "res/Conf/LevelTable.json",
			sheet = {"LevelTable"}
		},
		{
			name = "res/Conf/Items.json",
			sheet = {"Items"}
		},
		{
			name = "res/Conf/Store.json",
			sheet = {"Store"}
		},
		{
			name = "res/Conf/ItemUnlock.json",
			sheet = {"ItemUnlock"}
		},
		{
			name = "res/Conf/FriendsAward.json",
			sheet = {"FriendsAward"}
		},
		{
			name = "res/Conf/PVPAward.json",
			sheet = {"PVPAward"}
		},
		{
			name = "res/Conf/ItemUnlockPVE.json",
			sheet = {"ItemUnlockPVE"}
		},
		{
			name = "res/Conf/ItemUnlockPVP.json",
			sheet = {"ItemUnlockPVP"}
		},
		{
			name = "res/Conf/Head.json",
			sheet = {"Head"}
		},
		{
			name = "res/Conf/PVPLevel.json",
			sheet = {"PVPLevel"}
		},
		{
			name = "res/Conf/StarRankingRewardClient.json",
			sheet = {"StarRankingRewardClient"}
		},
		{
			name = "res/Conf/TrophyRewardClient.json",
			sheet = {"TrophyRewardClient"}
		},
		{
			name = "res/Conf/LevelChest.json",
			sheet = {"LevelChest"}
		},
		{
			name = "res/Conf/StarChest.json",
			sheet = {"StarChest"}
		},
		{
			name = "res/Conf/common.json",
			sheet = {"common"}
		},	
	}
	return list
end