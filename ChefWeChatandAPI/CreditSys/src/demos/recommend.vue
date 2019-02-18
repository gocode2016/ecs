<template>
  <div class="recommend">
    <div class="recom-top">
    	<div class="recom-text">
    		<div><img :src="headImg"/></div>
    		<div>
    			<p>{{MemberName}}</p>
    			<p>厨友们快来注册会员，不仅福利多，学习机会更多！</p>
    		</div>
    	</div>
    </div>
    
    <p class="recom_title">注册流程</p>
    <hr style="width: 40px; height:1px;border:none;border-top:1px solid #e83428;margin: 0 auto;"/>
    <p class="recom_pro">长按以下二维码关注微信并完成信息填写即可成为会员</p>
	  <img :src="codeImg" class="codeimg" v-show="qrcodeFlag"/>
	  <p class="recom_title">会员权益</p>
    <hr style="width: 40px; height:1px;border:none;border-top:1px solid #e83428;margin: 0 auto;"/>
	  <img src="../../static/credit/recom_img1.jpg" class="recomImg"/>
    
  </div>
</template>

<script>
import { cookie} from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		cookie
	},
	data(){
		return{
			userId:'',
			headImg:'',
			MemberName:'',
			userType:'',
			codeImg:'',
			qrcodeFlag:false
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
	  			_this.MemberName=data[0].MemberName;
	  		})
	  },
	  getSaleManInfo(){//获取队员信息
  		var _this=this;
  		var params={
  		  "SalemanId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSaleManInfo,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.MemberName=data.Name;
  		})
  	},
  	getRecommendPic(){//获取二维码
        var _this=this;
	  		this.$http({
	  			method:'get',
	  			url:apiUrl.getRecommendPic+'?MemberId='+this.userId
	  		}).then(function(response){
	  			_this.codeImg='https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket='+response.data;
	  		})
		}
	},
	mounted(){
    this.userId=Number(this.$route.query.userId);
    this.userType=Number(this.$route.query.userType);
    this.headImg=this.$route.query.img;
    if(this.userType==1){//队员
    	this.getSaleManInfo();
    	this.qrcodeFlag=false;
    }else if(this.userType==2){//会员
    	this.getMemberById();
    	this.getRecommendPic();
    	this.qrcodeFlag=true;
    }
	}
}
</script>

<style scoped>
	.recom-top{width: 100%;height: 250px;background: url('../../static/credit/recommendback02.jpg') no-repeat center;background-size:100% 100% ;overflow: hidden;}
	.recom-text{width: 85%;margin-top: 30px;margin-left: 10%;display: flex;}
	.recom-text div:nth-child(1){width: 30%;margin-right: 10px;}
	.recom-text div:nth-child(1) img{width: 100%;border-radius: 50%;}
	.recom-text div:nth-child(2){width: 70%;color: #fff;}
	.recom-text p:nth-child(1){font-size: 20px;}
	.recom-text p:nth-child(2){font-size: 15px;}
	.codeimg{width: 40%;margin-left: 30%;}
	.recom_title{color: #3e3e3e;font-size: 16px;text-align: center;}
	.recom_pro{color: #3e3e3e;font-size: 13px;text-align: center;margin-top: 10px;}
	.recomImg{width: 90%;margin: 20px 5%;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
</style>