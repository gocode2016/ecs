<template>
  <div class="dishshow" style="height: 100%;">
  	
    <!--tab切换-->
    <div class="dish-top">
	    <tab :line-width="1" custom-bar-width="30px">
	      <tab-item selected @click.native="onClick(1)">我的</tab-item>
	      <tab-item @click.native="onClick(2)">收藏</tab-item>
	    </tab>
    </div>
    
    <!--我的-->
    <div class="dish-wrap" v-if="myFlag" v-bind:style="{height:''+centerHeight+'px'}">
    	<div v-for="(item,index) in myDishList" class="dish-box" v-show="mydishFlag">
    		<div class="dishImg" v-bind:style="{height:''+Height+'px'}" @click="dishDetail(item.DishId,item.PeopleType)"><img :src="item.DishPicUrl"></div>
    		<p class="dishname">{{item.DishName}}</p>
    		<p class="dishgood"><img :src="item.collectImg" class="loveImg" @click="collectClick('mydish',item.IsCollect,item.DishId,item.DishType,index)"><span class="lookNum">{{item.VisitCount}}人看过</span></p>
    	</div>
    	<div class="dish-none" v-show="nodishFlag">
    		<img src="../../static/credit/dishnone.png"/>
    		<p>你还没有菜品哦，等待活动开始吧！</p>
    	</div>
	  </div>
	    
    <!--收藏-->
    <div class="dish-wrap" v-else v-bind:style="{height:''+centerHeight+'px'}">
    	<div v-for="(item,index) in myCollectList" class="dish-box" v-show="collectFlag">
    		<div class="dishImg" v-bind:style="{height:''+Height+'px'}" @click="dishDetail(item.DishId,item.PeopleType)"><img :src="item.DishPicUrl"></div>
    		<p class="dishname">{{item.DishName}}</p>
    		<p class="dishgood"><img :src="item.collectImg" class="loveImg" @click="collectClick('collectdish',item.IsCollect,item.DishId,item.DishType,index)"><span class="lookNum">{{item.VisitCount}}人看过</span></p>
    	</div>
    	<div class="dish-none" v-show="nocollectFlag">
    		<img src="../../static/credit/dishnone.png"/>
    		<p>你还没有菜品哦 快去寻找你的菜！</p>
    	</div>
    </div>
    
    <!--按钮-->
    <!--<div class="foots">
    	<button class="dish-btn" @click="jump('/component/dishselectgoods')">上传菜品</button>
    </div>-->
    
    <!--收藏成功图片-->
    <img src="../../static/credit/collect.png" class="colSuc" v-show="colShow"/>
   
  </div>
</template>

<script>
import {  Tab, TabItem,cookie,Toast } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Tab, 
		TabItem,
		cookie,
		Toast
	},
	data(){
		return{
			Height:'',//图片高度
			centerHeight:'',
			myFlag:true,
			mydishFlag:'',
			nodishFlag:'',
			collectFlag:'',
			nocollectFlag:'',
			colShow:false,//收藏成功
			userData:{},
			openId:'',
			userId:'',
			userType:'',
			myDishList:[],//我的菜品
			myCollectList:[]//收藏菜品
		}
	},
	methods:{
		getMyDish(){//获取我的菜品
			var _this=this;
			var params={
  		  "OpenId":this.openId,
  		  "PageIndex":0,
  		  "MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMyDish,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
//			  console.log(data);
  			_this.myDishList=data.Info;
				if(data.totalCount==0){//没有菜品
					_this.mydishFlag=false;
					_this.nodishFlag=true;
				}else{
					_this.mydishFlag=true;
					_this.nodishFlag=false;
				}
				for(var i in _this.myDishList){
					var item=_this.myDishList[i];
					item.IsCollect=='f' ? item.collectImg='../../static/credit/love01.png' : item.collectImg='../../static/credit/love02.png';
				}
  		})
		},
		getMyCollect(){//获取收藏菜品
			var _this=this;
			var params={
  		  "OpenId":this.openId,
  		  "PageIndex":0,
  		  "MemberId":this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMyCollect,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
//			  console.log(data);
  			_this.myCollectList=data.Info;
				if(data.totalCount==0){//没有菜品
					_this.collectFlag=false;
			    _this.nocollectFlag=true;
				}else{
					_this.collectFlag=true;
			    _this.nocollectFlag=false;
				}
				for(var i in _this.myCollectList){
					var item=_this.myCollectList[i];
					item.IsCollect=='f' ? item.collectImg='../../static/credit/love01.png' : item.collectImg='../../static/credit/love02.png';
				}
  		})
		},
		collectClick(val,IsCollect,dishId,dishType,index){//点击收藏  全部菜品列表
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
						 	 if(val=='mydish'){
							 	 	_this.myDishList[index].collectImg='../../static/credit/love02.png';
							 	  _this.myDishList[index].IsCollect='t';				 	 						 	 
							 	  _this.myDishList.splice(index,1,_this.myDishList[index]);
						 	 }else if(val=='collectdish'){
							 	 	_this.myCollectList[index].collectImg='../../static/credit/love02.png';
							 	  _this.myCollectList[index].IsCollect='t';				 	 						 	 
							 	  _this.myCollectList.splice(index,1,_this.myCollectList[index]);
						 	 }
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
						 	 if(val=='mydish'){
							 	 _this.myDishList[index].collectImg='../../static/credit/love01.png';
							 	 _this.myDishList[index].IsCollect='f';						 
							 	 _this.myDishList.splice(index,1,_this.myDishList[index]);
						 	 }else if(val=='collectdish'){
						 	 	 _this.myCollectList[index].collectImg='../../static/credit/love01.png';
							 	 _this.myCollectList[index].IsCollect='f';						 
							 	 _this.myCollectList.splice(index,1,_this.myCollectList[index]);
						 	 }
				     } 				
				})
			}			
		},
		dishDetail(dishId,peopleType){//查看菜品详情
			if(peopleType==1){//跳转到导师菜品详情
				this.$router.push({path:'/component/tutordish',query:{dishId:dishId}});
			}else if(peopleType==3){//跳转到星厨菜品详情
				this.$router.push({path:'/component/starkitchendish',query:{dishId:dishId}});
			}else if(peopleType==4){
				this.$router.push({path:'/component/dishdetails',query:{dishId:dishId}});
			}
		},
		jump(link){//点击上传菜品
			var myDate = new Date();
			var localedate=myDate.toLocaleDateString(); //获取当前日期
			if(localedate!='2018/4/14'&&localedate!='2018/4/15'){
		  	this.$vux.toast.text('活动已结束', 'middle');
			}else if(this.userType==0){//未注册
				this.$router.push('/component/goregister');
			}else if(this.userType==1){//队员
				this.$vux.toast.text('队员不能参与哦', 'middle');
			}else if(this.userType==2){//会员
				this.$router.push(link);
			}
		},
		onChange (val) {
      const _this = this
      if (val) {
        this.$vux.toast.show({
          text: 'Hello World',
          onShow () {
            console.log('Plugin: I\'m showing')
          },
          onHide () {
            console.log('Plugin: I\'m hiding')
            _this.show9 = false
          }
        })
      } else {
        this.$vux.toast.hide()
      }
	  },
		onClick(val){//tab切换
			if(val==1){
				this.myFlag=true;
			}else{
				this.myFlag=false;
			}
		}
		
	},
	mounted(){
		
		$('.dish-box:odd').css('margin-top','20px');
		$('.dish-box').eq(1).css('margin-top','0px');
		
		this.Height=$(window).width()*0.45;//图片高度
//		this.centerHeight=($(window).height()-106)*0.94;//中间图片部分高度
		this.centerHeight=$(window).height()-66;//中间图片部分高度
		
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
	  this.userType=parseInt(this.userData.UserType);
    this.openId=this.userData.Openid;
    
		this.getMyDish();//获取我的菜品
		this.getMyCollect();//获取收藏菜品
	}
}
</script>

<style scoped>
.dish-top{border-bottom:2px solid #f8f8f8;width: 100%;height: 44px;background: #fff;}
.dish-wrap{width: 100%;display: flex;flex-wrap: wrap;margin-top: 20px;overflow:auto;}
.dish-box{width: 45%;margin-left: 3.3%;}
.dishImg{width: 100%;}
.dish-box div img{width: 100%;height: 100%;}
.dishname{font-size: 12px;color:#000;}
.dishgood *{display: inline-block;vertical-align: middle;}
.dishgood{margin-bottom: 10px;}
.dishgood img{width: 13px;}
.dishgood span{font-size: 9px;color: #8b8b8b;margin-left: 8%;}
.dish-none{width:100%;text-align: center;font-size: 12px;color: #868686;letter-spacing: 1px;margin-top: 15%;}
.dish-none img{width: 50%;}
.dish-none p{margin-top: 20px;}
.dish-btn{width: 90%;height:38px;background:url('../../static/credit/dishbtn.png');border: none;outline:none;margin-left: 5%;color: #fff;
          border-radius: 5px;}
.dishshow .foots{width:100%;background: #fff;height: 62px;}
.colSuc{width: 50%;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.dishshow .vux-tab .vux-tab-item{color: #3E3E3E;background: none;}
.dishshow .vux-tab .vux-tab-item.vux-tab-selected{color: #e91e24;}
.dishshow .vux-tab-bar-inner{background-color:#e91e24;height: 1.5px;position: relative;top: 1px;}
.dishshow .vux-tab{width: 50%; margin-left: 25%;}
</style>