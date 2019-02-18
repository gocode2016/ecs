<template>
  <div class="dishproductlist">
  	
  	<div class="page">
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
import { cookie } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
  components:{
		cookie
  },
  data(){
	  return{
		userData:{},
		userId:'',
		userType:'',
		openId:'',
        productId:'',
        productList:[]
	  }
  },
  methods:{
        getProductList(){//获取产品库列表
     		var self = this;
	  		this.$http({
	  			method:'get',
	  			url:apiUrl.getGuiGeList + '?ProductId=' + this.productId + '&OpenId=' + this.openId
	  		}).then(function(response){
//	  			var data=JSON.parse(response.data);
	  			var data=response.data;//测试
//	  			console.log(data);
	  			self.productList=data;
	  			$('title').html(self.productList[0].ProductName)//标题
	  			for(let i in self.productList){
	  				var productlist=self.productList[i];
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
		jump(ProductId,SpecificationId,ProductFirstId){//跳转到产品详情
			this.$router.push({path:"/component/productstoredetail",query:{ProductId:ProductId,SpecificationId:SpecificationId,ProductFirstId:ProductFirstId}});
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
	this.userId=this.userData.UserId;
	this.userType=this.userData.UserType;
	this.openId=this.userData.Openid;
	  
  	this.productId=this.$route.query.productId;
  	this.getProductList();//获取产品库列表
  }
}
</script>

<style >
  /*产品列表*/
  .shoplist{width:100%;margin-top: 14px;display: flex;flex-wrap:wrap;}
  .shoplist .shop_box{width: 44%;background: #fff;margin-bottom: 15px;margin-left: 4%;}
  .shopImgDiv{width: 100%;}
  .shopImgDiv img{width: 100%;}
  .shop_box p{font-size: 12px;color: #3E3E3E;margin-top: 5px;}
  .good *{display: inline-block;vertical-align:middle}
  .good img{width: 20px;margin-right: 10px;}
  .good span{font-size: 10px;color: #959595;position: relative;top: 3px;}
    
</style>
<style>
  #vux_view_box_body{background: #fff;}
  
</style>