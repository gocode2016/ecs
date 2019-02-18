<template>
  <div class="shoporder">
    <!--收货地址-->
    <div class="user-address" v-if='addressFlag' @click="chooseAddress(2)">
      <p>{{addressInfo.FullName}}&nbsp;&nbsp;{{addressInfo.Mobile}}</p>
      <p><img src="../../static/credit/address.png"><span>{{addressInfo.ProvinceName}}{{addressInfo.CityName}}{{addressInfo.AreaName}}{{addressInfo.MemberAddress}}</span></p>
    </div>
    <div class="user-address" v-else @click="chooseAddress(1)">新增地址</div>
    
	  <div class="orderflex" v-for="(item,index) in orderlist">
			<div><img :src="item.imgurl"></div>
			<div>
			  <p>{{item.SkuName}}</p>
			  <!--<p class="order_p">
			  	<span class="money">{{item.Price}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span>
			  	<span class="pro_num">x{{item.Count}}</span>
			  </p>-->
			  <p style="text-align: right;margin-top: 5%;"><span class="money">{{item.oPrice}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span></p>
			  <p style="text-align: right;" v-show="item.SalePrice!=-1"><span class="moneys">{{item.Price}}&nbsp;<img src="../../static/credit/money_c.png" class="moneyImg"></span></p>
			  <p><span class="pro_num">x{{item.Count}}</span></p>
			</div>
    </div>
    <div class="foot">合计：<span>{{total}}</span><button @click.stop="submitOrder">提交订单</button></div>
    
    <!--订单成功页面-->
    <div class="success" v-show="successFlag">
    	<img src="../../static/credit/success.png" style="width: 30%;margin-top: 10%;"/>
    	<p style="color: #E83428;">订单提交成功!</p>
    	<img src="../../static/credit/scan_code.jpg" style="width: 50%;margin-top: 15%;"/>
    	<p>长按识别二维码关注平台，查看我的订单</p>
    	<!--<button @click="lookorder">查看订单</button>-->
    </div>
    
  </div>
</template>

<script>
import { Loading,cookie,Toast } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Loading,
		cookie,
		Toast
	},
	data(){
		return{
			skudata:'',//实物详情
			addressFlag:true,
			selectArr:'',//购物车
			userData:'',
			userId:'',
			goodsList:'',//购物车列表
			orderlist:[],//订单列表
			productId:'',
			total:'',//购物车
			addressInfo:{},
			addressId:null,
			orderId:'',
			awardTime:'',
			successFlag:false,//订单提交成功页面
			btnNum:0,
		}
	},
	methods:{
		getOrderList(){//获取订单列表
			var _this=this;
  		this.$http({
  			method:'post',
  			url:apiUrl.getCartList+'?memberId='+this.userId
  		}).then(function(response){
  			_this.goodsList=JSON.parse(response.data);
        for(var i=0;i<_this.selectArr.length;i++){
        	if(_this.selectArr[i]==1){
        		_this.orderlist.push(_this.goodsList[i]);
        	}
        }
        for(var i=0;i<_this.orderlist.length;i++){
        	var order=_this.orderlist[i];
        	
        	if(order.SalePrice!=-1){//活动商品
        		order.oPrice=order.SalePrice;
        	}else{//非活动商品
        		order.oPrice=order.Price;
        	}
        	
        	if(order.IsShow==0){//商品下架
        	  _this.orderlist.splice(i,1);
        		i--;
        	}else{
        		_this.productId=order.ProductId;
        	  _this.getProductDetail(i);
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
  			_this.orderlist[index].imgurl=data[0].ImgUrl;
  			_this.orderlist.splice(index,1,_this.orderlist[index]);
//			  console.log(_this.orderlist);
  		})
  	},
    getAddressList(){//收货地址列表
    	var _this=this;
    	if(this.addressId==0){
    		var params="?memberId="+this.userId;
    	}else{
    		var params="?memberId="+this.userId+'&AddressId='+this.addressId;
    	}
  		this.$http({
  			method:'post',
  			url:apiUrl.getAddressList+params,
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			if(data.length==0){
  				_this.addressFlag=false;
  			}else{
  				_this.addressFlag=true;
  				_this.addressInfo=data[0];
  			}
  		})
    },
    chooseAddress(isAddress){//1没有地址 需要添加地址  2有地址 进行选择
    	if(this.$route.query.source=='shopcar'){
    		this.$router.push({path:'/component/shopaddress',query:{isAddress:isAddress,selectArr:this.selectArr,total:this.total,source:'shopcar'}});
    	}else if(this.$route.query.source=='shopdetail'){
    		this.$router.push({path:'/component/shopaddress',query:{isAddress:isAddress,skudata:this.skudata,source:'shopdetail'}});
    	}else if(this.$route.query.source=='thanksdraw'){
    		this.$router.push({path:'/component/shopaddress',query:{isAddress:isAddress,skudata:this.skudata,source:'thanksdraw',orderId:this.orderId}});
    	}
    },
    submitOrder(){//提交订单
    	if(this.addressFlag==true&&this.addressInfo.ProvinceName!=''&&this.addressInfo.CityName!=''&&this.addressInfo.AreaName!=''&&this.addressInfo.MemberAddress!=''&&this.addressInfo.FullName!=''&&this.addressInfo.Mobile!=''){
    		this.btnNum++;
    		if(this.$route.query.source=='thanksdraw'&&this.btnNum==1){
    			this.getTime();
    			this.homeLuckyOrderSubmit();
    		}else if(this.$route.query.source=='shopcar'&&this.btnNum==1){
    			this.getMemberById();//获取用户积分
    		}else if(this.$route.query.source=='shopdetail'&&this.btnNum==1){
    			this.getMemberById();//获取用户积分
    		}
    	}else{//没有地址或地址不完整
    		this.$vux.toast.text('请完善地址再提交订单哦', 'middle');
    	}
    },
    getMemberById(){//获取用户积分
  		var _this=this;
  		var params={
  		  'MemberId':this.userId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getMemberById,
  			data:params
  		}).then(function(response){  	
  			var data=JSON.parse(response.data); 
			  if(data.length!=0){
			  	if(data[0].LeaveIntegral<_this.total){//用户积分小于商品所需积分
			  		_this.$vux.toast.text('积分不足', 'middle');
			  		_this.btnNum=0;
			  	}else{//结算
			  		_this.addOrder();//新增订单
			  	}
			  }
  		})
  	},
    addOrder(){//新增订单
    		this.showLoading ();
    		var _this=this;
				var OrderDetaile=[];//购物车商品
				for(var i=0;i<this.orderlist.length;i++){
					var orderlist=this.orderlist[i];
	  			var item={
	  				"OrderDetaileId": 0,
	          "OrderId": 0,
		        "SkuId": orderlist.SkuId,
		        "SkuName": orderlist.SkuName,
		        "ProductId":orderlist.ProductId,
		        "Count": orderlist.Count
				  }
	  			OrderDetaile.push(item);
				}
				var params={
			    "MemberId": this.userId,
			    "OrderAddress": this.addressInfo.ProvinceName+this.addressInfo.CityName+this.addressInfo.AreaName+this.addressInfo.MemberAddress,
			    "OrderName":this.addressInfo.FullName,
			    "OrderTelephone":this.addressInfo.Mobile,
			    "OrderState": "备货中",
			    "OrderPrice": this.total,
			    "OrderType":1,
			    "InventedType":0,
			    "OrderFrom":"积分商城",
			    "OrderRemark":"",
			    "OrderDetaile":OrderDetaile
				}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addOrder,
	  			data:params
	  		}).then(function(response){
//	  			_this.updateMemberIntegral();//支付 更改积分
	          if(_this.$route.query.source=='shopcar'){
	  					//删除商品
	  			    _this.deleteCart();
	  				}else if(_this.$route.query.source=='shopdetail'){
	  					_this.$vux.loading.hide();//loading图标消失
	  					_this.successFlag=true;
	  					_this.btnNum=0;
	  				}
	  		})
  	},
  	updateMemberIntegral(){//更改积分
  		var OrderRemark='';//订单备注 所有商品名称
			for(var i=0;i<this.orderlist.length;i++){
				var orderlist=this.orderlist[i];
				OrderRemark+=orderlist.SkuName+" ";
			}
  		var _this=this;
  		this.total=parseInt(this.total);
  		var params={
  		  'Operation':'minus',
				'Integral':this.total,
				'MemberId':this.userId,
				'IntegralSource':'积分商城',
				'Remark':OrderRemark
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
  			if(response.data=='Excute Success'){
  				if(_this.$route.query.source=='shopcar'){
  					//删除商品
  			    _this.deleteCart();
  				}else if(_this.$route.query.source=='shopdetail'){
  					_this.$vux.loading.hide();//loading图标消失
  					_this.successFlag=true;
  				}
  			}
  		})
  	},
  	deleteCart(){//删除购物车商品
  		var _this=this;
			var params=[];
			for (let i in this.orderlist){
				params.push(this.orderlist[i].CartId);
			}
			this.$http({
				method:'post',
				url:apiUrl.deleteCart,
				data:params
			}).then(function(response){
	      if(response.data=='Excute Success'){
	      	//显示订单提交成功页面
	      	_this.$vux.loading.hide();//loading图标消失
				  _this.successFlag=true;
				  _this.btnNum=0;
	      }
			})
		},
		homeLuckyOrderSubmit(){//回写订单
			var _this=this;
			var params={
		    "OrderId":this.orderId,
				"Address":this.addressInfo.ProvinceName+this.addressInfo.CityName+this.addressInfo.AreaName+this.addressInfo.MemberAddress,
				"State":'备货中',
				"OrderName":this.addressInfo.FullName,
				"Telephone":this.addressInfo.Mobile
		  }
			this.$http({
				method:'post',
				url:apiUrl.homeLuckyOrderSubmit,
				data:params
			}).then(function(response){
//				console.log(response.data);
        if(response.data==1&&_this.awardTime!=""){
        	_this.$router.push({path:'/component/thankscode'});
        }
			})
		},
		getTime(){//获取当前时间
	  		var myDate = new Date();
				var year=myDate.getFullYear();    //获取完整的年份(4位,1970-????)
				var month=myDate.getMonth()+1;       //获取当前月份(0-11,0代表1月)
				var date=myDate.getDate();        //获取当前日(1-31)
				var hour=myDate.getHours();       //获取当前小时数(0-23)
        var minute=myDate.getMinutes();     //获取当前分钟数(0-59)
        
        var month=time(month);
        var date=time(date);
        var hour=time(hour);
        var minute=time(minute);
        function time(a){
        	if(a<10){
        		a='0'+a;
        	}
        	return a;
        }
        this.awardTime=year+'-'+month+'-'+date+' '+hour+':'+minute;
	  },
		lookorder(){//点击查看订单
  		this.$router.push('/component/orderlist');
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
	  this.userId=parseInt(this.userData.UserId);
	  
	  if(this.$route.query.source=='shopdetail'){
	  	this.skudata=this.$route.query.skudata;
	  	this.orderlist.push(this.skudata);
	  	this.productId=this.orderlist[0].ProductId;
      this.getProductDetail(0);//获取商品图片salePrice
      if(this.skudata.SalePrice==-1){//非活动商品
      	this.skudata.oPrice=this.skudata.Price;
      	this.total=this.skudata.Count*this.skudata.Price;
      }else{//活动商品
      	this.skudata.oPrice=this.skudata.SalePrice;
      	this.total=this.skudata.Count*this.skudata.SalePrice;
      }
	  }else if(this.$route.query.source=='shopcar'){
	  	this.selectArr=this.$route.query.selectArr;
			this.total=this.$route.query.total;
			this.getOrderList();//获取订单列表
	  }else if(this.$route.query.source=='thanksdraw'){
	  	this.skudata=this.$route.query.skudata;
//	  	console.log(this.skudata.SkuName);
	  	if(this.skudata.SkuName == undefined){
	  		this.$router.push('/component/thanksdraw');
	  	}
	  	this.skudata.oPrice=0;//当前价格
	  	this.orderlist.push(this.skudata);
	  	this.orderId=this.$route.query.orderId;
	  	this.total=0;
	  }
	  
		this.addressId=this.$route.query.addressId;
		this.getAddressList();//收货地址列表
	}
}
</script>

<style scoped>
  .user-address{padding: 24px 16px 24px 14px;font-size: 14px;color: #3E3E3E;border-bottom: 3px solid #f4f4f4;letter-spacing: 1px;}
  .user-address p:nth-child(1){font-weight: bold;margin-left: 28px;}
  .user-address p:nth-child(2) img{width: 18px;margin-right: 10px;}
  .user-address p:nth-child(2):after{content: " ";display: inline-block;height: 8px;width: 8px;border-width: 2px 2px 0 0; border-color: #C8C8CD;
                                     border-style: solid;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0);position: absolute;right: 16px;}
  .orderflex{display: flex;padding: 17px 25px 15px 15px;border-bottom: 1.5px solid #F4F4F4;font-size: 12px;}
  .orderflex div:nth-child(1){width: 30%;height:92px;border: 1px solid #f1f1f1;}
  .orderflex div:nth-child(1) img {width: 100%;height: 100%;}
  .orderflex div:nth-child(2){width: 65%;margin-left: 5%;position: relative;}
  .order_p{margin-top: 30px;}
  .money{color: #de0607;}
  .moneys{color: #ccc;text-decoration: line-through;}
  .pro_num{color: #b6b6b6;position: absolute;right: 0;}
  .moneyImg{width: 15px;position: relative;top: 3px;}
  .foot{width:100%;height: 45px;line-height:45px;font-size:10px;background:#fff;border-top: 1px solid #F4F4F4;position: absolute;bottom: 0;text-align: right;}
  .foot span{color: #E83428;font-size: 14px;}
  .foot button{width: 105px;height: 100%;background: #E83428;color: #fff;border: none;margin-left: 15px;}
  .success{width: 100%;height: 100%;position: fixed;top: 0px;background: #fff;text-align: center;}
  /*.success img{width: 30%;margin-top: 50px;}*/
  /*.success p{color: #E83428;}*/
  .success button{width: 80%;height: 40px;background: #E83428;color: #fff;border: none;margin-top: 20%;border-radius: 5px;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
</style>