<template>
  <div class="inventedorder">
    
    <div class="invent-top">
      <p>注意：本订单将直接充值到账或发送卡号卡密至此手机</p>
      <input type="text" placeholder="请输入手机号" v-model="mobile"/>
    </div>
	  <div class="orderflex" v-for="(item,index) in 1">
		  <div><img :src="ImgUrl"></div>
			<div>
			  <p>{{skudata.SkuName}}</p>
			  <p class="order_p"><span class="money">{{skudata.Price}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span></p>
			  <p class="pro_num">x{{count}}</p>
			</div>
    </div>
    <div class="foot">合计：<span>{{total}}</span><button @click="submitOrder">提交订单</button></div>
    <!--订单成功页面-->
    <div class="success" v-show="successFlag">
    	<img src="../../static/credit/success.png"/>
    	<p>订单提交成功!</p>
    	<button @click="lookorder">查看订单</button>
    </div>
    
  </div>
</template>

<script>
import {  Loading,Toast,cookie } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Loading,
		Toast,
		cookie
	},
	data(){
		return{
			userData:{},
			userId:'',
			skudata:{},//商品信息
			count:'',//数量
			productId:'',
			total:'',//合计
			ImgUrl:'',//商品图片
			mobile:'',//电话
			MemberName:'',//姓名
			orderId:'',
			successFlag:false,//订单提交成功页面
			btnNum:0
		}
	},
	methods:{
		getProductDetail(){//获取商品详情 商品图片
  		var _this=this;
  		var params='?productId='+this.productId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getProductDetail+params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.ImgUrl=data[0].ImgUrl;
  		})
  	},
  	submitOrder(){//点击 提交订单
  		var box=/^1[3|4|5|7|8]\d{9}$/;
 	    if(!box.test(this.mobile)){//判断手机号
 	    	this.$vux.toast.text('请输入正确的手机号码', 'middle');
 	    }else{
 	    	this.btnNum++;
 	    	if(this.btnNum==1){
 	    		this.getMemberById();//获取用户积分
 	    	}
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
//			console.log(data);
			  if(data.length!=0){
			  	if(data[0].LeaveIntegral<_this.total){//用户积分小于商品所需积分
			  		_this.$vux.toast.text('积分不足', 'middle');
			  		_this.btnNum=0;
			  	}else{//提交订单
            _this.MemberName=data[0].MemberName;
            if(_this.MemberName == null){
            	_this.MemberName = "";
            }
            _this.addOrder();//新增订单
			  	}
			  }
  		})
  	},
  	addOrder(){//新增订单
  		  this.showLoading ();
    		var _this=this;
				var OrderDetaile=[];//提交的商品
	  		var item={
	  				"OrderDetaileId": 0,
	          "OrderId": 0,
		        "SkuId": this.skudata.SkuId,
		        "SkuName": this.skudata.SkuName,
		        "ProductId":this.skudata.ProductId,
		        "Count": this.count
				}
	  		OrderDetaile.push(item);
				var params={
			    "MemberId": this.userId,
			    "OrderAddress": '',
			    "OrderName":this.MemberName,
			    "OrderTelephone":this.mobile,
			    "OrderState": "未提交",
			    "OrderPrice": this.total,
			    "OrderType":2,
			    "InventedType":1,
			    "OrderFrom":"积分商城",
			    "OrderRemark":"",
			    "OrderDetaile":OrderDetaile
				}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addOrder,
	  			data:params
	  		}).then(function(response){
	  			if(typeof(JSON.parse(response.data))!="number"){
	  				_this.$vux.toast.text('订单提交失败，请稍后再试', 'middle');
	  				_this.$vux.loading.hide();//loading图标消失
			  		_this.btnNum=0;
	  			}else{
	  				_this.orderId=JSON.parse(response.data);
//	  			  _this.updateMemberIntegral();//支付 更改积分
            _this.updateOrderState();//更改订单状态
	  			}
	  		}).catch(function(response){
	  			_this.$vux.loading.hide();//loading图标消失
	  			_this.$vux.toast.text('订单提交失败，请稍后再试', 'middle');
			  	_this.btnNum=0;
	  		})
  	},
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		this.total=parseInt(this.total);
  		var params={
  		  'Operation':'minus',
				'Integral':this.total,
				'MemberId':this.userId,
				'IntegralSource':'积分商城',
				'Remark':this.skudata.SkuName
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
  			if(response.data=='Excute Success'){
  				_this.updateOrderState();//更改订单状态
  			}
  		})
  	},
  	updateOrderState(){//更改订单状态
  		var _this=this;
  		var params={
  		  'OrderId':this.orderId,
        'State':'已发货'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateOrderState,
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
  	lookorder(){//点击查看订单
  		this.$router.push('/component/orderlist');
  	},
  	onChange (val) {//toast组件 提示
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
		this.skudata=this.$route.query.skudata;//商品信息
		this.count=this.$route.query.count;//商品数量
		this.productId=this.skudata.ProductId;//商品id
		this.total=this.count*this.skudata.Price;//合计
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
		this.getProductDetail();//获取商品图片
	}
}
</script>

<style scoped>
  .invent-top{width: 100%;padding: 24px 16px 24px 14px;border-bottom: 1px solid #f4f4f4;}
  .invent-top p{font-size: 12px;color: #E83428;}
  .invent-top input{width: 80%;height: 45px;border-radius: 5px;border: 1px solid #ccc;margin-top: 15px;outline: none;text-indent: 1em;}
  .orderflex{display: flex;padding: 17px 25px 15px 15px;border-bottom: 1.5px solid #F4F4F4;font-size: 12px;}
  .orderflex div:nth-child(1){width: 30%;height:92px;border: 1px solid #f1f1f1;}
  .orderflex div:nth-child(1) img {width: 100%;height: 100%;}
  .orderflex div:nth-child(2){width: 65%;margin-left: 5%;position: relative;}
  .order_p{margin-top: 10px;}
  .money{color: #de0607;position:absolute;right: 0;}
  .pro_num{text-align: right;color: #b6b6b6;margin-top: 50px;}
  .moneyImg{width: 15px;position: relative;top: 3px;}
  .foot{width:100%;height: 45px;line-height:45px;font-size:10px;background:#fff;border-top: 1px solid #F4F4F4;position: absolute;bottom: 0;text-align: right;}
  .foot span{color: #E83428;font-size: 14px;}
  .foot button{width: 105px;height: 100%;background: #E83428;color: #fff;border: none;margin-left: 15px;}
  .success{width: 100%;height: 100%;position: fixed;top: 0px;background: #fff;text-align: center;}
  .success img{width: 30%;margin-top: 50px;}
  .success p{color: #E83428;}
  .success button{width: 80%;height: 40px;background: #E83428;color: #fff;border: none;margin-top: 200px;border-radius: 5px;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
</style>