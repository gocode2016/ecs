<template>
  <div class="shopcar">
    <div  class="vux-1px-t" v-for="(item,index) in goodsList" :key="index">
      <swipeout>
        <swipeout-item :threshold=".5" underlay-color="#ccc">
          <div slot="right-menu">
            <swipeout-button @click.native="onButtonClick(index,item.CartId)" background-color="#D23934">{{$t('Delete')}}</swipeout-button>
          </div>
          <div slot="content" class="demo-content vux-1px-tb">
              <div class="box1">
              	<span v-show='item.IsShow==0'>失效</span>
                <a v-show='item.IsShow==1' href="javascript:void 0" class="item-check-btn"  @click="checkProduct(item,index)"></a> 
              </div>
              <div class="box2">
                  <img :src="item.imgurl" alt="">
              </div>
              <div class="box3">
                  <p v-html="item.SkuName"></p>
                  <p>
                  	<span v-html="item.oPrice"></span><img src="../../static/credit/money.png" alt="">
                    <span v-show="item.SalePrice!=-1" v-html="item.Price" style="color: #ccc;text-decoration: line-through;"></span><img v-show="item.SalePrice!=-1" src="../../static/credit/money_c.png" alt="">
                  </p>
                  <div class="XNumber">
                  	<span @click="reduce(index,item.CartId)">-</span><span>{{item.Count}}</span><span @click="add(index,item.CartId)">+</span>
                  </div>
                  <p v-show="item.isOutside==1" class="outside">注:该商品限购{{item.Limit}}件</p>
              </div>
          </div>
        </swipeout-item> 
      </swipeout>      
    </div>
    
    <div class="footers"></div>
    <div slot="content" class="demo-content" id="footer"> 
      <div class="f-box1">
          <a href="javascript:void 0">
            <span class="item-check-btn" @click="checkAllProduct()"></span>
          </a> 
      </div>
      <div class="f-box2">全选
      	<img src="../../static/credit/set_icon.png">
      	<span>{{total}}</span>
      </div>
      <div class="f-box3" >
      	<span>部分商品超出限购数量</span>
      </div>
      <div class="f-box4" @click="compute">结算(<span>{{settlement}}</span>)</div>
    </div>
    
  </div>
</template>

<script>

import { GroupTitle, Swipeout, SwipeoutItem, SwipeoutButton, XButton, XNumber,CheckIcon,cookie,Toast} from 'vux'
import apiUrl from '../apiUrls.js'
var goodsList=[];
export default {
  components: {
    GroupTitle,
    Swipeout,
    SwipeoutItem,
    SwipeoutButton,
    XButton,
    XNumber,
    CheckIcon,
    cookie,
    Toast
  },
  data () {
    return {
    	userData:{},
    	userId:'',
    	goodsList:[],//商品信息
    	productId:0,
      checkAllFlag:true,
      selectArr : [],//单个商品选中 未选中 状态
      allselect : 0,//0全选 未选中
      cartId:'',
      stock:'',
      total:0,//合计
      settlement:0,//结算
      isJumpHotel:''//是否跳转酒店信息 1跳转 0不跳转
    }
  },
  created:function(){
    
  },
  mounted:function(){
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
	  
	  this.getCartList();//获取购物车列表
	  this.getMemberProfile();//获取简历信息 
  },
  methods: {
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
  	reduce(index,CartId){//减
  		if(this.goodsList[index].Count<=1){//数量最小为1
  			this.goodsList[index].Count=1;
  		}else{
  			this.goodsList[index].Count--;
	  		this.cartId=CartId;
	  		this.updateCart(this.goodsList[index].Count);//更新商品数量
	  		if(this.selectArr[index]==1||this.selectArr[index]==2){//选中
	    		if(this.goodsList[index].IsShow==1){//上架
	    			this.total-=this.goodsList[index].oPrice;
  			  }
    	  }
  		}
  	},
  	add(index,CartId){//加
//		    this.goodsList[index].Count++;
//	  		this.cartId=CartId;
//	  		this.updateCart(this.goodsList[index].Count);//更新商品数量
//	  		if(this.selectArr[index]==1){//选中
//	  			if(this.goodsList[index].IsShow==1){//上架
//	  				this.total+=this.goodsList[index].Price;
//	  			}
//	    	}
  		
			if(this.goodsList[index].SalePrice!=-1&&this.goodsList[index].Limit!=0){//活动商品需判断限购
				if(this.goodsList[index].Count>=this.goodsList[index].Limit){
				  this.$vux.toast.text('此商品限购'+this.goodsList[index].Limit+'件', 'middle');
				}else{
					  this.goodsList[index].Count++;
			  		this.cartId=CartId;
			  		this.updateCart(this.goodsList[index].Count);//更新商品数量
			  		if(this.selectArr[index]==1||this.selectArr[index]==2){//选中
			  			if(this.goodsList[index].IsShow==1){//上架
			  				this.total+=this.goodsList[index].oPrice;
			  			}
			    	}
				}
			}else{
				  this.goodsList[index].Count++;
		  		this.cartId=CartId;
		  		this.updateCart(this.goodsList[index].Count);//更新商品数量
		  		if(this.selectArr[index]==1||this.selectArr[index]==2){//选中
		  			if(this.goodsList[index].IsShow==1){//上架
		  				this.total+=this.goodsList[index].oPrice;
		  			}
		    	}
			}
  		
  	},
  	compute(){//点击结算按钮
//			if(this.settlement!=0){
//				this.$router.push({path:'/component/shoporder',query:{selectArr:this.selectArr,total:this.total,addressId:0,source:'shopcar'}});
//			}else{
//				this.$vux.toast.text('请选择商品', 'middle');
//			}

      var _this=this;
      if(this.settlement!=0){
      	for(let i=0;i<this.goodsList.length;i++){//判断限购
      		var goodsList=this.goodsList[i];
      		if(this.selectArr[i]==1||this.selectArr[i]==2){//商品为选中状态
      			if(goodsList.SalePrice!=-1&&goodsList.Limit!=0){//活动商品
	      			this.checkMemberSale(goodsList.SkuId,goodsList.Count,goodsList.Limit,i);
	      		}
      		}
      	}
      	setTimeout(function(){
      		var select=_this.selectArr.join('');
      		if(select.indexOf('2')<0){//全部可以买
      			$('.f-box3 span').css('visibility','hidden');
				    if(_this.isJumpHotel == 0){//不需要完善酒店信息
				   		_this.$router.push({path:'/component/shoporder',query:{selectArr:_this.selectArr,total:_this.total,addressId:0,source:'shopcar'}});
				    }else if(_this.isJumpHotel == 1){//跳转到酒店信息
				   		_this.$router.push({path:'/component/resumehotel',query:{selectArr:_this.selectArr,total:_this.total,addressId:0,source:'shopcar'}});
				    }
      		}else{//部分不能买
      			for(var i in _this.selectArr){
      				if(_this.selectArr[i]==2){
      					_this.goodsList[i].isOutside=1;
      					_this.goodsList.splice(i,1,_this.goodsList[i]);
      				}else{
      					_this.goodsList[i].isOutside=0;
      					_this.goodsList.splice(i,1,_this.goodsList[i]);
      				}
      			}
      			$('.f-box3 span').css('visibility','visible');
      		}
      	},500)
			}else{
				this.$vux.toast.text('请选择商品', 'middle');
			}

  	},
  	checkMemberSale(SkuId,Count,Limit,index){//商品限购
  		var _this=this;
  		var params={
  		  "MemberId":this.userId,
		    "SkuId":SkuId
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
				if(Count<=Limit-data){//可以买
					_this.selectArr[index]=1;
				}else{//不能买
					_this.selectArr[index]=2;
					_this.prompt+=_this.goodsList[index].SkuName+'限购'+Limit+'件;'
				}
  		})
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
  	getCartList(){//获取购物车列表
  		var _this=this;
  		this.$http({
  			method:'post',
  			url:apiUrl.getCartList+'?memberId='+this.userId
  		}).then(function(response){
  			_this.goodsList=JSON.parse(response.data);
  			for(var i=0;i<_this.goodsList.length;i++){
  				var good=_this.goodsList[i];
  				_this.productId=good.ProductId;
  				_this.getProductDetail(i);
  				if(good.IsShow==0){//商品是否下架  0下架  1未下架   下架则默认选中状态
  					_this.selectArr.push(1);//商品选中 未选中 状态
  				}else if(good.IsShow==1){
  					_this.selectArr.push(0);//商品选中 未选中 状态
  				}
  				
					if(good.SalePrice!=-1){//商品有活动价格
						good.oPrice=good.SalePrice;//活动价展示在前面
					}else{
						good.oPrice=good.Price;//原价展示在前面
					}
  				
  			}
  		})
  	},
  	getProductDetail(index){//获取商品详情 商品图片
  		var _this=this;
  		var params='?productId='+this.productId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getProductDetail+params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.goodsList[index].imgurl=data[0].ImgUrl;
  			_this.goodsList.splice(index,1,_this.goodsList[index]);
  		})
  	},
  	onButtonClick (index,cartId) {//删除按钮
    	this.cartId=cartId;
    	if(this.selectArr[index]==1||this.selectArr[index]==2){//选中 更改总价
    		if(this.goodsList[index].IsShow==1){//上架商品
    			var total=this.goodsList[index].oPrice*this.goodsList[index].Count;
    		  this.total-=total;
    		  this.settlement-=1;
    		}
    	}
    	this.goodsList.splice(index,1);
    	this.selectArr.splice(index,1);
    	this.deleteCart();//删除商品
			for(var i=0;i<this.selectArr.length;i++){
    		if(this.selectArr[i]==0){
    			$('.box1 .item-check-btn').eq(i).removeClass('check');
    		}else if(this.selectArr[i]==1){
    			$('.box1 .item-check-btn').eq(i).addClass('check');
    		}
    	}
    	var select=this.selectArr.join('|');
	    if(select.indexOf('0')==-1&&select!=''){//全部选中
	     	this.allselect=1;
	     	$('.f-box1 .item-check-btn').addClass('check');
	    }else{
	     	this.allselect=0;
	     	$('.f-box1 .item-check-btn').removeClass('check');
	    }
    },
		deleteCart(){//删除购物车商品
			var params=[];
			params.push(this.cartId);
			this.$http({
				method:'post',
				url:apiUrl.deleteCart,
				data:params
			}).then(function(response){
//				console.log(response.data);
			})
		},
		updateCart(count){//更新购物车数量
			var params={
			  'Count':count,
			  'CartId':this.cartId
			}
			this.$http({
				method:'post',
				url:apiUrl.updateCart,
				data:params
			}).then(function(response){
//				console.log(response.data);
			})
		},
    
    checkProduct(item,index){//选中单个商品
          if(this.selectArr[index]==0){//未选中
          	this.selectArr[index]=1;//选中
          	$('.box1 .item-check-btn').eq(index).addClass('check');
          	this.settlement+=1;
          	this.total+=this.goodsList[index].oPrice*this.goodsList[index].Count;
          }else{//选中
          	this.selectArr[index]=0;//未选中
          	$('.box1 .item-check-btn').eq(index).removeClass('check');
          	this.settlement-=1;
          	this.total-=this.goodsList[index].oPrice*this.goodsList[index].Count;
          }
          
         var select=this.selectArr.join('|');
         if(select.indexOf('0')==-1){//全部选中
         	 this.allselect=1;
         	 $('.f-box1 .item-check-btn').addClass('check');
         }else{
         	 this.allselect=0;
         	 $('.f-box1 .item-check-btn').removeClass('check');
         }
    },
    // 点击全选
     checkAllProduct(){
        if(this.allselect==1){//选中
        	this.allselect=0;
        	$('.f-box1 .item-check-btn').removeClass('check');//未选中
        	this.settlement=0;
        	this.total=0;
        	for(let i in this.goodsList){
        		var good=this.goodsList[i];
        		if(good.IsShow==0){//商品下架
        			this.selectArr[i]=1;
        		}else{
        			$('.box1 .item-check-btn').eq(i).removeClass('check');
        			this.selectArr[i]=0;
        		}
        	}
        }else{//未选中
        	this.allselect=1;
        	$('.f-box1 .item-check-btn').addClass('check');//选中
        	for(var i=0;i<this.selectArr.length;i++){
        		$('.box1 .item-check-btn').eq(i).addClass('check');
        		this.selectArr[i]=1;
        	}
        	var total=0;
          for(var i=0;i<this.goodsList.length;i++){
          	var goodlist=this.goodsList[i];
          	if(goodlist.IsShow==1){//0为下架 不计算价格
          		total+=goodlist.oPrice*goodlist.Count;
          		this.settlement++;
          	}
          }
          this.total=total;
        }

    }
  }  
}

</script>

<style lang="less">
	.shopcar{letter-spacing: 1px;}
	.demo-content {padding:0px;}
	.vux-1px-tb{height: 120px;display: flex;}
	/*这是商品列表下面的边框，可以修改left改变长度*/
	.vux-1px-tb:before{border-color: transparent;}
	.vux-1px-tb:after{left: 12%;}
	.vux-1px-tb:nth-child(1){border: none !important;}
	.box1{height: 100%;width: 13%;display: flex;align-items:center;justify-content:center;color: #a5a5a5;}
	.box2{width: 24.5%;height: 100%;text-align: center;display: flex; align-items:center;}
	.box2 img{ width: 90px;height: 90px;border: 1px solid #f8f8f8;display: inline-block;}
	.box3{height: 100%;width: 62%;padding-left: 10px;position: relative;}
	.box3 p:nth-child(1){margin-top: 12px;font-size: 12px;color: #3e3e3e;line-height: 16px;}
	.box3 p:nth-child(2){position: relative;top: 40%;}
	.box3 p:nth-child(2) *{display: inline-block;vertical-align:middle;}
	.box3 p:nth-child(2) span{font-size: 12px;color: red;}
	.box3 p:nth-child(2) img{width: 7%;margin-left: 2px;}
	.outside{font-size: 10px;color: #E83428;position: absolute;bottom: 0;right: 10px;}
	@media screen and (max-width:350px ){
		.box3 p:nth-child(2) span{font-size: 10px;}
	}
	.shopcar .XNumber{position: absolute;width: 40%;bottom: 14px;right: 10px;border: 1px solid #ccc;text-align: center;}
	.shopcar .weui-cell{padding: 0!important;}
	/*数量按钮样式*/
	.XNumber span{display: inline-block;width: 30%;height: 30px;line-height: 30px;}
	.XNumber span:nth-child(2){border-left: 1px solid #ccc;border-right: 1px solid #ccc;}
	/*结算部分*/
	.footers{height: 55px;}
	#footer{height: 55px;width: 100%;border-top: 1px solid #e3e3e3;position: fixed;bottom: 0;left: 0;z-index: 10;background: #ffffff;display: flex;}
	.f-box1{height: 100%;width: 10%;display: flex;align-items:center;justify-content:center;} 
	.f-box2{width: 30%;line-height: 55px;text-align: left;font-size: 14px; color: #3e3e3e;}
	.f-box3{width: 35%; height: 100%;line-height: 55px;}
	.f-box3 span{position: relative;top: -2px;font-size: 12px;visibility:hidden}
	.f-box2 img{width: 12px;position: relative;top: 3px;}
	.f-box2 span,.f-box3 span{color: #e7322b;}
	@media screen and (max-device-width:360px){/*屏幕<=360*/
		.f-box2{font-size: 12px;}
		.f-box2 img{width: 10px;}
		.f-box3 span{font-size: 10px;}
	}
	.f-box4{width: 25%;height: 100%;background: #e93429;color: #fff;text-align: center;font-size: 15px;font-weight: bold;line-height: 55px;}
	/*选中样式*/
	.item-check-btn {position: relative;top: -2px;display: inline-block;width: 16px;height: 16px;box-sizing:border-box;border: 1px solid #a5a5a5;border-radius: 50%;text-align: center;vertical-align: middle;}
	.check {background: url(../../static/credit/xz.png) no-repeat;background-size:100% 100%;border:none !important;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.shopcar .vux-number-selector svg{fill: #ccc !important;}
	.shopcar .vux-1px-tb:after{bottom: auto !important;}
	.shopcar .vux-1px-tb:after{border-bottom: 0;}
	.shopcar .vux-swipeout{border-bottom: 1px solid #e3e3e3;}
</style>