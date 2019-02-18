<template>
  <div class="submitorder">
    
    <div class="success">
    	<div>
	    	<img src="../../static/credit/success.png"/>
	    	<p>订单提交成功!</p>
    	</div>
    	<div>
    		<p>{{SkuName}}(通用) x 1</p>
    		<p>订单来源：{{source}}</p>
    		<p>消耗积分：{{inteNum}}</p>
    		<p>领奖时间：{{awardTime}}</p>
    		<button @click="jump()">继续抽奖</button>
    		<button @click="lookorder('/component/orderlist')">查看订单</button>
    	</div>
    </div>
    <div class="mask" @click="clickMask">
    	<img src="../../static/credit/pyq.png"/>
    	<p>请点击右上角</p>
    	<p>将它发送给指定朋友</p>
    	<p>或分享到朋友圈</p>
    </div>
    
  </div>
</template>

<script>
import { cookie } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
export default{
	components:{
		cookie
	},
	data(){
		return{
			SkuName:'',
			awardTime:'',
			timestamp:'',
      nonceStr:'',
      signature:'',
      url:'',
      openId:'',
      userId:'',
      userType:'',
      userData:{},
      source:'',
      inteNum:'',
      img:'',
      sharetext:'',
      shareicon:'',
      params:''//区分是家味抽奖还是积分抽奖
		}
	},
	methods:{
		clickMask(){
			$('.mask').hide();
		},
		lookorder(link){//查看订单
			this.$router.push(link);
		},
		jump(){//继续抽奖
			if(this.source=='家味抽奖'){
				this.$router.push('/component/hometastedraw');
			}else if(this.source=='积分抽奖'){
				this.$router.push('/component/integraldraw');
			}
    },
    addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
    	var params={
  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/"+this.params,
  		  "Parameter":"",
  		  "OpenId":this.openId,
  		  "TranspondType":type
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addECWXTranspondDetails,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  		})
    },
    share(){
    	  var _this=this;
        var localIds ='';
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
					        'onMenuShareTimeline',//分享到朋友圈
					        'onMenuShareAppMessage'//分享给朋友
					      ]
					    })
		  			  
		  			  wx.ready(function(){
		    				wx.onMenuShareTimeline({
				            title:_this.sharetext,
				            link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.img+'&userType='+_this.userType,
				            imgUrl:_this.shareicon,
				            success: function () { 
				              // 用户确认分享后执行的回调函数
				              _this.addECWXTranspondDetails(2);
				            },
				            cancel: function () { 
				              // 用户取消分享后执行的回调函数
//				              alert('你没有分享到朋友圈');
				            },
				            fail:function(res){
				            	alert(res.errMsg);
				            }
		            });
		            wx.onMenuShareAppMessage({
										title: _this.sharetext, // 分享标题
										desc: _this.sharetext, // 分享描述
				            link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.img+'&userType='+_this.userType,
										imgUrl:_this.shareicon,// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
//										  alert('分享给朋友成功');
										  _this.addECWXTranspondDetails(1);
										},
										cancel: function () {
										// 用户取消分享后执行的回调函数
//										  alert('你没有分享给朋友');
										},
										fail:function(res){
//				            	alert(res.errMsg);
				            }
								});
    			    })
		  		})
		  	})
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
	  this.openId=this.userData.Openid;
	  this.userId=parseInt(this.userData.UserId);
	  this.userType=parseInt(this.userData.UserType);
	  this.img=this.userData.Headimgurl;
		this.SkuName=this.$route.query.SkuName;
		this.awardTime=this.$route.query.awardTime;
		if(this.$route.query.source=='tastedraw'){
			this.source='家味抽奖';
			this.inteNum=0;
			this.params='hometaste';
			this.sharetext='我中奖啦！快来搓搓小手一起欣春赢家味！';
			this.shareicon='https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquKc65tSsZcOdMS533QNwn2H18AuGAawEfqwTfpWxV681hAe1H2BibpnXwSGjXLg64eGyDEn84bNlw/0?wx_fmt=jpeg';
		}else if(this.$route.query.source=='intedraw'){
			this.source='积分抽奖';
			this.inteNum=40;
			this.params='intedraw';
			this.sharetext='厨大大们快来看，搓搓小手抽大奖，欣和味达美积分活动惊喜不断!';
			this.shareicon='http://shkapi4qas.shinho.net.cn/cts/common/UploadFiles/Tools/201712201650222795.png';
		}
		
    var uri=window.location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
	}
}
</script>

<style scoped>
  .success{width: 100%;height: 100%;position: fixed;top: 0px;background: #f4f4f4;}
  .success div{background: #fff;padding: 20px 0;}
  .success div:nth-child(1){border-bottom: 1px solid #eeeeee;color: #E83428;text-align: center;}
  .success div:nth-child(2){color: #888888;text-indent: 1em;font-size: 14px;line-height: 20px;}
  .success div:nth-child(2) p{line-height: 30px;}
  .success img{width: 30%;}
  .success button{width: 40%;height: 40px;background: #E83428;color: #fff;border: none;margin-top: 20px;border-radius: 5px;margin-left: 7%;}
  .mask{width: 100%;height: 100%;background: rgba(0,0,0,0.6);position: fixed;top: 0px;}
  .mask img{width: 50px;position: relative;left: 80%;top:20px}
  .mask p{color: #fff;text-align: center;line-height: 40px;letter-spacing: 1px;}
</style>
<style>

</style>