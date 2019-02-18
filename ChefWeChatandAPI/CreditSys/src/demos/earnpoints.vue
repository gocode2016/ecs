<template>
  <div class="earnpoints">
    
    <div class="top">
    	<p class="top_a">当前积分：100积分</p>
    	<p class="top_b"><img src="../../static/credit/money_c.png"><a style="color: #3e3e3e;" href="#/component/integraldetail">积分明细</a></p>
    </div>
    
    <div class="wrap">
    	<p class="title"><img src="../../static/credit/money_c.png"><span>赚积分</span></p>
      	<cell is-link :link='item.url' v-for="(item,index) in inte" :key="index" :title="item.title" :value="item.value"  v-bind:style="{background:'url('+item.img+') no-repeat 20px center',backgroundSize:'15px'}"></cell>      
    </div>
    
    <p class="title"><img src="../../static/credit/money_c.png"><span>花积分</span></p>
    <div class="footer">
    	<div @click="jump('/component/shop')"><img src="../../static/credit/active_current.png" /><span>进入商城</span></div>
    	<div @click="jump('/component/integraldraw')"><img src="../../static/credit/active_current.png" /><span>积分抽奖</span></div>
    </div>
    
  </div>
</template>

<script>
import { Cell } from 'vux'
import apiUrl from '../apiUrls.js'

export default{
	components:{
		Cell
	},
	data(){
		return{
			inte:[
			    {
		  			img:'../../static/credit/jf2.png',
		  			title:'简历完善',
		  			value:'0%',
		  			url:'/component/resume'
	  		    },
				{
		  			img:'../../static/credit/jf1.png',
		  			title:'每日签到',
		  			value:'已签到',
		  			url:'/component/sign'
	  			},
	  			{
					img:'../../static/credit/jf3.png',
					title:'推荐同行注册',
					value:'10/20',
					url:'/component/recommendstart'
				},
				{
					img:'../../static/credit/jf3.png',
					title:'扫一扫',
					value:'10/20',
					url:''
				},
	  	    ]
		}
	},
	methods:{
		jump(link){
			this.$router.push(link);
		}
	},
	mounted(){
		
	}
}
</script>

<style scoped>
.top{padding: 24px;font-size: 12px;text-align: center;}
.top_a{color: #e83428;}
.top_b{color: #3E3E3E;margin-top: 10px;}
.top_b *{vertical-align: middle;}
.top_b img{width: 20px;margin-right: 10px;}
.wrap{border-top: 5px solid #fbfbfb;border-bottom: 3px solid #fbfbfb;}
.title{font-size: 14px;color: #e83428;line-height:50px;margin-left: 5%;}
.title *{vertical-align: middle;}
.title img{width: 20px;margin-right: 10px;}
.footer{display: flex;margin-top: 10px;}
.footer div{width: 42.5%;margin-left: 5%;border: 1px solid #ccc;line-height: 55px;text-align: center;}
.footer div img{vertical-align: middle;width: 27px;margin-right: 10%;}
.footer div span{font-size: 14px;color: #3E3E3E;}
</style>
<style>
#vux_view_box_body{background: #fff;}
.earnpoints .weui-cell{height: 35px !important;padding: 10px 20px;}
.earnpoints .weui-cell:before {border-top: 1px solid #f1f1f1 !important;}
.earnpoints .vux-label {font-size: 12px; color: #3e3e3e;margin-left: 27px;}
.earnpoints .weui-cell__ft{color: #3e3e3e !important;font-size: 12px;}
.earnpoints .weui-cell__ft:after {height: 7px !important; width: 7px !important; border-width: 1px 1px 0 0 !important;border-color: #ccc !important;}
</style>