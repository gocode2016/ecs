<template>
  <div class="sign">

    <div class="sign_top">
    	<div class="imgDiv">
    		<img :src="userImg"/>
    	</div>
    </div>
    <div class="sign_style">已连续签到{{ContinuousDay}}天!</div>
    <flow>
      <flow-state state="" :title="time[6]" :is-done="isDraw[6]!=2"></flow-state>
      <flow-line is-done></flow-line>

      <flow-state state="" :title="time[5]" :is-done="isDraw[5]!=2"></flow-state>
      <flow-line is-done></flow-line>

      <flow-state state="" :title="time[4]" :is-done="isDraw[4]!=2"></flow-state>
      <flow-line is-done></flow-line>

      <flow-state state="" :title="time[3]" :is-done="isDraw[3]!=2"></flow-state>
      <flow-line is-done></flow-line>
      
      <flow-state state="" :title="time[2]" :is-done="isDraw[2]!=2"></flow-state>
      <flow-line is-done></flow-line>
      
      <flow-state state="" :title="time[1]" :is-done="isDraw[1]!=2"></flow-state>
      <flow-line is-done></flow-line>
      
      <flow-state state="" :title="time[0]" class='flow_style' :is-done="isDraw[0]!=2"></flow-state>
      
    </flow>
    <p class="sign_text">每日签到可得<span>1积分</span>,连续签到7天可获得一次<span>抽奖</span>机会！</p>
    <button @click='signClick' class="signBtn">一键签到</button>
    
    <!--产品展示-->
    <div class="goodshow">
    	<p>最新资讯</p>
    	<div class="goodsDiv">
    		<div v-for="(item,index) in goodsList"><img :src="item.url"/></div>
    	</div>
    </div>
    
    <!--提示注册弹框-->
    <div class="mask" v-show="maskFlag">
    	<div>
    		<p>需要注册才能参与哦！</p>
    		<button @click='jump'>马上注册</button>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { Flow, FlowState, FlowLine , XButton,cookie,Toast } from 'vux'
import apiUrl from '../apiUrls'
var signNum=0;//未点击签到按钮
export default {
  components: {
    Flow,
    FlowState,
    FlowLine,
    XButton,
    cookie,
    Toast
  },
  data(){
  	return{
  		userData:{},
  		userId:'',
  		userType:'',
  		openId:'',
  		signList:[],//签到列表
  		IsSiginCredit:'',//当天是否签到
  		ContinuousDay:'',//连续签到天数
		  time:[],//日期数组
		  isDraw:[],//签到状态
		  wxImg:'',//微信头像
		  userImg:'',
  		goodsList:[{
  			url:'../../static/credit/goods.jpg'
  		},{
  			url:'../../static/credit/sign-good.jpg'
  		}],
  		maskFlag:false,
  		Ecurl:'',
  		Parameter:''
  	}
  },
  methods:{
  	jump(){
  		this.$router.push('/component/register');
  	},
  	addECBrowseDetails(){//记录页面访问
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
    	
			var params={
			  "ECURL":this.Ecurl,
			  "PageName":" ",
			  "Parameter":this.Parameter,
			  "OpenId":this.openId,
			  "PageShort":"每日签到",
			  "PageDetail":"每日签到",
			  "PageType":"1"
			}
			this.$http({
				method:'post',
				url:apiUrl.addECBrowseDetails,
				data:params
			}).then(function(response){
//				console.log(response.data);
			})
		},
  	getMemberById(){//个人中心首页信息
  		var _this=this;
  		var params={
  		  'MemberId':this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMemberById,
  			data:params
  		}).then(function(response){  	
  			var data=JSON.parse(response.data); 
			  if(data.length!=0){
			  	if(data[0].ImgUrl==''||data[0].ImgUrl==null){
				  	_this.userImg=_this.wxImg;//微信头像
				  }else{
				  	_this.userImg=data[0].ImgUrl;
				  }
			  }else{
			  	_this.userImg=_this.wxImg;//微信头像
			  }
  		})
  	},
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':1,
				'MemberId':this.userId,
				'IntegralSource':'每日签到',
				'Remark':'每日签到'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
  			_this.$vux.toast.text('成功+1积分', 'middle');
  		})
  	},
  	signClick(){//点击一键签到
  		if(this.userType==1){//队员
		  	this.$vux.toast.text('队员不能参与哦！', 'middle');
		  }else if(this.userType==0){//未注册
		  	//弹框
		  	this.maskFlag=true;
		  }else if(this.userType==2){//会员
			  if($('.signBtn').html()=='一键签到'){
			  	signNum++;
			  	if(signNum==1){
	  			  this.addSigninCredit();
			  	}
	  		}else if($('.signBtn').html()=='点击抽奖'){
  			  this.$router.push('/component/draw');
  		  }
		  }
  	},
  	addSigninCredit(){//签到
  		var _this=this;
  		var params={
  		  "MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addSigninCredit,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
			  if(response.data=='OK'){
  				$('.signBtn').css('background','#959595');
  				$('.signBtn').html('签到成功');
  				_this.getSignin();
  				_this.updateMemberIntegral();
				  _this.addECBrowseDetails();
			  }
  		})
  	},
  	getSignin(){//最近7天签到状态
  		var _this=this;
  		var params={
  			"MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSignin,
  			data:params
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
  			_this.signList=data.SigninCredit;
  			_this.IsSiginCredit=data.IsSiginCredit;//当天是否签到
  			_this.ContinuousDay=data.ContinuousDay;//连续签到天数
        var drawList=[];
  			for(let i in _this.signList){
  				var signList=_this.signList[i];
  				drawList.push(signList.IsDraw);
  			}
			  drawList=drawList.join(',');
		    if(drawList.indexOf('1')<0&&drawList.indexOf('2')<0){
					$('.signBtn').css('background','#e43226');
					$('.signBtn').html('点击抽奖');
			  }else if(_this.IsSiginCredit=='f'){//当天未签到
			  	$('.signBtn').css('background','#e43226');
  				$('.signBtn').html('一键签到');
			  }else if(_this.IsSiginCredit=='t'){//当天已签到
			  	$('.signBtn').css('background','#959595');
  				$('.signBtn').html('签到成功');
			  }
			  
			  _this.isDraw=[];
			  for(let i in _this.signList){
			  	_this.signList[i].SigninData=_this.signList[i].SigninData.substring(5);
			  	_this.signList[i].SigninData=_this.signList[i].SigninData.replace('-','.');
			  	_this.time.push(_this.signList[i].SigninData);
			  	_this.isDraw.push(_this.signList[i].IsDraw);
			  }
			  
			  for(let i in _this.isDraw){
			  	if(_this.isDraw[i]==1){
			  		_this.time[i]='已领奖';
			  	}
			  }
			  
  		})
  	},
  	onChange (val) {
      const _this = this
      if (val) {
        this.$vux.toast.show({
          text: 'Hello World',
          onShow () {
            console.log('Plugin: I\'m showing')
          },
          onHide () {
            console.log('Plugin: I\'m hiding')
            _this.show9 = false
          }
        })
      } else {
        this.$vux.toast.hide()
      }
	  }
  },
  mounted(){
    this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
  	}) 
	  this.userId=parseInt(this.userData.UserId);
	  this.userType=parseInt(this.userData.UserType);
	  this.openId=this.userData.Openid;
	  this.wxImg=this.userData.Headimgurl;//微信头像  	
    this.getMemberById();//获取用户头像
	  this.getSignin();//获取7天签到状态
  }
}
</script>

<style scoped>
  .sign_top{width: 100%;height: 96px;margin-top: 25px;position: relative;}
  .imgDiv{width:90px;height: 90px;border: 3px solid #ededed;border-radius: 50%;position: absolute;left: 50%;margin-left: -45px;overflow: hidden;}
  .imgDiv img{width: 100%;}
  .sign_style{width:80%;height:47px;margin-left:10%;text-align:center;color:#fff;font-size:13px;line-height:47px;background: url('../../static/credit/sign.png') no-repeat center;background-size: 100% 100%;}
  .sign_text{font-size: 11px;color: #3E3E3E;text-align: center;}
  .sign_text span{color: #E83428;}
  .goodshow{border-top: 1px dashed #d0d0d0;margin-top: 35px;font-size: 15px;color: #E83428;padding-left: 13px;padding-right: 13px;}
  .goodshow p{margin-top: 23px;}
  .goodsDiv{display: flex;flex-wrap: wrap;justify-content: space-between;}
  .goodsDiv div{width: 48%;margin-top: 10px;}
  .goodsDiv img{width: 100%;}
  .sign .signBtn{width: 80%;height: 30px;margin-left: 10%;border-radius: 5px;border: none;margin-top: 10px;color:#fff;}
  .mask{width:100%;height:100%;position: fixed;top: 0;background: rgba(0,0,0,0.5);z-index: 1000;}
  .mask div{background: #fff;width: 50%;height:105px;position: absolute;top: 180px;left:25%;text-align: center;}
  .mask p{font-size: 12px;color: #E83428;margin-top: 30px;}
  .mask button{width: 50%;height: 25px;background: #E83428;color: #fff;border: none;outline: none;font-size: 12px;margin-top: 20px;}
</style>
<style>
  #vux_view_box_body{background: #fff;letter-spacing: 1px;}	
  .sign .weui-btn{width: 70% !important;margin-top: 20px;border-radius: 5px;}
  .sign .weui-btn_default{color: #fff !important;background-color: #959595 !important;}
  .sign .weui-wepay-flow{padding-top:30px !important;}
  .sign .weui-wepay-flow__process{background-color:#e93527 !important;}
  .weui-wepay-flow__state{background: #969698 !important;width: 10px !important;height: 10px !important;top: 0px !important;}
  .weui-wepay-flow__li_done .weui-wepay-flow__state{background: #e93527 !important;width: 14px !important;height: 14px !important;top: -2px !important;}
  .sign .weui-wepay-flow__title-bottom{color:#969698 !important;font-size: 12px;}
  .weui-wepay-flow__li_done .weui-wepay-flow__title-bottom{color:#e93527 !important;}
  .weui-wepay-flow__li{width: 10px !important;height: 10px !important;}
  .weui-wepay-flow__li_done .weui-wepay-flow__li{width: 14px !important;height: 14px !important;}
</style>
