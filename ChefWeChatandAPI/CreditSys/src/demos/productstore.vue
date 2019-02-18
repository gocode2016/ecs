<template>
  <div class="productstore">
    <!-- 头部图片 -->
  	<swiper auto :list="bannerList" v-model="bannerIndex" dots-position='center' :show-desc-mask=false height="194px"></swiper>
    <!-- 图标部分 -->
    <div class="productstoreicon">
      <grid :cols="3">
	      <grid-item v-for="(item,index) in iconBtnList" :key="index" :label="item.ProductFirstName">
	        <img slot="icon" :src="item.ProductFirstIcon" @click="jumplist('/component/productstorelist',0,item.ProductFirstId)">
	      </grid-item>
	    </grid>
    </div>
    
    <!-- 商品列表 -->
	  <!-- 新品上架和经典爆款 -->
	  <div class="shop_list" v-for="(item,index) in filterproductList" :key="index">
		  <cell :title="item.VCName" value='更多' @click.native="jumplist('/component/productstorelist',1,item.VCId)" style="background: url('../../static/credit/productback.png') no-repeat -15px center;background-size:100px 26px ;"></cell>
	    <div class="categorydesc"><p>{{item.ShortIntroduce}}</p></div>
	    <div class="productWrap">
		    	<div class="productBox" v-for="(item,indexs) in item.proList" :key="indexs">
		    		<div @click="jumpdetail('/component/productstoredetail',item.ProductId,item.SpecificationId,item.ProductFirstId)"><img :src="item.SpecificationPicUrl"></div>
		    		<p>{{item.Amount}}{{item.Unit}}{{item.ProductName}}</p>
		    		<p><img :src="item.praiseimg" @click="praiseClick(index,indexs,item.praiseimg,item.SpecificationId)"><span>{{item.VisitCount}}人看过</span></p>
		    	</div>
			</div>
		</div>
		
		<!-- EC新品 -->
		<div class="shop_list" v-for="(item,index) in filterproductListEC">
		  <cell :title="item.VCName" value='更多' @click.native="jumplist('/component/productstorelist',1,item.VCId)" style="background: url('../../static/credit/productback.png') no-repeat -15px center;background-size:100px 26px ;"></cell>
	    <div class="categorydesc"><p>{{item.ShortIntroduce}}</p></div>
	    <div class="newproWrap">
	    	<div class="newproBox" v-for="(item,index) in item.proList" @click="jumpdetail('/component/productstoredetail',item.ProductId,item.SpecificationId,item.ProductFirstId)">
	    		<div class="newpro_img">
	    			<img :src="item.SpecificationPicUrl">
	    		</div>
	    		<div class="newpro_name">
	    			<p>{{item.ProductName}}</p>
	    			<p>{{item.Introduce}}</p>
	    		</div>
	    	</div>
	    </div>
		</div>
		
		<!-- 试用专区 -->
		<div class="shop_list">
		  <cell title="试用专区" value='更多' @click.native="jumpshoplist" style="background: url('../../static/credit/productback.png') no-repeat -15px center;background-size:100px 26px ;"></cell>
	    <div class="categorydesc"><p>品鉴尝新技高一筹</p></div>
	    <div class="productWrap">
		    	<div class="productBox" v-for="(item,index) in syList" @click="jumpshopdetail(item.ProductId,item.ProductType)">
		    		<div><img :src="item.ImgUrl"></div>
		    		<p style="margin-bottom: 10px;">{{item.ProductName}}</p>
		    	</div>
			</div>
		</div>
		
		<!--备案-->
    <div style="position: relative;margin-top: 20px;margin-bottom: 80px;" >
    	<span class="record" @click="recordClick">鲁ICP备10013060号</span>
    </div>
		
  </div>
</template>

<script>
  import { Swiper,Grid, Cell,GridItem,cookie,Scroller } from 'vux'
	import apiUrl from '../apiUrls.js'
  import wx from 'weixin-js-sdk'
	
	export default{
		components:{
			Swiper,
			Grid, 
			GridItem,
	    cookie,
	    Scroller,
	    Cell
    },
    data(){
      return{
      	  timestamp:'',
			    nonceStr:'',
			    signature:'',
			    url:'',
			    shareicon:'',
	      	userData:{},
	      	userId:'',
	      	userType:'',
	      	openId:'',
				  bannerIndex:0,
				  bannerList:[],
	        iconBtnList: [],
					productList:[],
	        filterproductList:[],
		  		filterproductListEC:[],
	        syList:[]//试用专区列表
      }
    },
    methods:{
    	  recordClick(){//点击备案
				  window.location.href = 'http://www.miitbeian.gov.cn/state/outPortal/loginPortal.action;jsessionid=yZGh3fHt8OZyxjnAk5z89RzZPqOh3mGi-rolRbIEDyLR4-2KQcQp!-736616813'
			  },
    	  getProductBanner(){//获取轮播图
    	  	var _this=this;
    	  	var params={
  		      "BannerType":2
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getProductBanner,
		  			data:params
		  		}).then(function(response){
		  			var data=JSON.parse(response.data);
		  			for(let i in data){
		  				var item={
		  					url:data[i].Link,
							  img:data[i].PicUrl
		  				}
		  				_this.bannerList.push(item);
	  			  }
		  		})
    	  },
    	  getProductFirst(){//获取一级分类
    	  	var _this=this;
		  		this.$http({
		  			method:'get',
		  			url:apiUrl.getProductFirst
		  		}).then(function(response){
//		  			console.log(JSON.parse(response.data));
		  			var data=JSON.parse(response.data);
		  			_this.iconBtnList=data;
		  		})
    	  },
    	  getProductVC(){//获取列表
    	  	var _this=this;
		  		this.$http({
		  			method:'get',
		  			url:apiUrl.getProductVC
		  		}).then(function(response){
		  			var data=JSON.parse(response.data);
		  			_this.productList=data;
		  			for(var i in _this.productList){
		  				var productlist=_this.productList[i];
		  				_this.getSpecificationByVCId(i,productlist.VCId);
		  			}
		  		})
    	  },
    	  getSpecificationByVCId(index,VCId){//获取列表产品
    	  	var _this=this;
    	  	var params={
    	  		"VCId":VCId,
    	  		"OpenId":this.openId
    	  	}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getSpecificationByVCId,
		  			data:params
		  		}).then(function(response){
		  			var data=JSON.parse(response.data);
		  			_this.productList[index].proList=data;
//		  			console.log(_this.productList);

		  			if(index==2){
		  				setTimeout(function(){
	            	_this.filterproductList=_this.productList.slice(0,2);
			  				_this.filterproductListEC=_this.productList.slice(2,3);
//			  				console.log(_this.filterproductList);
//		  				  console.log(_this.filterproductListEC);
			  				for(var i in _this.filterproductList){
			  					var proList=_this.filterproductList[i].proList;
			  					for(var i in proList){
			  						var prolist=proList[i];
			  						if(prolist.IsPraise=="f"){
			  							prolist.praiseimg="../../static/credit/zanw.png";
			  						}else{
			  							prolist.praiseimg="../../static/credit/zanr.png";
			  						}
			  					}
			  				}
              },10)
		  			}
		  			
		  		})
    	  },
    	  getProductList(){//获取商品列表 试用专区
					var _this=this;
					var params={
				    "productType":0,
						"categoryId":0,
						"productName":'',
						"PageSize" :999,
						'IndexPage':1,
						"priceOD"  :0,
					  "saleOD"   :0
				  }
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getProductList,
		  			data:params
		  		}).then(function(response){
		  			var data=JSON.parse(response.data);
		  			var productArr=data.data;
//		  			console.log(productArr);
	          for(var i=0;i<productArr.length;i++){
	           	var productlist=productArr[i];
	           	var item={
		       	 	 	ImgUrl:productlist.ImgUrl,
		       	 	 	ProductId:productlist.ProductId,
		       	 	 	ProductType:productlist.ProductType,
		       	 	 	ProductName:productlist.ProductName,
		       	 	 	ProductPrice:productlist.ProductPrice
	           	}
	           	if(productlist.CategoryId==1001){
	           	 	_this.syList.push(item);
	           	}
	          }
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
								    title: '调味首页', // 分享标题
								    link: apiUrl.getUrl+'/#/component/productstore', // 分享链接
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
										title: '调味首页', // 分享标题
										desc:  '调味首页', // 分享描述
										link:apiUrl.getUrl+'/#/component/productstore', // 分享链接
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
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/productstore",
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
		    },
				praiseClick(index,indexs,praiseimg,specificationId){//点赞
					if(praiseimg=='../../static/credit/zanw.png'){
						this.addSpecificationPraiseLog(index,indexs,specificationId);//增加规格点赞
					}
				},
				addSpecificationPraiseLog(index,indexs,specificationId){//增加规格点赞
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
            var proList=_this.filterproductList[index].proList;
            proList[indexs].praiseimg="../../static/credit/zanr.png";
            proList.splice(indexs,1,proList[indexs]);
             _this.filterproductList.splice(index,1,_this.filterproductList[index]);
            
		  		})
				},
        jumplist(link,isList,getId){//跳转到产品列表 
        	this.$router.push({path:link,query:{isList:isList,getId:getId}});
        },
        jumpdetail(link,ProductId,SpecificationId,ProductFirstId){//跳转到产品详情
        	this.$router.push({path:link,query:{ProductId:ProductId,SpecificationId:SpecificationId,ProductFirstId:ProductFirstId}});
        },
        jumpshoplist(){//跳转到商城列表
        	this.$router.push({path:'/component/readlist',query:{categoryId:1001}});
        },
        jumpshopdetail(productId,productType){//跳转到商城详情
				  this.$router.push({path:'/component/shopdetail',query:{ProductId:productId,ProductType:productType}});
        }
    },
    mounted(){
    	//用户信息
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
	    this.openId=this.userData.Openid;
	    this.userType=parseInt(this.userData.UserType);
      //分享
	    var uri=location.href.split('#')[0];
			this.url=encodeURIComponent(uri);
			this.share();
			
	    //数据
	    this.getProductBanner();//获取轮播图
	    this.getProductFirst();//获取一级分类
	    this.getProductVC();//获取列表
	    this.getProductList();//获取试用专区列表
	    
		},
		computed:{
//			filterproductList:function(){
//      return this.productList.slice(0,2)
//			},
//			filterproductListEC:function(){
//      return this.productList.slice(2,3)
//			},
//			filterproductListSY:function(){
//      return this.productList.slice(3,4)
//			}
		}
  }
</script>
<style>
  /*商品列表*/
  .shop_list{width:100%;border-top: 5px solid #f8f8f8;padding-top: 10px}
  .categorydesc{font-size: 13px;color: #ababab;margin:0px 10px;}
	.productWrap{width: 100%;display: flex;overflow-x: auto;margin-top: 10px;}
	.productBox{margin-left: 8px;}
	.productBox:nth-child(1){margin-left: 10px;}
	.productBox div{width:156px;}
	.productBox div img{width:100%;}
	.productBox p:nth-child(2){color: #3E3E3E;font-size: 13px;}
	.productBox p:nth-child(3) *{display: inline-block;vertical-align: middle;}
	.productBox p:nth-child(3){color: #949494;font-size: 11px;margin-top: 10px;margin-bottom: 15px;}
	.productBox p:nth-child(3) img{width: 20px;margin-right: 10px;}
	.productBox p:nth-child(3) span{position: relative;top: 4px;}
	.productWrap::-webkit-scrollbar {display:none}/*隐藏滚动条*/
	.newproWrap{padding-left: 10px;margin-bottom: 10px;}
	.newproBox{display: flex;flex-wrap: wrap;margin-top: 10px;}
	.newproBox div:nth-child(1){width: 40%;}
	.newproBox div:nth-child(2){width: 50%;margin-left:5%;}
	.newproBox .newpro_img img{width: 100%;}
	.newproBox .newpro_name p:nth-child(1){color: #e92e25;font-size: 14px;margin-top: 30px;}
	.newproBox .newpro_name p:nth-child(2){color: #3E3E3E;font-size: 12px;margin-top: 10px;}
	/*备案*/
  .record{padding:10px 16px;background: #f6f6f6;color: #818181;font-size: 12px;text-align: center;border-radius: 10px;letter-spacing: 1px;
          position: absolute;left: 50%;transform: translateX(-50%);}
</style>
<style>
  #vux_view_box_body{background: #fff;}
  /*图标*/
  .productstore .weui-grid__label {color: #3e3e3e !important;font-size: 12px !important;}
  .productstore .weui-grid__icon{width: 45px !important;height: 45px !important;margin: 10px auto;}
  .productstore .weui-grid{padding:0 !important;}
  .productstore .weui-grids:after{content: none !important;}
  .productstore .weui-grids:before{content: none !important;}
  .productstore .weui-grid:after{content: none !important;}
  .productstore .weui-grid:before{content: none !important;}
  .productstoreicon{padding:10px 0px;}
   /*商品列表*/
  .productstore .shop_list .vux-label{font-size: 14px !important;color: white;}
  .productstore .shop_list .weui-cell__ft{font-size: 13px !important;position: relative;top: 10px;}
  .productstore .shop_list .weui-cell{padding: 10px 10px;}
	.productstore .shop_list .weui-cell__ft:after {content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);position: relative;top: -1px;right: 2px;}
</style>

