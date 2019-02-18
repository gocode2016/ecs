<template>
  <div class="dishstarkitchen">
  	
    <!--搜索-->
    <div class="searchDiv">
    	<input type="text" placeholder="搜索菜名\食材" v-model='searchValue' @input="searchMethod"/>
    	<img src="../../static/credit/search.png" @click="searchClick"/>
    </div>
    <div style="display: inline-block;width:18%;position: relative; right: -6%;">
    	<span style="color: #3E3E3E;font-size: 13px;">分类</span>
    	<img src="../../static/credit/dishstore_menu.png" @click="menuClick" style="width: 36%;vertical-align: middle; "/>
    </div>
    <!--<img src="../../static/credit/address.png" style="width: 6%;vertical-align: middle; position: relative; right: -10%;" @click="areaClick"/>-->
    
    <!--tab切换-->
    <div class="star-tab">
	    <tab :line-width="1" custom-bar-width="50px">
	      	<tab-item selected @click.native="tabClick(1)">导师团</tab-item>
	      	<tab-item @click.native="tabClick(2)">星厨团</tab-item>
	    </tab>
    </div>
    
    <!--导师团-->
    <div v-if="showFlag">
    	<div v-for="(item,index) in tutorGroupList" :key="index"  style="padding:20px 10px;border-bottom: 1px solid #f1f1f1;">
    		<div class="box">
    			<div class="headDiv">
    				<img :src="item.HeadPicUrl" class="headImg">
    			</div>
    			<img src="../../static/credit/starkitchen_dsicon.png" class="iconImg"/>
    			<div>
    				<p style="margin-top: 1%;">{{item.ChefName}}</p>
    				<p style="font-size: 13px;color:#8C8C8C">{{item.Position}} {{item.HotelName}}</p>
    			</div>
    		</div>
    		<img :src="item.DishPicUrl" style="width: 100%;margin-top: 5px;" @click="jump('tutordish',item.CaiPinId)"/>
    		<!--<div @click="jump('tutordish',item.CaiPinId)" style="height: 175px;" v-bind:style="{background:'url('+item.DishPicUrl+') no-repeat center',backgroundSize:'100% 100%'}"></div>-->
    		<p>
    			<span class="dishname" @click="jump('tutordish',item.CaiPinId)">{{item.DishName}}</span>
    			<span class="looknum"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,'tutor',index)"><span>{{item.LyCount}}人看过</span></span>
    		</p>
    	</div>
    </div>
    
    <!--星厨团-->
    <div v-else>
    	<div v-for="(item,index) in starChefList" :key="index"  style="padding:20px 10px;border-bottom: 1px solid #f1f1f1;">
    		<div class="box">
    			<div class="headDiv">
    				<img :src="item.HeadPicUrl" class="headImg">
    			</div>
    			<img src="../../static/credit/starkitchen_xcicon.png" class="iconImg"/>
    			<div>
    				<p style="margin-top: 1%;">{{item.ChefName}}</p>
    				<p style="font-size: 13px;color:#8C8C8C">{{item.Position}} {{item.HotelName}}</p>
    			</div>
    		</div>
    		<img :src="item.DishPicUrl" style="width: 100%;margin-top: 5px;" @click="jump('starkitchendish',item.CaiPinId,item.DishType)"/>
    		<!--<div @click="jump('starkitchendish',item.CaiPinId,item.DishType)" style="height: 175px;" v-bind:style="{background:'url('+item.DishPicUrl+') no-repeat center',backgroundSize:'100% 100%'}"></div>-->
    		<p>
    			<span class="dishname" @click="jump('starkitchendish',item.CaiPinId,item.DishType)">{{item.DishName}}</span>
    			<span class="looknum"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,'star',index)"><span>{{item.LyCount}}人看过</span></span>
    		</p>
    	</div>
    </div>
    
    <!--赛区选择  -->
	  <div v-transfer-dom>
		    <popup v-model="areaFlag" position="top">
	          <div class='zone'>
	              <div class="cityDiv" v-for="(item,index) in areaList" :key="index">
	                <span style="font-size: 13px;">{{item.name}}</span>
	                <checker v-model="areaCity" default-item-class="areacity-item" selected-item-class="areacity-item-selected">
	                  <checker-item v-for="(i,index) in item.list" :key="index" :value="i.AreaName" >{{i.AreaName}}</checker-item>
	                </checker>
	              </div>          
	            	<div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>        
	          </div>
		    </popup>
	  </div>	  
	  
  </div>
</template>

<script>
import { Tab, TabItem, TransferDom, Popup, Checker, CheckerItem, cookie } from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	directives: {
    TransferDom
  },
	components:{
		Tab,
		TabItem,
		Popup,
		Checker,
		CheckerItem,
		cookie
	},
	data(){
		return{
			areaFlag:false,//赛区是否显示
			areaCity:'',
			showFlag:true,//tab切换
			userData:{},
			openId:'',
			searchValue:'',//搜索关键字
			tutorGroupList:[],//导师团
			starChefList:[],//星厨团
			areaList:[
			  {
			  	name:'第一季',
			  	list:[
			  	  {
			  	  	AreaName:'洛阳'
			  	  },{
			  	  	AreaName:'安阳'
			  	  },{
			  	  	AreaName:'郑州'
			  	  },{
			  	  	AreaName:'北京'
			  	  },{
			  	  	AreaName:'上海'
			  	  }
			  	]
			  },
			  {
			  	name:'第二季',
			  	list:[
			  	  {
			  	  	AreaName:'许昌'
			  	  },{
			  	  	AreaName:'南京'
			  	  },{
			  	  	AreaName:'厦门'
			  	  },{
			  	  	AreaName:'三亚'
			  	  },{
			  	  	AreaName:'成都'
			  	  }
			  	]
			  }
			]
			
		}
	},
	methods:{
		searchMethod(){//关键字为空时 搜索全部
			if(this.searchValue == ''){
		    this.getXcxcCaiPinList('导师团');//获取导师列表
		    this.getXcxcCaiPinList('星厨团');//获取星厨列表
			}
		},
		searchClick(){//点击搜索
			if(this.searchValue != ''){
		    this.getXcxcCaiPinList('导师团');//获取导师列表
		    this.getXcxcCaiPinList('星厨团');//获取星厨列表
			}
		},
		menuClick(){//点击菜谱按钮
			this.$router.push('/component/dishcookbook');
		},
		tabClick(val){//tab切换
			if(val == 1){
				this.showFlag = true;
			}else{
				this.showFlag = false;
			}
		},
		areaClick(){
      this.areaFlag ? this.areaFlag = false : this.areaFlag = true;
		},
		cancel(){//取消
			this.areaFlag ? this.areaFlag = false : this.areaFlag = true;
		},
		confirm(){//确定
			console.log(this.areaCity);
			this.areaFlag ? this.areaFlag = false : this.areaFlag = true;
		},
		getXcxcCaiPinList(Type){//获取导师 星厨列表
			var self = this;
			var params = {
		    "page": 1,
		    "pagesize": 1000,
		    "CaiPinName": this.searchValue,
		    "OpenId":this.openId,
		    "Type":Type
			}
			this.$http({
				method:'post',
				url:apiUrl.getXcxcCaiPinList,
				data:params
			}).then(function(res){
//				console.log(JSON.parse(res.data));
//				var dataArr = JSON.parse(res.data).data;
				var dataArr = res.data.data;//测试
				for(let i in dataArr){
					var data = dataArr[i];
					if(data.IsCollect == 0){//未收藏
					  data.collectImg = '../../static/credit/love01.png'
					}else if(data.IsCollect == 1){//已收藏
					  data.collectImg = '../../static/credit/love02.png'
					}
				}
				
				if(Type == '导师团'){//导师团
					self.tutorGroupList = dataArr;
				}else if(Type == '星厨团'){//星厨团
					self.starChefList = dataArr;
				}
			})
		},
		collectClick(dishId,collectImg,arr,index){//点击收藏
			var self = this;
			var ActionFlag = '';
			if(collectImg == '../../static/credit/love01.png'){//未收藏
				ActionFlag = '1';//收藏
			}else if(collectImg == '../../static/credit/love02.png'){//收藏
				ActionFlag = '0';//取消收藏
			}
			
			var params = {
				"ModuleName":"菜品库",
				"OpenId":this.openId,
				"RecordAction":"collect",
				"RecordKeyType":"菜品",
				"RecordKeyId":dishId,//菜品id
				"ActionFlag":ActionFlag//1收藏 0取消收藏
			}
			this.$http({
				method:'post',
				url:apiUrl.addActionRecord,
				data:params
			}).then(function(res){
				if(res.data == 'succ'){
					if(arr == 'tutor'){//导师团
						if(collectImg == '../../static/credit/love01.png'){//未收藏
							self.tutorGroupList[index].collectImg = '../../static/credit/love02.png';
					  }else if(collectImg == '../../static/credit/love02.png'){//收藏
							self.tutorGroupList[index].collectImg = '../../static/credit/love01.png';
					  }
					}else if(arr == 'star'){//星厨团
						if(collectImg == '../../static/credit/love01.png'){//未收藏
							self.starChefList[index].collectImg = '../../static/credit/love02.png';
					  }else if(collectImg == '../../static/credit/love02.png'){//收藏
							self.starChefList[index].collectImg = '../../static/credit/love01.png';
					  }
					}
				}else{
					alert('收藏失败，请稍后再试');
				}
			})
		},
		jump(links,dishId,dishType){//页面跳转
			this.$router.push({path:links,query:{dishId:dishId,dishType:dishType}});
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
	  this.openId=this.userData.Openid;
	  
		this.getXcxcCaiPinList('导师团');//获取导师列表
		this.getXcxcCaiPinList('星厨团');//获取星厨列表
	}
}
</script>

<style scoped>
.searchDiv{width: 70%;height: 40px; background: #f4f4f4;margin-left: 20px 3%; margin: 20px 0 0 3%;border-radius: 5px;display: inline-block;}
.searchDiv *,.looknum *{display: inline-block;vertical-align: middle;}
.searchDiv img{width: 8%;}
.searchDiv input{width: 87%;height: 100%;border: none;background:#f4f4f4;text-indent: 6%;border-radius: 5px;letter-spacing: 1px;outline: none;font-size: 15px;}
.star-tab{border-bottom:2px solid #f8f8f8;width: 100%;height: 44px;background: #fff;}
.box{display: flex;position: relative;}
/*.box div:nth-child(1){width: 16%;}*/
.box div:nth-child(2){width: 80%;}
.looknum{float: right;}
.looknum img{width: 15px;margin-right: 10px;}
.looknum span{font-size: 12px;color: #8C8C8C;}
/*赛区选择*/
.zone{background: #fff;}
.cityDiv{width: 90%;margin-left: 7%;padding: 10px 0;}		
.areacity-item{width:auto;line-height: 26px;color: #3E3E3E;font-size: 12px;margin-left: 15%;margin-top: 10px;}
.areacity-item-selected {color: #E83428; border-bottom:1px solid #E83428;}
.areaBtn button{width: 50%;height: 50px;font-size: 13px;margin-top: 30px;border: none;outline: none;border-top: 1px solid #E83428;box-sizing: border-box;}	
.areaBtn button:nth-child(1){background: #fff;color: #E83428;}
.areaBtn button:nth-child(2){background: #E83428;color: white;}
.headDiv{margin-right: 4%;width: 50px;height: 50px;overflow: hidden;border: 1px solid #f3f3f3;border-radius: 50%;background: #fafafa;}
.headImg{width: 100%;width: 110%;margin-left: -5%;margin-top: 5px;}
.iconImg{width: 15px;height: 15px;position: absolute;top: 35px;left: 35px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.dishstarkitchen .vux-tab .vux-tab-item{color: #3E3E3E;background: none;}
.dishstarkitchen .vux-tab .vux-tab-item.vux-tab-selected{color: #e91e24;}
.dishstarkitchen .vux-tab-bar-inner{background-color:#e91e24;height: 2px;position: relative;top: 1px;}
.dishstarkitchen .vux-tab{width: 50%; margin-left: 25%;}
</style>