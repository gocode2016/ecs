<template>
  <div class="feedback">
  	
    <!--<div class="feedDiv">
    	<img src="../../static/credit/search.png" class="searchImg"/>
    	<input type="text" placeholder="搜索问题关键字"/>
    </div>
    <hr style="width: 100%;height: 3px;background: #f8f8f8;border: none;margin-top: 12px;" />-->
    
    <div v-for="(item,index) in list">
	    <cell	     
	      is-link
	      :border-intent="false"
	      :arrow-direction="item.showContent ? 'up' : 'down'"
	      @click.native="item.showContent = !item.showContent"><span class="matterstyle">问：</span>{{item.title}}</cell>
	    <template v-if="item.showContent">
	      <cell-box :border-intent="false" class="sub-item"><span class="keystyle">答：</span>{{item.question}}</cell-box>
	    </template>
    </div>
    
  </div>
</template>

<script>
import { Cell, CellBox } from 'vux'
export default{
	components:{
		Cell,
		CellBox
	},
	data(){
		return{
			list:[{
				title:'现在会员账户显示未认证，怎么转为已认证？',
				question:'未认证会员转为已认证，在【我的】-【会员未认证】，选择实名认证或者注册码认证，按照规则填写信息即可；',
				showContent:false
			},{
				title:'积分商城下单后，物流信息在哪里查看？',
				question:'您好，成功下单后，可进入【我的订单】-【已发货】，点击【查看物流】查看',
				showContent:false
			},{
				title:'积分商城商城兑换的话费，如何查询话费充值是否到账？',
				question:'您好，点【我的】-【我的订单】，若显示【已发货】即为充值到账；若显示【未提交】再次提交即可',
				showContent:false
			},{
				title:'商品没有库存了！',
				question:'2-3天我们会及时补充库存',
				showContent:false
			},{
				title:'积分抽奖中奖之后没有及时领取，去哪里查看？',
				question:'您好，可以去【我的】-【我的订单】-【未提交】查看',
				showContent:false
			},{
				title:'为什么我注册了会员，却不能扫描积分卡片？',
				question:'您属于未认证会员，需要在个人中心【会员未认证】，选择两种认证方式中的任意一种认证会员身份。',
				showContent:false
			},{
				title:'我扫描的积分在哪里查看？',
				question:'您好，在【我的】-【积分明细】查看即可',
				showContent:false
			}]
			 
			
		}
	},
	methods:{
		
	},
	mounted(){
		
	}
}
</script>

<style scoped>
.feedDiv{width: 295px;height: 30px;background: #f8f8f8;border-radius: 5px;margin-top: 12px;margin-left: 10px;}
.searchImg{width:17px;margin-left: 11px;margin-right: 11px;}
.feedDiv img,.feedDiv input{display: inline-block;vertical-align: middle;}
.feedDiv input{border: none;background: none;outline: none;}
.sub-item {color: #888;}
.matterstyle{color: #E83428;}
.keystyle{color: #579447;}

</style>
<style>
#vux_view_box_body{background: #fff;}
.feedback .weui-cell_access .weui-cell__ft.vux-cell-arrow-down:after{transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
.feedback .weui-cell_access .weui-cell__ft.vux-cell-arrow-up:after{transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(90deg);}
.feedback .weui-cell_access .weui-cell__ft:after{border-width: 1px 1px 0 0;}
/*.weui-cell{padding: 10px 15px 10px 0 !important;}*/
.feedback .vux-label{font-size: 12px;color: #3E3E3E;}
.feedback .sub-item{font-size: 12px;color: #3E3E3E;background: #f8f8f8;}
.feedback .weui-cell{height: 52px;}
.feedback .weui-cell {border-bottom:1px solid #e9e9e9 !important;}
.feedback .weui-cell:before{display: none;}
.feedback .vux-cell-arrow-transition {width: 100% !important;text-align: left;color: #3e3e3e;font-size: 12px;}
    
</style>
