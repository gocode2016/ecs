<template>
  <div class="dishstorebanner">
    <img  v-for="(item,index) in bannerList" :src="item.img" @click="jump(item.url)"/>
  </div>
</template>

<script>
//	import { } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		
	},
	data(){
		return{
			bannerList:[]
		}
	},
	methods:{
	  getBanner(){//获取轮播图
    	  	var self = this;
    	  	var params={
  		      "BannerType":3
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getBanner,
		  			data:params
		  		}).then(function(response){
		  			var obj = response.data;
						var data = JSON.parse(obj);
						console.log(data);
						for(var i=0;i<data.length;i++){
							var bannerlist=data[i];
							var item={
								url:bannerlist.Link,
								img:bannerlist.PicUrl
							}
							self.bannerList.push(item);
						}
		  		})
    	},
	  jump(link){//页面跳转
	  	this.$router.push(link);
	  }
	},
	mounted(){
		this.getBanner();//获取轮播图 活动列表
	}
}
</script>

<style scoped>
.dishstorebanner img{width: 94%;border-radius: 5px;margin: 10px 3% 0 3%;}
</style>
<style>

</style>