<template>
  <div class="scansuccess">
    
    <img src="../../static/credit/scan_title.png" class="scanImg"/>
    <div class="getInte">
    	<p>第<span>{{num}}</span>次开奖</p>
    	<p>{{integral}}</p>
    </div>
    <!--积分展示-->
    <div class="sacnBox">
    	<div>
	    	<p v-for="(item,index) in getScanList">
	    		<span class="userPhoto"><img :src="item.HeadImgUrl"></span>
	    		<span class="userText"><span style="width: 45px;display: inline-block;margin-right: 5px;text-align: center;overflow: hidden;text-overflow:ellipsis;white-space: nowrap;">{{item.Nickname}}</span><span>获得{{item.IntegralNum}}积分 {{item.CreatDate}}</span></span>
	    	</p>
    	</div>
    </div>
    <!--按钮-->
    <div class="scanBtn">
    	<button @click="scanClick">继续扫描</button>
    	<button @click="jump">我的账户</button>
    </div>
    
  </div>
</template>

<script>
import { cookie } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'

export default{
	components:{
		
	},
	data(){
		return{
			getScanList:[],//获取积分列表
			timestamp:'',
  		nonceStr:'',
  		signature:'',
  		currentUrl:'',//当前地址
  		integral:'',//获取积分展示
  		resultUrl:'',//扫描返回url
  		userData:{},
  		openId:'',
  		num:1//开奖次数
		}
	},
	methods:{
		getScanChanceLog(){
			var _this=this;
  		this.$http({
  			method:'get',
  			url:apiUrl.getScanChanceLog
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
  			_this.getScanList=JSON.parse(response.data);
				for(var i in _this.getScanList){
					var getScanList=_this.getScanList[i];
					getScanList.CreatDate=getScanList.CreatDate.substring(5,16);//时间
					getScanList.CreatDate=getScanList.CreatDate.replace('-','月');
					getScanList.CreatDate=getScanList.CreatDate.replace('T','日');
				}
  		})
		},
		jump(){
			this.$router.push({path:'/component/integraldetail'});
		},
		act(){
			var _this=this;
	  	$('.sacnBox div').animate({
		  	marginTop:'-43px'
		  },1000,function(){
		  	$(this).css({marginTop:'0px'});
		  	$(".sacnBox div p").eq(0).appendTo($(".sacnBox div"));
		  	_this.act();
		  })	
		},
		setConfig(){//配置config
        var _this=this;
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.currentUrl;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			 _this.signature=response.data;
		  			 wx.config({
					      debug: false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'scanQRCode'
					      ]
					   })
		  		})		  		
	  	  })
     },
     scanClick(){
     	  var _this=this;
	     	wx.scanQRCode({
					needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
					scanType: ["qrCode","barCode"], // 可以指定扫二维码还是一维码，默认二者都有
					success: function (res) {
				   	_this.resultUrl = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
            if(_this.resultUrl.indexOf('sn')>0){
            	_this.scanIntegral();
            }
					}
				});
     },
     scanIntegral(){
     	  var _this=this;
	     	var params="?url="+this.resultUrl+"&openId="+this.openId;
	  		this.$http({
	  			method:'get',
	  			url:apiUrl.scanIntegral+params,
	  		}).then(function(response){
	  			_this.num++;
	  			if(response.data.indexOf('积分')>0){
    	      _this.integral=response.data.substring(8);
	  			}else{
	  				_this.integral=response.data;
	  			}
	  		})
     }
	},
	mounted(){
		$('.scansuccess').height($(window).height());
		
		//获取用户信息
		this.userData=cookie.get('WeiXinUser',function(val){
	    var a = val.split("&");
			var obj = {};
			for (var i = 0; i < a.length; i++) {
				var b = a[i].split("=");
				obj[b[0]] = b[1];
			}
			return obj;
  	}) 
	  this.openId=this.userData.Openid;
	  
	  //获取当前url 配置config
		var uri=window.location.href.split('#')[0];
		this.currentUrl=encodeURIComponent(uri);
		this.setConfig();
		
    var integral=window.location.href.split('?')[1];
    this.integral=decodeURIComponent(integral.split('=')[1]);//展示获取多少积分
    
    if(this.integral.indexOf('积分')>0){
    	this.integral=this.integral.substring(9,this.integral.length-1);
	  }
		
		this.getScanChanceLog();//获取积分列表
		this.act();//积分列表滚动展示
	}
}
</script>

<style scoped>
.scansuccess{width: 100%;/*height: 100%;*/background: #fff url('../../static/credit/scanBack.png') no-repeat center;background-size: 100% 100%;}
.scanImg{width: 22%;margin-left: 39%;margin-top: 10%;}
.getInte{width: 56%;height:150px;margin-left: 22%;background: url('../../static/credit/scan_inte.png') no-repeat center;background-size:100% 100% ;
        overflow: hidden;position: relative;top: -2%;letter-spacing: 1px;}
.getInte span{display: inline-block;width: 18px;height: 20px;background: #ea2f28;color: #fff;border-radius: 5px;margin-left: 2px;margin-right: 2px;}
.getInte p:nth-child(1){font-size: 13px;color: #333;text-align: center;margin-top: 36px;}
.getInte p:nth-child(2){color: #fff;text-align: center;margin-top:12px;}
.sacnBox{height: 23%;overflow: hidden;border: 1px solid #e73229;width: 90%;margin-left: 5%;position: relative;top: -1%;}
.sacnBox p *{display: inline-block;vertical-align: middle;}
.sacnBox p{text-align: center;margin-top: 5%;}
.userPhoto{width: 30px;height: 30px;display:inline-block;border-radius: 50%;border: 1.5px solid #ff8f00;}
.userPhoto img{width: 30px;height: 30px;border-radius: 50%;overflow: hidden;}
.userText{color: #320701;font-size: 13px;letter-spacing: 1px;}
.scanBtn{margin-top: 20%;height:10%;}
.scanBtn button{width: 35%;height: 50%;margin-left: 7%;margin-right: 7%;background: #e73229;color: #fff;border: none;border-radius: 5px;}
.scanBtn button{ /*按钮颜色渐变*/
	background: linear-gradient(bottom,#e7141b,#f16f19); 
	background: -ms-linear-gradient(bottom,#e7141b,#f16f19); 
	background: -webkit-linear-gradient(bottom,#e7141b,#f16f19); 
	background: -o-linear-gradient(bottom,#e7141b,#f16f19); 
	background: -moz-linear-gradient(bottom,#e7141b,#f16f19);
}
@media screen and (max-height: 480px) {
	.scanImg{margin-top: 5%;}
	.getInte{height: 135px;}
	.getInte p:nth-child(1){margin-top: 28px;}
	.getInte p:nth-child(2){margin-top:10px;}
	.sacnBox{height: 27%;}
	.sacnBox p{margin-top: 3%;}
	.scanBtn{margin-top: 18%;}
	.scanBtn button{height: 60%;}
}

</style>
<style>

</style>