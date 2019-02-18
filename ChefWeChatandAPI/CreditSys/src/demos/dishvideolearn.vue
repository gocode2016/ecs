<template>
  <div class="dishvideolearn">
    
    <!--搜索-->
    <div class="searchDiv">
    	<input type="text" placeholder="搜索菜名\食材" v-model='searchValue' @input="searchMethod"/>
    	<img src="../../static/credit/search.png"  @click="searchClick"/>
    </div>
    <div style="display: inline-block;width:18%;position: relative; right: -6%;">
    	<span style="color: #3E3E3E;font-size: 13px;">分类</span>
    	<img src="../../static/credit/dishstore_menu.png" @click="menuClick" style="width: 36%;vertical-align: middle; "/>
    </div>
    
    <!--tab切换-->
    <div class="star-tab">
	    <tab :line-width="1" custom-bar-width="50px">
	      	<tab-item selected @click.native="tabClick(1)">更新时间</tab-item>
	      	<tab-item @click.native="tabClick(2)">播放量</tab-item>
	    </tab>
    </div>
    
    <!--更新时间-->
    <div v-if="showFlag">
    	<div v-for="(item,index) in dishUpdateList" :key="index" style="padding: 20px 10px 0 10px ;">
    		<img v-if="item.VideoUrl==''||item.VideoUrl==null" :src="item.Images" style="width: 100%;" />
				<iframe v-else frameborder='0' allowfullscreen='true' :src='item.VideoUrl' width="100%" height="176px"></iframe>
				<!--<div style="width: 94%;margin: 0 3%;">-->
      		<!--<video :src='item.VideoUrl' width="100%" controls="controls">您的浏览器不支持 video 标签。</video>-->
      	<!--</div>-->
    		<p>
    		  <span class="dishname" @click="jump(item.CaiPinId)">{{item.CaiPinName}}</span>
    		  <span class="looknum"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,index,'update')"><span>{{item.LlCount}}人看过</span></span>
    		</p>
    	</div>
    </div>
    
    <!--播放量-->
    <div v-else>
    	<div v-for="(item,index) in dishPopList" :key="index" style="padding: 20px 10px 0 10px ;">
    		<img v-if="item.VideoUrl==''||item.VideoUrl==null" :src="item.Images" style="width: 100%;" />
				<iframe v-else frameborder='0' allowfullscreen='true' :src='item.VideoUrl' width="100%" height="176px"></iframe>
      	<!--<video :src='item.VideoUrl' width="100%" controls="controls">您的浏览器不支持 video 标签。</video>-->
    		<p>
    		  <span class="dishname" @click="jump(item.CaiPinId)">{{item.CaiPinName}}</span>
    		  <span class="looknum"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,index,'pop')"><span>{{item.LlCount}}人看过</span></span>
    		</p>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { Tab, TabItem, cookie } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Tab, 
		TabItem,
		cookie
	},
	data(){
		return{
			showFlag:true,
			userData:{},
			openId:'',
			searchValue:'',//搜索关键字
			dishUpdateList:[],//更新时间
			dishPopList:[]//人气
		}
	},
	methods:{
		searchMethod(){//关键字为空时 搜索全部
			if(this.searchValue == ''){
		    this.getCaiPinList('UpdateDate','视频学菜');//获取菜品列表 更新时间
		    this.getCaiPinList('RenQi','视频学菜');//获取菜品列表 人气
			}
		},
		searchClick(){//点击搜索
			if(this.searchValue != ''){
		    this.getCaiPinList('UpdateDate','');//获取菜品列表 更新时间
		    this.getCaiPinList('RenQi','');//获取菜品列表 人气
			}
		},
		menuClick(){//点击菜谱按钮
			this.$router.push('/component/dishcookbook');
		},
		tabClick(val){
			if(val == 1){
				this.showFlag = true;
			}else{
				this.showFlag = false;
			}
		},
		getCaiPinList(OrderBy,BanKuaiName){//获取菜品列表
			var self = this;
			var params = {
		    "page": 1,
		    "pagesize": 1000,
		    "OrderBy": OrderBy,
		    "SecondId": '',
		    "BanKuaiName": BanKuaiName,
		    "CaiPinName": this.searchValue,
		    "OpenId":this.openId
			}
			this.$http({
				method:'post',
				url:apiUrl.getCaiPinList,
				data:params
			}).then(function(res){
//				console.log(JSON.parse(res.data));
//				var dataArr = JSON.parse(res.data).data;
				var dataArr = res.data.data;//测试
				for(let i in dataArr){
					var data = dataArr[i];
//					data.Images = data.Images.split(',')[0];
					if(data.IsCollect == 0){//未收藏
						data.collectImg = '../../static/credit/love01.png';
					}else if(data.IsCollect == 1){//收藏
						data.collectImg = '../../static/credit/love02.png';
					}
				}
				
				if(OrderBy == 'UpdateDate'){//更新时间
					self.dishUpdateList = dataArr;
					
//					var item = {
//						VideoUrl:'https://s3.cn-north-1.amazonaws.com.cn/s3-010-shinho-ecshool-prd-bjs/mov/DJI_0050.MP4'
//					}
//					self.dishUpdateList.unshift(item);
					
				}else if(OrderBy == 'RenQi'){//人气
					self.dishPopList = dataArr;
				}
			})
		},
		collectClick(dishId,collectImg,index,txt){//点击收藏
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
						if(txt == 'update'){//更新时间
							if(collectImg == '../../static/credit/love01.png'){//未收藏
								self.dishUpdateList[index].collectImg = '../../static/credit/love02.png';
						  }else if(collectImg == '../../static/credit/love02.png'){//收藏
								self.dishUpdateList[index].collectImg = '../../static/credit/love01.png';
						  }
						}else if(txt == 'pop'){//人气
							if(collectImg == '../../static/credit/love01.png'){//未收藏
								self.dishPopList[index].collectImg = '../../static/credit/love02.png';
						  }else if(collectImg == '../../static/credit/love02.png'){//收藏
								self.dishPopList[index].collectImg = '../../static/credit/love01.png';
						  }
						}
					}else{
						alert('收藏失败，请稍后再试');
					}
				})
		},
		jump(dishId){
			this.$router.push({path:'/component/dishstoredetail',query:{dishId:dishId}});
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
	  
		this.getCaiPinList('UpdateDate','视频学菜');//获取菜品列表 更新时间
		this.getCaiPinList('RenQi','视频学菜');//获取菜品列表 人气
	}
}
</script>

<style scoped>
.searchDiv{width: 70%;height: 40px; background: #f4f4f4;margin-left: 20px 3%; margin: 20px 0 0 3%;border-radius: 5px;display: inline-block;}
.searchDiv *{display: inline-block;vertical-align: middle;}
.searchDiv img{width: 8%;}
.searchDiv input{width: 87%;height: 100%;border: none;background:#f4f4f4;text-indent: 6%;border-radius: 5px;letter-spacing: 1px;outline: none;font-size: 15px;}
.star-tab{border-bottom:2px solid #f8f8f8;width: 100%;height: 44px;background: #fff;}
.dishname{font-size: 14px;}
.dishname:after{content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1px 1px 0 0;border-color: #000;border-style: solid;
    position: relative;top:0px;right: -1%;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
.looknum *{display: inline-block;vertical-align: middle;}
.looknum{float: right;}
.looknum img{width: 15px;margin-right: 10px;}
.looknum span{font-size: 12px;color: #8C8C8C;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.dishvideolearn .vux-tab .vux-tab-item{color: #3E3E3E;background: none;}
.dishvideolearn .vux-tab .vux-tab-item.vux-tab-selected{color: #e91e24;}
.dishvideolearn .vux-tab-bar-inner{background-color:#e91e24;height: 2px;position: relative;top: 1px;}
.dishvideolearn .vux-tab{width: 50%; margin-left: 25%;}
</style>