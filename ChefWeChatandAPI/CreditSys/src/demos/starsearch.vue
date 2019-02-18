<template>
  <div class="starsearch">
  	<span v-show="flag">暂无搜索结果</span>
    <panel  :list="list" :type="type"></panel>
  </div>
</template>

<script>
import { Panel } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Panel
	},
	data(){
		return{
			flag:false,
			content:'',//姓名、酒店
			cityName:'',//城市
			type: '1',
    	list:[],
    	dataList:[]
		}
	},
	methods:{
		getStarList(val){//星厨家列表
		  	var _this=this;	  	
		  	if(val=='name'){
		  		var params={
			  		"pageIndex":1,
			  		"ChefActivityId":1,
			  		"Name":this.content
		  	  }
		  	}else if(val=='city'){
		  		var params={
			  		"pageIndex":1,
			  		"ChefActivityId":1,
		  		  "City":this.cityName	
		  	  }
		  	}
		  	this.$http({
		  		method:'POST',
		  		url:apiUrl.getStarList,
		  		data:params
		  	}).then(function(response){
		  		 var data=JSON.parse(response.data);
		  		 if(data.data.length==0){
		  		 	 _this.flag=true;
		  		 }else{
			  		 	_this.dataList=data.data;
					    for(var i =0;i<_this.dataList.length;i++){
				  			var li = _this.dataList[i];
				  			var item = {
				  				src: li.HeadPicUrl,
						        title: li.ChefStarName,
						        desc:li.ChefStarPosition,
				                ChefStarId:li.ChefStarId,
					  		}		
				  			_this.list.push(item);	
					    }	 
		  		 }
		  	})
		},
	},
	mounted(){
		this.content=this.$route.query.content;//姓名 酒店
		this.cityName=this.$route.query.cityName;//城市
		if(this.content!=undefined){
			this.getStarList('name');
		}else if(this.cityName!=undefined){
			this.getStarList('city');
		}
	}
}
</script>

<style scoped>
.starsearch span{text-align: center;color: #3E3E3E;font-size: 14px;display: inline-block;width: 100%;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
  .starsearch .weui-media-box{height: 120px;padding: 0 !important;}
  .starsearch .weui-media-box__hd{width: 90px !important;height: 115px !important;padding-left: 12%;
    padding-right: 15%;margin-right: 0 !important;}
  .starsearch  .weui-media-box__title{font-size: 15px;color: #E83428;}
  .starsearch .weui-media-box__desc{color: #3E3E3E !important;font-size: 13px !important;margin-top: 5px;}
  .starsearch .weui-panel:before{border-top: none;}
  .starsearch .weui-media-box_appmsg .weui-media-box__hd{overflow: hidden;}
  .starsearch .weui-media-box_appmsg .weui-media-box__thumb{width: 130%;max-height: 200%;}
</style>