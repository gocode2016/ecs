<template>
  <div class="cookdays" >
  	
    <!--抽奖 抽中奖品-->
    <div v-if="showFlag">
	    <img src="../../static/credit/cook_stxt.png" class="img_txt" />
	    <div class="box_s">
	    	<p style="color: #fff;margin-top: 11%;font-weight: bold;">{{DrawName}}</p>
	    	<div class="boxStyle">
	    		<img :src="PrizeImg"  width="60%">
	    		<p style="font-size: 14px;">{{PrizeName}}</p>
	    	</div>
	    </div>
	    <p class="tips">凭手机号去欣和展位前台领奖哦！</p>
    </div>
    
    <!--抽奖 未抽中奖品-->
    <div v-else>
	    <img src="../../static/credit/cook_ftxt.png" class="img_txt" />
	    <div class="box_f">
	    	<p style="color: #fff;">未中奖~</p>
	    	<div style="line-height: 200px;">
	    		<p style="font-size: 14px;">等待下轮开奖!</p>
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
				showFlag:false,//true中奖 false未中奖
				PrizeName:'',//奖品名称
				PrizeImg:'',//奖品图片
				DrawName:''//奖项
			}
		},
		methods:{
			checkIsPrize(){//获取用户当前抽奖状态
				var _this=this;
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.checkIsPrize+'?memberId='+this.userId
	  		}).then(function(response){
	  			var data=JSON.parse(response.data)
//	  			console.log(data);
	  			if(data.length == 0){//未中奖
	  				_this.showFlag=false;
	  			}else{//中奖
	  				_this.showFlag=true;
	  				_this.PrizeName=data[0].PrizeName;//奖品名称
	  				_this.PrizeImg=data[0].PrizeImg;//奖品图片
	  				_this.DrawName=data[0].DrawName;//奖项
	  			}
	  		})
			}
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
		  this.checkIsPrize();//获取用户当前抽奖状态

		}
	}
</script>

<style scoped>
.cookdays{width: 100%;height: 100%;overflow: hidden;background:url('../../static/credit/cookdays_back.png') no-repeat center;background-size: 100% 100%;
    text-align: center;}
.img_txt{width:60%;margin-top: 18%;}
.box_s{width: 85%;margin:0 auto;background: url('../../static/credit/cook_sdraw.png') no-repeat center;background-size: 100% 100%;
    margin-top: 5%;overflow: hidden;}
.box_f{width: 85%;height:237px;margin:0 auto;background: url('../../static/credit/cook_fdraw.png') no-repeat center;background-size: 100% 100%;
    margin-top: 15%;padding-top: 6px;}
.boxStyle{padding: 10%;}
.tips{width: 70%;height:42px;line-height: 34px;background:url('../../static/credit/cook_btn.png') no-repeat center;background-size: 100% 100%;color: #fff;margin: 0 auto;margin-top: 16%;
    font-size: 15px;}
@media screen and (max-height: 568px){
	.tips{font-size: 13px;margin-top: 12%;}
	/*.boxStyle{padding: 6%;}*/
}
</style>
<style>

</style>