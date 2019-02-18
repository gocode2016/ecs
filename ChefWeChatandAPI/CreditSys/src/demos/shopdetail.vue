<template>
  <div class="shopdetail" style="height: 100%">
     <div style="height: 93%;overflow: auto;width: 100%;" class="detail-wrap">
     	 <div class="detail-box">
		     <div class="pro-content">
		     	 <div class="pro-img" v-bind:style="{background:'url('+productInfo.ImgUrl+') no-repeat',backgroundSize:'100% 100%'}"><img class="productImg" :src="productInfo.ImgUrl"/></div>
		     	 <p class="pro-name">{{productInfo.ProductName}}</p>
		     	 <p><span class="pro-inte">{{productPrice}}</span><img src="../../static/credit/money.png" class="inteImg"/></p>
		     	 <p v-show="opriceFlag"><span class="pro-intec">{{oprice}}</span><img src="../../static/credit/money_c.png" class="inteImg"/></p>
		     </div>
		     <div class="pro-box">
		     	 <!--库存-->
		     	 <span class="pro-num" v-show="stockFlag"><span>库存</span>{{skuData.Stock}}</span>
		     	 <!--规格-->
		     	 <div class="pro-type" v-for="(item,index) in typeList" :key="index" >
		     		 <p>{{item.typename}}</p>
				     <checker v-model="demo"  radio-required  default-item-class="demo-item" selected-item-class="demo-item-selected">
					     <checker-item  v-for="(item,index) in item.typevalue" :value="item" :key="index" @click.native="productClick(item.normsId)">{{item.value}}</checker-item>			   
				     </checker>
		     	 </div>
		     	 <!--数量-->
		     	 <p>数量<span class="shop-xg" v-show="limitFlag"><span>限购</span>{{skuData.Limit}}件</span></p>
		     	 <div class="XNumber">
		     	    <x-number :name="$t('Quantity')" :min="1" :max="maxNum"  v-model="numValue" align='left' width='35px'></x-number>
		     	 </div>
		     </div>
		     
		     <div class="show">
				    <p class="pro-detail">继续拖动，查看图文详情</p>
		     </div>
		     <p class="proContent"></p>
	     </div>
     </div>
     
     <!--底部按钮-->
		 <div class="pro-foot">
		 	 <span v-if="isShowCar" class="foot-car">
		 	 	 <span class="carImg" @click="jump('/component/shopcar')" v-show="categoryId!=1001">
		 	 	 	 <img src="../../static/credit/shopcar.png"/>
		 	 	 	 <badge :text="carnum" v-show='carnumFlag'></badge>
		 	 	 </span>
		     <span class="addCar" @click='addCartClick' v-show="categoryId!=1001">加入购物车</span>
		     <button @click="exchange('sw')" class="swBtn">立即兑换</button>
		 	 </span>
		 	 <button v-else @click="exchange('xn')" class="xnBtn">立即兑换</button>
		 </div>
		 
		 <!--提示注册弹框-->
    <div class="mask" v-show="maskFlag">
    	<div>
    		<p>需要注册才能参与哦！</p>
    		<button @click="jump('/component/registeredmembers')">马上注册</button>
    	</div>
    </div>
		 
  </div>
</template>

<script>
import { Checker, CheckerItem ,XNumber,cookie,Badge,Toast,Loading} from 'vux'
import apiUrl from '../apiUrls.js'

export default{
  components:{
	  Checker,
    CheckerItem,
    XNumber,
    cookie,
    Badge,
    Toast,
    Loading
  },
  data(){
		return{
			userId:'',
			userType:'',
			userData:{},
			openId:"",
			skuData:'',
		  demo:'',
		  maskFlag:false,//未注册弹框提示
      limitFlag:false,//限购 
      stockFlag:false,//库存
		  typeList:[],
		  productPrice:'',//商品价格
		  productId:'',//商品id
		  normsId:'',//规格id
		  productType:'',//商品类型 1是实物 2是虚拟
		  categoryId:'',//类目id 1001试用专区时 跳转到酒店信息
		  isPerfect:'',
		  productInfo:{},//商品信息
		  numValue:1,//加入的商品数量
		  isShowCar:true,//true实物详情页  false虚拟商品详情页
      maxNum:10000,//商品最大数量显示
      carnum:0,//购物车数量
      carnumFlag:false,//购物车数量是否显示 
      Sale:[],//商品折扣
      opriceFlag:false,//展示原价
      oprice:'',//原价
      hotelProvinceId:'',//用户所在省份
      isStockZero:'',//库存是否显示为0  yes0 no不为0
      isJumpHotel:''//是否跳转酒店信息 1跳转 0不跳转
		}
  },
  methods:{
  	getMemberById(){//获取简历信息
	  		var _this=this;
				var params={
				  'MemberId':this.userId
				}
				this.$http({
					method:'POST',
					url:apiUrl.getMemberById,
					data:params
				}).then(function(response){
					var data=JSON.parse(response.data);
					if(data.length!=0){
						var userinfo=data[0];
						_this.hotelProvinceId=userinfo.ProvinceId;
//						console.log(_this.hotelProvinceId);
						_this.getProductDetail();//获取商品详情
					}
			  })
		},
		getMemberProfile(){//获取简历信息 
				var _this=this;
				var params={
				  'MemberId':this.userId
				}
				this.$http({
					method:'POST',
					url:apiUrl.getMemberProfile,
					data:params
				}).then(function(response){	
					var data=JSON.parse(response.data);
					if((data[0].PerConsumption==null||data[0].PerConsumption=="")&&(data[0].HomeIncome==null||data[0].HomeIncome=="")&&(data[0].WholesaleName==null||data[0].WholesaleName=='')){
						_this.isJumpHotel=1;//是否跳转酒店信息 1跳转 0不跳转 没有完善过酒店信息  跳转酒店信息页
					}else{
						_this.isJumpHotel=0;
					}
		    })
		},
		addCartClick(){//点击加入购物车
			if(this.userType==1){//队员
				this.$vux.toast.text('队员不能参与哦!', 'middle');
			}else if(this.userType==0){//未注册
				this.maskFlag=true;
			}else if(this.userType==2){//会员
				if($('.vux-checker-box .demo-item-selected').length==0){
					this.$vux.toast.text('请选择商品', 'middle');
				}else{
					if(this.skuData.Stock<=0){//库存不足
						this.$vux.toast.text('库存不足', 'middle');
					}else{
						this.showLoading();
						if(this.Sale.length!=0){//商品为限购商品
							this.checkMemberSale('cartBtn');//判断限购
						}else{
							this.addCart();//加入购物车接口
						}
					}
				}        
			}
		},
		addCart(){//加入购物车接口
				var _this=this;
	      var params={
	  		  'MemberId':this.userId,
					'SkuId':this.skuData.SkuId,
					'Count':this.numValue,
					'SkuName':this.skuData.SkuName,
					'Price':this.skuData.Price,
					'ProductId':this.skuData.ProductId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addCart,
	  			data:params
	  		}).then(function(response){
	  			_this.$vux.loading.hide();//loading图标消失
	  			_this.getCartList();//显示购物车商品数量
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
  			if(count!=0&&_this.userType==2){
  				_this.carnumFlag=true;
  				_this.carnum=count;
  			}  			
  		})
  	},
  	getProductDetail(){//获取商品详情
  		var _this=this;
  		var params='?productId='+this.productId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getProductDetail+params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.productInfo=data[0];
  			_this.categoryId=data[0].CategoryId;
  			if(_this.categoryId == 1001){
			  	$('.swBtn').html('立即领取');
			  }
  			_this.productPrice=_this.productInfo.ProductPrice;
  			
			  //判断商品销售区域
			  if(_this.userType==2){
	        if(_this.productInfo.SaleProvince!='0'){//销售区域限制
	          if(_this.productInfo.SaleProvince.indexOf('|')<0){//单个省份
				  		if(_this.productInfo.SaleProvince!=_this.hotelProvinceId){//销售区域与用户所在省份不匹配 设置库存为0
				  			_this.isStockZero="yes";
				  		}
	          }else{//多个省份
	          	var saleArr=_this.productInfo.SaleProvince.split('|');
	          	var savenum=[];
	          	for(var i=0;i<saleArr.length;i++){
	          		if(saleArr[i]!=_this.hotelProvinceId){//不匹配
	          			savenum.push(0);
	          		}else{//匹配
	          			savenum.push(1);
	          		}
	          	}
	          	savenum=savenum.join('');
	          	if(savenum.indexOf(1)<0){
				  			_this.isStockZero="yes";
	          	}
	          }
				  }
			  }
			  
	      _this.getProductNorms();//获取商品规格
			  
  		})
  	},
  	getProductNorms(){//获取商品规格
  		var _this=this;
  		var params='?productId='+this.productId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getProductNorms+params
  		}).then(function(response){
  			var dataList=JSON.parse(response.data);
//			  console.log(dataList);
        var typeValue=[];
        var typeName=dataList[0].TypeName;
        for(var i=0;i<dataList.length;i++){
        	var data=dataList[i];
        	var item={
        		normsId:data.NormsId,
        		value:data.TypeValue
        	}
        	typeValue.push(item);
        }
        _this.typeList=[{
        	typename:typeName,
				  typevalue:typeValue
        }]
        
        _this.demo=typeValue[0];//默认选中第一个规格
        _this.normsId=typeValue[0].normsId;
        _this.getSku();//获取商品第一个规格的信息
        
  		})
  	},
  	getSku(){//获取商品sku
  		var _this=this;
		  var params='?normsId='+this.normsId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getSku+params
  		}).then(function(response){
        var data=JSON.parse(response.data);
        _this.skuData=data.SkuData[0];
        _this.Sale=data.Sale;
        
        if(_this.Sale.length!=0){//此商品有活动
        	var sale=_this.Sale[0];
			  	if(sale.Limit!=0){
					  _this.limitFlag=true;//显示限购
					  _this.skuData.Limit=sale.Limit;
						//取商品最大值
						_this.maxNum=sale.Limit;
					}else{
						_this.limitFlag=false;//不显示限购
						_this.maxNum=_this.skuData.Stock;
					}
					_this.opriceFlag=true;
					_this.oprice=_this.skuData.Price;//原价
			  	_this.productPrice=sale.SalePrice;//活动价
        }else{//此商品没有活动
        	if(_this.skuData.Limit!=0){
					  _this.limitFlag=true;//显示限购
						//取商品最大值
						_this.skuData.Limit<=_this.skuData.Stock ? _this.maxNum=_this.skuData.Limit : _this.maxNum=_this.skuData.Stock;
					}else{
						_this.limitFlag=false;//不显示限购
						_this.maxNum=_this.skuData.Stock;
					}
					_this.opriceFlag=false;
				  _this.productPrice=_this.skuData.Price;//价格
        }
        _this.numValue=1;//数量初始值
        
        if(_this.isStockZero=='yes'){//产品销售区域与会员省份不匹配时 设置库存为0
        	_this.skuData.Stock=0;
        }
			  
			  if(_this.skuData.SkuId==1043){
			  	_this.checkMemberSale("norms");//点击规格 返回-1 库存为0
			  }else{
			  	_this.stockFlag=true;//显示库存
			  }

  		})
  	},
  	checkMemberSale(val){//商品限购
  		var _this=this;
  		if(this.userType==1){//队员 库存显示0
  			this.userId=0;
  		}
  		var params={
  		  "MemberId":this.userId,
		    "SkuId":this.skuData.SkuId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.checkMemberSale,
  			data:params
  		}).then(function(response){
        var data=null;
  			if(response.data==''){
  				data=0;
  			}else{
  				data=JSON.parse(response.data);
  			}
  			
  			if(val=="norms"){
  				if(data==-1){
  					_this.skuData.Stock=0;
  					_this.maxNum=_this.skuData.Stock;
  				}
			  	_this.stockFlag=true;//显示库存
  			}else{
  				if(_this.numValue<=_this.Sale[0].Limit-data){//可以买
	  				if(val=='cartBtn'){//点击加入购物车
	  					_this.addCart();//加入购物车接口
	  				}else if(val=='exchangeBtn'){//点击立即兑换
	  					_this.skuData.Count=_this.numValue;
	  					_this.skuData.SalePrice=_this.Sale[0].SalePrice;
	  					
	  					if(_this.categoryId==1001){//categoryId为1001  先完善酒店信息
								_this.$router.push({path:'/component/resumehotel',query:{skudata:_this.skuData,productType:_this.productType,source:'shopdetail',addressId:0}})
							}else{
								if(_this.isJumpHotel == 1){//跳转酒店信息
									_this.$router.push({path:'/component/resumehotel',query:{skudata:_this.skuData,productType:_this.productType,source:'shopdetail',addressId:0}});
								}else if(_this.isJumpHotel == 0){//不跳转酒店信息
									_this.$router.push({path:'/component/shoporder',query:{skudata:_this.skuData,source:'shopdetail',addressId:0}});
								}
							}
	  				}
	  			}else{//不能买
	  				_this.$vux.loading.hide();//loading图标消失
						_this.$vux.toast.text('此商品限购'+_this.Sale[0].Limit+'件', 'middle');
	  			}
  			}
  			
  		})
  	},
  	productClick(normsId){//点击sku
  		this.normsId=normsId;
  		this.getSku();//获取商品sku
  	},
	  jump(link){
	  	if(link=='/component/shopcar'&&this.userType==2){
	  	  this.$router.push(link);
	  	}else if(link=='/component/registeredmembers'){
        if(this.categoryId==1001){//参与申领试用
        	this.skuData.Count=this.numValue;
	  		  this.skuData.SalePrice=this.Sale[0].SalePrice;
					this.$router.push({path:link,query:{skudata:this.skuData,source:'shopdetail',addressId:0}})
        }else{
	  			this.$router.push(link);
        }
	  	}
	  },
	  exchange(val){//点击 立即兑换
	  	if(this.userType==1){//队员
				this.$vux.toast.text('队员不能参与哦!', 'middle');
			}else if(this.userType==0){//未注册
				this.maskFlag=true;
			}else if(this.userType==2){//会员
				var _this=this;
				if($('.vux-checker-box .demo-item-selected').length==0){
						this.$vux.toast.text('请选择商品', 'middle');
				}else{
					if(this.skuData.Stock<=0){//库存不足
							this.$vux.toast.text('库存不足', 'middle');
					}else{
						
						
//						if(val=='xn'){
//							this.$router.push({path:'/component/inventedorder',query:{skudata:this.skuData,count:this.numValue}});
//						}else if(val=='sw'){
////							this.skuData.Count=this.numValue;
////							this.$router.push({path:'/component/shoporder',query:{skudata:this.skuData,source:'shopdetail',addressId:0}});
//							if(this.Sale.length!=0){//商品为限购商品
//								this.checkMemberSale('exchangeBtn');//判断限购
//							}else{
//								this.skuData.Count=this.numValue;
//								this.skuData.SalePrice=-1;
//							  this.$router.push({path:'/component/shoporder',query:{skudata:this.skuData,source:'shopdetail',addressId:0}});
//							}
//						}
						
						if(this.isJumpHotel == 0){//不跳转到酒店信息
							if(val=='xn'){
								this.$router.push({path:'/component/inventedorder',query:{skudata:this.skuData,count:this.numValue}});
							}else if(val=='sw'){
								if(this.Sale.length!=0){//商品为限购商品
									this.checkMemberSale('exchangeBtn');//判断限购
								}else{
									this.skuData.Count=this.numValue;
									this.skuData.SalePrice=-1;
								  this.$router.push({path:'/component/shoporder',query:{skudata:this.skuData,source:'shopdetail',addressId:0}});
								}
						  }
						}else if(this.isJumpHotel == 1){//跳转到酒店信息
						  if(val=='xn'){
								this.$router.push({path:'/component/resumehotel',query:{skudata:this.skuData,count:this.numValue,productType:this.productType,source:'shopdetail'}});
							}else if(val=='sw'){
								if(this.Sale.length!=0){//商品为限购商品
									this.checkMemberSale('exchangeBtn');//判断限购
								}else{
									this.skuData.Count=this.numValue;
									this.skuData.SalePrice=-1;
								  this.$router.push({path:'/component/resumehotel',query:{skudata:this.skuData,productType:this.productType,source:'shopdetail',addressId:0}});
								}
						  }
						}
						
						
					}
				}
			}
	  },
	  addECBrowseDetails(){//记录页面访问
	    var source = this.$route.query.source;//来源
	  	if(source != 'undefined'){
	  		var params={
				  "ECURL":"http://jifenweixin.shinho.net.cn/#/component/shopdetail",
				  "PageName":"商品详情页",
				  "Parameter":source,
				  "OpenId":this.openId
				}
				this.$http({
					method:'post',
					url:apiUrl.addECBrowseDetails,
					data:params
				}).then(function(response){
	//				console.log(response.data);
				})
	  	}
		},
	  onChange (val) {//toast组件
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
	  showLoading () {//loading 图标
      this.$vux.loading.show({
        text: 'Loading'
      })
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
	  this.userId=parseInt(this.userData.UserId);//用户id int类型
	  
	  this.userType=parseInt(this.userData.UserType);
	  this.openId=this.userData.Openid;
	  this.getMemberProfile();//获取酒店信息 判断是否填写酒店信息
	  
	  this.productType=this.$route.query.ProductType;//商品类型
	  this.productId=this.$route.query.ProductId;//商品id
	  this.isPerfect=this.$route.query.isPerfect;//是否完善酒店信息 undefinde未完善 1已完善
	  this.addECBrowseDetails();//记录页面访问
	  
	  if(this.productType==2){//2虚拟商品 改变样式
		  this.isShowCar=false;//true实物详情  false虚拟商品详情
	  }
	  
	  if(this.userType==1||this.userType==0){//队员或未注册
	  	this.getProductDetail();//获取商品详情
	  }else  if(this.userType==2){//会员
	  	this.getMemberById();//获取用户所在省份
	  }
	  
//	  this.getProductNorms();//获取商品规格
	  this.getCartList();//获取购物车列表 商品数量
	  $('.addCar').css({
	  	'line-height':$('.addCar').height()+'px'
	  })
	  
	  var _this=this;
  	var scrollTop=$('.detail-box').height()-$('.detail-wrap').height();
    $('.detail-box').bind('touchend',function(){//拖动查看详情
    	if($('.detail-wrap').scrollTop()+0.2>=scrollTop){
  		  $('.show').hide();
      	var val=_this.productInfo.ProductContent;
      	$('.proContent').html(val);
	      $('.proContent').addClass('ql-editor ql-blank');
	      $('.proContent img').css({
	      	'width':'100%',
	      })
	      $('.proContent img').parent().css({
	      	'font-size':0
	      })
  	  }
    })
    
  }
}
</script>

<style scoped>
	@import '../../static/css/quill.bubble.css';
	@import '../../static/css/quill.core.css';
	@import '../../static/css/quill.snow.css';
	.shopdetail{background: #f3f3f3;}
  .pro-content{width: 100%;/*height:450px;*/background: #fff;font-size: 13px;color: #3E3E3E;}
  .pro-content p{padding-left: 8px;}
  .pro-img{width: 100%;/*height: 400px;*/background-size: 100% 100% !important;}
  .pro-img img{width: 100%;}
  .pro-name{margin-top: 5px;}
  .pro-inte,.pro-intec{color: #df0504;margin-right: 3px;}
  .pro-intec{color: #ccc;text-decoration:line-through;}
  .inteImg{width: 15px;position: relative;top: 3px;}
  .pro-box{border-top: 3px solid #f3f3f3 !important;font-size: 12px;color: #1d1c21;position:relative;padding-left: 8px;width: 100%;background: #fff;border: 1px solid #fff;box-sizing: border-box;}
  .pro-box p{margin-top: 10px;}
  .pro-num{position: absolute;top: 15px;right: 20px;}
  .pro-num span{color: #e83428;margin-right: 6px;}
  .pro-type{margin-top: 15px;}
  .shop-xg{float: right;margin-right: 16px;}
  .shop-xg span{color: #E83428;margin-right: 6px;}
  .demo-item {width: 45px;height: 25px;line-height:25px;text-align: center; background: #f4f4f4;margin-right: 20px;margin-top: 10px;}
  .demo-item-selected {background: #e93429;color: #fff;}
  .pro-detail{width: 100%;height:75px;background: #f3f3f3;color: #3E3E3E;font-size: 12px;text-align: center;line-height: 75px;}
  .proContent{background: #fff;height: auto;}
  /*底部*/
  .pro-foot{width: 100%;height: 7%;background: #fff;border-top:1px solid #ccc;box-sizing: border-box;}
  .foot-car{float: right;width: 80%;text-align: center;height: 100%;}
  .foot-car *{display: inline-block;vertical-align: middle;}
  .foot-car img{width: 20px;}
  .foot-car .carImg{width: 25%;}
  .foot-car .addCar{width: 35%;height:80%;border-left: 1px solid #ccc;font-size: 14px;color: #3E3E3E;margin-top: 1.5%;}
  .foot-car .swBtn{width: 35%;height: 100%;background: #e93429;color: #fff;border: none;outline: none;float: right;}
  .xnBtn{width: 35%;height: 100%;background: #e93429;color: #fff;border: none;outline: none;float: right;}
  /*未注册提示*/
  .mask{width:100%;height:100%;position: fixed;top: 0;background: rgba(0,0,0,0.5);z-index: 1000;}
  .mask div{background: #fff;width: 50%;height:105px;position: absolute;top: 200px;left:25%;text-align: center;}
  .mask p{font-size: 12px;color: #E83428;margin-top: 30px;}
  .mask button{width: 50%;height: 25px;background: #E83428;color: #fff;border: none;outline: none;font-size: 12px;margin-top: 20px;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
  .shopdetail .weui-cell:before{content: none !important;}
  .shopdetail .weui-cell{padding: 10px 0 20px 0 !important;}
  .shopdetail .vux-number-selector svg{fill: #ccc !important;}
  .shopdetail .vux-number-input{font-size: 15px !important;}
  .shopdetail .vux-badge{position: relative;top: -5px;left: -5px;}
  .shopdetail .xs-plugin-pullup-container{display: none;}
</style>