<template>
  <div class="teamsearch">
    <div class="tab-swiper vux-center" v-for="(item,index) in textlist" :key="index" @click="lookChefInfo(item.MemberId)">
		 	<div>
		 	  <p class="swiper_text">ID:{{item.MemberId}}</p>
		 	</div>
		 	<div>
		 	  <p class="swiper_text">{{item.MemberName}}</p>
		 	  <p class="swiper_date">{{item.MemberTelePhone}}</p>
		 	</div>
		 	<div>
		 	  <p class="swiper_text">{{item.HotelName}}</p>
		 	  <p class="swiper_date swiper_date_act"><span>业务代表：{{item.Name}}</span></p>
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
				index:'',//0全部 1待完善  2已完善
				searchValue:'',//姓名
				areaId:0,//区域Id
				textlist:[]//搜索结果列表
			}
		},
		methods:{
		  lookChefInfo(memberId){//查看厨师主页信息
		  	if(this.index==2){
		  		this.$router.push({path:'/component/chefhomepage',query:{memberId:memberId}})
		  	}
		  },
		  getMemberByArea(index,searchValue,areaId){//获取厨师信息
	  		var _this=this;
	  		if(index==0){
	  			var SearchType=2;
	  			var AreaId=areaId;
	  			var userId=0;
	  		}else if(index==1){
	  			var SearchType=0;
	  			var AreaId=areaId;
	  			var userId=0;
	  		}else if(index==2){
	  			var SearchType=0;
	  			var AreaId=0;
	  			var userId=this.userId;
	  		}
	  		var params={
		  		'AreaId':AreaId,
					'SalemanId':userId,
					'SearchType':SearchType,     //2是第一部分
					'ActiveOB': 0,    //1正序 2倒序
					'IntegralOB':0,       //1 2
					'AuthType':0,        //1实名认证 2注册码认证
					'MemberCode':0,       // 1已完善  2未完善
					'Name':searchValue        //关键字搜索
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getMemberByArea,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.textlist=data;
	  		})
	  	},
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
			this.index=this.$route.query.index;
			this.searchValue=this.$route.query.searchValue;
			this.areaId=this.$route.query.areaId;
			this.getMemberByArea(this.index,this.searchValue,this.areaId);//查询绑定厨师搜索结果
		}
	}
</script>

<style scoped>
	.tab-swiper{width: 100%;height: 76px;border-bottom:1px solid #ebebeb;font-size: 13px;display: flex;}
	.tab-swiper div:nth-child(1){width: 20%; padding-left: 5%; }
	.tab-swiper div:nth-child(2){width: 35%; margin-left: 8%;  }
	.tab-swiper div:nth-child(3){width: 45%;text-align: right;padding-right: 5%;}
	.swiper_text{margin-top: 15px;}
	.swiper_date{margin-top: 5px;color: #787878;font-size: 12px;}
	.swiper_date_act{color:#1c1c1c ;}
</style>
<style>
 #vux_view_box_body{background: #fff;}
</style>