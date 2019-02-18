<template>
  <div class="integraldraw">
    
    <div class="wrap">
    	<div class="topImg">
    		<img src="../../static/credit/inteback1.png"/>
    	</div>
    	<div class="intezp">
    		<img src="../../static/credit/intezp.png" class="intezpImg"/>
    		<img src="../../static/credit/intebtn.png" class="intebtnImg" @click="startdraw"/>
    	</div>
    	<div class="drawnotice">
		    <cell>
		      <img slot="icon" width="20" style="display:block;margin-right:5px;" src="../../static/credit/inteicon.png">
		      <marquee>
		        <marquee-item v-for="(item,index) in drawList" :key="index" class="align-center">会员ID: {{item.MemberId}} 中了 {{item.SkuName}} </marquee-item>
		      </marquee>
		    </cell>
        </div>
        <span class="giftset">奖品设置</span>
        <ul class="gift">
        	<li>一等奖：200元手机充值卡(通用)</li>
        	<li>二等奖：50元手机充值卡(通用)</li>
        	<li>三等奖：10元手机充值卡(通用)</li>
        	<li>幸运奖：5元手机充值卡(通用)</li>
        </ul>
        <div style="height: 55px;"></div>
    </div>
    
    <!--中奖 未中奖弹框-->
    <div class="mask" v-show="maskFlag">
    	<div class="mask-wrap" v-if="prizeFlag">
    		<div class="mask-text">中奖了</div>
	    	<div class="mask-box">
	    		<p>小哥! 看你面色红润, 骨骼惊奇, 是个练厨的奇才, 大奖不中都不行, 恭喜您摇获<span class="prizeName">{{prizeText}}</span></p>
	    		<button @click="jump('prize')">去领奖</button>
	    	</div>
    	</div>
    	<div class="mask-wrap" v-else>
    		<div class="mask-text">没中奖</div>
	    	<div class="mask-box">
	    		<img src="../../static/credit/inteclose.png" @click="closeClick"/>
	    		<p class="noprize">没中奖？搓搓小手再来一次</p>
	    		<button @click="closeClick">继续抽奖</button>
	    	</div>
    	</div>
    </div>
    
    <!--提示注册弹框-->
    <div class="masks" v-show="masksFlag">
    	<div>
    		<p>需要注册才能参与哦！</p>
    		<button @click="jump('register')">马上注册</button>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { Cell, Marquee, MarqueeItem,cookie,Toast } from 'vux'
import apiUrl from '../apiUrls.js'
var isDraw=0;//是否抽奖
export default{
	components:{
		Cell,
		Marquee, 
		MarqueeItem,
		cookie,
		Toast
	},
	data(){
		return{
			userData:{},
			userId:'',
			userType:'',
			LeaveIntegral:'',//当前积分
			total:'',//抽奖后积分
			maskFlag:false,
			masksFlag:false,
			prizeDeg:0,//中奖位置 角度
			prizeText:'',//中奖文字
			prizeFlag:true,//中奖弹框
			drawList:[],
			SkuId:'',
			ProductId:'',
			SkuName:'',
			MemberName:'',
			orderId:'',
			isJumpHotel:''//1跳转 0不跳转酒店信息
		}
	},
	methods:{
		chance(){
			var randomNum=diu_Randomize(1,1000);//获取随机数
			function diu_Randomize(min,max){   
		    return Math.floor( ( Math.random() * max ) + min );   
			}   
			if(randomNum==1){//一等奖[1]
				this.prizeDeg=240;
				this.prizeFlag=true;
				this.prizeText='一等奖';
				this.SkuName='200元话费';
				this.SkuId=1030;
				this.ProductId=1027;
			}else if(randomNum>1&&randomNum<=6){//二等奖[2-6]
				this.prizeDeg=180;
				this.prizeFlag=true;
				this.prizeText='二等奖';
				this.SkuName='50元话费';
				this.SkuId=1004;
				this.ProductId=1004;
			}else if(randomNum>6&&randomNum<=126){//三等奖[7-126]
				this.prizeDeg=60;
				this.prizeFlag=true;
				this.prizeText='三等奖';
				this.SkuName='10元话费';
				this.SkuId=1006;
				this.ProductId=1006;
			}else if(randomNum>126&&randomNum<=356){//幸运奖[127-356]
				this.prizeDeg=0;
				this.prizeFlag=true;
				this.prizeText='幸运奖';
				this.SkuName='5元话费';
				this.SkuId=1031;
				this.ProductId=1028;
			}else{//谢谢参与[357-1000]
				this.prizeDeg=120;
				this.prizeFlag=false;
			}

//    只抽中谢谢参与
//    this.prizeDeg=120;
//		this.prizeFlag=false;
			
			if(this.prizeFlag==true && this.isJumpHotel == 0){
				this.addOrder();//新增订单
			}
		},
		startdraw(){//点击开始抽奖 判断用户身份
			if(this.userType==1){//队员
				this.$vux.toast.text('队员不能参与哦！', 'middle');
			}else if(this.userType==0){//未注册
				//弹框
		  	this.masksFlag=true;
			}else if(this.userType==2){//会员 判断积分
				this.getMemberById();// 获取会员当前积分
			}
			
//			this.$vux.toast.text('系统维护中，请稍后', 'middle');
			
		},
		draw(){//抽奖
			if(isDraw==0){
				isDraw=1;
				this.chance();//抽奖概率 获取抽中奖项
			  this.updateMemberIntegral();//更改积分
			  var _this=this;
		  	var deg=0;//旋转度数
		  	var draws = null;//定时器
		    var endDeg=1800-this.prizeDeg;  
		    var deg1=endDeg-270;
		    draws = setInterval(function(){
		    	deg +=5;
		 			$('.intezpImg').css({
		  		  'transform':'rotate('+deg+'deg)'
		  	  })
		 			if(deg >= deg1){
		 				clearInterval(draws);
		 			  draws = setInterval(drawTime,12);
		 			}
		    },10)
		    
		    function drawTime(){
		    	deg += 1;
		    	$('.intezpImg').css({
		  		  'transform':'rotate('+deg+'deg)'
		  	  })
		    	if(deg >= endDeg){
		    		clearInterval(draws);
		    		setTimeout(function(){
		    			_this.maskFlag=true;
		    			isDraw=0;
		    		},500)
		    	}
		    }
			}
		},
		addOrder(){//新增订单
			  var _this=this;
				var OrderDetaile=[];//提交的商品
	  		var item={
					"OrderDetaileId": 0,
		      "OrderId": 0,
	        "SkuId": this.SkuId,
	        "SkuName": this.SkuName,
	        "ProductId":this.ProductId,
	        "Count": 1
				}
	  		OrderDetaile.push(item);
				var params={
			    "MemberId": this.userId,
			    "OrderAddress": "",
			    "OrderName":this.MemberName,
			    "OrderTelephone":"",
			    "OrderState": "未提交",
			    "OrderPrice": 0,
			    "OrderType":2,
			    "InventedType":1,
			    "OrderFrom":"积分抽奖",
			    "OrderRemark":"积分抽奖",
			    "OrderDetaile":OrderDetaile
				}
	  		this.$http({
	  			method:'post',
	  			url:apiUrl.addOrder,
	  			data:params
	  		}).then(function(response){
//	  			console.log(response.data);
		      _this.orderId=JSON.parse(response.data);
	  		})
	  },
		getIntegrakDrawList(){//获取中奖列表
			var _this=this;
  		this.$http({
  			method:'get',
  			url:apiUrl.getIntegrakDrawList
  		}).then(function(response){
  			_this.drawList=JSON.parse(response.data);
  		})
		},
		getMemberById(){//个人中心首页信息 获取会员积分
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
//			  console.log(data);
			  var drawcount = data[0].DrawCount;//今日抽奖次数 10次以内可抽奖
  			_this.LeaveIntegral = data[0].LeaveIntegral;
  			_this.MemberName = data[0].MemberName;
  			
			  if(drawcount < 10){//可以抽奖
				if(_this.LeaveIntegral >= 40){//判断当前积分
					  _this.draw();
	  			}else{
		        _this.$vux.toast.text('积分不太够哦，快去赚积分吧', 'middle');
					}
			  }else if(drawcount >= 10){
	        _this.$vux.toast.text('明天再来哦', 'middle');
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
  	updateMemberIntegral(){//更改积分
  		var _this=this;
  		var params={
  		  'Operation':'minus',
				'Integral':40,
				'MemberId':this.userId,
				'IntegralSource':'积分抽奖',
				'Remark':'积分抽奖'
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.updateMemberIntegral,
  			data:params
  		}).then(function(response){
//			console.log(response);
  		})
  	},
		closeClick(){
			this.maskFlag=false;
		},
		jump(val){//去领奖
			if(val=='prize'){//去领奖
				if(this.isJumpHotel == 0){
					this.$router.push({path:'/component/receiveprice',query:{prize:this.prizeText,orderId:this.orderId,source:'integraldraw'}})
				}else if(this.isJumpHotel == 1){//完善酒店信息
					this.$router.push({path:'/component/resumehotel',query:{prize:this.prizeText,skuId:this.SkuId,skuName:this.SkuName,productId:this.ProductId,source:'integraldraw'}})
				}
			}else if(val=='register'){
				this.$router.push('/component/registeredmembers');
			}
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
	  }
	},
	mounted(){
		var width=$('window').width();
		$('.intezp').css({
			'height':width*0.8
		})
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
  	this.userType=parseInt(this.userData.UserType);
  	this.getIntegrakDrawList();
  	this.getMemberProfile();//获取用户信息 判断是否填写酒店信息
	}
}
</script>

<style scoped>
  .wrap{width:100%;text-align: center;background: #c72121;}
  .topImg img{width:100%;}
  .intezp{width: 80%;margin-left: 10%;position: relative;background: url(../../static/credit/intemoney.png) no-repeat left bottom;background-size:87% ;}
  .intezpImg{width:100%;}
  .intebtnImg{width: 30%;position:absolute;top:50% ;left: 50%;transform:translate(-45%,-50%);transform-origin: center 40px;}
  .drawnotice{width:64%;margin-left: 18%;margin-top: 10px;}
  .drawnotice img{width: 15px;}
  .giftset{display: block; font-size: 1em;color: #ffcb3f;position: relative;margin-top: 25px;}
  .giftset:before, .giftset:after {content: '';position: absolute;top: 52%;background: #ffcb3f;width: 25%;height: 1px;}          
  .giftset:before{left: 10%;} /*调整背景横线的左右距离*/
  .giftset:after {right: 10%;}              
  .wrap .gift{padding: 10px 0;list-style: none;width:90%;background: #b31a1c;text-align: left;color: #fff;font-size: 12px;margin-left: 5%;margin-top: 20px;}  
  .gift li{line-height: 30px;padding-left: 10px;}
  .mask{width: 100%;height: 100%;position: fixed;top: 0px;background: rgba(0,0,0,0.3);} 
  .mask-wrap{margin-top:170px;}
  .mask-text{font-size: 20px;position:relative;top:20px;z-index:100;width: 60%;height:50px;line-height:35px;margin-left: 20%;background: url(../../static/credit/intetk.png) no-repeat center;background-size: 100% 100%;text-align: center;color: #f6ebb3;}     
  .mask-box{width: 80%;background: #e72041;margin-left: 10%;position: relative;border-radius: 5px;}
  .mask-box p{color: #f6e9b2;font-size: 14px;padding: 50px 50px 20px 50px;}
  .mask-box .noprize{padding:50px;text-align: center;}
  .prizeName{color: #ffd101;}
  .mask-box button{width: 60%;height: 40px;background: #b90020;color: #f6e9b4;border: none;border-radius: 5px;margin-left: 20%;margin-bottom: 20px;font-size: 14px;}
  .mask-box img{width: 20px;position: absolute;right: -8px;top: -8px;}
  .masks{width:100%;height:100%;position: fixed;top: 0;background: rgba(0,0,0,0.5);z-index: 1000;}
  .masks div{background: #fff;width: 50%;height:105px;position: absolute;top: 200px;left:25%;text-align: center;}
  .masks p{font-size: 12px;color: #E83428;margin-top: 30px;}
  .masks button{width: 50%;height: 25px;background: #E83428;color: #fff;border: none;outline: none;font-size: 12px;margin-top: 20px;}
</style>
<style>
  .integraldraw .vux-cell-primary{flex:0 !important;}
  .integraldraw .weui-cell{background: #b31a1c;padding: 5px 15px;}
  .integraldraw .vux-marquee-box li{color: #fff;font-size: 12px;}
</style>