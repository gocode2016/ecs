<template>
  <div style="height:100%;">
    <div v-transfer-dom>
      <loading v-model="isLoading"></loading>
    </div>
    <div v-transfer-dom>
      <actionsheet :menus="menus" v-model="showMenu" @on-click-menu="changeLocale"></actionsheet>
    </div>

    <drawer
    width="200px;"
    :show.sync="drawerVisibility"
    :show-mode="showModeValue"
    :placement="showPlacementValue"
    :drawer-style="{'background-color':'#35495e', width: '200px'}">

      <!-- drawer content -->
      <div slot="drawer">
        <group title="Drawer demo(beta)" style="margin-top:20px;">
          <cell title="Demo" link="/demo" value="演示" @click.native="drawerVisibility = false">
          </cell>
        </group>
        <group title="showMode">
          <radio v-model="showMode" :options="['push', 'overlay']" @on-change="onShowModeChange"></radio>
        </group>
        <group title="placement">
          <radio v-model="showPlacement" :options="['left', 'right']" @on-change="onPlacementChange"></radio>
        </group>
      </div>

      <!-- main content -->
      <view-box ref="viewBox" body-padding-top="0px" body-padding-bottom="0px">
        
       <x-header slot="header"
        style="width:100%;left:0;top:0;z-index:100;display: none;"
        :left-options="leftOptions"
        :right-options="rightOptions"
        :title="title"
        :transition="headerTransition"
        >
        <!-- @on-click-more="onClickMore" 加 x-header 上-->
          <span v-if="route.path === '/' || route.path === '/component/drawer'" slot="overwrite-left">
           <!-- @click="drawerVisibility = !drawerVisibility" 加span上-->
            <x-icon type="navicon" size="35" style="fill:#fff;position:relative;top:-8px;left:-3px;"></x-icon>
          </span>
        </x-header>

        <transition :name="'vux-pop-' + (direction === 'forward' ? 'in' : 'out')">
          <router-view  class="router-view"></router-view>
        </transition>
        
        <tabbar class="vux-demo-tabbar" icon-class="vux-center" v-show="showNavi" slot="bottom">
        	<tabbar-item  :link="{path:'/component/shop'}" :selected="route.path === '/component/shop'" >
            <span class="demo-icon-22 img-icon shop" slot="icon"></span>
            <span slot="label">商城</span>
          </tabbar-item >
          <tabbar-item :link="{path:'/component/recommendstart'}" :selected="route.path === '/component/recommendstart'" >
            <span class="demo-icon-22 img-icon recom" slot="icon"></span>
            <span slot="label">推荐</span>
          </tabbar-item>
          <tabbar-item :link="{path:'/component/dishstore'}" :selected="route.path === '/component/dishstore'" >
            <span class="demo-icon-22 img-icon cp" slot="icon"></span>
            <span slot="label">电子菜谱</span>
          </tabbar-item>
          <tabbar-item  :link="{path:'/component/productstore'}" :selected="route.path === '/component/productstore'" >
            <span class="demo-icon-22 img-icon tw" slot="icon"></span>
            <span slot="label">调味</span>
          </tabbar-item>
          <tabbar-item  :selected="route.path === '/component/personal'"  @on-item-click="tabItemClick('user')">
            <span class="demo-icon-22 img-icon my" slot="icon"></span>
            <span slot="label">我的</span>
          </tabbar-item>
        	
          <!--<tabbar-item  :link="{path:'/component/shop'}" :selected="route.path === '/component/shop'" >
            <span class="demo-icon-22 img-icon shop" slot="icon"></span>
            <span slot="label">商城</span>
          </tabbar-item >
          <tabbar-item  :link="{path:'/component/menu'}" :selected="route.path === '/component/menu'" >
            <span class="demo-icon-22 img-icon menu" slot="icon"></span>
            <span slot="label">菜谱</span>
          </tabbar-item>
          <tabbar-item :link="{path:'/component/riddleshome'}" :selected="route.path === '/component/riddleshome'">
            <span class="demo-icon-22 img-icon taste taste_home" slot="icon"></span>
            <span slot="label">家味</span>
          </tabbar-item>
          <tabbar-item :link="{path:'/'}"  :selected="route.path === '/'">
            <span class="demo-icon-22 img-icon active" slot="icon"></span>
            <span slot="label">活动</span>
          </tabbar-item>
          <tabbar-item  :selected="route.path === '/component/personal'" @on-item-click="tabItemClick('user')">
            <span class="demo-icon-22 img-icon my" slot="icon"></span>
            <span slot="label">我的</span>
          </tabbar-item>-->
          
        </tabbar>

      </view-box>
    </drawer>
  </div>
</template>

<script>
import { Radio, Group, Cell, Badge, Drawer, Actionsheet, ButtonTab, ButtonTabItem, ViewBox, XHeader, Tabbar, TabbarItem, Loading, TransferDom } from 'vux'
import { mapState, mapActions } from 'vuex'
import apiUrl from './apiUrls.js'

export default {
  directives: {
    TransferDom
  },
  components: {
    Radio,
    Group,
    Cell,
    Badge,
    Drawer,
    ButtonTab,
    ButtonTabItem,
    ViewBox,
    XHeader,
    Tabbar,
    TabbarItem,
    Loading,
    Actionsheet
  },
  methods: {
  	menutitle () {
      // 配置各页面 title
      if (this.route.path === '/component/shop')                      return '商城'    
      if (this.route.path === '/')                                    return ''//知味·达美活动首页
      if (this.route.path === '/component/personal')                  return '个人中心'
      if (this.route.path === '/component/teammember')                return '队员中心'
      if (this.route.path === '/component/expect')                    return '敬请期待'
      if (this.route.path === '/component/teammemberedit')            return '队员编辑'
      if (this.route.path === '/component/teamcode')                  return '二维码'//队员 二维码
      if (this.route.path === '/component/associatechef')             return '绑定厨师'
      if (this.route.path === '/component/teamsearch')                return '搜索结果'//队员绑定厨师搜索结果
      if (this.route.path === '/component/chefhomepage')              return '厨师主页'// 队员
      if (this.route.path === '/component/chefdetail')                return '厨师详情页'// 队员
      if (this.route.path === '/component/teammemberintegraldetail')  return '积分明细'// 队员
      if (this.route.path === '/component/integraldraw')              return '积分抽奖'
      if (this.route.path === '/component/receiveprice')              return '领奖'
      if (this.route.path === '/component/submitorder')               return '成功提交订单'
      if (this.route.path === '/component/registercode')              return '认证码'
      if (this.route.path === '/component/contribution')              return '生意贡献度'
      if (this.route.path === '/component/chefcontribution')          return '生意贡献度'//厨师生意贡献度
      if (this.route.path === '/component/extensioncalendar')         return '推广日历'
      if (this.route.path === '/component/extensioncalendarmonth')    return '推广日历月份'
      if (this.route.path === '/component/menu')                      return '菜谱'
      if (this.route.path === '/component/recommend')                 return '推荐注册'//打开
      if (this.route.path === '/component/recommendstart')            return '推荐注册'//发起
      if (this.route.path === '/component/membernotice')              return '会员须知'
      if (this.route.path === '/component/activityrule')              return '扫码规则'
      if (this.route.path === '/component/register')                  return '注册页面'
      if (this.route.path === '/component/signregister')              return '签到注册'
      if (this.route.path === '/component/teammemberregister')        return '队员注册页面'
      if (this.route.path === '/component/tutorlist')                 return '导师列表'
      if (this.route.path === '/component/tutor')                     return '导师介绍'
      if (this.route.path === '/component/tutordish')                 return '导师菜品介绍'
      if (this.route.path === '/component/competitiondishlist')       return '参赛菜品列表'
      if (this.route.path === '/component/competitiondish')           return '参赛菜品详情'
      if (this.route.path === '/component/starkitchen')               return '星厨介绍'
      if (this.route.path === '/component/starkitchenlist')           return '星厨列表'
      if (this.route.path === '/component/starkitchendish')           return '星厨菜品详情'
      if (this.route.path === '/component/starsearch')                return '搜索星厨结果'
      if (this.route.path === '/component/enterstep')                 return '活动报名'
      if (this.route.path === '/component/enterstepst')               return '活动报名基本信息'
      if (this.route.path === '/component/enterstepnd')               return '创新菜'
      if (this.route.path === '/component/entersteprd')               return '本地菜'
      if (this.route.path === '/component/trainlist')                 return '培训交流列表'
      if (this.route.path === '/component/traindetail')               return '培训详情'
	    if (this.route.path === '/component/integraldetail')  		      return '积分明细' 
	    if (this.route.path === '/component/myfootmark')  		          return '我的足迹' //会员
	    if (this.route.path === '/component/footmark')  		            return '活动足迹' //队员
	    if (this.route.path === '/component/cheffootmark')  		        return '厨师活动足迹' //厨师
	    if (this.route.path === '/component/footresult')  		          return '搜索结果' //足迹搜索结果
	    if (this.route.path === '/component/feedback')  		            return '问题反馈' 
	    if (this.route.path === '/component/sign')  				            return '签到' 
	    if (this.route.path === '/component/draw')  				            return '抽奖' 
	    if (this.route.path === '/component/orderlist')  				        return '订单' 
	    if (this.route.path === '/component/logistics')  				        return '物流' 
	    if (this.route.path === '/component/resume')  				          return '简历' 
	    if (this.route.path === '/component/resumeinformation')  				return '基本信息'
	    if (this.route.path === '/component/resumehotel')  				 			return '酒店信息' 
	    if (this.route.path === '/component/resumeconcept')  				    return '从厨理念' 
	    if (this.route.path === '/component/resumeskill')  				      return '职业技能' 
	    if (this.route.path === '/component/resumehonor')  				      return '荣誉资格' 
	    if (this.route.path === '/component/resumeexperience')  				return '从业经历' 
	    if (this.route.path === '/component/resumeexperiences')  				return '从业经历展示'
	    if (this.route.path === '/component/resumeintroduce')  				  return '自我介绍' 
	    if (this.route.path === '/component/resumepreview')  				    return '简历预览' 
	    if (this.route.path === '/component/readlist')  				        return '读书一角' 
	    if (this.route.path === '/component/shopdetail')  				      return '商品详情' 
	    if (this.route.path === '/component/shopcar')  				          return '购物车' 
	    if (this.route.path === '/component/shoporder')  				        return '确认订单' //实物
	    if (this.route.path === '/component/inventedorder')  				    return '确认订单'//虚拟
	    if (this.route.path === '/component/shopaddress')  				      return '收货地址'
	    if (this.route.path === '/component/goregister')  				      return '个人中心'//提示注册
	    if (this.route.path === '/component/pageshare')  				        return '指定页面分享'
	    if (this.route.path === '/component/tasteshare')  				      return ''//家味分享
	    if (this.route.path === '/component/signsuccess')  				      return '签到成功'//线下签到
	    if (this.route.path === '/component/authenmethod')  				    return '厨师认证'//选择认证方式
	    if (this.route.path === '/component/authencode')  				      return '厨师认证'//注册码认证
	    if (this.route.path === '/component/authenname')  				      return '厨师认证'//实名认证
	    if (this.route.path === '/component/authenexamine')  				    return '提交成功'//审核中
	    if (this.route.path === '/component/authenhotel')  				      return '酒店信息'//实名认证完善信息
	    if (this.route.path === '/component/dishshow')  				        return '个人菜品库'//菜品库
	    if (this.route.path === '/component/dishselectgoods')  				  return '产品选择'
	    if (this.route.path === '/component/dishupload')  				      return '上传菜品'
	    if (this.route.path === '/component/dishdetails')  				      return '菜品详情'
	    if (this.route.path === '/component/scansuccess')  				      return '扫描成功'
	    if (this.route.path === '/component/productstore')  				    return '调味'//产品库首页
	    if (this.route.path === '/component/productstorelist')  				return '产品列表'//产品库列表
	    if (this.route.path === '/component/productstoredetail')  			return '产品详情'//产品库产品详情
	    if (this.route.path === '/component/productdishdetail')  			  return '菜品详情'//产品库菜品详情
      if (this.route.path === '/component/registeredmembers')  			  return '会员注册'
      if (this.route.path === '/component/registeredplayers')  			  return '队员注册'
      if (this.route.path === '/component/registeredrecommend')  		  return '推荐同行注册'
      if (this.route.path === '/component/thanksdraw')  		          return '欣和餐饮服务'//感恩节抽奖
      if (this.route.path === '/component/michelinshow')  		        return '欣和味达美餐饮服务'//米其林长图
      if (this.route.path === '/component/thankscode')  		          return '欣和餐饮服务'//感恩节二维码
      if (this.route.path === '/component/cookdays')  		            return '助力厨师成长'//厨师节 已抽奖
      if (this.route.path === '/component/cookday')  		              return '助力厨师成长'//厨师节 未抽奖
      if (this.route.path === '/component/cookregister')  		        return '会员注册'//厨师节注册
      if (this.route.path === '/component/dishstore')  		            return '欣和餐饮服务-欣鲜菜谱库'//菜品库首页
      if (this.route.path === '/component/dishstorebanner')  		      return '活动列表页'//活动列表页
      if (this.route.path === '/component/dishsearchlist')  		      return '查询列表页'//查询列表页
      if (this.route.path === '/component/dishcookbook')  		        return '菜谱'//全部菜谱
      if (this.route.path === '/component/dishstyle')  		            return '全部菜谱'//菜式
      if (this.route.path === '/component/dishstarkitchen')  		      return '星厨星菜'//星厨星菜
      if (this.route.path === '/component/dishvideolearn')  		      return '视频学菜'//视频学菜
      if (this.route.path === '/component/dishstoredetail')  		      return '菜品详情'//菜品详情
      if (this.route.path === '/component/dishhotshop')  		          return '旺店热卖'//旺店热卖
      if (this.route.path === '/component/dishmonthpop')  		        return '当月人气'//当月人气
      if (this.route.path === '/component/dishmenu')  		            return '产品菜谱'//产品菜谱
      if (this.route.path === '/component/dishproductlist')  		      return ''//菜品库 产品列表
      if (this.route.path === '/component/earnpoints')  		          return '赚积分'//赚积分
      if (this.route.path === '/component/dishspecial')  		          return ''//菜品库专题页
      if (this.route.path === '/component/special')  		              return '极简捞拌 鲜爽夏日'//捞汁专题页
      if (this.route.path === '/component/specialsoy')  		          return '好生抽调本味'//生抽专题页
      if (this.route.path === '/component/specialsoup')  		          return '酸汤酱专题页'//酸汤酱专题页
      if (this.route.path === '/component/specialoyster')  		        return '蚝香浓郁，提香出色'//蚝油专题页
      if (this.route.path === '/component/scanhome')  		            return '码上抢红包'//扫码首页
      if (this.route.path === '/component/scandraw')  		            return '扫码抽奖'//扫码抽奖
      if (this.route.path === '/component/scanreceive')  		          return '扫码领取'//扫码领取
      if (this.route.path === '/component/scanregister')  		        return '扫码注册'//扫码注册
      if (this.route.path === '/component/scantips')  		            return '提示'    //提示
      if (this.route.path === '/component/scanrule')  		            return '兑奖规则'//兑奖规则
      if (this.route.path === '/component/scanrecord')  		          return '兑奖记录'//兑奖记录
      
    },
  	//设置cookie
		setCookie(cname,cvalue,exdays){
		  var d = new Date();
		  d.setTime(d.getTime()+(exdays*24*60*60*1000));
		  var expires = "expires="+d.toGMTString();
		  document.cookie = cname + "=" + cvalue + "; " + expires;
		},
  	// 获取cookie
    getCookie(cname){	
		  var name = cname + "=";
		  var ca = document.cookie.split(';');
		  for(var i=0; i<ca.length; i++) 
		  {
		    var c = ca[i].trim();
		    if (c.indexOf(name)==0) return c.substring(name.length,c.length);
		  }
		  return "";
    },
    // 将cookie的value转为对象
    parse(num){
			var a = num.split("&");
			var obj = {};
			for (var i = 0; i < a.length; i++) {
				var b = a[i].split("=");
				obj[b[0]] = b[1];
			}
			return obj;
		},
		//获取用户Id 用户类型data.UserId
		getUserId(){
			var _this=this;
      var params='?openId='+this.openId;  
      this.$http({
      	method:'GET',
      	url:apiUrl.getUserId+params,
      }).then(function(response){
      	var data=JSON.parse(response.data);
      	if(data.length!=0){
      		var userInfo=data[0]; 
      		_this.memberData.UserId=userInfo.UserId;
		      _this.memberData.UserType=userInfo.UserType;
		      _this.userId=userInfo.UserId;
		      _this.userType=userInfo.UserType;
		      var cookieValue = [];
					for(var i in _this.memberData){
						cookieValue.push(i + "=" + _this.memberData[i]);
					}
					cookieValue=cookieValue.join("&");
					_this.setCookie('WeiXinUser',cookieValue);  
//					alert(_this.getCookie('WeiXinUser'));//测试
      	}else{
      		_this.memberData.UserId=0;
		      _this.memberData.UserType=0;
		      _this.userId=0;
		      _this.userType=0;
		      var cookieValue = [];
					for(var i in _this.memberData){
						cookieValue.push(i + "=" + _this.memberData[i]);
					}
					cookieValue=cookieValue.join("&");
					_this.setCookie('WeiXinUser',cookieValue);  	
      	}  
      })
		},
		addECBrowseDetails(){//记录页面访问
			var params={
			  "ECURL":this.Ecurl,
			  "PageName":" ",
			  "Parameter":this.Parameter,
			  "OpenId":this.openId
			}
			this.$http({
				method:'post',
				url:apiUrl.addECBrowseDetails,
				data:params
			}).then(function(response){
//				console.log(response.data);
			})
		},
		tabItemClick(val){//跳转页面 商城和我的 判断
  	  this.memberData=this.parse(this.getCookie("WeiXinUser"));
			this.userType=this.memberData.UserType;
			if(val=='user'){//点击【我的】
				if(this.userType==0){//提示注册
					this.$router.push('/component/goregister');
				}else if(this.userType==1){//队员首页
					this.$router.push('/component/teammember');
				}else if(this.userType==2){//会员首页
					this.$router.push('/component/personal');
				}
			}
		},
    onShowModeChange (val) {
      /** hide drawer before changing showMode **/
      this.drawerVisibility = false
      setTimeout(one => {
        this.showModeValue = val
      }, 400)
    },
    onPlacementChange (val) {
      /** hide drawer before changing position **/
      this.drawerVisibility = false
      setTimeout(one => {
        this.showPlacementValue = val
        if(val == 'left') {
          $('.vux-notice').css('color', 'red') // 测试 jquery 引入成功
        }else{
          $('.vux-notice').css('color', 'blue') // 测试 jquery 引入成功
        }
      }, 400)     
    },
    onClickMore () {
      this.showMenu = true
    },
    changeLocale (locale) {
      this.$i18n.set(locale)
      this.$locale.set(locale)
    },
    ...mapActions([
      'updateDemoPosition'
    ])
  },
  mounted () {
    this.handler = () => {
      if (this.path === '/demo') {
        this.box = document.querySelector('#demo_list_box')
        this.updateDemoPosition(this.box.scrollTop)
      }
    }
    
    //获取openId   
    this.memberData=this.parse(this.getCookie("WeiXinUser"));
    this.openId=this.memberData.Openid;
    this.getUserId();//获取userid usertype
//  this.setCookie("WeiXinUser","UserId=12208&UserType=2&Openid=o11Z-jjgramCWa2NUJY5y8HVpst4&Nickname=%e6%8c%91%e5%b1%b1%e6%b3%89%e7%9a%84%e5%b0%8f%e6%9c%a8%e6%a1%b6&Sex=1&Province=%e5%b1%b1%e4%b8%9c&City=%e7%83%9f%e5%8f%b0&Country=%e4%b8%ad%e5%9b%bd&Headimgurl=http://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKKlicnRBVQwmzJfa6GPu5dHP4zuLCnMYeUcSjBYFTA16nGNGoc5oJzncRK0hNvQFUCIaXibqicPhPvQ/0",1);
 
//  this.ECUrl=window.location.href.split('?');
//  this.addECBrowseDetails();
    $('title').html(this.menutitle());
  },
  beforeDestroy () {
    this.box && this.box.removeEventListener('scroll', this.handler, false)
  },
  watch: {
    path (path) {
    	var _this=this;
    	var url=window.location.href;
    	var arrUrl=url.split('?');
    	if(url.indexOf('from')<0){
    		this.Ecurl=arrUrl[0];
    		this.Parameter=arrUrl[1];
    	}else{
    		this.Ecurl=arrUrl[0]+'?'+arrUrl[1];
    		this.Parameter=arrUrl[2];
    	}
    	$('title').html(this.menutitle());
    	setTimeout(function(){
    		_this.addECBrowseDetails();
    	},3000)
      
      if (path === '/component/demo') {
        this.$router.replace('/demo')
        return
      }
      if (path === '/demo') {
        setTimeout(() => {
          this.box = document.querySelector('#demo_list_box')
          if (this.box) {
            this.box.removeEventListener('scroll', this.handler, false)
            this.box.addEventListener('scroll', this.handler, false)
          }
        }, 1000)
      } else {
        this.box && this.box.removeEventListener('scroll', this.handler, false)
      }
    }
  },
  computed: {
    ...mapState({
      route: state => state.route,
      path: state => state.route.path,
      deviceready: state => state.app.deviceready,
      demoTop: state => state.vux.demoScrollTop,
      isLoading: state => state.vux.isLoading,
      direction: state => state.vux.direction
    }),
    isShowBar () {
      if (/component/.test(this.path)) {
        return true
      }
      return false
    },
    leftOptions () {
      return {
        showBack: this.route.path !== '/'
      }
    },
    rightOptions () {
      return {
        showMore: true
      }
    },
    headerTransition () {
      return this.direction === 'forward' ? 'vux-header-fade-in-right' : 'vux-header-fade-in-left'
    },
    componentName () {
      if (this.route.path) {
        const parts = this.route.path.split('/')
        if (/component/.test(this.route.path) && parts[2]) return parts[2]
      }
    },
    isDemo () {
      return /component|demo/.test(this.route.path)
    },
    isTabbarDemo () {
      return /tabbar/.test(this.route.path)
    },
    title () {
      // 配置底部导航是否显示
      
      if (this.route.path === '/component/recommendstart' || this.route.path === '/component/dishstoredetail' ||this.route.path === '/component/productstoredetail' ||this.route.path === '/component/dishstore' || this.route.path === '/component/personal' || this.route.path==='/component/shop' ||  this.route.path==='/component/productstore' || this.route.path==='/component/teammember' || this.route.path==='/component/specialoyster') {
        this.showNavi = true;
        $('#vux_view_box_body').css({
        	"padding-bottom":"50px"
        })
      }else{
        this.showNavi = false;
        $('#vux_view_box_body').css({
        	"padding-bottom":"0px"
        })
      }

      // 配置各页面 title
      if (this.route.path === '/component/shop')                      return '商城'    
      if (this.route.path === '/')                                    return ''
      if (this.route.path === '/component/personal')                  return '个人中心'
      if (this.route.path === '/component/teammember')                return '队员中心'
      if (this.route.path === '/component/expect')                    return '敬请期待'
      if (this.route.path === '/component/teammemberedit')            return '队员编辑'
      if (this.route.path === '/component/teamcode')                  return '二维码'//队员 二维码
      if (this.route.path === '/component/associatechef')             return '绑定厨师'
      if (this.route.path === '/component/teamsearch')                return '搜索结果'//队员绑定厨师搜索结果
      if (this.route.path === '/component/chefhomepage')              return '厨师主页'// 队员
      if (this.route.path === '/component/chefdetail')                return '厨师详情页'// 队员
      if (this.route.path === '/component/teammemberintegraldetail')  return '积分明细'// 队员
      if (this.route.path === '/component/integraldraw')              return '积分抽奖'
      if (this.route.path === '/component/receiveprice')              return '领奖'
      if (this.route.path === '/component/submitorder')               return '成功提交订单'
      if (this.route.path === '/component/registercode')              return '认证码'
      if (this.route.path === '/component/contribution')              return '生意贡献度'
      if (this.route.path === '/component/chefcontribution')          return '生意贡献度'//厨师生意贡献度
      if (this.route.path === '/component/extensioncalendar')         return '推广日历'
      if (this.route.path === '/component/extensioncalendarmonth')    return '推广日历月份'
      if (this.route.path === '/component/menu')                      return '菜谱'
      if (this.route.path === '/component/recommend')                 return '推荐注册'//打开
      if (this.route.path === '/component/recommendstart')            return '推荐注册'//发起
      if (this.route.path === '/component/membernotice')              return '会员须知'
      if (this.route.path === '/component/activityrule')              return '扫码规则'
      if (this.route.path === '/component/register')                  return '注册页面'
      if (this.route.path === '/component/signregister')              return '签到注册'
      if (this.route.path === '/component/teammemberregister')        return '队员注册页面'
      if (this.route.path === '/component/tutorlist')                 return '导师列表'
      if (this.route.path === '/component/tutor')                     return '导师介绍'
      if (this.route.path === '/component/tutordish')                 return '导师菜品介绍'
      if (this.route.path === '/component/competitiondishlist')       return '参赛菜品列表'
      if (this.route.path === '/component/competitiondish')           return '参赛菜品详情'
      if (this.route.path === '/component/starkitchen')               return '星厨介绍'
      if (this.route.path === '/component/starkitchenlist')           return '星厨列表'
      if (this.route.path === '/component/starkitchendish')           return '星厨菜品详情'
      if (this.route.path === '/component/starsearch')                return '搜索星厨结果'
      if (this.route.path === '/component/enterstep')                 return '活动报名'
      if (this.route.path === '/component/enterstepst')               return '活动报名基本信息'
      if (this.route.path === '/component/enterstepnd')               return '创新菜'
      if (this.route.path === '/component/entersteprd')               return '本地菜'
      if (this.route.path === '/component/trainlist')                 return '培训交流列表'
      if (this.route.path === '/component/traindetail')               return '视频详情'
	    if (this.route.path === '/component/integraldetail')  		      return '积分明细' 
	    if (this.route.path === '/component/myfootmark')  		          return '我的足迹' //会员
	    if (this.route.path === '/component/footmark')  		            return '活动足迹' //队员
	    if (this.route.path === '/component/cheffootmark')  		        return '厨师活动足迹' //厨师
	    if (this.route.path === '/component/footresult')  		          return '搜索结果' //足迹搜索结果
	    if (this.route.path === '/component/feedback')  		            return '问题反馈' 
	    if (this.route.path === '/component/sign')  				            return '签到' 
	    if (this.route.path === '/component/draw')  				            return '抽奖' 
	    if (this.route.path === '/component/orderlist')  				        return '订单' 
	    if (this.route.path === '/component/logistics')  				        return '物流' 
	    if (this.route.path === '/component/resume')  				          return '简历' 
	    if (this.route.path === '/component/resumeinformation')  				return '基本信息' 
	    if (this.route.path === '/component/resumehotel')  				 			return '酒店信息' 
	    if (this.route.path === '/component/resumeconcept')  				    return '从厨理念' 
	    if (this.route.path === '/component/resumeskill')  				      return '职业技能' 
	    if (this.route.path === '/component/resumehonor')  				      return '荣誉资格' 
	    if (this.route.path === '/component/resumeexperience')  				return '从业经历' 
	    if (this.route.path === '/component/resumeexperiences')  				return '从业经历展示'
	    if (this.route.path === '/component/resumeintroduce')  				  return '自我介绍' 
	    if (this.route.path === '/component/resumepreview')  				    return '简历预览' 
	    if (this.route.path === '/component/readlist')  				        return '读书一角' 
	    if (this.route.path === '/component/shopdetail')  				      return '商品详情' 
	    if (this.route.path === '/component/shopcar')  				          return '购物车' 
	    if (this.route.path === '/component/shoporder')  				        return '确认订单' //实物
	    if (this.route.path === '/component/inventedorder')  				    return '确认订单'//虚拟
	    if (this.route.path === '/component/shopaddress')  				      return '收货地址'
	    if (this.route.path === '/component/goregister')  				      return '提示注册'
	    if (this.route.path === '/component/pageshare')  				        return '指定页面分享'
	    if (this.route.path === '/component/tasteshare')  				      return ''//家味分享
	    if (this.route.path === '/component/signsuccess')  				      return '签到成功'//线下签到
	    if (this.route.path === '/component/authenmethod')  				    return '厨师认证'//选择认证方式
	    if (this.route.path === '/component/authencode')  				      return '厨师认证'//注册码认证
	    if (this.route.path === '/component/authenname')  				      return '厨师认证'//实名认证
	    if (this.route.path === '/component/authenexamine')  				    return '提交成功'//审核中
	    if (this.route.path === '/component/authenhotel')  				      return '酒店信息'//实名认证完善信息
	    if (this.route.path === '/component/dishshow')  				        return '个人菜品库'//菜品库
	    if (this.route.path === '/component/dishselectgoods')  				  return '产品选择'
	    if (this.route.path === '/component/dishupload')  				      return '上传菜品'
	    if (this.route.path === '/component/dishdetails')  				      return '菜品详情'
	    if (this.route.path === '/component/scansuccess')  				      return '扫描成功'
	    if (this.route.path === '/component/productstore')  				    return '调味'//产品库首页
	    if (this.route.path === '/component/productstorelist')  				return '产品列表'//产品库列表
	    if (this.route.path === '/component/productstoredetail')  			return '产品详情'//产品库产品详情
	    if (this.route.path === '/component/productdishdetail')  			  return '菜品详情'//产品库菜品详情
      if (this.route.path === '/component/registeredmembers')  			  return '会员注册'
      if (this.route.path === '/component/registeredplayers')  			  return '队员注册'
      if (this.route.path === '/component/registeredrecommend')  		  return '推荐同行注册'
      if (this.route.path === '/component/thanksdraw')  		          return '欣和餐饮服务'//感恩节抽奖
      if (this.route.path === '/component/michelinshow')  		        return '欣和味达美餐饮服务'//米其林长图
      if (this.route.path === '/component/thankscode')  		          return '欣和餐饮服务'//感恩节二维码
      if (this.route.path === '/component/cookdays')  		            return '助力厨师成长'//厨师节 已抽奖
      if (this.route.path === '/component/cookday')  		              return '助力厨师成长'//厨师节 未抽奖
      if (this.route.path === '/component/cookregister')  		        return '会员注册'//厨师节注册
      if (this.route.path === '/component/dishstore')  		            return '欣和餐饮服务-欣鲜菜谱库'//菜品库首页
      if (this.route.path === '/component/dishstorebanner')  		      return '活动列表页'//活动列表页
      if (this.route.path === '/component/dishsearchlist')  		      return '查询列表页'//查询列表页
      if (this.route.path === '/component/dishcookbook')  		        return '菜谱'//全部菜谱
      if (this.route.path === '/component/dishstyle')  		            return '全部菜谱'//菜式
      if (this.route.path === '/component/dishstarkitchen')  		      return '星厨星菜'//星厨星菜
      if (this.route.path === '/component/dishvideolearn')  		      return '视频学菜'//视频学菜
      if (this.route.path === '/component/dishstoredetail')  		      return '菜品详情'//菜品详情
      if (this.route.path === '/component/dishhotshop')  		          return '旺店热卖'//旺店热卖
      if (this.route.path === '/component/dishmonthpop')  		        return '当月人气'//当月人气
      if (this.route.path === '/component/dishmenu')  		            return '产品菜谱'//产品菜谱
      if (this.route.path === '/component/dishproductlist')  		      return ''//菜品库 产品列表
      if (this.route.path === '/component/earnpoints')  		          return '赚积分'//赚积分
      if (this.route.path === '/component/dishspecial')  		          return ''//菜品库专题页
      if (this.route.path === '/component/special')  		              return '极简捞拌 鲜爽夏日'//捞汁专题页
      if (this.route.path === '/component/specialsoy')  		          return '好生抽调本味'//生抽专题页
      if (this.route.path === '/component/specialsoup')  		          return '酸汤酱专题页'//酸汤酱专题页
      if (this.route.path === '/component/specialoyster')  		        return '蚝香浓郁，提香出色'//蚝油专题页
      if (this.route.path === '/component/scanhome')  		            return '码上抢红包'//扫码首页
      if (this.route.path === '/component/scandraw')  		            return '扫码抽奖'//扫码抽奖
      if (this.route.path === '/component/scanreceive')  		          return '扫码领取'//扫码领取
      if (this.route.path === '/component/scanregister')  		        return '扫码注册'//扫码注册
      if (this.route.path === '/component/scantips')  		            return '提示'    //提示
      if (this.route.path === '/component/scanrule')  		            return '兑奖规则'//兑奖规则
      if (this.route.path === '/component/scanrecord')  		          return '兑奖记录'//兑奖记录
      
      if (this.route.path === '/project/donate') return 'Donate'
      if (this.route.path === '/demo') return 'Demo list'
      return this.componentName ? `Demo/${this.componentName}` : 'Demo/~~'
    }
  },
  data () {
    return {
      showNavi: false,
      showMenu: false,
      menus: {
        'language.noop': '<span class="menu-title">Language</span>',
        'zh-CN': '中文',
        'en': 'English'
      },
      drawerVisibility: false,
      showMode: 'push',
      showModeValue: 'push',
      showPlacement: 'left',
      showPlacementValue: 'left',
      openId:'',
      memberData:{},
      userId:'',
      userType:'',
      Ecurl:'',//当前页面地址
      Parameter:''//参数
    }
  }
}
</script>

<style lang="less">
@import '~vux/src/styles/reset.less';
@import '~vux/src/styles/1px.less';
@import '~vux/src/styles/tap.less';

body {
  background-color: #fbf9fe;
}
html, body {
  height: 100%;
  width: 100%;
  overflow-x: hidden;
}

.demo-icon-22 {
  font-family: 'vux-demo';
  font-size: 22px;
  color: #888;
}
.weui-tabbar.vux-demo-tabbar {
	z-index: 5000;
  /** backdrop-filter: blur(10px);
  background-color: none;
  background: rgba(247, 247, 250, 0.5);**/
}
.vux-demo-tabbar .weui-bar__item_on .demo-icon-22 {
  color: #F70968;
}
.vux-demo-tabbar .weui-tabbar_item.weui-bar__item_on .vux-demo-tabbar-icon-home {
  color: rgb(53, 73, 94);
}
.demo-icon-22:before {
  content: attr(icon);
}
.vux-demo-tabbar-component {
  background-color: #F70968;
  color: #fff;
  border-radius: 7px;
  padding: 0 4px;
  line-height: 14px;
}
.weui-tabbar__icon + .weui-tabbar__label {
  margin-top: 0!important;
}
.vux-demo-header-box {
  z-index: 100;
  position: absolute;
  width: 100%;
  left: 0;
  top: 0;
}

@font-face {
  font-family: 'vux-demo';  /* project id 70323 */
  src: url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.eot');
  src: url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.eot?#iefix') format('embedded-opentype'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.woff') format('woff'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.ttf') format('truetype'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.svg#iconfont') format('svg');
}

.demo-icon {
  font-family: 'vux-demo';
  font-size: 20px;
  color: #04BE02;
}

.demo-icon-big {
  font-size: 28px;
}

.demo-icon:before {
  content: attr(icon);
}

.router-view {
  width: 100%;
  /*top: 46px;*/
}
.vux-pop-out-enter-active,
.vux-pop-out-leave-active,
.vux-pop-in-enter-active,
.vux-pop-in-leave-active {
  will-change: transform;
  transition: all 500ms;
  height: 100%;
  /*top: 46px;*/
  position: absolute;
  backface-visibility: hidden;
  perspective: 1000;
}
.vux-pop-out-enter {
  opacity: 0;
  transform: translate3d(-100%, 0, 0);
}
.vux-pop-out-leave-active {
  opacity: 0;
  transform: translate3d(100%, 0, 0);
}
.vux-pop-in-enter {
  opacity: 0;
  transform: translate3d(100%, 0, 0);
}
.vux-pop-in-leave-active {
  opacity: 0;
  transform: translate3d(-100%, 0, 0);
}
.menu-title {
  color: #888;
}


/*部分样式重构*/
/*上下边框缩进15px修正*/
.weui-cell:before,
.weui-media-box:before,
.weui-panel__hd:after { left: 0 !important; }

/*首页底部导航菜单*/
.weui-tabbar__label {color: #3d3d3d !important;}/*默认样式*/
.weui-bar__item_on span { color: #E83428;}/*选中样式*/
/*.weui-tabbar__icon{width:50px !important; height: 33.3px !important;}*/
.weui-tabbar__icon{width:27px !important; height: 27px !important;}
.img-icon { width: 100%; height: 100%; display: block;}
/*.weui-tabbar__item{padding: 2px 0 0 !important;}*/

.weui-tabbar__item .shop { background: url('../static/credit/shop.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .recom { background: url('../static/credit/icon_recom.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .cp { width:127px !important;height:70px !important;background: url('../static/credit/icon_shinmenu.png') no-repeat center; background-size: 52%;position: relative;top: -35px;left: -50px;}
.weui-tabbar__item .tw { background: url('../static/credit/twp.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .my { background: url('../static/credit/my.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .shop { background: url('../static/credit/shop_current.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .recom { background: url('../static/credit/icon_recom1.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .cp { width:127px !important;height:70px !important;background: url('../static/credit/icon_shinmenu.png') no-repeat center; background-size: 52%;position: relative;top: -35px;left: -50px;}
.weui-bar__item_on .tw { background: url('../static/credit/twp_current.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .my { background: url('../static/credit/my_current.png') no-repeat center; background-size: 100%; }

/*.weui-tabbar__item .shop { background: url('../static/credit/icon_shop.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .menu { background: url('../static/credit/icon_menu.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .taste { background: url('../static/credit/icon_taste.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .taste { width:80px !important;height:50px !important;background: url('../static/credit/icon_riddle.png') no-repeat center; background-size:70%;position: relative;left: -20%;top: -20%; }
.weui-tabbar__item .active { background: url('../static/credit/icon_act.png') no-repeat center; background-size: 100%; }
.weui-tabbar__item .my { background: url('../static/credit/icon_my.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .shop { background: url('../static/credit/icon_shops.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .menu { background: url('../static/credit/icon_menus.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .taste { background: url('../static/credit/icon_tastes.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .taste { width:80px !important;height:50px !important;background: url('../static/credit/icon_riddle.png') no-repeat center; background-size: 70%;position: relative;left: -20%;top: -20%;}
.weui-bar__item_on .active { background: url('../static/credit/icon_acts.png') no-repeat center; background-size: 100%; }
.weui-bar__item_on .my { background: url('../static/credit/icon_mys.png') no-repeat center; background-size: 100%; }*/
/*.taste_home{
	  -webkit-animation-name: tastehome;
    -webkit-animation-timing-function: ease-in-out;
    -webkit-animation-iteration-count: infinite;
    -webkit-animation-duration: 1s;
    -webkit-animation-direction: alternate;
    -webkit-transform-origin:center 90%;
}
@-webkit-keyframes tastehome{
	  0% {
	    -webkit-transform: scale(1.3);   
    }
    100% {
	    -webkit-transform: scale(1);   
    }
}*/
</style>