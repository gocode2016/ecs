<template>
  <div class="productstorelist" style="height: 100%;">
  	
  	<div class="page">
	    <!-- 筛选部分 -->
	    <div class="wrap" v-show="isList==0">
		    <div class="productstorelisticon" :class="{productstorelisticonbgc: num==0}" @click="allClick">全部</div>
				<div class="productstorelisticonbox">
					<div v-for="(item,index) in iconBtnList" :key="index">
				    <div class="productstorelisticon" :label="item.ProductSecondName" :class="{productstorelisticonbgc:index+1 == num}" @click='screenClick(item.ProductSecondId,index)'>{{item.ProductSecondName}}</div>
				  </div> 
				</div>
			</div>
		
	    <div class="read_top">
	    	<div @click="popularClick">人气值<img :src="img1" class="topImg"><img :src="img2" class="botImg"></div>
	    	<div @click="updateClick">更新时间<img :src="img3" class="topImg"><img :src="img4" class="botImg"></div>
	    </div>
	    <!--列表内容-->
	    <div class="content">
		    <div class="shoplist">
		    	<div class="shop_box" v-for="(item,index) in productList" :key="index" >
		    		<div class="shopImgDiv" @click="jump(item.ProductId,item.SpecificationId,item.ProductFirstId)"><img :src="item.SpecificationPicUrl"></div>
		    		<p @click="jump(item.ProductId,item.SpecificationId,item.ProductFirstId)">{{item.Amount}}{{item.Unit}}{{item.ProductName}}</p>
		    	  <p class="good">
					     	<img v-bind:src="item.praiseImg" @click="praiseClick(index,item.praiseImg,item.SpecificationId)"/>
					     	<span>{{item.VisitCount}}人看过</span>
					  </p>
		    	</div>
		    </div>
	    </div>
    </div>
    
  </div>
</template>

<script>
import { cookie,Badge } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
var popularNum=1;
var updateNum=0;
export default{
  components:{
		cookie,
		Badge
  },
  data(){
	  return{
	  	timestamp:'',
	    nonceStr:'',
	    signature:'',
	    url:'',
	    shareicon:'',
	    num: 0,
      img:'../../static/credit/zanw.png',
			img1:'',
			img2:'',
			img3:'',
			img4:'',
			userData:{},
			userId:'',
			userType:'',
			openId:'',
      imgHeight:'',
      isList:'',//0 点击首页按钮 显示顶部分类 ，1 点击更多 不显示分类
      getId:'',
      ProductSecondId:0,//一级分类规格id  0代表全部
      productList:[],
      iconBtnList: []
	  }
  },
  methods:{
  	allClick(){//点击全部
  		this.num=0;
  		this.ProductSecondId=0;
  		//获取商品列表信息
  		this.imgList();
			this.img1='../../static/credit/topr.png';
			popularNum=1;
			updateNum=0;
  		this.getProductFirstList("count","f");//获取一级分类规格列表
  	},
  	screenClick(ProductSecondId,index){//顶部筛选
      var _this=this;
		  this.num = index+1;
		  this.ProductSecondId=ProductSecondId;
     //获取商品列表信息
      this.imgList();
			this.img1='../../static/credit/topr.png';
			popularNum=1;
			updateNum=0;
  		this.getProductFirstList("count","f");//获取一级分类规格列表
		},      
  	popularClick(){//点击人气值
			$(".content").animate({scrollTop:0},300);
			if(popularNum==1){
				this.imgList();
				this.img2='../../static/credit/botr.png';
				popularNum=2;
				updateNum=0;
				//获取商品列表信息
  	    if(this.isList==0){//有顶部分类
		  		this.getProductFirstList("count","t");//获取一级分类规格列表
		  	}else if(this.isList==1){//没有顶部分类
  	      this.getSpecificationByVCIdList("count","t");//获取产品库列表
		  	}
			}else if(popularNum==2||popularNum==0){
				this.imgList();
				this.img1='../../static/credit/topr.png';
				popularNum=1;
				updateNum=0;
				//获取商品列表信息
  	    if(this.isList==0){//有顶部分类
		  		this.getProductFirstList("count","f");//获取一级分类规格列表
		  	}else if(this.isList==1){//没有顶部分类
  	      this.getSpecificationByVCIdList("count","f");//获取产品库列表
		  	}
			}
  	},
  	updateClick(){//点击更新时间
  		$(".content").animate({scrollTop:0},300);
  		if(updateNum==1){
				this.imgList();
				this.img4='../../static/credit/botr.png';
				updateNum=2;
				popularNum=0;
				//获取商品列表信息
  	    if(this.isList==0){//有顶部分类
		  		this.getProductFirstList("time","t");//获取一级分类规格列表
		  	}else if(this.isList==1){//没有顶部分类
		  		this.getSpecificationByVCIdList("time","t");//获取产品库列表
		  	}
			}else if(updateNum==2||updateNum==0){
				this.imgList();
				this.img3='../../static/credit/topr.png';
				updateNum=1;
				popularNum=0;
				//获取商品列表信息
  	    if(this.isList==0){//有顶部分类
		  		this.getProductFirstList("time","f");//获取一级分类规格列表
		  	}else if(this.isList==1){//没有顶部分类
		  		this.getSpecificationByVCIdList("time","f");//获取产品库列表
		  	}
			}
  	},
		jump(ProductId,SpecificationId,ProductFirstId){//跳转到产品详情
			this.$router.push({path:"/component/productstoredetail",query:{ProductId:ProductId,SpecificationId:SpecificationId,ProductFirstId:ProductFirstId}});
		},
		imgList(){
			this.img1='../../static/credit/topc.png';
			this.img2='../../static/credit/botc.png';
			this.img3='../../static/credit/topc.png';
			this.img4='../../static/credit/botc.png';
		},
    getSpecificationByVCIdList(rank,isDesc){//获取产品库列表
     		var _this=this;
     		var params={
  		    "VCId":this.getId,
  		    "OpenId":this.openId,
  		    "PageIndex":0,
  		    "Rank":rank,
  		    "IsDesc":isDesc
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getSpecificationByVCIdList,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.productList=data.Info;
	  			for(var i=0;i<_this.productList.length;i++){
	  				var productlist=_this.productList[i];
	  				if(productlist.IsPraise=='f'){
	  					productlist.praiseImg='../../static/credit/zanw.png';
	  				}else{
	  					productlist.praiseImg='../../static/credit/zanr.png';
	  				}
	  			}
	  		})
     },
     getProductSecondWX(){//获取顶部分类
     	  var _this=this;
	     	var params={
	  		  "ProductFirstId":this.getId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getProductSecondWX,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.iconBtnList=data;
	  		})
     },
     getProductFirstList(rank,isDesc){//获取一级分类规格列表
     	  var _this=this;
	     	var params={
	  		  "ProductFirstId":this.getId,
	  		  "ProductSecondId":this.ProductSecondId,
	  		  "OpenId":this.openId,
	  		  "Rank":rank,
	  		  "IsDesc":isDesc,
	  		  "PageIndex":0
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.getProductFirstList,
	  			data:params
	  		}).then(function(response){
	  			var data=JSON.parse(response.data);
	  			_this.productList=data.Info;
	  			for(var i=0;i<_this.productList.length;i++){
	  				var productlist=_this.productList[i];
	  				if(productlist.IsPraise=='f'){
	  					productlist.praiseImg='../../static/credit/zanw.png';
	  				}else{
	  					productlist.praiseImg='../../static/credit/zanr.png';
	  				}
	  			}
	  		})
      },
      praiseClick(index,praiseimg,specificationId){//点赞
				if(praiseimg=='../../static/credit/zanw.png'){
					this.addSpecificationPraiseLog(index,specificationId);//增加规格点赞
				}
			},
			addSpecificationPraiseLog(index,specificationId){//增加规格点赞
				var _this=this;
				var params={
	  		  "SpecificationId":specificationId,
	  		  "OpenId":this.openId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addSpecificationPraiseLog,
	  			data:params
	  		}).then(function(response){
           _this.productList[index].praiseImg="../../static/credit/zanr.png";
           _this.productList.splice(index,1,_this.productList[index]);
	  		})
			},
			share(){//分享到朋友圈
	    	  var _this=this;
			  	this.$http({
			  		method:'GET',
			  		url:apiUrl.getTimestampAndNonceStr
			  	}).then(function(response){
			  		var data=response.data;
			  		var dataArr=data.split('|');
			  		_this.timestamp=parseInt(dataArr[0]);//时间戳
			  		_this.nonceStr=dataArr[1];//随机字符串
			  		
			      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
			  		_this.$http({
			  			method:'GET',
			  			url:apiUrl.getJsapiTicketSignature+params
			  		}).then(function(response){
			  			_this.signature=response.data;
			  			  
		  			  wx.config({
					      debug:false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'onMenuShareTimeline',//分享到朋友圈
					        'onMenuShareAppMessage'//分享给朋友
					      ]
					    })
			  			  
			  			wx.ready(function(){
			    			wx.onMenuShareTimeline({
								    title: '调味列表页', // 分享标题
								    link: apiUrl.getUrl+'/#/component/productstorelist?getId='+_this.getId+'&isList='+_this.isList, // 分享链接
										imgUrl: _this.shareicon, // 分享图标
										success: function () { 
										// 用户确认分享后执行的回调函数
										  _this.addECWXTranspondDetails(2);
										},
										cancel: function () {
	//										alert('取消');
										// 用户取消分享后执行的回调函数
										}
								});
			          wx.onMenuShareAppMessage({
										title: '调味列表页', // 分享标题
										desc:  '调味列表页', // 分享描述
										link:apiUrl.getUrl+'/#/component/productstorelist?getId='+_this.getId+'&isList='+_this.isList, // 分享链接
										imgUrl: _this.shareicon,// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
	//									  alert('分享成功');
										  _this.addECWXTranspondDetails(1);
										},
										cancel: function () {
	//										alert('取消');
										// 用户取消分享后执行的回调函数
										},
										fail:function(res){
	//						        alert(res.errMsg);
							      }
								});
	    			  })
			  			
			  		})
			  	})
	    },
	    addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
	      	var _this=this;
		    	var params={
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/productstorelist",
		  		  "Parameter":"",
		  		  "OpenId":this.openId,
		  		  "TranspondType":type
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.addECWXTranspondDetails,
		  			data:params
		  		}).then(function(response){
		//			console.log(response.data);
		  		})
		  }
  },
  mounted(){
    this.imgHeight=$(window).width()*0.44;
  	this.imgList();//三角
  	this.img1='../../static/credit/topr.png';
  	this.userData=cookie.get('WeiXinUser',function(val){
  		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
  	}) 
	  this.userId=this.userData.UserId;
	  this.userType=this.userData.UserType;
	  this.openId=this.userData.Openid;
	  
  	this.getId=this.$route.query.getId;
  	this.isList=this.$route.query.isList;
  	if(this.isList==0){//有顶部分类
  		this.getProductSecondWX();//获取顶部分类
  		this.getProductFirstList("count","f");//获取一级分类规格列表
  	}else if(this.isList==1){//没有顶部分类
  		this.getSpecificationByVCIdList("count","f");//获取产品库列表
  	}
  	
		//分享
    var uri=location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
  	
//	if(this.categoryId==1000){
//		$('title').html('新品上架');
//	}else if(this.categoryId==1003){
//		$('title').html('经典爆款');
//	}else if(this.categoryId==1004){
//		$('title').html('EC新品');
//	}else if(this.categoryId==1005){
//		$('title').html('试用专区');
//	}else if(this.categoryId==2000){
//		$('title').html('酱');
//	}else if(this.categoryId==2001){
//		$('title').html('酱油');
//	}else if(this.categoryId==2002){
//		$('title').html('耗油');
//	}else if(this.categoryId==2003){
//		$('title').html('复合酱汁');
//	}else if(this.categoryId==2004){
//		$('title').html('料酒');
//	}else if(this.categoryId==2005){
//		$('title').html('香脆椒');
//	}
  	
  }
}
</script>

<style >
	/*顶部滑动*/
	.wrap{display: flex; width: 100%;padding: 15px 0;min-height: 30px;padding-bottom: 0;}
	.productstorelisticonbox{width: 90%;height:30px;display: flex;overflow-x:auto; }
  .productstorelisticonbox::-webkit-scrollbar{display: none;}
  .productstorelisticon{width: 90px;height: 30px;background-color: #f4f4f4; border-radius:5px;margin:0px 5px;text-align:center;line-height:30px;font-size:12px;}
  .productstorelisticonbgc{background-color: #e93027!important;color: white;}
 
  /*人气值 更新时间*/
  .read_top{width: 100%;background: #fff;display: flex;min-height: 26px;padding-top: 10px;}
  .read_top div{width: 30%;text-align: center;font-size: 13px;color: #3E3E3E;}
  .read_top div:nth-child(1){margin-left: 20%;}
  .read_top img{width: 9px;}
  .topImg{position: relative;top:-6px;left: 4px;}
  .botImg{position: relative;top:2px;left: -5px;}
  /*产品列表*/
  .shoplist{width:100%;margin-top: 6px;display: flex;flex-wrap:wrap;}
  .shoplist .shop_box{width: 44%;background: #fff;margin-bottom: 15px;margin-left: 4%;}
  .shopImgDiv{width: 100%;}
  .shopImgDiv img{width: 100%;}
  .shop_box p{font-size: 12px;color: #3E3E3E;margin-top: 5px;}
  .good *{display: inline-block;vertical-align:middle}
  .good img{width: 20px;margin-right: 10px;}
  .good span{font-size: 10px;color: #959595;position: relative;top: 3px;}
    
  /*顶部固定*/
  .page{height: 100%;display: flex;flex-direction: column;}
  .content::-webkit-scrollbar{display: none;}
  .content{overflow: auto;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
  
</style>