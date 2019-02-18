<template>
  <div class="activityrule">
    
    <p class="rule-title">1.购买以下产品，即可随箱获得积分卡片一张</p>
    <img src="../../static/credit/rulebanners.png" class="rulebanner"/>
    <p class="rule-prompt">积分卡片示意图</p>
    
    <p class="rule-title rule-titles">2.打开微信扫一扫，扫描卡片二维码，关注平台</p>
    <img src="../../static/credit/ruleimg1.png" class="ruleimg1"/>
    
    <p class="rule-title rule-titles">3.打开平台内菜单【我的】【扫一扫】</p>
    <img src="../../static/credit/ruleimg02.png" class="ruleimg2"/>
    
    <p class="rule-title rule-titles">4.积分产品说明</p>
    <div class="rulebox" v-for="(item,index) in prolist">
    	<p class="rulebox-title">{{item.title}}</p>
    	<div class="rule-z">
    		<p><span class="red">{{item.pro}}</span>{{item.one}}</p>
    		<p>{{item.two}}</p>
    		<p>{{item.three}}</p>
    		<p>{{item.four}}</p>
    	</div>
    	<div class="rule-box">
    		<div v-for="(item,index) in item.proarr">
    			<img :src="item.img"/>
    			<p class="rulebox-txt">{{item.text}}</p>
    		</div>
    	</div>
    </div>
    
    <p class="rule-title rule-titles">5.其他说明</p>
    <p class="rule-other">该积分活动针对酒店终端用户已认证会员开展，经销商、二批商等供货商不得参与，会员须知请<span class="red" @click="jump">点击此查看</span></p>
    
  </div>
</template>

<script>
//	import { cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
  import wx from 'weixin-js-sdk'
	export default{
		components:{
//			cookie
		},
		data(){
			return{
				timestamp:'',
        nonceStr:'',
        signature:'',
        url:'',//当前页面地址
				prolist:[
				  {
					  title:'40积分/箱',
					  pro:'注：',
					  one:'蚝油卡片随箱生产投放截止时间：2018年12月31日。',
					  two:'蚝油卡片积分扫描有效截止时间：2019年3月31日。',
					  three:'2019年3月31日之前扫描该卡片可正常获得积分并使用，',
					  four:'2019年3月31日之后活动失效，该积分卡片无法继续扫描。',
					  proarr:[
					    {
								img:'../../static/credit/rule10.png',
								text:'6.18KG味达美尚品蚝油/1x2'
					    }
					  ]
				  },{
					  title:'50积分/箱',
					  proarr:[
						  {
								img:'../../static/credit/rule1.png',
								text:'1.8L味达美冰糖老抽酱油/新包装/1x6'
						  },{
								img:'../../static/credit/rule2.png',
								text:'1L味达美冰糖老抽酱油/1x12'
						  },{
								img:'../../static/credit/rule3.png',
								text:'1L味达美剁椒鱼头鲜豉油/1x6'
						  },{
								img:'../../static/credit/rule4.png',
								text:'600G味达美压锅酱/1x6'
						  }
					  ]
				  },{
						title:'60积分/箱',
						proarr:[
						  {
								img:'../../static/credit/rule05.png',
								text:'2.2kg葱伴侣黄豆酱/1x6'	
						  },{
								img:'../../static/credit/rule06.png',
								text:'5KG葱伴侣黄豆酱/桶装/新结构1x2'
						  }
						]
				  },{
						title:'100积分/箱',
						proarr:[
						  {
								img:'../../static/credit/rule7.png',
								text:'2L味达美剁椒鱼头鲜豉油/1x6'	
						  }
						]
				  },{
						title:'150积分/箱',
						proarr:[
						  {
								img:'../../static/credit/rule8.png',
								text:'2L味达美酸辣捞汁/1x6'
						  },{
								img:'../../static/credit/rule9.png',
								text:'2L味达美海鲜捞汁/1x6'
						  }
						]
				  }
				]
			}
		},
		methods:{
			share(){
    	  var _this=this;
        var localIds ='';
		  	this.$http({
		  		method:'GET',
		  		url:apiUrl.getTimestampAndNonceStr
		  	}).then(function(response){
		  		var data=response.data;
		  		var dataArr=data.split('|');
		  		_this.timestamp=parseInt(dataArr[0]);//时间戳
		  		_this.nonceStr=dataArr[1];//随机字符串
		  		
		      var params='?noncestr='+_this.nonceStr+'&timestamp='+_this.timestamp+'&url='+_this.url;
		  		_this.$http({
		  			method:'GET',
		  			url:apiUrl.getJsapiTicketSignature+params
		  		}).then(function(response){
		  			  _this.signature=response.data;
		  			  
		  			  wx.config({
					      debug:false,
					      appId: apiUrl.appId,
					      timestamp: _this.timestamp,
					      nonceStr: _this.nonceStr,
					      signature: _this.signature,
					      jsApiList: [
					        'onMenuShareTimeline',//分享到朋友圈
					        'onMenuShareAppMessage'//分享给朋友
					      ]
					    })
		  			  
		  			  wx.ready(function(){
		    				wx.onMenuShareTimeline({
				            title:'味达美餐饮积分扫码规则',
				            link:apiUrl.getUrl+'/#/component/activityrule',
				            imgUrl:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTmOMvo4cAWhQ0mwkXO23bZCpDRXp3GXWJ998B6C1Svp2dJJNlpXoQWg/0?wx_fmt=jpeg',
				            success: function () { 
				              // 用户确认分享后执行的回调函数
//				              _this.addECWXTranspondDetails(2);
				            },
				            cancel: function () { 
				              // 用户取消分享后执行的回调函数
//				              alert('你没有分享到朋友圈');
				            },
				            fail:function(res){
//				            	alert(res.errMsg);
				            }
		            });
		            wx.onMenuShareAppMessage({
										title:'味达美餐饮积分扫码规则', // 分享标题
										desc:'味达美餐饮积分扫码规则', // 分享描述
				            link:apiUrl.getUrl+'/#/component/activityrule',
										imgUrl:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTmOMvo4cAWhQ0mwkXO23bZCpDRXp3GXWJ998B6C1Svp2dJJNlpXoQWg/0?wx_fmt=jpeg',// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
//										  alert('分享给朋友成功');
//										  _this.addECWXTranspondDetails(1);
										},
										cancel: function () {
										// 用户取消分享后执行的回调函数
//										  alert('你没有分享给朋友');
										},
										fail:function(res){
//				            	alert(res.errMsg);
				            }
								});
    			    })
		  		})
		  	})
      },
			jump(){
				this.$router.push('/component/membernotice');
			}
		},
		mounted(){
			var uri=window.location.href.split('#')[0];
		  this.url=encodeURIComponent(uri);
		  this.share();
		}
	}
</script>

<style scoped>
.rule-title{font-size: 14px;color: #d90b18;padding-left: 12px;margin-top: 16px;letter-spacing: 1px;}
.rule-titles{margin-top: 35px;}
.rulebanner{width:80%;margin-left: 10%;margin-top: 25px;}
.rule-prompt{font-size: 11px;text-align: center;color: #333333;}
.ruleimg1{width: 90%;margin-left: 5%;margin-top: 25px;}
.ruleimg2{width: 60%;margin-left: 20%;margin-top: 25px;}
.rule-box{width: 100%;display: flex;flex-wrap:wrap;}
.rule-box div{width: 30%;margin-left: 2.5%;margin-top: 15px;}
.rule-box img{width: 100%;}
.rulebox-title{font-size: 12px;color: #d60d15;text-align: center;margin-bottom: 5px;margin-top: 25px;}
.rulebox-txt{font-size: 10px;color: #3f3f3f;}
.rule-other{padding-left: 12px;font-size: 12px;color: #3f3f3f;margin-top: 10px;margin-bottom: 70px;}
.red{color: #d80c17;}
.rule-z{font-size: 12px;color: #333;padding-left:12px ;}
.rule-z p{text-indent: 24px;}
.rule-z p:nth-child(1){text-indent: 0px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>