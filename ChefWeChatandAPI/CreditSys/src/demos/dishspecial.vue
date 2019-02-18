<template>
  <div class="dishspecial">
		<p class="dish_title"><span>{{ztName}}</span></p>
		<div style="padding: 0 3%;">
		  <div v-for="(item,index) in ztInfo" :key="index">
		  	<div v-html="item.Content"></div>
		  	<div class="wrap">
		    	<div class="box" v-for="(items,index) in item.CaiPinList">
		    		<p><img :src="items.Images"/></p>
		    		<p @click="jump(items.CaiPinId)">{{items.CaiPinName}}</p>
		    	</div>
		    </div>
		  </div>
		</div>
  </div>
</template>

<script>
//import { } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		
	},
	data(){
		return{
			specialId:2,
			ztInfo:[],
			ztName:''
		}
	},
	methods:{
		getZhuanTi(){//获取专题信息
			var self = this;
			this.$http({
				method:'get',
				url:apiUrl.getZhuanTiInfo+'?ZhuanTiId=' + this.specialId
			}).then(function(res){
//				console.log(JSON.parse(res.data));
//				var data = JSON.parse(res.data);
				var data = res.data;//测试
				self.ztInfo = data;
				self.ztName = data[0].ZhuanTiName
			  $('title').html(self.ztName);
			  for(let i in self.ztInfo){
			  	var info = self.ztInfo[i];
			  	for(let i in info.CaiPinList){
			  		var CaiPinList = info.CaiPinList[i];
			  		CaiPinList.Images = CaiPinList.Images.split(',')[0];
//			  		console.log(CaiPinList.Images);
			  	}
			  }
			})
		},
		jump(CaiPinId){//跳转到菜品详情页
		  this.$router.push({path:'/component/dishstoredetail',query:{dishId:CaiPinId}});
		}
	},
	mounted(){
//		if(this.$route.query.specialId.indexOf('#/')!=-1){
//			this.specialId = this.$route.query.specialId.slice(0,-2)
//		}else{
//			this.specialId = this.$route.query.specialId
//		}
		this.getZhuanTi();//获取专题信息
	}
}
</script>

<style scoped>
.dish_title{text-align: center;margin: 14px 0;}
.dish_title span{display: inline-block;border-bottom: 2px solid #e83428;font-weight: 600;letter-spacing: 1px;}
.dish_text{font-size: 14px;color: #8c8c8c;overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp:3;-webkit-box-orient: vertical;}
.wrap{width: 100%;display: flex;flex-wrap: wrap;}
.box{width: 48%;margin: 10px 0;}
.box:nth-child(even){margin-left: 3%;}
.box p:nth-child(1) img{width: 100%;}
.box p:nth-child(2){font-size: 14px;color: #323232;}
.box p:nth-child(2):after{content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1px 1px 0 0;border-color: #323232;border-style: solid;
    position: relative;top: 0px;left: 5px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
.dish_intro{padding: 20px 10px;background: #f9f9f9;margin-top: 14px;font-size: 13px;color: #3E3E3E;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>