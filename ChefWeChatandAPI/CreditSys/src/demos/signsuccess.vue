<template>
  <div class="signsuccess">
  	
    <div v-show="sucFlag">
	    <img src="../../static/credit/sign_suc.png" class="sign_suc"/>
	    <div class="sign_box" >
	    	<div class="sign_boxs">
	    		<p class="sign_name">{{MemberName}}您好！</p>
	    		<p class="sign_stext">感谢您参加{{activityName}}，请出示此界面给工作人员入场</p>
	    	</div>
	    </div>
    </div>
    
    <div v-show="failFlag">
	    <img src="../../static/credit/sign_fail.png" class="sign_fail"/>
	    <div class="sign_box">
	    	<div class="sign_boxs">
	    		<p class="sign_ftext">队员无法参与活动签到</p>
	    	</div>
	    </div>
    </div>
    
  </div>
</template>

<script>
	import { cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
	
	export default{
		components:{
			cookie
		},
		data(){
			return{
				userData:{},
				userId:'',
				openId:'',
				userType:'',
				MemberName:'',
				Ecurl:'',
				Parameter:'',
				activityName:'',//活动名称
				count:'',//从注册页跳转到签到页
				sucFlag:false,//签到成功
				failFlag:false//签到失败
			}
		},
		methods:{
			addActivity(activityId){//签到接口
				var _this=this;
				var params={
					"ActivityId":activityId,
					"UserId":this.userId,
					"OpenId":this.openId,
					"SignDate":""
				}
				this.$http({
					method:'post',
					url:apiUrl.signIn,
					data:params
				}).then(function(res){
					var data=JSON.parse(res.data);
					var count=parseInt(data.Count);
					_this.activityName=data.text;
					if(count==-1){//-1活动结束
						$('.sign_ftext').html("活动已结束，无法参与活动签到");
						_this.failFlag=true;
					}else if(count==1){//1成功 
						_this.sucFlag=true;
						_this.addECBrowseDetails();//记录页面访问
					}else if(count==2){//2曾经签到成功
						_this.sucFlag=true;
					}
				})
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
				  "PageShort":"线下签到",
				  "PageDetail":this.activityName,
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
	  			_this.MemberName=data[0].MemberName;
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
	    this.openId=this.userData.Openid;
	    
		  this.count=this.$route.query.count;//从注册页跳转到签到页
		  this.activityName=this.$route.query.activityName;//从注册页跳转到签到页
		  
		  if(this.count!=undefined){//从注册页跳转到签到页
		  	this.getMemberById();
	  	  if(this.count==-1){//-1活动结束
					$('.sign_ftext').html("活动已结束，无法参与活动签到");
					this.failFlag=true;
				}else if(this.count==1){//1成功 
					this.sucFlag=true;
					this.addECBrowseDetails();//记录页面访问
				}else if(this.count==2){//2曾经签到成功
					this.sucFlag=true;
				}
		  }else{//点击推送图文跳转到签到页
		  	var activityId = ''
		  	if(this.$route.query.activityId.indexOf('#/')!=-1){
					activityId = this.$route.query.activityId.slice(0,-2)
				}else{
					activityId = this.$route.query.activityId
				}
		  	
//		  	if(this.userType==1){//队员
//			  	this.failFlag=true;
//			  }else if(this.userType==2){//会员 调签到接口
//			  	//调接口
//			  	this.addActivity(activityId);
//			  	this.getMemberById();
//			  }else if(this.userType==0){//未注册
//			  	this.$router.push({path:'/component/signregister',query:{signsuccess:0,activityId:activityId}})
//			  }
			  
			  if(this.userType == 0 && this.userId == 0){//未注册
			  	this.$router.push({path:'/component/signregister',query:{signsuccess:0,activityId:activityId}})
			  }else if(this.userType == 1 && this.userId > 0){//队员
			  	this.failFlag=true;
			  }else if(this.userType == 2 && this.userId > 0){//会员 调签到接口
			  	//调接口
			  	this.addActivity(activityId);
			  	this.getMemberById();
			  }
			  
		  }
		  
		  
		}
	}
</script>

<style scoped>
.signsuccess{width: 100%;height: 100%;background: url('../../static/credit/sign_back.png') no-repeat center;background-size: 100% 100%;overflow: hidden;}
.sign_suc,.sign_fail{width: 90%;margin-left: 5%;margin-top: 30%;position: relative;z-index: 20;}
.sign_box{width: 74%;height: 180px;background: #f1f1f1;border-radius: 10px;margin-left: 13%;overflow: hidden;color: #e0292e;position: relative;top: -40px;}
.sign_boxs{width: 90%;height: 160px;margin-left: 5%;margin-top: 10px;background: #fff;border-radius: 5px;overflow: hidden;}
.sign_boxs .sign_name{text-align: center;font-size: 20px;margin-top: 20px;}
.sign_boxs .sign_stext{padding:15px;font-size: 15px;}
.sign_boxs .sign_ftext{margin-top: 70px;text-align: center;}
</style>
<style>

</style>