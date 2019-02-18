<template>
  <div class="scanrule">
    
    <p class="title"><span>兑奖规则</span></p>
    <p class="text" v-for="(item,index) in list">
    	<span>{{index+1}}.</span>
    	<span>{{item.text}}</span>
    </p>
    <p class="title"><span>法律声明</span></p>
    <p class="text" v-for="(item,index) in lists">
    	<span>{{index+1}}.</span>
    	<span>{{item.text}}</span>
    </p>
    <img src="../../static/credit/scan_icon.png" class="icon"/>
    
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
			list:[
			  {
			  	text:'活动主题： 码上抢红包 100%有奖'
			  },
			  {
			  	text:'活动截止日期： 2018年12月31日'
			  },
			  {
			  	text:'兑奖截止日期： 2019年3月31日'
			  },
			  {
			  	text:'活动区域： 全国'
			  },
			  {
			  	text:'活动对象： 活动仅针对酒店从业者和美食爱好者，经销商、二批商不能参与'
			  },
			  {
			  	text:'综合中奖率： 100%'
			  },
			  {
			  	text:'即日起至2019年3月31日凡购买2L味达美尚品生抽促销装产品（包装印有"码上抢红包"活动信息)，即可扫描瓶身二维码，随机领取0.08-88元不等的微信现金红包（现金红包金额最低0.08元，最高88元）；根据微信支付平台规定，现金奖励需要累积到1元及以上方可提现到微信零钱，不足1元将累计在欣和餐饮服务平台"我的零钱包"里'
			  },
			  {
			  	text:'用户按照页面提示操作领取现金红包，获得的现金奖励将存入用户个人微信零钱'
			  },
			  {
			  	text:'每个兑奖二维码仅限参与1次拆红包活动，且每个用户每天最多可参与10次'
			  },
			  {
			  	text:'微信现金红包需在指定时间内进行兑奖，兑奖截止日期2019年3月31日，逾期中奖金额将自动取消无法提现'
			  }
			],
			lists:[
			  {
			  	text:'用户不得提供伪造、虚假信息，一经发现将取消参与活动资格'
			  },
			  {
			  	text:'因网络传输、微信系统或用户微信账号风险等问题导致的用户提交信息错误或延迟、或红包发送失败的风险由用户自行承担'
			  },
			  {
			  	text:'对于任何通过不正当手段参与活动者（包括而不限于利用系统问题刷奖及其他作弊行为的），活动主办方有权取消用户参加活动的资格，且不需要通知用户'
			  },
			  {
			  	text:'凡参与活动的客户应仔细阅读并遵守本活动相关规定'
			  }
			]
		}
	},
	methods:{
		
	},
	mounted(){
		
	}
}
</script>

<style scoped>
.scanrule{width:100%;color:#343434;border: 1px solid #ffe4c7;box-sizing:border-box;background: url('../../static/credit/scan_ys.png') no-repeat center bottom #ffe4c7;background-size: 100% 10%;}
.title{font-size: 16px;color: #ff8901;text-align: center;margin: 25px 0 10px 0;}
.title span{display: inline-block;border-bottom: 1px solid #ff8901;line-height: 30px;}
.text{padding: 0 2%;font-size: 12px;color: #333333;}
.text span{display: inline-block;}
.text span:nth-child(1){width: 5%;vertical-align: top;}
.text span:nth-child(2){width: 90%;}
.icon{margin-top: 20px;width: 20%;margin-left: 40%;margin-bottom: 20%;}
@media screen and (min-height:700px){
	.scanrule{height: 100%;}
	.icon{
		margin-bottom: 0;
		position: absolute;
		bottom: 10%;
	}
}   
</style>
<style>

</style>