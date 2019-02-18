<template>
  <div class="scandraw" style="height: 100%;">
    
    <div class="wrap">
    	<p>恭喜获得</p>
    	<p class="money">{{money}}元</p>
    	<div v-if="showFlag" class="box">
	    	<button @click="txClick">立即提现</button>
	    	<p style="margin-top: 7%;">温馨提示: 您的红包累计金额为{{kTxMoney}}元</p>
	    	<p>点击上方按钮把钱拿走吧</p>
    	</div>
    	<div v-else class="box">
	    	<button @click="lqbClick">存入我的零钱包</button>
	    	<p style="margin-top: 7%;">温馨提示: 根据微信平台要求，红包</p>
	    	<p>累计到1元及以上即可提现，不足1元</p>
	    	<p>的红包将存入零钱包</p>
    	</div>
    </div>
    
  </div>
</template>

<script>
import { Toast,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
var btnNum = 0;
export default{
	components:{
		Toast,
		cookie
	},
	data(){
		return{
			showFlag:false,
			money:'',
			kTxMoney:'',
			isKeTiXian:'',
			isRegist:'',
			userData:{},
			openId:''
		}
	},
	methods:{
		txClick(){//点击提现
			if(this.isRegist == 0){//未注册
				//去注册
				this.$router.push({path:'/component/scanregister',query:{money:this.kTxMoney,isType:1}});//提现 isType 1
			}else if(this.isRegist == 1){//已注册
//				提现
        btnNum++;
        if(btnNum == 1){
        	this.getMoney();
        }
			}
		},
		getMoney(){//提现接口
			var _this = this;
			var params={
  			"OpenId": this.openId
  		}
  		this.$http({
  			method:'post',
				url:apiUrl.redPackTiXian,
  			data:params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			var data = JSON.parse(response.data);
  			if(data.result_status == 'succ'){//提现成功
					_this.$router.push({path:'/component/scanreceive',query:{money:_this.kTxMoney,isType:1}});
  			}else if(data.result_status == 'fail'){//提现失败 提示 isType 2
					_this.$router.push({path:'/component/scanreceive',query:{isType:2,message:data.message}});
  			}
  		})
		},
		lqbClick(){//点击存入零钱包
			if(this.isRegist == 0){//未注册
				//去注册
				this.$router.push({path:'/component/scanregister',query:{money:this.money,isType:0}});//存入零钱 isType 0
			}else if(this.isRegist == 1){//已注册
        //存入零钱
				this.$router.push({path:'/component/scanreceive',query:{money:this.money,isType:0}});
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
		this.money = this.$route.query.money;//扫码获得金额
		this.kTxMoney = this.$route.query.kTxMoney;//累计红包金额
	  this.isKeTiXian = this.$route.query.isKeTiXian;//能否提现
		this.isRegist = this.$route.query.isRegist;//是否注册
		
		if(this.isKeTiXian == 0){//不可提现，存入零钱包
			this.showFlag = false;
		}else if(this.isKeTiXian == 1){//可提现
			this.showFlag = true;
		}
		
		//获取用户信息
	  this.userData=cookie.get('WeiXinUser',function(val){
		    var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
	  }) 
	  this.openId=this.userData.Openid;
	}
}
</script>

<style scoped>
.wrap{color: #fff;background: url('../../static/credit/scan_back.png') no-repeat center;background-size:100% 100%;width: 100%;height: 100%;text-align: center;overflow: hidden;}
.wrap p:nth-child(1){margin-top: 45%;font-size: 18px;}
.wrap .money{margin-top: 4%;font-size: 35px;}
.box button{margin-top: 5%;font-size: 14px;width: 40%;height: 42px;background: #ff8a00;color: #fff;border: none;border-radius: 5px;}
.box p{font-size: 13px;letter-spacing: 1px;margin-top: 4px;}
</style>
<style>

</style>