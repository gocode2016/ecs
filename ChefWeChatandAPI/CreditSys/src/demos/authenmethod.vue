<template>
  <div class="authenmethod">
    
    <p class="authenmethod-top">请选择认证方式</p>
    <div class="authenmethod-wrap">
    	<div @click="jump(1)">
    		<img src="../../static/credit/authen_a.png"/>
    		<img src="../../static/credit/authen_c.png" class="recomImg"/>
    		<p>实名认证</p>
    	</div>
    	<div @click="jump(2)">
    		<img src="../../static/credit/authen_b.png"/>
    		<p>认证码认证</p>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { cookie,Toast } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		cookie,
		Toast
	},
	data(){
		return{
			userData:{},
			userId:''
		}
	},
	methods:{
		jump(val){//1实名认证 2注册码认证
			//通过认证码认证成功，不能再点击实名认证和认证码认证；通过实名认证成功，不能再点击认证码认证
			this.getMemberProfile(val);//判断是否通过注册码认证
		},
    getMemberAuth(val){//获取认证信息 -1不通过 0审核中 1通过 null没有提交过认证信息
				var _this=this;
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberAuth+'?memberId='+this.userId
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			//实名认证审核中和审核通过 不能点击注册码认证
	  			//注册码认证成功 不能点击再点击任意一个
	  			if(val==1){//点击实名认证
	  				if(data==null||data.AuthState==-1){//未认证或认证不通过
							_this.$router.push('/component/authenhotel');//先完善酒店信息 再实名认证
		  			}else if(data.AuthState==0||data.AuthState==1){//查看实名认证信息
							_this.$router.push('/component/authenname');
		  			}
	  			}else if(val==2){//点击注册码认证
		  			if(data==null||data.AuthState==-1){//实名未认证或认证不通过
							_this.$router.push('/component/authencode');//注册码认证
		  			}else if(data.AuthState==1){
		  				//提示已实名认证
							_this.$vux.toast.text('您已经认证通过', 'middle');
		  			}else if(data.AuthState==0){
		  				//提示实名认证审核中
							_this.$vux.toast.text('实名认证审核中', 'middle');
		  			}
	  			}
	  		})
		},
		getMemberProfile(val){//获取简历信息
  		var _this=this;
			var params={
			  'MemberId':this.userId
			}
			this.$http({
				method:'POST',
				url:apiUrl.getMemberProfile,
				data:params
			}).then(function(response){
				var data=JSON.parse(response.data);
				var RecommendId=data[0].RecommendId;
				if(RecommendId>99){//已注册码认证 不能进入实名认证
					//提示已认证
					_this.$vux.toast.text('您已经认证通过', 'middle');
				}else if(RecommendId<=99){//未认证且点击实名认证
					_this.getMemberAuth(val);//判断是否实名认证
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
	  this.userId=Number(this.userData.UserId);

	}
}
</script>

<style scoped>
.authenmethod{height: 100%;background: #fff;overflow: hidden;}
.authenmethod-top{width: 100%;text-align: center;color: #E83428;margin-top: 5%;}
.authenmethod-wrap{width: 100%;height:90%;border-top: 1px solid #f4f4f4;margin-top: 3%;}
.authenmethod-wrap div{padding: 8% 0;width: 80%;background: #f8f8f8;border-radius: 5px;margin-left: 10%;margin-top: 7%;text-align: center;color: #3e3e3e;font-size: 14px;}
.authenmethod-wrap div p{margin-top: 5px;}
.authenmethod-wrap img{width: 25%;}
.recomImg{width: 6% !important;position: absolute;right: 32%;}
</style>
<style>

</style>