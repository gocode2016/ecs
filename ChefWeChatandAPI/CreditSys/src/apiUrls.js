// 8001 Common类的   apirootCom
// 8002 用户类       apirootUser
// 8003 EC类		 apiroot
// 8004 积分商城   apirootSc
// 8005 积分卡片   apirootInte
// 8006 营销活动   apirootAc

//let apirootCom  = 'http://10.211.1.249:8001/';  //公共类
//let apirootUser = 'http://10.211.1.249:8002/';  //用户
//let apiroot     = 'http://10.211.1.249:8003/';  //EC
//let apirootSc   = 'http://10.211.1.249:8004/';  //商城
//let apirootInte = 'http://10.211.1.249:8005/';  //积分卡片
//let apirootAc   = 'http://10.211.1.249:8006/';  //营销活动

//测试地址(新) 
let apirootCom  = 'https://shkapi4uat.shinho.net.cn/ecs/common/';    //公共类
let apirootUser = 'https://shkapi4uat.shinho.net.cn/ecs/user/'  ;    //用户
let apiroot     = 'https://shkapi4uat.shinho.net.cn/ecs/ec/' ;        //EC
let apirootSc   = 'https://shkapi4uat.shinho.net.cn/ecs/mall/' ;   //商城
let apirootInte = 'https://shkapi4uat.shinho.net.cn/ecs/integral/';   //积分卡片
let apirootAc   = 'https://shkapi4uat.shinho.net.cn/ecs/activity/' ;  //营销活动
let apiRedpack  = 'https://shkapi4uat.shinho.net.cn/ecs/redpack/' ;  //扫码红包
//let apicpk      = 'https://shkapi4uat.shinho.net.cn/ecs/cpk/' ;  //菜品库
let apicpk      = 'https://apimarket-test.shinho.net.cn/ecs-cpk/' ;  //菜品库java测试


//正式上线地址（新）
// let apirootCom  = 'https://kongapi.shinho.net.cn/ecs/common/';
// let apirootUser = 'https://kongapi.shinho.net.cn/ecs/user/';
// let apiroot     = 'https://kongapi.shinho.net.cn/ecs/ec/';
// let apirootSc   = 'https://kongapi.shinho.net.cn/ecs/mall/';
// let apirootInte = 'https://kongapi.shinho.net.cn/ecs/integral/';
// let apirootAc   = 'https://kongapi.shinho.net.cn/ecs/activity/';
// let apiRedpack  = 'https://kongapi.shinho.net.cn/ecs/redpack/';
// let apicpk      = 'https://kongapi.shinho.net.cn/ecs/cpk/' ;  //菜品库

export default {
	//--------------------------获取时间戳字符串签名---------------------------------------------
	// 正式
//	 getTimestampAndNonceStr:'https://jifenweixin.shinho.net.cn/Wechat/GetTimestampAndNonceStr',//获取时间戳 随机字符串    正式
//	 getJsapiTicketSignature:'https://jifenweixin.shinho.net.cn/Wechat/GetJsapiTicketSignature',//获取签名  正式
//	 saveWeChatImage:'https://jifenweixin.shinho.net.cn/Wechat/SaveWeChatImage',//将图片上传到服务器  正式
//	 createSubscribeQRcode:'https://jifenweixin.shinho.net.cn/WeChat/CreateSubscribeQRcode',//队员二维码
//	 getUrl:'https://jifenweixin.shinho.net.cn',//地址 正式
//	 appId:'wx9657251bde1a71c4',

	//测试
	getTimestampAndNonceStr:'https://uatjifenweixin.shinho.net.cn/Wechat/GetTimestampAndNonceStr',//获取时间戳 随机字符串   测试
	getJsapiTicketSignature:'https://uatjifenweixin.shinho.net.cn/Wechat/GetJsapiTicketSignature',//获取签名  测试
	saveWeChatImage:'https://uatjifenweixin.shinho.net.cn/Wechat/SaveWeChatImage',//将图片上传到服务器  测试
	createSubscribeQRcode:'https://uatjifenweixin.shinho.net.cn/WeChat/CreateSubscribeQRcode',//队员二维码
	getUrl:'https://uatjifenweixin.shinho.net.cn',//地址 测试
    appId:'wxa9067ffebe95ca17',
	
	// ---------------- 获取userId userType接口 --------------------
	getUserId:apirootUser+"api/OpenIdAssociated/FindUserByOpenId",
	
	//-------------------线下签到 接口------------------------------
	signIn:apirootAc+"api/SignIn/SignIn",
	
	//---------------------积分二维码 接口-----------------------------------
	getScanChanceLog:apirootInte+"api/IntegralRule/GetScanChanceLog",//获取用户积分
	scanIntegral:apirootInte+"api/ScanIntegral/ScanIntegral",//积分扫码
	
	//------------------页面访问接口-----------------------
	addECBrowseDetails:apiroot+"api/ECBrowse/AddECBrowseDetails",//增加页面访问
	addECWXTranspondDetails:apiroot+"api/ECBrowse/AddECWXTranspondDetails",//转发次数记录
	
	//-------------------业绩贡献 接口-----------------------------------
	getBCList:apiroot+"api/BusinessContribution/GetBCList",//业绩贡献列表
	
	//---------------------产品库 接口-----------------------------------
	getProductBanner:apiroot+"api/Banner/GetBanner",//获取产品库首页轮播图
	getProductFirst:apiroot+"api/Product/GetProductFirst",//获取产品库首页一级分类
	getProductVC:apiroot+"api/Product/GetVC",//获取产品库首页产品列表
	getSpecificationByVCId:apiroot+"api/Product/GetSpecificationByVCId",//获取产品库首页产品
	getSpecificationByVCIdList:apiroot+"api/Product/GetSpecificationByVCIdList",//获取产品库产品列表
	getProductSecondWX:apiroot+"api/Product/GetProductSecondWX",//获取列表页顶部二级分类
	getProductFirstList:apiroot+"api/Product/GetProductFirstList",//获取产品库一级分类列表
	getProductKuDetail:apiroot+"api/Product/GetProductDetail",//获取产品详情页
	addSpecificationPraiseLog:apiroot+"api/Product/AddSpecificationPraiseLog",//规格点赞
	addSpecificationVisitLog:apiroot+"api/Product/AddSpecificationVisitLog",//增加规格浏览量
	getSpecificationComment:apiroot+"api/Product/GetSpecificationComment",//获取规格留言数据
	addSpecificationComment:apiroot+"api/Product/AddSpecificationComment",//增加规格留言
	getDishProductWX:apiroot+"api/Product/GetDishProductWX",//获取产品库菜品详情
	addDishProductPraise:apiroot+"api/Product/AddDishProductPrasie",//菜品点赞
	addDishVisit:apiroot+"api/Product/AddDishVisit",//菜品浏览次数
	getDishProductComment:apiroot+"api/Product/GetDishComment",//查询菜品留言数据
	addDishProductComment:apiroot+"api/Product/AddDishComment",//新增菜品留言数据
	addDishProductCollect:apiroot+"api/Product/AddDishCollect",//收藏及取消收藏
	
	//-------------------菜品互动 接口---------------------------------
	getMyDish:apiroot+"api/MySelfDishKu/GetMy",//个人菜品库 获取我的菜品
	getMyCollect:apiroot+"api/MySelfDishKu/GetMyCollect",//个人菜品库 获取收藏的菜品
	getMySelfFR:apiroot+"api/MySelfDishKu/GetMySelfFR",//产品选择 获取推荐调料
	addDishUpload:apiroot+"api/MySelfDishKu/AddDish",//上传菜品
	addDishVisitLog:apiroot+"api/MySelfDishKu/AddDishVisitLog",//增加个人菜品库菜品浏览次数
	getMySelfDish:apiroot+"api/MySelfDishKu/GetMySelfDish",//获取菜品详情  进行编辑
	isOverLoad:apiroot+"api/MySelfDishKu/IsOverLoad",//上传菜品数量是否超出
	updateMySelfDish:apiroot+"api/MySelfDishKu/UpdateMySelfDishWX",//修改菜品信息
	getMySelfDishComment:apiroot+"api/MySelfDishKu/GetDishComment",//获取菜品详情留言
	getDishCollectState:apiroot+"api/DishCollecLog/GetDishCollectState",//获取菜品详情是否收藏
	
	// ---------------- EC活动首页 接口 --------------------
    getBanner: apiroot + "api/Banner/GetBanner", // 获取EC活动信息及顶部banner
    getNotice: apiroot + "api/Banner/GetNotice", // 获取公告信息
    getTutor: apiroot + "api/Banner/GetTutorByChefActivityId", // 获取导师列表信息
	getDishList: apiroot + "api/Banner/GetDishChefHomePage", // 获取参赛菜品（最新入选 newSelectSqlJson, 人气抢手 newTopVisitCountJson）
	
	// ----------------- 导师部分 接口 ------------------
	getTutorList: apiroot + "api/Tutor/GetTutorListByChefActivityId",   // 获取导师列表	
	getTutorIntroduce: apiroot + "api/Tutor/GetTutorDTByTutorId",    	//获取导师介绍页面信息
	getTutorComment: apiroot + "api/TutorComment/GetTutorCommentAllByTutorId", 	//获取导师介绍页面留言
	addComment: apiroot + "api/TutorComment/AddTutorComment",  	//导师介绍新增留言
	getTutorDish: apiroot + "api/Tutor/GetDishTutorDetail",    	//获取导师菜品详情信息
	getDishComment: apiroot + "api/TutorComment/GetDishCommentByDishId", //获取导师菜品详情留言信息 
	addDishComment: apiroot + "api/TutorComment/AddDishComment", //新增菜品留言 
	addPrasie: apiroot + "api/TutorComment/AddDishPrasieLog", 	 //点赞 
	addCollect: apiroot + "api/DishCollecLog/AddDishCollectLog", //收藏
	delCollect: apiroot + "api/DishCollecLog/DelDishCollectLog", //取消收藏
	
	// ----------------- 星厨家部分 接口 ------------------
	getStarList: apiroot + "api/DishChefStar/GetChefStarAll",   // 获取星厨列表	
	getStarDetail: apiroot + "api/DishChefStar/GetChefStarDetail",   // 获取星厨介绍页面信息
	getStarComment: apiroot + "api/DishChefStar/GetChefStarComment",   // 获取星厨介绍留言信息
	addStarComment: apiroot + "api/DishChefStar/AddChefStarComment",   // 星厨介绍新增留言
	getStarDish: apiroot + "api/DishChefStar/GetDishChefStar",   // 星厨菜品详情
	getStarDishComment: apiroot + "api/DishChefStar/GetDishChefComment",   // 星厨菜品留言
	addStarDishComment: apiroot + "api/DishChefStar/AddDishComment",   // 星厨菜品新增留言
	addStarPrasie: apiroot + "api/DishChefStar/AddDishPrasieLog",   // 星厨菜品点赞
    
    // ----------------- 厨师报名部分 接口 ------------------
    isApply:apiroot +"api/Banner/IsApply",   //判断是否可以报名
	addChef: apiroot + "api/Chef/AddChef",   //新增厨师基本信息
	getMatch: apiroot + "api/Chef/GetMatchRegion", //获取赛区
	getChefMess:apiroot+"api/Chef/GetChefByMBIdCAId",//获取厨师基本信息	
	getFRChef:apiroot+"api/DishChef/GetFRChef",//获取厨师推荐调料
	addDishChef:apiroot+"api/DishChef/AddDishChef",//增加厨师菜品基本信息
	addDishMaterial:apiroot+"api/DishChef/AddDishMaterial",//增加厨师菜品调料
	addDishStep:apiroot+"api/DishChef/AddDishStep",//增加菜品步骤
	addFR:apiroot+"api/DishChef/AddFR",//新增调料推荐
	getDishChef:apiroot+"api/DishChef/GetDishChefByMemberId",//读取菜品信息
	
	// ----------------- 参赛菜品部分 接口 ------------------
	getDishs: apiroot + "api/DishChef/GetDishListApply",   //获取全部菜品列表
	getVoteDishs: apiroot + "api/DishChef/GetDishListApplySelected",   //获取投票菜品列表
	getDishArea: apiroot + "api/DishChef/GetMatchRegionByCAId", //获取赛区
	getDishDetail:apiroot+"api/DishChef/GetDishChefDetailByDishId",//获取选手菜品详情
	getVoteDishDetail:apiroot+"api/DishChef/GetSelectedDishChef",//获取投票菜品详情
	addDishPrasie:apiroot+"api/DishChef/AddDishPrasieLog",//参赛菜品点赞
	getPlayerIntroduce:apiroot+"api/DishChef/GetChefInfo",//获取选手介绍
	getDishChefComment:apiroot+"api/DishChef/GetDishChefComment",//获取选手菜品 留言信息
	addDishChefComment:apiroot+"api/DishChef/AddDishComment",//增加菜品留言信息
	addVoteNum:apiroot+"api/DishChef/AddDishSelectedLog",//增加投票数量
	
	//------------------培训交流 接口------------------------
	getFlowList:apiroot+"api/Lecturer/GetCultivateInterflowList",//获取培训交流列表
	addCultivatePraise:apiroot+"api/Lecturer/AddCultivatePraise",//点赞
	getCultivateDetail:apiroot+"api/Lecturer/GetCultivateDetail",//培训交流详情
	getCultivateComment:apiroot+"api/Lecturer/GetCultivateComment",//获取培训交流详情评论
	addCultivateComment:apiroot+"api/Lecturer/AddCultivateComment",//增加评论
	
	//----------------------个人中心 会员求职简历部分接口------------
	getMemberById:apirootUser+"api/Member/GetMemberById",//会员个人中心首页 获取会员积分、认证
	getMemberProfile:apirootUser+"api/MemberProfile/GetMemberProfile",//简历预览页面 获取简历信息
	updateMemberInfo:apirootUser+"api/MemberProfile/UpdateMemberInfo",//新增基本信息
	updateHotelInfo:apirootUser+"api/MemberProfile/UpdateHotelInfo",//新增酒店信息
	updateCookIdea:apirootUser+"api/MemberProfile/UpdateCookIdea",//新增从厨理念
	updateJobSkill:apirootUser+"api/MemberProfile/UpdateJobSkill",//新增职业技能
	updateHonor:apirootUser+"api/MemberProfile/UpdateHonor",//新增荣誉资格
	UpdateIntroduction:apirootUser+"api/MemberProfile/UpdateIntroduction",//新增自我介绍
	saveMemberResume:apirootUser+"api/Resume/SaveMemberResume",//新增从业经历
	getMemberResumeList:apirootUser+"api/Resume/GetMemberResumeList",//查询从业经历列表
	getMemberResume:apirootUser+"api/Resume/GetMemberResume",//获取单个从业经历
	updateMemberResume:apirootUser+"api/Resume/UpdateMemberResume",//更新从业经历
	getMemberIntegralList:apirootUser+"api/MemberIntegralDetail/GetMemberIntegralList",//获取厨师积分
	addSigninCredit:apiroot+"api/SigninCredit/AddSigninCredit",//签到
	isDraw:apiroot+"api/SigninCredit/IsDraw",//当天是否签到
	flagDraw:apiroot+"api/SigninCredit/FlagDraw",//记录哪天抽奖
	getSignin:apiroot+"api/SigninCredit/GetSignin",//获取最近7天签到状态
	isCommTranspond:apirootCom+"api/HomeTastePacket/IsCommTranspond",//是否分享朋友圈   指定页面分享

	//----------------------个人中心 队员部分接口------------
	getSaleManInfo:apirootUser+'api/SaleMan/GetSaleManInfo',//队员首页
	updateSaleMan:apirootUser+'api/SaleMan/UpdateSaleMan',//队员修改信息
	getMemberBySaleman:apirootUser+'api/Member/GetMemberBySaleman',//查询绑定厨师
	registCodeList:apirootUser+'api/RegistCode/RegistCodeList',//获取注册码列表
	updateMember:apirootUser+'api/Member/UpdateMember',//修改厨师信息
	addRegistCode:apirootUser+'api/RegistCode/AddRegistCode',//队员生成注册码
	getMemberByArea:apirootUser+'api/Member/GetMemberByArea',//绑定厨师 新
	bindMember:apirootUser+'api/RegistCode/BindMember',//点击成为我的，成为该队员名下会员
	getMemberRegistCode:apirootUser+'api/RegistCode/GetMemberRegistCode',//获取认证信息 认证码
	updateBindMemberInfo:apirootUser+'api/MemberProfile/UpdateBindMemberInfo',//更改厨师主页的厨师信息
	
	//--------------------------积分商城 接口---------------------------------
	getShopingBanner:apiroot+'api/Banner/GetShopingBanner',//获取首页轮播图
	getProductList:apirootSc+'api/Product/GetWeChatProductList',//获取商品列表
	getProductDetail:apirootSc+'api/Product/GetProductById',//获取商品详情
	getProductNorms:apirootSc+'api/Norms/GetProductNorms',//获取商品规格
	getSku:apirootSc+'api/SKU/GetSkuByNorms',//获取商品sku
	addCart:apirootSc+'api/Cart/AddCart',//添加到购物车
	getCartList:apirootSc+'api/Cart/GetCartList',//获取购物车列表
	deleteCart:apirootSc+'api/Cart/DeleteCart',//删除购物车商品
	updateCart:apirootSc+'api/Cart/UpdateCart',//修改购物车商品数量
	addOrder:apirootSc+'api/Order/AddOrder',//确认订单页面 新增订单
	luckyOrderSubmit:apirootSc+'api/Order/LuckyOrderSubmit',//领奖页面 提交订单
	getAddressList:apirootCom+'api/Address/GetMemberAddressList',//获取收货地址列表
	saveAddress:apirootCom+'api/Address/SaveAddress',//新增收货地址
	updateAddress:apirootCom+'api/Address/UpdateAddress',//更新收货地址
	updateMemberIntegral:apirootUser+'api/Member/UpdateMemberIntegral',//支付 更改用户积分
	getMemberOrderList:apirootSc+"api/Order/GetMemberOrderList",//我的订单页面
	updateOrderState:apirootSc+"api/Order/UpdateOrderState",//更改订单状态
	getLogisticsInfo:apirootSc+"api/Order/GetLogisticsInfo",//获取物流信息
	getIntegrakDrawList:apirootSc+"api/Order/GetIntegrakDrawList",//积分抽奖  获取中奖列表
	checkMemberSale:apirootSc+"api/Order/CheckMemberSale",//商品限购
	
	//---------------------------家味抽奖 接口----------------------------------------------
	addPrizeHTD:apirootCom+"api/HomeTasteDraw/AddPrizeHTD",//记录通过家味抽奖注册的用户
	isTodyPrize:apirootCom+"api/HomeTasteDraw/IsTodyPrize",//判断当天是否抽奖
	prizeProbability:apirootCom+"api/HomeTasteDraw/PrizeProbability",//抽奖
	getWinning:apirootCom+"api/HomeTasteDraw/GetWinning",//获取公告列表
	homeLuckyOrderSubmit:apirootSc+"api/Order/HomeLuckyOrderSubmit",//家味修改订单
	
	//-----------------------------积分红包 接口--------------------------------------------
	addRedPacket:apirootCom+"api/HomeTastePacket/AddRedPacket",//抽取随机红包
	isWXTranspond:apirootCom+"api/HomeTastePacket/IsWXTranspond",//是否转发朋友圈
	listRedPacket:apirootCom+"api/HomeTastePacket/ListRedPacket",//获奖名单
	isDrawRed:apirootCom+"api/HomeTastePacket/IsDrawRed",//能否参与抽奖
	getMemberSeniority:apirootUser+"api/Member/GetMemberSeniority",//是否达标   
	
	//-----------------------------猜灯谜 接口--------------------------------------------
	getSubject:apirootCom+"api/Subject/GetSubject",//获取题目
	setSubLog:apirootCom+"api/Subject/SetSubLog",//是否参与过猜灯谜活动
	
	//-----------------------------推荐同行注册---------------------------------------------
	getRecommendPic:apirootUser+'api/Member/GetRecommendPic',//获取二维码
	getRecQualifications:apirootUser+'api/Member/GetRecQualifications',//获取推荐名额
	recommendMemberRegist:apirootUser+'api/Member/RecommendMemberRegist',//推荐同行注册提交 （新）
	
	//------------------------------未认证转已认证--------------------------------------------
	realAuthAdd:apirootUser+'api/MemberAuth/RealAuthAdd',//实名认证  提交信息接口
	getMemberAuth:apirootUser+'api/MemberAuth/GetMemberAuth',//实名认证  获取信息接口
	realAuthUpdate:apirootUser+'api/MemberAuth/RealAuthUpdate',//实名认证  修改信息接口
	findSalemanByArea:apirootUser+'api/Saleman/FindSalemanByArea',//自动地位 根据区id获取该区队员信息
	bindRegistCode:apirootUser+'api/RegistCode/BindRegistCode',//认证码验证
	
	//--------------------------------活动足迹接口----------------------------------------------
	getFootPirntMember:apiroot+"api/MemberFootPrint/FootPirntMember",//会员足迹
	
	//--------------------------------米其林抽奖-------------------------------------------
	addDrawLog:apirootCom+"api/LuckDraw/AddDrawLog",//抽奖日志
	getDrawList:apirootCom+"api/LuckDraw/GetDrawList",//获取中奖列表
	checkDraw:apirootCom+"api/LuckDraw/CheckDraw",//是否能抽奖
	shareDraw:apirootCom+"api/LuckDraw/ShareDraw",//分享成功后增加一次抽奖机会
	memberDrawing:apirootCom+"api/LuckDraw/MemberDrawing",//抽奖
	
	//---------------------------------厨师节活动---------------------------------------------
	checkIsPrize: apirootCom +"api/MemberPrize/CheckIsPrize",//获取中奖奖品
	createVisitorByCookday: apirootUser +"api/member/CreateVisitorByCookday",//厨师节活动注册
	
	//---------------------------------扫码赢红包-----------------------------------------
	redPackScan:apiRedpack+"Api/RedPack/Scan",//扫一扫
	redPackTiXian:apiRedpack+"Api/RedPack/TiXian",//提现
	redPackCreate:apiRedpack+"Api/RedPack/CreateVisitor",//注册
	redPackGetHistory:apiRedpack+"Api/RedPack/GetHistory",//记录
	
	//----------------------------------菜品库---------------------------------------------
//	getCaiPinNameListByParms:apicpk+'Api/WeiXin/GetCaiPinNameListByParms',//通过关键字搜索菜品名称
//	getCaiPinNameList:apicpk+'Api/WeiXin/index',//获取首页各版块菜品信息
//	getDyrqList:apicpk+'Api/WeiXin/GetDyrq',//获取首页当月人气
//	getCaiPinBasicInfo:apicpk+'Api/WeiXin/GetCaiPinBasicInfoById',//获取菜品详情页 基本信息
//	getCaiPinRichInfo:apicpk+'Api/WeiXin/GetCaiPinRichInfoById',//获取菜品详情页 富文本信息
//	getCaiPinMaterial:apicpk+'Api/WeiXin/GetCaiPinMaterialById',//获取菜品详情页 调料步骤信息
//	getLiuYanInfo:apicpk+'Api/WeiXin/GetLiuYanInfo',//获取菜品详情页 留言
//	addLiuYanInfo:apicpk+'Api/WeiXin/AddLiuYanInfo',//新增菜品详情页 留言
//	getAllCategory:apicpk+'Api/WeiXin/GetAllCategory',//获取菜谱分类
//	getCaiPinCount:apicpk+'Api/WeiXin/GetCaiPinCountBySecondId',//获取菜品数量
//	getCaiPinList:apicpk+'Api/WeiXin/GetCaiPinListByParms',// 获取菜品列表
//	getXcxcCaiPinList:apicpk+'Api/WeiXin/GetXcxcCaiPinList',// 获取星厨星菜列表
//	addActionRecord:apicpk+'api/weixin/AddActionRecord',// 记录浏览 收藏 点赞 播放
//	getZhuanTiInfo:apicpk+'Api/ZhuanTi/GetById',//获取专题信息
//	getGuiGeList:apicpk+'Api/WeiXin/GetGuiGeListByProductId',//获取产品规格列表
	
	//----------------------------------菜品库  Java测试---------------------------------------------
	getCaiPinNameListByParms:apicpk+'weixin/getCaiPinNameListByParms',//通过关键字搜索菜品名称
	getCaiPinNameList:apicpk+'weixin/index',//获取首页各版块菜品信息
	getDyrqList:apicpk+'weixin/getDyrq',//获取首页当月人气
	getCaiPinBasicInfo:apicpk+'weixin/getCaiPinBasicInfoById',//获取菜品详情页 基本信息
	getCaiPinRichInfo:apicpk+'weixin/getCaiPinRichInfoById',//获取菜品详情页 富文本信息
	getCaiPinMaterial:apicpk+'weixin/getCaiPinMaterialById',//获取菜品详情页 调料步骤信息
	getLiuYanInfo:apicpk+'weixin/getLiuYanInfo',//获取菜品详情页 留言
	addLiuYanInfo:apicpk+'weixin/addLiuYanInfo',//新增菜品详情页 留言
	getAllCategory:apicpk+'weixin/getAllCategory',//获取菜谱分类
	getCaiPinCount:apicpk+'weixin/getCaiPinCountBySecondId',//获取菜品数量
	getCaiPinList:apicpk+'weixin/getCaiPinListByParms',// 获取菜品列表
	getXcxcCaiPinList:apicpk+'weixin/getXcxcCaiPinList',// 获取星厨星菜列表
	addActionRecord:apicpk+'weixin/addActionRecord',// 记录浏览 收藏 点赞 播放
	getZhuanTiInfo:apicpk+'weixin/getById',//获取专题信息
	getGuiGeList:apicpk+'weixin/getGuiGeListByProductId',//获取产品规格列表
	
	// ----------------- 省市区 接口 ----------------------------
	getAllProvince: apirootCom + "api/Regions/GetAllProvince",			// 省
	getCityList: apirootCom + "api/Regions/GetCityList?provinceId=0",   	// 市
	getAreaList: apirootCom + "api/Regions/GetAreaList?cityId=0",    		// 区
	getRegion: apirootCom + "api/Regions/GetRegion",    		// 省市区
	findCity: apirootCom + "api/Regions/FindCity",    		// 根据省市区名称获取省市区id
	
	// ----------------- register 会员注册 接口 -------------------	
	checkMemberTelephone: apirootUser + "api/Member/CheckMemberTelephone", 	// 手机号API
	sendRegistSMS: apirootCom + "api/SMS/SendRegistSMS",   				    // 验证码API
	checkRegistCode: apirootUser + "api/Member/CheckRegistCode",    		// 注册码API
	createMember: apirootUser + "api/Member/CreateMember", 					// 会员注册提交API
    createVisitor: apirootUser + "api/Member/CreateVisitor", 				// 会员注册提交API（新）
    createSignMember: apirootUser + "api/Member/CreateSignMember", 			// 签到注册 提交API

	// ----------------- teammemberregister 队员注册 接口 --------------
	addRegistSaleMan: apirootUser + "api/SaleMan/AddRegistSaleMan", 	    // 队员注册提交API
	// ----------------- extensioncalendar 推广日历 接口 --------------
	getSalemanContribute: apirootUser + "api/SaleMan/GetSalemanContribute", //  推广日历API







}