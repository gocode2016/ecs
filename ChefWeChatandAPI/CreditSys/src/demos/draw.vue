<template>
  <div class="draw">
    <div class="draw_box">
	    <div class="draw_top">
	    	<div class="imgDiv">
	    		<img :src="userImg" />
	    	</div>
	    </div>
	    <div class="draw_style">您已连续签到7天，快快抽奖吧!</div>
    </div>
    <!--抽奖-->
    <div class="draw_div">
    	<img src='../../static/credit/drawbtn.png' @click="getSignin"/>
    </div>
    <!--获奖名单-->
    <div class="draw_list">获奖名单</div>
    <div class="draw_listname">
    	<p><span>获奖者ID</span><span class="inte_text">所获积分</span></p>
    	<ul>
    		<li><span>10078</span><span class="inte_text">39积分</span></li>
    		<li><span>10035</span><span class="inte_text">9积分</span></li>
    		<li><span>10061</span><span class="inte_text">29积分</span></li>
    		<li><span>10024</span><span class="inte_text">1积分</span></li>
    		<li><span>10058</span><span class="inte_text">9积分</span></li>
    	</ul>
    </div>
    <!--中奖页面-->
    <div class="mask" v-show='drawFlag'>
    	<div class="drawbox">
    		<img src="../../static/credit/zj.png" class="drawText"/>
    		<p>免费获得{{inteNum}}积分</p>
    		<div class="drawBtn">
    			<button @click="knowBtn">知道了</button>
    		</div>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { cookie,Toast } from 'vux'
import apiUrl from '../apiUrls'
import wx from 'weixin-js-sdk'
export default{
	components:{
		cookie,
		Toast
	},
	data(){
		return{
			btnNum:0,//未点击开始
			timestamp:'',
      nonceStr:'',
      signature:'',
      url:'',
			drawFlag:false,
			userData:{},
			openId:'',
  		userId:'',
  		userType:'',
  		userImg:'',
  		wxImg:'',
  		shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvVqsgyWzYDibvEfDPS8vsXoDLYGZ87rtdKRiaq7K4bl9VRiaYOic6u3B1aDgH32qbmia2iax6XRUVkLAiaA/0?wx_fmt=jpeg',
  		prizeDeg:'',//中奖位置
  		inteNum:'',//中奖积分
		}
	},
	methods:{
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
			  	_this.data=data[0];
			  	if(_this.data.ImgUrl==null||_this.data.ImgUrl==""){
				  	_this.userImg=_this.wxImg;//微信头像
				  }else{
				  	_this.userImg=_this.data.ImgUrl;//基本信息头像
				  }
			  }else{
			  	_this.userImg=_this.wxImg;//微信头像
			  }
  		})
  	},
		chance(){//抽奖概率
			var randomNum=diu_Randomize(1,1000);//获取随机数
			function diu_Randomize(min,max){   
		    return Math.floor( ( Math.random() * max ) + min );   
			}   
			if(randomNum<=600){// 1积分 60%
				this.prizeDeg=315;
				this.inteNum=1;
			}else if(randomNum>600&&randomNum<=880){//9积分 28%
				this.prizeDeg=225;
				this.inteNum=9;
			}else if(randomNum>880&&randomNum<=960){//29积分 8%
				this.prizeDeg=45;
				this.inteNum=29;
			}else if(randomNum>960&&randomNum<=1000){//39积分 4%
				this.prizeDeg=135;
				this.inteNum=39;
			}
		},
		flagDraw(){//记录哪天抽奖
			var _this=this;
  		var params={
  		  "MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.flagDraw,
  			data:params
  		}).then(function(response){
  			_this.updateMemberIntegral();
  		})
  	},
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'plus',
				'Integral':this.inteNum,
				'MemberId':this.userId,
				'IntegralSource':'签到抽奖',
				'Remark':'签到抽奖'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			console.log(response);
  		})
  	},
  	getSignin(){//判断能否抽奖
  		var _this=this;
  		var params={
  			"MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSignin,
  			data:params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
  			var signList=data.SigninCredit;
        var drawList=[];
  			for(let i in signList){
  				var signlist=signList[i];
  				drawList.push(signlist.IsDraw);
  			} 
			  drawList=drawList.join(',');
			  
		    if(drawList.indexOf('1')<0&&drawList.indexOf('2')<0){//可以抽奖
          _this.btnNum++;
          if(_this.btnNum==1){
          	_this.chance();//抽奖概率
    		  	_this.flagDraw();//记录哪天抽奖
    		  	var deg=0;//旋转度数
		      	var draws = null;//定时器
		        var endDeg=1440+_this.prizeDeg;  
		        var deg1=endDeg-270;
		        draws = setInterval(function(){
		        	deg +=5;
		     			$('.draw_div img').css({
		      		  'transform':'rotate('+deg+'deg)'
		      	  })
		     			if(deg >= deg1){
		     				clearInterval(draws);
		     			  draws = setInterval(drawTime,12);
		     			}
		        },1)
		        function drawTime(){
		        	deg += 1;
		        	$('.draw_div img').css({
		      		  'transform':'rotate('+deg+'deg)'
		      	  })
		        	if(deg >= endDeg){
		        		clearInterval(draws);
		//	        		clearInterval(lightback);
		        		setTimeout(function(){
		        			_this.drawFlag=true;
		        			$('.draw').bind("touchmove",function(e){  
	                  e.preventDefault();  
	                });  
		        		},200)
		        	}
		        }
          }
			  }else{//不可以抽奖
    	  	_this.$vux.toast.text('连续签到7天只有一次抽奖机会哦', 'middle');
			  }
  		})
  	},
    flash(){//撒花效果
      	var position=-180;
				var backposition=setInterval(function(){
					position+=10;
					$('.draw_box').css({
					  'background-position':'0 '+position+'px'
				  })
					if(position>=0){
						clearInterval(backposition);
					}
				},100)
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
				            title:'签到送积分啦，连续签到还能抽奖哦！',
				            link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.wxImg+'&userType='+_this.userType,
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
//				            	alert(res.errMsg);
				            }
		            });
		            wx.onMenuShareAppMessage({
										title: '签到送积分啦，连续签到还能抽奖哦！', // 分享标题
										desc: '签到送积分啦，连续签到还能抽奖哦！', // 分享描述
				            link:apiUrl.getUrl+'/#/component/recommend?userId='+_this.userId+'&img='+_this.wxImg+'&userType='+_this.userType,
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
    },
    addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
    	var params={
  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/draw",
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
    knowBtn(){
    	this.$router.push('/component/integraldetail');
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
		$(".draw").unbind('touchmove');
		//设置抽奖转盘高度
		var height=($(window).width())*0.8;		
		$('.draw_div').css({
			'height':height
		})
		this.flash();
		
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
	  this.wxImg=this.userData.Headimgurl;
	  this.getMemberById();
	  var uri=window.location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
	}
}
</script>

<style scoped>
	.draw_box{width: 100%;height: 180px;background: url('../../static/credit/flower.png') no-repeat ;background-size: 100% 100%;}
  .draw_top{width: 100%;height: 96px;position: relative;z-index:0;padding-top: 25px;}
  .imgDiv{width:90px;height: 90px;border: 3px solid #ededed;border-radius: 50%;position: absolute;left: 50%;margin-left: -45px;overflow: hidden;}
  .imgDiv img{width: 100%;}
  .draw_style{width:80%;height:47px;margin-left:10%;position: relative;top:-6px;z-index:1;text-align:center;color:#fff;font-size:13px;line-height:47px;
              background: url('../../static/credit/sign.png') no-repeat center;background-size: 100% 100%;}
  .draw_div{text-align:center;position:relative;width:80%;margin-left:10%;background: url('../../static/credit/zp1.png') no-repeat center;background-size:100% 100% ;}
  .draw_div img{width: 60px;position:absolute;top:50% ;left: 50%;margin-left:-30px;margin-top:-35px;transform-origin: center 40px;}
  .draw_list{width: 40%;height: 30px;line-height:30px;text-align:center;background: #e93429;color: #fff;font-size: 13px;margin-left: 30%;margin-top: 35px;}
  .draw_listname{width: 80%;height: 215px;border: 1px solid #ddd;background: #fffaf4;margin-left: 10%;font-size: 14px;margin-bottom: 50px;}
  .draw_listname p{margin-top: 10px;}
  .draw_listname span{display: inline-block;width: 120px;text-align: center;}
  .inte_text{float: right;}
  .draw_listname ul li{list-style: none;color: #8c3d16;margin-top: 13px;}
  .mask{width: 100%;height: 2000px;position: fixed;top: 0;background: rgba(0,0,0,0.5);z-index:2;font-size: 18px;color: #fff;font-weight: bold;text-align: center;}
  .mask .drawbox{width: 60%;margin-top: 130px;margin-left: 20%;}
  .mask p{margin-top: 40px;}
  .mask .drawText{width: 100%;}
  .drawbox div{width: 100%;margin-top: 80px;}
  .drawBtn *{display: inline-block;vertical-align: middle;}
  .drawbox button{width: 68%;height: 40px;outline:none;background: #E83428;color: #fff;border: none;font-size: 18px;font-weight: bold;border-radius: 2px;}
</style>
<style>
	 #vux_view_box_body{background: #fff;letter-spacing: 1px;}	
</style>