<template>
  <div class="readlist">
    
    <div class="read_top">
    	<div @click="priceClick">价格<img :src="img1" class="topImg"><img :src="img2" class="botImg"></div>
    	<div @click="saleClick">销量<img :src="img3" class="topImg"><img :src="img4" class="botImg"></div>
    </div>
    <div class="shoplist">
    	<div class="shop_box" v-for="(item,index) in productList" :key="index" @click="jump(item.ProductType,item.ProductId)">
    		<div class="shopImgDiv"><img :src="item.ImgUrl"></div>
    		<p>{{item.ProductName}}</p>
    		<p class="money"><span>{{item.ProductPrice}}</span><img src="../../static/credit/money.png" /></p>
    	</div>
    </div>
    <div class="carIcon" @click="carClick" v-show="carIconFlag">
    	<img src="../../static/credit/caricon.png"/>
      <badge :text="carnum" v-show='carnumFlag'></badge>
    </div>
    
  </div>
</template>

<script>
import { cookie,Badge } from 'vux'
import apiUrl from '../apiUrls.js'
var priceNum=1;
var saleNum=0;
export default{
  components:{
		cookie,
		Badge
  },
  data(){
	  return{
	  	carIconFlag:false,//购物车按钮
	  	carnumFlag:false,//购物车商品数量
	  	carnum:0,
			img1:'',
			img2:'',
			img3:'',
			img4:'',
      categoryId:'',
      productList:[],
//    hotelProvinceId:'',//用户注册时填写的酒店区域省id
      userType:'',
      imgHeight:''
	  }
  },
  methods:{
//  getMemberById(){//获取简历信息
//		var _this=this;
//			var params={
//			  'MemberId':this.userId
//			}
//			this.$http({
//				method:'POST',
//				url:apiUrl.getMemberById,
//				data:params
//			}).then(function(response){
//				var data=JSON.parse(response.data);
//				if(data.length!=0){
//					var userinfo=data[0];
//					_this.hotelProvinceId=userinfo.ProvinceId;
//					_this.getProductList(1,0);//获取商品列表信息
//					_this.img1='../../static/credit/topr.png';
//				}
//			})
//  },
		getProductList(priceOD,saleOD){//获取商品列表
			var _this=this;
			var params={
		    "productType":0,
				"categoryId":this.categoryId,
				"productName":'',
				"PageSize" :999,
				"IndexPage":1,
				"priceOD"  :priceOD,
				"saleOD"   :saleOD
		  }
  		this.$http({
  			method:'post',
  			url:apiUrl.getProductList,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.productList=data.data;
  			
//			  for(var i=0;i<_this.productList.length;i++){
//					_this.productList[i].ImgUrl=_this.productList[i].ImgUrl.replace('Product','SmallProduct');//更换图片地址
//				}
  			
//			var productList=data.data;
//			var productArr=[];
//			if(_this.userType==1||_this.userType==0){//队员或未注册  可以查看全部商品 不限制销售区域
//				for(var i=0;i<productList.length;i++){//更换图片地址
//					productList[i].ImgUrl=productList[i].ImgUrl.replace('Product','SmallProduct');//更换图片地址
//				}
//				_this.productList=productList;
//			}else if(_this.userType==2){//会员限制销售区域
//				for(var i=0;i<productList.length;i++){//判断商品销售区域  0是全部省份
//	  				var productlist=productList[i];
//					  productlist.ImgUrl=productlist.ImgUrl.replace('Product','SmallProduct');//更换图片地址
//	  				if(productlist.SaleProvince!='0'){
//							if(productlist.SaleProvince.indexOf('|')==-1){//单个省份
//								productlist.SaleProvince=parseInt(productlist.SaleProvince);
//								if(productlist.SaleProvince==_this.hotelProvinceId){//显示此商品
//		                productArr.push(productlist);
//								}
//							}else{
//								var saleArr=productlist.SaleProvince.split('|');
//							  for(var j=0;j<saleArr.length;j++){
//							  	var sale=saleArr[j];
//							  	sale=parseInt(sale);
//							  	if(sale==_this.hotelProvinceId){
//							  		productArr.push(productlist);
//							  	}
//							  }
//						  } 
//	  				}else if(productlist.SaleProvince=='0'){//0 全部省份
//	  					productArr.push(productlist);
//	  				}
//	  		  }
//			  _this.productList=productArr;
//			}
  		})
		},
		getCartList(){//获取购物车列表 商品数量
			var _this=this;
  		this.$http({
  			method:'post',
  			url:apiUrl.getCartList+'?memberId='+this.userId
  		}).then(function(response){
  			var dataList=JSON.parse(response.data);
  			var count=0;//商品数量
  			for(var i=0;i<dataList.length;i++){
  				var data=dataList[i];
  				count+=data.Count;
  			}
  			if(count!=0){
  				_this.carnumFlag=true;
  				_this.carnum=count;
  			}  			
  		})
  	},
  	carClick(){//点击购物车按钮
  		this.$router.push('/component/shopcar');
  	},
		jump(ProductType,ProductId){
			this.$router.push({path:"/component/shopdetail",query:{ProductType:ProductType,ProductId:ProductId}});
		},
		imgList(){
			this.img1='../../static/credit/topc.png';
			this.img2='../../static/credit/botc.png';
			this.img3='../../static/credit/topc.png';
			this.img4='../../static/credit/botc.png';
		},
		priceClick(){//价格
			if(priceNum==1){
				this.imgList();
				this.img2='../../static/credit/botr.png';
				priceNum=2;
				saleNum=0;
				this.getProductList(2,0);//获取商品列表信息
			}else if(priceNum==2||priceNum==0){
				this.imgList();
				this.img1='../../static/credit/topr.png';
				priceNum=1;
				saleNum=0;
				this.getProductList(1,0);//获取商品列表信息
			}
		},
		saleClick(){//销量
			if(saleNum==1){
				this.imgList();
				this.img4='../../static/credit/botr.png';
				saleNum=2;
				priceNum=0;
				this.getProductList(0,2);//获取商品列表信息
			}else if(saleNum==2||saleNum==0){
				this.imgList();
				this.img3='../../static/credit/topr.png';
				saleNum=1;
				priceNum=0;
				this.getProductList(0,1);//获取商品列表信息
			}
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
	  
  	this.categoryId=this.$route.query.categoryId;//获取categoryId
  	if(this.categoryId==1000){
  		$('title').html('读书一角');
  	}else if(this.categoryId==1001){
  		$('title').html('试用专区');
  	}else if(this.categoryId==1002){
  		$('title').html('活动名额');
  	}else if(this.categoryId==1003){
  		$('title').html('后厨物料');
  	}else if(this.categoryId==1004){
  		$('title').html('健康美味');
  	}else if(this.categoryId==1005){
  		$('title').html('生活用品');
  	}
  	
  	this.getProductList(1,0);//获取商品列表信息
	  if(this.userType==2){//会员 展示购物车图标及购物车数量
  	  this.carIconFlag=true;
  	  this.getCartList();
	  }
	  
  }
}
</script>

<style scoped>
  .read_top{width: 100%;height:50px;background: #fff;line-height: 50px;display: flex;}
  .read_top div{width: 30%;text-align: center;font-size: 13px;color: #3E3E3E;}
  .read_top div:nth-child(1){margin-left: 20%;}
  .read_top img{width: 9px;}
  .topImg{position: relative;top:-6px;left: 4px;}
  .botImg{position: relative;top:2px;left: -5px;}
  .shoplist{width:100%;margin-top: 6px;background: #fcfcfc;display: flex;flex-wrap:wrap;}
  .shoplist .shop_box{width: 44%;background: #fff;margin-bottom: 10px;margin-left: 4%;}
  .shopImgDiv{width: 100%;}
  .shopImgDiv img{width: 100%;}
  .shop_box p{font-size: 12px;color: #3E3E3E;margin-top: 5px;}
  .shop_box .money{margin-bottom: 5px;}
  .shop_box .money span{color: #e00404; margin-right: 3px;}
  .shop_box .money img{width:15px;position: relative;top: 3px;}
  .carIcon{width: 50px;height: 50px;position: absolute;bottom: 50px;right: 20px;}
  .carIcon img{width: 100%;height: 100%;}
</style>
<style>
  #vux_view_box_body{background: #fcfcfc;}
  .readlist .vux-badge{position: relative;top: -65px;left: 35px;}
</style>