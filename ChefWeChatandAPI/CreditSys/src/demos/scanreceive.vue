<template>
  <div class="scanreceive">
    
    <p class="returnhome">
	    <img src="../../static/credit/scan_return.png" @click="jump('/component/scanhome')"/>
	    <span @click="jump('/component/scanhome')">返回首页</span>
	  </p>
	  <div style="text-align: center;">
	  	<div v-if="succFlag">
		    <p class="money">{{money}}{{tips}}</p>
		    <div class="tips" v-show="showFlag">
			    <p>温馨提示：根据微信平台要求，红包</p>
			    <p>累计到1元及以上即可提现，不足1元</p>
			    <p>的红包将存入零钱包</p>
		    </div>
	    </div>
	    <div v-else>
	    	<p class="money">领取失败</p>
	    	<div class="tips" v-if="textFlag">
			    <p>{{message}}</p>
		    </div>
		    <div class="tips" v-else>
			    <p>本活动仅针对酒店从业者和美食爱好者 </p>
			    <p>调味品供货商无法参与</p>
		    </div>
	    </div>
	    
	    <button @click="goScan" v-show="isType != 2">继续兑奖</button>
	    <p class="text">长按二维码关注平台，查看我的零钱包</p>
	    <p><img src="../../static/credit/scan_code.jpg" class="scancode"/></p>
	    <p><img src="../../static/credit/scan_icon.png" class="bottomImg"/></p>
    </div>
    
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
			succFlag:false,
			showFlag:true,
			textFlag:true,
			money:'',
			timestamp:'',
		  nonceStr:'',
		  signature:'',
		  url:'',
		  userData:{},
		  openId:'',
		  isType:'',//0存入零钱成功 1提现成功 2提现失败
		  tips:'元已领取成功',
		  message:''
		}
	},
	methods:{
		jump(link){//返回首页
			this.$router.push(link);
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
	      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
	  		_this.$http({
	  			method:'GET',
	  			url:apiUrl.getJsapiTicketSignature+params
	  		}).then(function(response){
	  			_this.signature=response.data;
  			  wx.config({
			      debug:false,
			      appId: apiUrl.appId,
			      timestamp: _this.timestamp,
			      nonceStr: _this.nonceStr,
			      signature: _this.signature,
			      jsApiList: [
			        'scanQRCode'
			      ]
			    })
	  		}).catch(function(res){
	  			_this.setConfig();
	  		})
	  	}).catch(function(res){
	  		_this.setConfig();
	  	})
	  },
	  goScan(){
	  	if(this.openId == '' || this.openId == null || this.openId == undefined){
	  		this.$vux.toast.text('网络延迟，请刷新页面后重试', 'middle');
	  	}else{
	  		this.scanCode();
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
          _this.setConfig();
//		  		alert('失败2:'+JSON.stringify(res));
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
  			data:params
  		}).then(function(response){
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
		this.money = this.$route.query.money;
		this.isType = this.$route.query.isType;//提现1 存入零钱0 2提现失败
		this.message = this.$route.query.message;

		if(this.isType == 1){//提现
			this.succFlag = true;
			this.showFlag = false;
		}else if(this.isType == 0){//存入零钱
			this.succFlag = true;
			this.showFlag = true;
			this.tips = '元已存入零钱包';
		}else if(this.isType == 2){//提现失败
			this.succFlag = false;
			if(this.message == '调味品供货商'){
				this.textFlag = false; 
			}else{
				this.textFlag = true;
			}
		}
		
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
	}
}
</script>

<style scoped>
.scanreceive{width: 100%;height: 100%;overflow: hidden;background: url('../../static/credit/scan_ys.png') no-repeat center bottom #ffe4c7;background-size: 100% 14%;}
.returnhome{margin-top: 1%;}
.returnhome *{display: inline-block;vertical-align: middle;}
.returnhome img{width: 20px;margin-left: 10px;}
.returnhome span{font-size: 12px;margin-left: 8px;}
.money{font-size: 22px;color: black;margin-top: 4%;}
.tips{font-size: 12px;letter-spacing: 1px;margin-top: 3%;}
button{width: 36%;height: 34px;background: #ff8a00;color: #fff;border: none;border-radius: 5px;margin-top: 6%;font-size: 16px;}
.text{font-size: 13px;margin:7% 0 6% 0;}
.scancode{width: 50%;}
.bottomImg{width:20%;position: absolute;bottom: 13%;left: 40%;}
@media screen and (max-height: 580px) {/*高度<=580 图片缩小*/
	.returnhome{margin-top: 4px;}
	.money{margin-top: 1%;}
	.tips{margin-top: 2%;}
	button{margin-top: 3%;}
  .text{font-size: 13px;margin:4% 0 3% 0;}
	.scancode{width: 46%;}
	.scanreceive{background-size: 100% 12%;}
	.bottomImg{bottom: 11%;}
}
    
</style>
<style>

</style>