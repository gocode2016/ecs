<template>
  <div class="scantips">
    
    <p class="returnhome">
	    <img src="../../static/credit/scan_return.png" @click="jump('/component/scanhome')" />
	    <span @click="jump('/component/scanhome')">返回首页</span>
	  </p>
	  <div style="text-align: center;">
	    <!--温馨提示-->
	    <div class="box" v-if="showFlag">
		    <p>温馨提示： </p>
		    <p class="tip_text" v-if="textFlag">{{tipText}}</p>
		    <div v-else>
			    <p class="tip_text">本活动仅针对酒店从业者和美食爱好者</p>
			    <p class="tip_text">调味品供货商无法参与</p>
		    </div>
	    </div>
	    <div class="box" v-else>
		    <p>温馨提示： </p>
		    <p style="width: 70%;margin-left: 15%;">每瓶仅限扫码一次，此码已于{{time}}扫描过,获得{{money}}元,去我的零钱包查看</p>
	    </div>
	    
	    <p class="text">长按识别二维码，关注欣和餐饮服务公众号</p>
	    <p><img src="../../static/credit/scan_code.jpg" class="scancode"/></p>
	    <p><img src="../../static/credit/scan_icon.png" class="bottomImg"/></p>
    </div>
    
  </div>
</template>

<script>
//import { } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		
	},
	data(){
		return{
			showFlag:true,
			textFlag:true,
			money:'',
			time:'',
			tipText:'此码不属于本次活动'
		}
	},
	methods:{
		jump(link){//返回首页
			this.$router.push(link);
		}
	},
	mounted(){
		var type = this.$route.query.type;
		if(type == 0){//非活动码
			this.showFlag = true;
			this.textFlag = true;
		}else if(type == 1){//已扫码
			this.showFlag = false;
			this.money = this.$route.query.money;
			this.time = this.$route.query.time;
		}else if(type == 2){//调味品供货商
			this.showFlag = true;
			this.textFlag = false;
		}else if(type == 3){//其他失败情况
			this.showFlag = true;
			this.textFlag = true;
		  var message = this.$route.query.message;
		  this.tipText=message;
		}
	}
}
</script>

<style scoped>
.scantips{width: 100%;height: 100%;overflow: hidden;background: url('../../static/credit/scan_ys.png') no-repeat center bottom #ffe4c7;
          background-size: 100% 14%;letter-spacing: 1px;}
.returnhome{margin-top: 2%;}
.returnhome *{display: inline-block;vertical-align: middle;}
.returnhome img{width: 20px;margin-left: 10px;}
.returnhome span{font-size: 12px;margin-left: 8px;}
.box{margin: 8% 0 14% 0;font-size: 13px;text-align: center;}
.box p{margin-top: 1%;}
.text{font-size: 13px;margin:0 0 6% 0;}
.scancode{width: 50%;}
.bottomImg{width:20%;position: absolute;bottom: 13%;left: 40%;}
@media screen and (max-height: 580px) {/*高度<=580 图片缩小*/
	.returnhome{margin-top: 6px;}
  .text{font-size: 13px;margin:0 0 4% 0;}
	.scancode{width: 46%;}
	.scantips{background-size: 100% 12%;}
	.bottomImg{bottom: 11%;}
}
    
</style>
<style>

</style>