<template>
  <div class="scanrecord">
    
    <!--中奖记录-->
    <div v-if="isShow==1">
	    <p class="returnhome">
	    	<img src="../../static/credit/scan_return.png" @click="jump('/component/scanhome')"/>
	    	<span @click="jump('/component/scanhome')">返回首页</span>
	    </p>
	    <div class="headimg">
	    	<img :src="HeadImgUrl" />
	    </div>
	    <div style="text-align: center;">
	    	<p class="username">{{NickName}}</p>
	    	<button @click="txClick">可提现{{info.kTxMoney}}元</button>
	    	<p style="font-size: 15px;margin-top: 5%;">共扫描味达美尚品生抽<span style="color: #ff8901;">{{info.scanCount}}</span>瓶</p>
	    	<p style="font-size: 15px;margin-top: 5%;">未领红包: <span style="color: #ff8901;">{{info.kTxMoney}}</span>元</p>
	    	<p style="font-size: 15px;margin-top: 2%;">已领红包: <span style="color: #ff8901;">{{info.haveTxMoney}}</span>元</p>
	    	<p class="tips" style="margin-top: 7%;">温馨提示：根据微信平台要求，红包</p>
	    	<p class="tips">累计到1元及以上即可提现，不足1元</p>
	    	<p class="tips">的红包将存入零钱包</p>
	        <p class="lookrecord"><span @click="lookorhide(2)">查看详细记录</span></p>
	    </div>
    </div>
    
    <!--兑奖记录-->
    <div v-else class="prizelist">
	    <p class="hiderecord"><span @click="lookorhide(1)">收起</span></p>
    	<ul>
    		<li v-for="(item,index) in list" :key="index" >
    			<p>
    				<span style="font-size: 13px;">{{item.text}}</span>
    				<span style="font-size: 12px;margin-top: 2px;">{{item.CreateDate}}</span>
    			</p>
    			<p>{{item.Action}}{{item.Money}}元</p>
    		</li>
    	</ul>
        <img src="../../static/credit/scan_icon.png" class="icon"/>
    </div>
    
  </div>
</template>

<script>
import { Toast,Loading,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
var btnNum = 0;
export default{
	components:{
		Toast,
		Loading,
		cookie
	},
	data(){
		return{
			isShow:1,//1 展示中奖记录 2展示兑奖记录列表
			userData:{},
			openId:'',
			NickName:'',
			HeadImgUrl:'',
			info:{},
			list:[]
		}
	},
	methods:{
		jump(link){//返回首页
			this.$router.push(link);
		},
		lookorhide(val){//查看详细记录
			this.isShow = val;
			this.getredpacklist();//用户扫描记录
		},
		getredpacklist(){//用户扫描记录
			var _this = this;
  		this.$http({
  			method:'get',
				url:apiUrl.redPackGetHistory + '?openid=' + this.openId
  		}).then(function(response){
  			_this.info = JSON.parse(response.data);
  			if(_this.info.isKeTiXian == 1){//可提现
  				$('button').addClass('btnStyle');
  			}
  			_this.list = _this.info.data;
  			for(var i in _this.list){
  				var i = _this.list[i];
  				if(i.Action == '+'){
  					i.text = '已领取';
  				}else if(i.Action == '-'){
  					i.text = '已提现';
  				}
  			}
  		})
		},
		txClick(){//点击提现
			if(this.info.isKeTiXian == 1){//可提现
				if(this.info.isRegist == 0){//未注册
					this.$router.push({path:'/component/scanregister',query:{money:this.info.kTxMoney,isType:1}});
				}else if(this.info.isRegist == -2){//调味品供货商
					this.$router.push({path:'/component/scanreceive',query:{isType:2,message:'调味品供货商'}});
				}else if(this.info.isRegist == 1){//已注册
					btnNum++;
					if(btnNum == 1){
						this.showLoading();//loading
						this.getMoney();//提现接口
					}
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
  			_this.$vux.loading.hide();//loading图标消失
//			console.log(JSON.parse(response.data));
  			var data = JSON.parse(response.data);
  			if(data.result_status == 'succ'){//提现成功
					_this.$router.push({path:'/component/scanreceive',query:{money:_this.info.kTxMoney,isType:1}});
  			}else if(data.result_status == 'fail'){//提现失败 提示
//				_this.$vux.toast.text(data.message, 'middle');
//				btnNum = 0;
					_this.$router.push({path:'/component/scanreceive',query:{isType:2,message:data.message}});
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
	  showLoading () {//loading 图标
      this.$vux.loading.show({
        text: 'Loading'
      })
    }
	},
	mounted(){
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
	  this.NickName=decodeURI(this.userData.Nickname);
	  this.HeadImgUrl=this.userData.Headimgurl;
		
		this.getredpacklist();//用户扫描记录
	}
}
</script>

<style scoped>
.scanrecord{width:100%;height:100%;background: #ffe4c7;overflow: hidden;letter-spacing: 1px;}
.returnhome{margin-top: 2%;}
.returnhome *{display: inline-block;vertical-align: middle;}
.returnhome img{width: 20px;margin-left: 10px;}
.returnhome span{font-size: 13px;margin-left: 8px;}
.headimg{width: 60px;height:60px;border: 1px solid #ccc;border-radius: 50%;margin:2% auto;}
.headimg img{width: 100%;height: 100%;overflow: hidden;border-radius: 50%;}
.username{font-size: 18px;}
button{background:#ccc;border: none;padding: 10px 20px;border-radius: 5px;color: #fff;margin-top: 5%;outline: none;font-size: 16px;}
.btnStyle{background:#ff8901;}
.tips{font-size: 13px;}
.lookrecord{font-size: 16px;color: #ff8901;position:absolute;bottom: 5%;text-align: center;width: 100%;}
.lookrecord span,.hiderecord span{border-bottom: 1px solid #ff8901;}
.hiderecord{font-size: 16px;color: #ff8901;text-align: center;width: 100%;margin-top: 6%;}
.prizelist{width:100%;height:100%;border: 1px solid #ffe4c7;box-sizing:border-box;background: url('../../static/credit/scan_ys.png') no-repeat center bottom #ffe4c7;background-size: 100% 13%;}
ul{padding: 0 5%;height: 60%;overflow: scroll;margin-top: 10%;}
ul li{list-style: none;display: flex;border-bottom: 1px solid #ff8901;padding: 3% 0;}
li p:nth-child(1){width: 60%;}
li p:nth-child(2){width: 34%;font-size: 14px;text-align: right;color: #ff8901;line-height: 34px;}
li span{display: block;}
.icon{width: 20%;margin-left: 40%;position: absolute;bottom: 13%;}

</style>
<style>

</style>