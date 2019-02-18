<template>
  <div class="cookday" v-bind:style="{background:'url('+backImg+') no-repeat center center',backgroundSize:'100% 100%'}">
    
    <img src="../../static/credit/cook_succ.png" class="cook_sucimg"/>
    <div class="cook_box" >
    	<div class="cook_boxs">
    		<p class="cook_name">您好！</p>
    		<p class="cook_stext">感谢您参加无锡美食文化节,请出示此界面找工作人员领取奖品</p>
    	</div>
    </div>
    <p class="cook_txt" style="font-size: 18px;margin-top: -10px;">另有精彩大奖即将揭晓</p>
    <p class="cook_txt" style="font-size: 14px;margin-top: 10px;">敬请期待！</p>
    
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
				userType:'',
				backImg:'../../static/credit/sign_back.png'
			}
		},
		methods:{
			
		},
		mounted(){
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
		  this.userId=parseInt(this.userData.UserId);
		  this.userType=parseInt(this.userData.UserType);
		  if(this.userType == 0 && this.userId == 0){//未注册
		  	this.$router.push('/component/cookregister');
		  }
		}
	}
</script>

<style scoped>
.cookday{width: 100%;height: 100%;overflow: hidden;}
.cook_sucimg{width: 90%;margin-left: 5%;margin-top: 20%;position: relative;z-index: 20;}
.cook_box{width: 74%;height: 180px;background: #f1f1f1;border-radius: 10px;margin-left: 13%;overflow: hidden;color: #e0292e;position: relative;top: -40px;}
.cook_boxs{width: 90%;height: 160px;margin-left: 5%;margin-top: 10px;background: #fff;border-radius: 5px;overflow: hidden;}
.cook_boxs .cook_name{text-align: center;font-size: 20px;margin-top: 24px;}
.cook_boxs .cook_stext{font-size: 15px;width: 74%;margin: 0 auto;}
.cook_txt{color: #fff;text-align: center;letter-spacing: 1px;}

</style>
<style>

</style>