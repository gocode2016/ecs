<template>
  <div class="competitiondishlist">
  	<!--头部-->
    <div style="height:44px;">
      <sticky scroll-box="vux_view_box_body" :offset="0" :check-sticky-support="false">
        <tab :line-width="1" >
          <span @click='alldish'><tab-item selected >全部菜品</tab-item></span>
          <span @click='votedish'><tab-item >投票菜品</tab-item></span>
        </tab>
      </sticky>
    </div>
    
    <!--全部菜品-->
    <div class="alldish" v-if="flag">
    	<div class="alldishtext">
    	  <div @click="chosen">最新入选<img v-bind:src="img1" class="topImg"><img v-bind:src="img2" class="botImg"></div>
    	  <div @click="goodsale">人气抢手<img v-bind:src="img3" class="topImg"><img v-bind:src="img4" class="botImg"></div>
    	  <div>{{areaText}}<img v-bind:src='iconImg' class="areaphoto" @click="areaClick"></div>
    	</div>
    	<!--图片展示  -->
    	<div class="alldishphoto">
    		<div class="photoDiv" v-for='(item,index) in dishList' :key="index">
    			<img :src="item.DishPicUrl" class="dishImg" @click="jump(item.DishId,item.DishType,item.ChefId,item.IsSelected)"/>
    			<p class="dishName">{{item.DishName}}</p>
    			<p class="tutorName"><span>{{item.MemberName}}</span><span class="hotelName">{{item.HotelName}}</span></p>
    			<p class="look"><img v-bind:src='item.collectImg' class="love" @click="collectClick(item.IsCollect,item.DishId,item.DishType,index)"><span>{{item.VisitCount}}人看过</span></p>
    		</div>
    	</div>
    	<!--赛区选择  -->
	    <div v-transfer-dom>
		    <popup v-model="show" position="top">
		      <div class="popup0">
		        <div class="mask">
		          <div class='zone'>
		            <input class="search" placeholder="输入所选择的赛区"/>
		            <div class="searchDiv">         
		              <div class="cityDiv" v-for="(item,index) in areaList" :key="index">
		                <span>{{item.ProvinceName}}</span>
		                <checker v-model="areaCity" default-item-class="areacity-item" selected-item-class="areacity-item-selected">
		                  <checker-item v-for="(i,index) in item.AreaName" :key="index" :value="i.AreaName"  style="width: auto;"  @click.native="MatchRegion(i.MatchRegionId)">{{i.AreaName}}</checker-item>
		                </checker>
		              </div>          
		            </div>
		            <div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>        
		          </div>
		        </div> 
		      </div>
		    </popup>
	    </div>	    
    </div>
    
     <!--投票菜品-->
    <div class="votedish" v-else>
    	<div class="votedishtext">   	 
    	  <div @click="votenum">投票数<img v-bind:src="img5" class="topImg"><img v-bind:src="img6" class="botImg"></div>
    	  <div>{{areaVoteText}}<img v-bind:src='iconImg' class="areaphoto" @click="areaClick"></div>
    	</div>
    	<!--图片展示 -->  
    	<div class="votedishphoto">
    		<div class="photoDiv" v-for='(item,index) in dishVoteList' :key="index">
    			<img  :src="item.DishPicUrl" class="dishImg" @click="jump(item.DishId,item.DishType,item.ChefId,1)"/>
    			<p class="dishName">{{item.DishName}}<span class="voteNum">{{item.SelectCount}}&nbsp;票</span></p>
    			<p class="tutorName"><span>{{item.MemberName}}</span><span class="hotelName">{{item.HotelName}}</span></p>
    			<p class="look"><img v-bind:src='item.collectImg' class="love"  @click="collectVoteClick(item.IsCollect,item.DishId,item.DishType,index)"><span>{{item.VisitCount}}人看过</span></p>
    		</div>
    	</div>
    	<!--赛区选择 -->
	    <div v-transfer-dom>
		    <popup v-model="show" position="top">
		      <div class="popup0">
		        <div class="mask">
		          <div class='zone'>
		            <input class="search" placeholder="输入所选择的赛区"/>
		            <div class="searchDiv">         
		              <div class="cityDiv" v-for="(item,index) in areaList" :key="index">
		                <span>{{item.ProvinceName}}</span>
		                <checker v-model="areaVoteCity" default-item-class="areacity-item" selected-item-class="areacity-item-selected">
		                  <checker-item v-for="(i,index) in item.AreaName" :key="index" :value="i.AreaName"  style="width: auto;"  @click.native="voteMatchRegion(i.MatchRegionId)">{{i.AreaName}}</checker-item>
		                </checker>
		              </div>          
		            </div>
		            <div class="areaBtn"><button @click="cancel">取消</button><button @click="confirm">确定</button></div>        
		          </div>
		        </div> 
		      </div>
		    </popup>
	    </div>    	
    </div>
    
    <!--收藏成功图片-->
    <img src="../../static/credit/collect.png" class="colSuc" v-show="colShow"/>
    
  </div>
</template>

<script>
import {Tab, TabItem, Sticky,Checker, CheckerItem ,TransferDom,Popup,cookie} from 'vux'
import apiUrl from '../apiUrls.js'

var allMatchId=382;//记录 全部菜品 赛区id
var voteMatchId=382;//记录 投票菜品 赛区id
var clickFlag=1;//记录 全部菜品列表 排序
var clickVoteFlag=1;//记录 投票菜品列表 排序
var chosen=1;//最新入选 1正序 2倒序
var goodsale=0;//人气抢手 1正序 2倒序
var votenum=1;//投票数 1正序 2倒序
export default {
	directives: {
    TransferDom
  },
  components: {
    Tab, 
    TabItem, 
    Sticky,
    Checker, 
    CheckerItem,
    Popup ,
    cookie
  },
  data () {
    return{
    	openId:'',
    	userData:{},
    	flag:true,
      show:false,
    	areaText:'赛区',//全部菜品
    	areaCity:'',//全部菜品 赛区
    	areaVoteText:'赛区',//投票菜品
    	areaVoteCity:'',//投票菜品 赛区
    	dishList:[],//全部菜品列表
    	dishVoteList:[],//投票菜品列表
    	areaList:[],//赛区列表
    	colShow:false,//收藏成功
    	iconImg:'../../static/credit/areaicon01.png',
    	MatchRegionId:-1,//全部菜品 赛区id
    	voteMatchRegionId:382,//投票菜品 赛区id
    	img1:'',
    	img2:'',
      img3:'',
    	img4:'',
    	img5:'',
    	img6:'',
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
  	this.timef();//获取全部菜品列表
  	this.votef();//获取投票菜品列表;
  	this.getArealist();//获取活动赛区
  },
  methods:{
  	getDish(orderBy,isDesc){//全部菜品列表 time count f升序t降序 
  		var _this=this; 		
	    var params='?chefActivityId='+1+'&matchRegionId='+this.MatchRegionId+'&openId='+this.openId+'&orderBy='+orderBy+'&isDesc='+isDesc+'&pageindex='+1;  		
  		this.$http({
  			method:'GET',
  			url:apiUrl.getDishs+params
  		}).then(function(response){
			    console.log(JSON.parse(response.data));
	  			_this.dishList=JSON.parse(response.data).data;
					for(var i=0;i<_this.dishList.length;i++){
						var item=_this.dishList[i];
						item.IsCollect=='f' ? item.collectImg='../../static/credit/love01.png' : item.collectImg='../../static/credit/love02.png';			
					}
	        console.log($('.allDiv'));
	        $('.photoDiv:odd').css("margin-top","40px");
	        $('.photoDiv').eq(1).css("margin-top","0px");
	        console.log('*');
      })
  	},
  	getVoteDish(isDesc){//投票菜品列表 time count f升序t降序
  		var _this=this;  		 		 		
			var params='?chefActivityId='+1+'&matchRegionId='+this.voteMatchRegionId+'&openId='+this.openId+'&isDesc='+isDesc+'&pageindex='+1; 	
  		this.$http({
  			method:'GET',
  			url:apiUrl.getVoteDishs+params
  		}).then(function(response){
//			    console.log(JSON.parse(response.data));
	  			_this.dishVoteList=JSON.parse(response.data).data;
					for(var i=0;i<_this.dishVoteList.length;i++){
						var item=_this.dishVoteList[i];
						item.IsCollect=='f' ? item.collectImg='../../static/credit/love01.png' : item.collectImg='../../static/credit/love02.png';				
					}
      })
  	},
  	chosen(){//点击最新入选
  		if(chosen==1){//当前正序
  			this.timet();
  			chosen=2;
  			goodsale=0;
  		}else if(chosen==2||chosen==0){
  			this.timef();
  			chosen=1;
  			goodsale=0;
  		}
  	},
  	goodsale(){//人气抢手
  		if(goodsale==1){//当前正序
  			this.countt();
  			goodsale=2;
  			chosen=0;
  		}else if(goodsale==2||goodsale==0){
  			this.countf();
  			goodsale=1;
  			chosen=0;
  		}
  	},
  	votenum(){//投票数
  		if(votenum==1){//当前正序
  			this.votet();
  			votenum=2;
  		}else if(votenum==2){
  			this.votef();
  			votenum=1;
  		}
  	},
  	changeImg(){
  		this.img1='../../static/credit/topc.png';
    	this.img2='../../static/credit/botc.png';
      this.img3='../../static/credit/topc.png';
    	this.img4='../../static/credit/botc.png';
  	},
  	timef(){//最新入选 升序
  		this.changeImg();
    	this.img1='../../static/credit/topr.png';
    	clickFlag=1;
    	this.getDish('time','f');//获取菜品列表
  	},
  	timet(){//最新入选 降序
  		this.changeImg();
    	this.img2='../../static/credit/botr.png';
    	clickFlag=2;
    	this.getDish('time','t');//获取菜品列表
  	},
  	countf(){//人气抢手 升序
  		this.changeImg();
    	this.img3='../../static/credit/topr.png';
    	clickFlag=3;
    	this.getDish('count','f');//获取菜品列表
  	},
  	countt(){//人气抢手 降序
  		this.changeImg();
    	this.img4='../../static/credit/botr.png';
    	clickFlag=4;
    	this.getDish('count','t');//获取菜品列表
  	},
  	votef(){//投票数升序   
      this.img5='../../static/credit/topr.png';
    	this.img6='../../static/credit/botc.png';	
    	clickVoteFlag=1;
    	this.getVoteDish('f');//获取菜品列表
  	},
  	votet(){//投票数降序
      this.img5='../../static/credit/topc.png';
    	this.img6='../../static/credit/botr.png';
    	clickVoteFlag=2;
    	this.getVoteDish('t');//获取菜品列表
  	},
  	getArealist(){//获取活动赛区
  		var _this=this;
  		var params='?CAId='+1;
  		this.$http({
  			method:'GET',
  			url:apiUrl.getDishArea+params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			_this.areaList=JSON.parse(response.data);
  		})
  	},
		collectClick(IsCollect,dishId,dishType,index){//点击收藏  全部菜品列表
			var _this=this;
			var params={
				"DishId":dishId,
				"OpenId":this.openId,
				"DishType":dishType
			}
			if(IsCollect=="f"){//未收藏				
				this.$http({ 	//收藏
					method:'POST',
					url:apiUrl.addCollect,
					data:params
				}).then(function(response){ 	
						 if(response.data=="OK"){					 	 	
						 	 _this.dishList[index].collectImg='../../static/credit/love02.png';
						 	 _this.dishList[index].IsCollect='t';				 	 						 	 
						 	 _this.dishList.splice(index,1,_this.dishList[index]);
				     //收藏成功样式
				       _this.colShow=true;
					     setTimeout(function(){
					     	_this.colShow=false;
					     },1000)               
				     } 				
				})
			}else{//已收藏				
				this.$http({ 	//取消收藏
					method:'POST',
					url:apiUrl.delCollect,
					data:params
				}).then(function(response){ 	
						 if(response.data=="OK"){					 	
						 	 _this.dishList[index].collectImg='../../static/credit/love01.png';
						 	 _this.dishList[index].IsCollect='f';						 
						 	 _this.dishList.splice(index,1,_this.dishList[index]);
				     } 				
				})
			}			
		},
		collectVoteClick(IsCollect,dishId,dishType,index){//点击收藏  投票菜品列表
			var _this=this;
			var params={
				"DishId":dishId,
				"OpenId":this.openId,
				"DishType":dishType
			}
			if(IsCollect=="f"){//未收藏				
				this.$http({ 	//收藏
					method:'POST',
					url:apiUrl.addCollect,
					data:params
				}).then(function(response){ 	
						 if(response.data=="OK"){					 	 	
						 	 _this.dishVoteList[index].collectImg='../../static/credit/love02.png';
						 	 _this.dishVoteList[index].IsCollect='t';				 	 						 	 
						 	 _this.dishVoteList.splice(index,1,_this.dishVoteList[index]);
				     //收藏成功样式
				       _this.colShow=true;
					     setTimeout(function(){
					     	_this.colShow=false;
					     },1000)               
				     } 				
				})
			}else{//已收藏				
				this.$http({ 	//取消收藏
					method:'POST',
					url:apiUrl.delCollect,
					data:params
				}).then(function(response){ 	
						 if(response.data=="OK"){					 	
						 	 _this.dishVoteList[index].collectImg='../../static/credit/love01.png';
						 	 _this.dishVoteList[index].IsCollect='f';						 
						 	 _this.dishVoteList.splice(index,1,_this.dishVoteList[index]);
				     } 				
				})
			}			
		},
  	votedish(){//点击投票菜品
  		this.flag=false;	 
  	},
  	alldish(){//点击全部菜品
  		this.flag=true;
  	},
  	jump(dishId,dishType,chefId,isVoteDish){//跳转到菜品详情
  		this.$router.push({path:'/component/competitiondish',query:{dishId:dishId,dishType:dishType,chefId:chefId,isVoteDish:isVoteDish}})
  	},
  	areaClick(){
      this.show ? this.show = false : this.show = true;
  	},
		MatchRegion(MatchRegionId){//全部菜品 赛区id
			allMatchId=MatchRegionId;
		},
		voteMatchRegion(MatchRegionId){//投票菜品 赛区id
			voteMatchId=MatchRegionId;
		},
  	cancel(){
    	this.show ? this.show = false : this.show = true;
    },
    confirm(){
    	this.show ? this.show = false : this.show = true; 
    	if(this.flag==true){
    		this.areaCity=="" ? this.areaText = '赛区' : this.areaText = this.areaCity;
    		this.MatchRegionId=allMatchId;   		
    		if(clickFlag==1){
    			this.timef();
    		}else if(clickFlag==2){
    			this.timet();
    		}else if(clickFlag==3){
    			this.countf();
    		}else{
    			this.countt();
    		}
    	}else{
    		this.areaVoteCity=="" ? this.areaVoteText = '赛区' : this.areaVoteText = this.areaVoteCity;
    		this.voteMatchRegionId=voteMatchId;    		
    		if(clickVoteFlag==1){
    			this.votef();
    		}else{
    			this.votet();
    		}
    	}    	
    }  	
  }
}
</script>
<style scoped>
	.competitiondishlist{background-color: #FFFFFF;} 	 
	.alldishtext{height: 60px;line-height: 60px;display:flex;font-size:15px;color:#5d5d5d;justify-content: space-around;text-align: center;}	
	.votedishtext{height: 60px;line-height: 60px;display:flex;font-size:15px;color:#5d5d5d;	text-align: center;justify-content: space-around;padding-left: 23.3%;padding-right: 23.3%;} 	 
	.topImg,.botImg{width:9px;}	 	 	 
	.topImg{position: relative;top: -6px;left: 5px;}
	.botImg{ position: relative;left: -4px;top: 1px;} 
	.areaphoto{position: relative;top: 3px;width: 18px;	margin-left: 8px;}
  .alldishphoto,.votedishphoto{display: flex;flex-wrap: wrap;}
  .photoDiv{width: 45%;margin-bottom: 10px;margin-left: 3.3%;} 	
  .dishImg{width: 100%;height: 150px;}  	
  .voteNum {float: right;font-size: 12px;color: #E83428;line-height: 25.6px;}
  .love{width: 18px;margin-right: 16px;}
  .look *{display: inline-block;vertical-align: middle;} 
  .look,.tutorName{font-size: 12px;color: #8f8f8f;}
  .hotelName{margin-left: 10px;}
	.alert-mask{ width: 100%; height: 100%; background: rgba(0,0,0,0.5); position: fixed; top: 46px; }
	.zone{width: 100%;height: 410px;background: #FFFFFF;}
	.search{width: 270px;height:30px;background: #f9f9f9 url('../../static/credit/search.png') no-repeat 230px center;border: none;border-radius: 5px;margin-left: 10%;margin-top: 20px;text-indent: 1em; background-size: 20px; outline: none;}        
	.searchDiv{width: 100%;height: 300px;overflow-y:auto;border-top: 1px solid #c8c8c8;margin-top: 10px;}	
	.cityDiv{width: 80%;margin-left: 10%;margin-top: 20px;}		
	.areacity-item{ width: 50px;height: 26px;line-height: 26px;text-align: center;color: #3E3E3E;font-size: 13px;margin-left:8%;}
	.areacity-item-selected {color: #E83428; border-bottom:1px solid #E83428;}
	.areaBtn button{width: 50%;height: 50px;font-size: 13px;}	
	.areaBtn button:nth-child(1){background: #fff;border: none;color: #E83428;border-top: 1px solid #E83428;box-sizing: border-box;}
	.areaBtn button:nth-child(2){background: #E83428;color: white;border: none;border-top: 1px solid #E83428;box-sizing: border-box;}
	.colSuc{width: 50%;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.competitiondishlist .vux-tab-ink-bar{background-color: #e81d23 !important;position: absolute; top: 44px; height: 2.5px !important;} 
  .competitiondishlist .vux-tab-selected{color: #e81d23 !important;}
  .competitiondishlist .vux-sticky-box{border-bottom: 2.5px solid #f8f8f8 !important;background: #FFFFFF;} 
  .competitiondishlist .vux-tab{width: 200px;margin-left: 23.3%;color: #3e3e3e !important;text-align: center;}  
  .competitiondishlist .vux-tab span{display: inline-block;width:50%;text-align: center;}	 
  .competitiondishlist .vux-tab-item{font-size: 16px !important;background: none !important;}  	 
</style>
