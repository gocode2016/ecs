<template>
  <div class="recommendstart">
    
    <div class="rstart_top">
    	<img src="../../static/credit/recom_text.png"/>
    	<p>总计<span class="numstyle">20</span>个名额，还有<span class="numstyle">{{recomNum}}</span>个名额</p>
    </div>
    <div class="rstart_text">
	    <p>【推荐说明】</p>
	    <p>推荐成功1-5人时，每次奖励推荐者20分</p>
	    <p>推荐成功6-10人时，每次奖励推荐者30分</p>
	    <p>推荐成功11-15人时，每次奖励推荐者40分</p>
	    <p>推荐成功16-20人时，每次奖励推荐者50分</p>
	    <p>点击右上角赶紧分享界面给同行好友或者出示此界面二维码也可扫描注册哦！</p>
    </div>
    <img :src="codeImg" class="codeimg"/>
    
    <div class="mask" v-show="maskFlag">
    	<p>队员无法参与该活动</p>
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
				  userData:{},
		  		userId:'',
		  		userType:'',
		  		openId:'',
				  timestamp:'',
			    nonceStr:'',
			    signature:'',
			    url:'',
			    img:'',
			    codeImg:'',
			    shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBquzmLjoT9ibc3MvrNXia56ibQTKQicNv6IRIoGttDFZ9uXQwxg3IglKasjoKL11wErjqdVctYMoRN17NQ/0?wx_fmt=jpeg',
			    recomNum:0,
			    maskFlag:false
			}
		},
		methods:{
			getRecommendPic(){//获取二维码
          var _this=this;
		  		this.$http({
		  			method:'get',
		  			url:apiUrl.getRecommendPic+'?MemberId='+this.userId
		  		}).then(function(response){
		  			_this.codeImg='https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket='+response.data;
		  		})
			},
			getRecQualifications(){//获取推荐名额
				  var _this=this;
		  		this.$http({
		  			method:'get',
		  			url:apiUrl.getRecQualifications+'?MemberId='+this.userId
		  		}).then(function(response){
		  			_this.recomNum=20-response.data;
		  			if(_this.recomNum<0){
		  				_this.recomNum=0;
		  			}
		  		})
			},
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
					          title:'您的好友邀您来注册，共享会员福利',
//									  link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.img+'&userType='+_this.userType,
									  link:apiUrl.getUrl+'/?action=recommend?userId='+_this.userId+'?img='+_this.img+'?userType='+_this.userType,
					          imgUrl:_this.shareicon,
					          success: function () { 
					              // 用户确认分享后执行的回调函数
									    _this.addECWXTranspondDetails(2);
					          },
				            cancel: function () { 
				              // 用户取消分享后执行的回调函数
//				              alert('取消');
				            },
				            fail:function(res){
//				            	alert('error:'+res.errMsg);
				            }
			          });
			    			
				        wx.onMenuShareAppMessage({
									title: '您的好友邀您来注册，共享会员福利', // 分享标题
									desc: '厨友们快来看，注册会员福利多多！', // 分享描述
//									link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.img+'&userType='+_this.userType,
									link:apiUrl.getUrl+'/?action=recommend?userId='+_this.userId+'?img='+_this.img+'?userType='+_this.userType,
									imgUrl:_this.shareicon,// 分享图标
									success: function () {
									// 用户确认分享后执行的回调函数
									  _this.addECWXTranspondDetails(1);
									},
									cancel: function () {
		//										alert('取消');
									// 用户取消分享后执行的回调函数
									},
									fail:function(res){
		//				            	alert(res.errMsg);
						      }
								});
	    			  })
			  	})
			  })
      },
      addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
		    	var params={
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/recommendstart",
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
		    if(this.userType == 1){//队员无法查看
		    	this.maskFlag=true;
		    }
		    this.getRecommendPic();//获取二维码
		    this.getRecQualifications();//获取名额
		    this.openId=this.userData.Openid;
		    this.img=this.userData.Headimgurl;
		    var uri=window.location.href.split('#')[0];
		    this.url=encodeURIComponent(uri);
		    this.share();
		}
	}
</script>

<style scoped>
  .recommendstart{background: #fff;height: 100%;overflow:scroll;}
  .rstart_top{height: 40%;width: 100%;background: url('../../static/credit/recommendback01.jpg') no-repeat center;background-size: 100% 100%;text-align: center;}
  .rstart_top img{width: 80%;margin-top: 5%;}
  .rstart_top p{font-size: 22px;color: #fff;font-family:STHupo;}
  .numstyle{font-size:24px;}
  .rstart_text{padding-left: 6%;padding-right: 2%;font-size: 14px;}
  .rstart_text p:nth-child(1),.rstart_text p:nth-child(6){color: #f72b2c;}
  .rstart_text p:nth-child(2),.rstart_text p:nth-child(3),.rstart_text p:nth-child(4),.rstart_text p:nth-child(5){color: #603a15;margin-top: 3%;}
  .rstart_text p:nth-child(1){position: relative;left: -2%;}
  .rstart_text p:nth-child(6){margin-top: 5%;}
  .codeimg{width: 50%;margin-left: 25%;margin-top: 5%;margin-bottom: 60px;}
  .mask{width: 100%;height: 100%;background: rgba(0,0,0,0.8);color: #fff;position: fixed;top: 0;}
  .mask p{margin-top:50%;text-align: center;font-size: 18px;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
</style>