<template>
  <div class="scanhome">
    
    <img src="../../static/credit/scan_home.png" style="width: 100%;"/>
    <div class="box_a">
    	<p style="margin-top: 22%;">兑奖金额：随机0.08-88元不等</p>
    	<p>兑奖截止日期：2019年3月31日</p>
    	<img src="../../static/credit/scan_home3.png"  class="scanImg" @click="goScan" />
    	<div class="boxs">
    		<p>
    			<img src="../../static/credit/scan_home4.png" />
    			<span @click="jump('/component/scanrecord')">我的零钱包</span>
    		</p>
    		<p>
    			<img src="../../static/credit/scan_home5.png" />
    			<span @click="jump('/component/scanrule')">兑奖规则</span>
    		</p>
    	</div>
    	<p style="margin-top: 30px;"><span class="method_text">参与方法</span></p>
    </div>
    
    <div class="box_b">
    	<img src="../../static/credit/scan_txt2.png" width="90%"/>
    </div>
    
    <!--遮罩层-->
    <!--<div class="mask">
    	<p>您来早了，活动尚未开始哦~</p>
    </div>-->
    
  </div>
</template>

<script>
import { cookie,Toast } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'

export default{
	components:{
		cookie,
		Toast
	},
	data(){
		return{
			timestamp:'',
		  nonceStr:'',
		  signature:'',
		  url:'',
		  userData:{},
		  openId:'',
		  userType:'',
		  shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquqY17U580YTjjAFyNBBFUjGkJVkDLmTRZicHkyF6mOCL4nhCltMtODia7IJicuGgBZXWTaJfdKTVDtQ/0?wx_fmt=jpeg'
		}
	},
	methods:{
		jump(link){
			this.$router.push(link);
		},
		setConfig(){//配置config
	    var _this=this;
	  	this.$http({
	  		method:'GET',
	  		url:apiUrl.getTimestampAndNonceStr
	  	}).then(function(response){
	  		var data=response.data;
//	  		console.log(data);
	  		var dataArr=data.split('|');
	  		_this.timestamp=parseInt(dataArr[0]);//时间戳
	  		_this.nonceStr=dataArr[1];//随机字符串
	      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
	  		_this.$http({
	  			method:'GET',
	  			url:apiUrl.getJsapiTicketSignature+params
	  		}).then(function(response){
	  			_this.signature=response.data;
//	  			console.log(_this.signature);
  			  wx.config({
			      debug:false,
			      appId: apiUrl.appId,
			      timestamp: _this.timestamp,
			      nonceStr: _this.nonceStr,
			      signature: _this.signature,
			      jsApiList: [
			        'scanQRCode',
			        'onMenuShareTimeline',//分享到朋友圈
					    'onMenuShareAppMessage'//分享给朋友
			      ]
			    })
  			  
  			  wx.ready(function(){
			    		wx.onMenuShareTimeline({
							    title: '码上抢红包 100%有奖', // 分享标题
							    link: apiUrl.getUrl+'/?action=scanhome', // 分享链接
									imgUrl: _this.shareicon, // 分享图标
									success: function () { 
									// 用户确认分享后执行的回调函数
//									  _this.addECWXTranspondDetails(2);
									},
									cancel: function () {
//										alert('取消');
									// 用户取消分享后执行的回调函数
									}
							});
			        wx.onMenuShareAppMessage({
									title: '码上抢红包 100%有奖', // 分享标题
									desc:  '味达美尚品生抽有奖装来袭，扫一扫领现金红包啦！', // 分享描述
							    link: apiUrl.getUrl+'/?action=scanhome', // 分享链接
									imgUrl: _this.shareicon,// 分享图标
									success: function () {
									// 用户确认分享后执行的回调函数
//									  alert('分享成功');
//									  _this.addECWXTranspondDetails(1);
									},
									cancel: function () {
//										alert('取消');
									// 用户取消分享后执行的回调函数
									},
									fail:function(res){
//						        alert(res.errMsg);
						      }
						  });
	    		})
  			  
	  		}).catch(function(res){
	  			_this.setConfig();
	  		})
	  	}).catch(function(res){
	  		_this.setConfig();
	  	})
	  },
	  goScan(){
//	  	this.$vux.toast.text('活动即将开始，敬请期待', 'middle');
	  	if(this.userType == 1){//队员
	  		this.$vux.toast.text('队员无法参与活动', 'middle');
	  	}else{
	  		if(this.openId == '' || this.openId == null || this.openId == undefined){
		  		this.$vux.toast.text('网络延迟，请刷新页面后重试', 'middle');
		  	}else{
		  		this.scanCode();
		  	}
	  	}
	  },
	  scanCode(){//扫一扫
	  	var _this = this;
	  	wx.ready(function(){
		  	wx.scanQRCode({
					needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
					scanType: ["qrCode","barCode"], // 可以指定扫二维码还是一维码，默认二者都有
					success: function (res) {
						var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
						if(result.indexOf('scanhome')<0){//非本活动二维码
							_this.$router.push({path:'/component/scantips',query:{type:0}});//type 0  非活动二维码
						}else{
							//获取Scode
							var params=result.split('&')[1];
							var code=params.split('=')[1];
						  _this.scanRedpack(code);//扫码兑红包
						}
					},
					fail:function(error){
//						alert('失败1:'+JSON.stringify(error));
					}
				})
		  	wx.error(function(res){
//		  		alert('失败2:'+JSON.stringify(res));
          _this.setConfig();
        });
	  	});
	  },
	  scanRedpack(code){//扫码兑红包
	  	var _this = this;
	  	var params={
  		  "OpenId": this.openId,
				"Scode": code
  		}
  		this.$http({
  			method:'post',
				url:apiUrl.redPackScan,
//      url:'http://kongapi.shinho.net.cn/ecs/redpack/Api/RedPack/ScanPressureTest',//测试
  			data:params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			var data = JSON.parse(response.data);
			  if(data.result_status == 'fail'){//失败
			  	if(data.code_status == '非活动码'){
						_this.$router.push({path:'/component/scantips',query:{type:0}});//type 0  非活动二维码
			  	}else if(data.code_status == '已被扫'){
						_this.$router.push({path:'/component/scantips',query:{type:1,money:data.money,time:data.scan_date}});//type 1 已被扫
			  	}else{
			  		if(data.message == '调味品供货商'){
							_this.$router.push({path:'/component/scantips',query:{type:2,message:data.message}});//type 2 调味品供货商
			  		}else{
							_this.$router.push({path:'/component/scantips',query:{type:3,message:data.message}});//type 3 其他情况
			  		}
			  	}
			  }else if(data.result_status == 'succ'){//成功
			  	if(data.code_status == '正常'){
						_this.$router.push({path:'/component/scandraw',query:{money:data.money,kTxMoney:data.kTxMoney,isKeTiXian:data.isKeTiXian,isRegist:data.isRegist}});
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
    var rTop = $(window).height()*0.14;
    $('.box_a').css({
    	'position':'relative',
    	'top':-rTop
    });
		
		//配置congfig
	  var uri=location.href.split('#')[0];
	  this.url=encodeURIComponent(uri);
		this.setConfig();
		
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
	  this.userType=parseInt(this.userData.UserType);
		
	}
}
</script>

<style scoped>
.scanhome{width: 100%;background: #e33e38;letter-spacing: 1px;overflow: auto;}
.box_a{width: 100%;height: 370px;background: url('../../static/credit/scan_home2.png') no-repeat center;background-size: 100% 100%;
    text-align: center;color: #fff;border-top: 1px solid transparent;box-sizing: border-box;z-index: 1;}
.box_a p:nth-child(1),.box_a p:nth-child(2){font-size: 13px;}
.scanImg{margin: 6% 0 4% 0;width: 80%;}
.boxs {display: flex;justify-content: space-around;}
.boxs p{width: 40%;}
.boxs p *{vertical-align: middle;}
.boxs p img{width: 16%;}
.boxs p span{font-size: 14px;}
.method_text{font-size: 15px;border-bottom: 1px solid #fff;line-height: 30px;}
.box_b{text-align: center;margin-top:-100px;margin-bottom: 20px;position: relative;z-index: 1000;}
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.7);position: absolute;top: 0;color: #fff;font-size: 20px;text-align: center;}
.mask p{margin-top: 50%;}
@media screen and (max-height:568px){
	.box_a{top: -84px !important;}
	.box_a p:nth-child(1),.box p:nth-child(2){font-size: 12px;}
	.box_b{margin-top: -130px;}
}
@media screen and (min-height:800px){
	.box_a{top: -94px !important;}
}
</style>
<style>

</style>