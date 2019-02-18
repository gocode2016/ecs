<template>
  <div class="starkitchenlist">
  	
    <div>
    	<span class="kitDiv">
    	  <input type="text" placeholder="姓名/酒店名" v-model="content"/>
    	  <img src="../../static/credit/search.png" class="searchImg" @click="lookup"/>
    	</span>
    	<span class="cityname" @click="showCity">城市<img src="../../static/credit/address.png" class="addressImg"/></span>
    </div>
    
  	<div class="kitlist">
  		<scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2" height="-47" @on-pullup-loading="load">
  		  <panel  :list="list" :type="type"></panel>
  		</scroller>
  	</div>
  	
  	<!--城市-->
  	<div v-transfer-dom>
        <popup v-model="show" position="top">
              <div class='zone'>
                <div class="searchDiv">
                  <div class="cityDiv">
                    <checker v-model="cityName" default-item-class="areacity-item" selected-item-class="areacity-item-selected">
                      <checker-item  v-for="(item,index) in cityNameList" :key="index" :value="item.AreaName" >{{item.AreaName}}</checker-item>
                    </checker>
                  </div>          
                </div>
                <div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>        
              </div>
        </popup>
    </div>
  	
  </div>
</template>

<script>
import { Panel,Scroller,Checker,CheckerItem,TransferDom,Popup } from 'vux'
import apiUrl from "../apiUrls.js"
export default {
	directives: {
    TransferDom
  },
  components: {
    Panel,
    Scroller,
    Checker,
    CheckerItem,
    Popup
  },
  data () {
    return{
    	show:false,
    	cityName:'',
    	cityNameList:[],
    	type: '1',
    	list:[],
    	datalist:[],
    	count:'',
    	content:'',//搜索内容
    	pageIndex:1,
    	params:{//参数
    		"pageIndex":1,
	  		"ChefActivityId":1
    	},
      pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      }
    }
  },
  mounted(){
	  this.getStarList();  
  },
  methods:{
  	cancel(){
  		this.show=false;
  	},
  	confirm(){
  		this.show=false;
  		this.$router.push({path:'/component/starsearch',query:{cityName:this.cityName}})
  	},
  	showCity(){
  		this.show=true;
  	},
  	load () {
	      setTimeout(() => {
	      	this.pageIndex++;
          this.params={
          	"pageIndex":this.pageIndex,
	  		    "ChefActivityId":1
          }
          this.getStarList();
	        setTimeout(() => {
	          this.$refs.demo2.donePullup()
	        }, 100)
	        if (this.list.length ==this.count) { // unload plugin
	          setTimeout(() => {
	            this.$refs.demo2.disablePullup()
	          }, 100)
	        }
	      }, 500)
	  },
	  getStarList(){//星厨家列表
	  	var _this=this;	  	
	  	this.$http({
	  		method:'POST',
	  		url:apiUrl.getStarList,
	  		data:this.params
	  	}).then(function(response){
	  		 var data=JSON.parse(response.data);
			   _this.dataList=data.data;
			   _this.count=data.Count;
			   _this.cityNameList=data.CityNameList;//城市列表
			   for(var i =0;i<_this.dataList.length;i++){
		  			var li = _this.dataList[i];
		  			var item = {
		  					src: li.HeadPicUrl,
				        title: li.ChefStarName,
				        desc:li.ChefStarPosition,
                ChefStarId:li.ChefStarId,
		  			}		
		  			if(i%2!=0){
		  				item.class='odd';
		  			}
		  			_this.list.push(item);	
			   }	 
	  	})
	  },
	  lookup(){
	  	if(this.content!=''){//搜索查找内容
        this.$router.push({path:'/component/starsearch',query:{content:this.content}})
	  	}
	  }
  }
}
</script>
<style scoped>
.kitDiv{width: 75%;height: 35px;background: #f8f8f8;border-radius: 5px;margin-top:12px;margin-left: 2%;display: inline-block;}
.kitDiv img,.kitDiv input{display: inline-block;vertical-align: middle;}
.kitDiv input{border: none;background: none;outline: none;height: 35px;margin-left: 3%;width: 82%;}
.searchImg{width:8%;}
.cityname{color:#a4a4a4;font-size: 14px;margin-left: 3%;}
.addressImg{width: 17px;margin-left: 1%;position: relative;top: 3px;}
.searchDiv{width: 100%; height: 300px; overflow-y:auto;background: #fff;}
.cityDiv{margin-top: 20px;}
.areacity-item { width: 12.5%; height: 26px; line-height: 26px; text-align: center; color: #3E3E3E; font-size: 13px; margin-left:10%; }
.areacity-item-selected { color: #E83428; border-bottom:1px solid #E83428; }
.areaBtn button{ width: 50%; height: 49px; font-size: 13px;border: none; border-top: 1px solid #E83428;box-sizing: border-box;  }
.areaBtn button:nth-child(1){ background: #fff; color: #E83428;}
.areaBtn button:nth-child(2){ background: #E83428; color: white;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.starkitchenlist .weui-media-box{height: 120px;padding: 0 !important;}
	.starkitchenlist .weui-media-box__hd{width: 90px !important;height: 115px !important;padding-left: 12%;
    padding-right: 15%;margin-right: 0 !important;}
  .starkitchenlist  .weui-media-box__title{font-size: 15px;color: #E83428;}
  .starkitchenlist .weui-media-box__desc{color: #3E3E3E !important;font-size: 13px !important;margin-top: 5px;}
  .starkitchenlist .weui-panel:before{border-top: none;}
  .starkitchenlist .weui-media-box_appmsg .weui-media-box__hd{overflow: hidden;}
  .starkitchenlist .weui-media-box_appmsg .weui-media-box__thumb{width: 130%;max-height: 200%;}
  .starkitchenlist .vux-checker-box{width: 100%;}
</style>