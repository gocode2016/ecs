<template>
  <div class="pageshare">
  	<img src="../../static/credit/pagesharebook.jpg" />
  </div>
</template>

<script>
import { cookie,Toast } from 'vux'
import wx from 'weixin-js-sdk'
import apiUrl from '../apiUrls.js'
	
export default{
	components:{
		cookie,
		Toast
	},
	data(){
		return{
			userData:'',
			userId:'',
			userType:'',
			openId:'',
			timestamp:'',
	    nonceStr:'',
	    signature:'',
	    url:'',
	    shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvXXicf0GRYqh1l6AvwUEtYIVmgIic4e647WYT3aQZm2PH4N1veICBOkciaZYhTS5gh4bibYWibwIvGA2g/0?wx_fmt=jpeg'
		}
	},
	methods:{
		  share(){
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
				        'onMenuShareTimeline',//分享到朋友圈
				        'onMenuShareAppMessage'//分享给朋友
				      ]
				    })
		  			  
		  			wx.ready(function(){
		    			wx.onMenuShareTimeline({
							    title: '想成为名厨？你得有颗爱读书的心...', // 分享标题
							    link: apiUrl.getUrl+'/#/component/tasteshare', // 分享链接
									imgUrl: _this.shareicon, // 分享图标
									success: function () { 
									// 用户确认分享后执行的回调函数
									  if(_this.userType==1){
									    _this.addECWXTranspondDetails(2);
									  }else if(_this.userType==2){
									  	_this.isCommTranspond();//是否分享过朋友圈
									  }
									},
									cancel: function () {
//										alert('取消');
									// 用户取消分享后执行的回调函数
									}
							});
		          wx.onMenuShareAppMessage({
									title: '想成为名厨？你得有颗爱读书的心...', // 分享标题
									desc: '来看看我们为你准备了哪些好书......', // 分享描述
									link:apiUrl.getUrl+'/#/component/tasteshare',
									imgUrl:_this.shareicon,// 分享图标
									success: function () {
									// 用户确认分享后执行的回调函数
//									  alert('分享成功');
									  _this.addECWXTranspondDetails(1);
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
		  			
		  		})
		  	})
      },
      addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
      	var _this=this;
	    	var params={
	  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/pageshare20180419",
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
      isCommTranspond(){//是否分享朋友圈
	    	var _this=this;
	    	var params={
	  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/pageshare20180419",
	  		  "OpenId":this.openId,
	  		  "TranspondType":2
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.isCommTranspond,
	  			data:params
	  		}).then(function(response){
	  			if(response.data=='OK'){//没有转发过
				    _this.updateMemberIntegral(10);
				    _this.addECBrowseDetails();//记录活动足迹
	        }else if(response.data=='No'){
			  	  _this.$vux.toast.text('只有第一次转发有积分奖励哦', 'middle');
	        }
				  _this.addECWXTranspondDetails(2);
	  		})
	    },
	    updateMemberIntegral(inte){//更改积分
	  		var _this=this;
	  		var params={
	  		  'Operation':'plus',
					'Integral':inte,
					'MemberId':this.userId,
					'IntegralSource':'指定页面分享',
					'Remark':'转发读书日活动赚积分'
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.updateMemberIntegral,
	  			data:params
	  		}).then(function(response){
	  			_this.$vux.toast.text('已成功添加10积分', 'middle');
	  		})
	  	},
	  	addECBrowseDetails(){//记录活动足迹
				var params={
				  "ECURL":apiUrl.getUrl+"/#/component/pageshare20180419",
				  "PageName":" ",
				  "Parameter":"",
				  "OpenId":this.openId,
				  "PageShort":"指定页面分享",
				  "PageDetail":"转发读书日活动赚积分",
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
	  if(this.userType==0){
	  	this.$router.push('/component/goregister');
	  }
	  this.openId=this.userData.Openid;
		var uri=location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
	}
}
</script>

<style scoped>
/*.pageshare {width:100%;height:100%;background: url('../../static/credit/pagesharebook.jpg') no-repeat center;background-size: 100% 100%;}*/
.pageshare img{width: 100%;}
</style>
<style>
#vux_view_box_body{background: rgba(0,0,0,0.7);}
</style>
	
