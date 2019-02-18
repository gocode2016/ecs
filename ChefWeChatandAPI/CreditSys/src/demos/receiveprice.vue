<template>
  <div class="receiveprice">
    
    <div class="invent-top">
      <p>注意：本订单将直接充值到账或发送卡号卡密至此手机</p>
      <input type="text" placeholder="请输入手机号" v-model="mobile"/>
    </div>
	  <div class="orderflex">
		  <div><img :src="img"></div>
			<div>{{SkuName}}</div>
    </div>
    <div class="foot">合计：<span>0</span><button @click="submitOrder">提交订单</button></div>
    
  </div>
</template>

<script>
	import { Toast,Loading,cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
	
	export default{
		components:{
			Toast,
			Loading,
			cookie
		},
		data(){
			return{
				img:'',
				userData:{},
				userId:'',
				prize:'',//奖项
				mobile:'',//电话
				MemberName:'',//姓名
				orderId:'',
				SkuId:'',
				SkuName:'',//中奖名称
				SkuId:'',
				awardTime:'',//领奖时间
				orderId:''
			}
		},
		methods:{
			submitOrder(){//点击 提交订单
	  		var box=/^1[3|4|5|7|8]\d{9}$/;
	 	    if(!box.test(this.mobile)){//判断手机号
	 	    	this.$vux.toast.text('请输入正确的手机号码', 'middle');
	 	    }else{
	 	    	this.getTime();
	 	    	this.luckyOrderSubmit();//新增订单
	 	    }
	  	},
  	  luckyOrderSubmit(){//提交订单
  	  	var _this=this;
  	  	var params={
  		    "OrderId":this.orderId,
					"State":'已发货',
					"Telephone":this.mobile,
					"skuId":this.SkuId
	  		}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.luckyOrderSubmit,
	  			data:params
	  		}).then(function(response){
          if(response.data=='Excute Success'){
          	_this.$router.push({path:'/component/submitorder',query:{SkuName:_this.SkuName,awardTime:_this.awardTime,source:'intedraw'}})
  			  }else{
  			  	_this.$vux.toast.text('订单提交失败，请稍后再试', 'middle');
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
			if(this.$route.query.source=='integraldraw'){//抽奖页面
				this.prize=this.$route.query.prize;
				this.orderId=this.$route.query.orderId;
				if(this.prize=='一等奖'){
					this.SkuName='200元话费';
					this.SkuId=1030;
					this.img='../../static/credit/eby.png'
				}else if(this.prize=='二等奖'){
					this.SkuName='50元话费';
					this.SkuId=1004;
					this.img='../../static/credit/wsy.png'
				}else if(this.prize=='三等奖'){
					this.SkuName='10元话费';
					this.SkuId=1006;
					this.img='../../static/credit/sy.png'
				}else if(this.prize=='幸运奖'){
					this.SkuName='5元话费';
					this.SkuId=1031;
					this.img='../../static/credit/wy.png'
				}
			}else if(this.$route.query.source=='orderlist'){//我的订单
				this.SkuName=this.$route.query.skuName;
				this.SkuId=this.$route.query.skuId;
				this.orderId=this.$route.query.orderId;
				this.img=this.$route.query.ImgUrl;
			}
			
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
  .orderflex div:nth-child(2){width: 65%;margin-left: 5%;position: relative;height:92px;line-height: 92px;color: #3e3e3e;font-size: 14px;}
  .foot{width:100%;height: 45px;line-height:45px;font-size:10px;background:#fff;border-top: 1px solid #F4F4F4;position: absolute;bottom: 0;text-align: right;}
  .foot span{color: #E83428;font-size: 14px;}
  .foot button{width: 105px;height: 100%;background: #E83428;color: #fff;border: none;margin-left: 15px;}
</style>
<style>
  #vux_view_box_body{background: #fff;}
</style>