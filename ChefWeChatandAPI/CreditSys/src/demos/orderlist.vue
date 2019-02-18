<template>
  <div class="orderlist">

      <tab :line-width=2  v-model="index" custom-bar-width="60px">
        <tab-item class="vux-center" :badge-label="item.count" :selected="demo === item.text" v-for="(item, index) in list" @click="demo = item.text" :key="index">{{item.text}}</tab-item>
      </tab>  
      <div class="height_box"></div>
      <swiper v-model="index" height="20000px" :show-dots="false" :min-moving-distance = 100000>
      	
        <swiper-item  :key="1">
          <div class="tab-swiper vux-center" v-for="item in wtjList">
          	<div class="orderbox1">
          		<p class="odernum">
          			<span>订单编号：{{item.OrderId}}</span>
          			<span>生成时间：{{item.AddDate}}</span>
          		</p>
          		<div class="orderflex" v-for="(item,index) in item.data">
          			<div><img :src="item.ImgUrl"></div>
          			<div>
          				<p>{{item.SkuName}}</p>
          				<p class="order_p">
          					<span class="money">{{item.Price}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span>
          					<span class="pro_num">x{{item.Count}}</span>
          				</p>
          			</div>
          		</div>
          	</div>
          	<div class="orderbox2">
          		<span>合计：{{item.OrderPrice}}</span> <img src="../../static/credit/money.png" class="moneyImg"/>
          		<button @click="gosubmit(item.data)">去提交</button>
          	</div>
          </div>          
        </swiper-item>
        
        <swiper-item  :key="2">
          <div class="tab-swiper vux-center" v-for="item in bhzList">
          	<div class="orderbox1">
          		<p class="odernum">
          			<span>订单编号：{{item.OrderId}}</span>
          			<span>生成时间：{{item.AddDate}}</span>
          		</p>
          		<div class="orderflex" v-for="(item,index) in item.data">
          			<div><img :src="item.ImgUrl"></div>
          			<div>
          				<p>{{item.SkuName}}</p>
          				<p class="order_p">
          					<span class="money">{{item.Price}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span>
          					<span class="pro_num">x{{item.Count}}</span>
          				</p>
          			</div>
          		</div>
          	</div>
          	<div class="orderbox2">
          		<span>合计：{{item.OrderPrice}}</span> <img src="../../static/credit/money.png" class="moneyImg"/>
          	</div>
          </div>          
        </swiper-item>
        
        <swiper-item  :key="3">
          <div class="tab-swiper vux-center" v-for="item in yfhList">
          	<div class="orderbox1">
          		<p class="odernum">
          			<span>订单编号：{{item.OrderId}}</span>
          			<span>生成时间：{{item.AddDate}}</span>
          		</p>
          		<div class="orderflex" v-for="(item,index) in item.data">
          			<div><img :src="item.ImgUrl"></div>
          			<div>
          				<p>{{item.SkuName}}</p>
          				<p class="order_p">
          					<span class="money">{{item.Price}}&nbsp;<img src="../../static/credit/money.png" class="moneyImg"></span>
          					<span class="pro_num">x{{item.Count}}</span>
          				</p>
          			</div>
          		</div>
          	</div>
          	<div class="orderbox2">
          		<span>合计：{{item.OrderPrice}}</span> <img src="../../static/credit/money.png" class="moneyImg"/>
          		<button @click="logistics(item.LogisticsNo,item.LogisticsType)" v-show='item.OrderType==1'>查看物流</button>
          	</div>
          </div>          
        </swiper-item>
        
      </swiper>

  </div>
</template>

<script>
import {Tab, TabItem, Swiper, SwiperItem,cookie,Badge} from 'vux'
import apiUrl from '../apiUrls.js'
export default{
	components:{
		Tab,
		TabItem, 
		Swiper, 
		SwiperItem,
		cookie,
		Badge
	},
	data(){
		return{
			index:0,
			demo:'求助中',
			list:[],
			userData:{},
			userId:'',
			wtjList:[],//未提交列表
			bhzList:[],//备货中列表
			yfhList:[],//已发货列表
			skuName:'',
			skuId:'',
			orderId:'',
			ImgUrl:'',
			skuData:{}
		}
	},
	methods:{
		gosubmit(arr){//去提交
			if(arr[0].OrderType==1){//实物  感恩节抽奖
				this.skuData={
					Count:1,
					SkuName:arr[0].SkuName,
					imgurl:arr[0].ImgUrl,
					Price:arr[0].Price
				}
				this.$router.push({path:'/component/shoporder',query:{skudata:this.skuData,source:'thanksdraw',addressId:0,orderId:arr[0].OrderId}})
			}else if(arr[0].OrderType==2){//虚拟 积分抽奖
				this.skuName=arr[0].SkuName;
				this.skuId=arr[0].SkuId;
				this.orderId=arr[0].OrderId;
			  this.ImgUrl=arr[0].ImgUrl;
				this.$router.push({path:'/component/receiveprice',query:{skuName:this.skuName,skuId:this.skuId,orderId:this.orderId,ImgUrl:this.ImgUrl,source:'orderlist'}});
			}
		},
		logistics(LogisticsNo,LogisticsType){//查看物流
			this.$router.push({path:'/component/logistics',query:{LogisticsNo:LogisticsNo,LogisticsType:LogisticsType}});
		},
		getMemberOrderList(){//订单列表
			var _this=this;
			var params='?memberId='+this.userId;
  		this.$http({
  			method:'get',
  			url:apiUrl.getMemberOrderList+params,
  		}).then(function(response){
  			var arr=JSON.parse(response.data);
				var map = {},
				    dest = [];
				for(var i = 0; i < arr.length; i++){
				    var ai = arr[i];
				    if(!map[ai.OrderId]){
				        dest.push({
				            OrderId: ai.OrderId,
				            AddDate: ai.AddDate,
				            OrderPrice: ai.OrderPrice,
				            OrderState:ai.OrderState,
				            OrderType:ai.OrderType,
				            LogisticsNo:ai.LogisticsNo,
				            LogisticsType:ai.LogisticsType,
				            data: [ai]
				        });
				        map[ai.OrderId] = ai;
				    }else{
				        for(var j = 0; j < dest.length; j++){
				            var dj = dest[j];
				            if(dj.OrderId == ai.OrderId){
				                dj.data.push(ai);
				                break;
				            }
				        }
				    }
				}
//				console.log(dest);
				for(let i in dest){
					dest[i].AddDate=dest[i].AddDate.substring(0,19);
					dest[i].AddDate=dest[i].AddDate.replace(/-/g,'/');
					dest[i].AddDate=dest[i].AddDate.replace('T',' ');
					if(dest[i].OrderState=='未提交'){
						_this.wtjList.push(dest[i]);
					}else if(dest[i].OrderState=='备货中'){
						_this.bhzList.push(dest[i]);
					}else if(dest[i].OrderState=='已发货'){
						_this.yfhList.push(dest[i]);
					}
				}
        _this.list=[{
        	 text:'未提交',
        	 count:(_this.wtjList.length).toString()
        },{
        	 text:'备货中',
        	 count:(_this.bhzList.length).toString()
        },{
        	 text:'已发货',
        	 count:(_this.yfhList.length).toString()
        }]
        
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
	  this.getMemberOrderList();//订单列表
	}
}
</script>

<style scoped>
  .vux-center{width: 100%;background: #fff;color: #3E3E3E;font-size: 12px;border-bottom: 5px solid #f4f4f4;}
  .orderbox2{height: 40px;line-height: 40px;padding-left: 4%;padding-right: 3%;position: relative;}
  .odernum{font-size: 11px;padding-top: 15px;padding-left: 4%;padding-right: 3%;}
  .odernum span:nth-child(2){float: right;}
  .orderflex{display: flex;padding-top: 15px;padding-bottom: 15px; border-bottom: 2px solid #f4f4f4;padding-left: 4%;padding-right: 3%;}
  .orderflex div:nth-child(1){width: 30%;height:100px;border: 1px solid #f1f1f1;}
  .orderflex div:nth-child(1) img {width: 100%;height: 100%;}
  .orderflex div:nth-child(2){width: 60%;margin-left: 5%;position: relative;}
  .order_p{width:100%;position: absolute;bottom: 10px;}
  .money{color: #de0607;}
  .pro_num{color: #b6b6b6;position: absolute;right: 0;}
  .orderbox2 span{color: #de0607;}
  .moneyImg{width: 15px;position: relative;top: 3px;}
  .orderbox2 button{font-size:12px;width: 18.6%;height: 25px;background: #fff;color: #e43a3a;border-radius: 3px; border: 1px solid #e83428;position: absolute;right: 0;margin-right: 3%;margin-top: 2%;}
  .height_box{height: 44px;}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.orderlist .vux-tab .vux-tab-item{position: relative !important;}
	.orderlist .vux-tab{position: fixed !important;top:0px !important;z-index: 100 !important;width: 100% !important;}
	.orderlist .vux-tab-item-badge {background: #df4337 !important;top: -10px !important;border-radius: 25px !important;}
	.vux-tab-bar-inner{background-color:#e81d23 !important;}
  .vux-tab .vux-tab-item.vux-tab-selected{color: #e81d23 !important;}
</style>