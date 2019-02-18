<template>
  <div class="membernotice">
    
    <div class="member_top">
    	<div @click="tabClick(0)"  :class="{member_tops:num==0}">会员须知</div>
    	<div @click="tabClick(1)" :class="{member_tops:num==1}">扫码规则</div>
    </div>
    
    <div class="wrap">
    <!--会员须知-->
    <div class="member_box" v-if="num==0">
	    <span class="notice-title notice-top">关于会员注册说明</span>
	    <div class="notice-box">
	    	<p><span class="smalltitle">申请对象：</span>酒店相关从业者，平台以服务厨师岗位优先。</p>
	    	<p><span class="smalltitle">申请地址：</span><span class="red" @click="jump('/component/registeredmembers')">点击此处</span></p>
	    	<p><span class="smalltitle">注意：</span>每个手机号只能注册一个会员账号，一旦申请成功永久有效，请确保您填写的信息真实有效，主办方有权针对信息进行审核，提供虚假信息的用户将被取消会员资格。</p>
	    </div>
	    
	    <span class="notice-title">会员分类</span>
	    <div class="notice-box">
	    	<p>平台会员分为已认证会员和未认证会员。</p>
	    	<p>未认证会员后期可以通过平台或者欣和酒店业务代表确认酒店厨师身份后可变成认证会员。</p>
	    </div>
	    
	    <span class="notice-title">会员权益</span>
	    <div class="notice-box">
	    	<p class="smalltitle">已认证会员</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;参与以下欣和餐饮产品积分扫码活动，其中经销商、二批商及相关供货商等不得参与该活动，经查核提供虚假信息者即取消会员资格。</p>
	    	<table border="1" cellpadding="0" cellspacing="0">
	    		<tr>
				    <td>1.8L味达美冰糖老抽酱油/新包装/1x6</td>
				    <td>1L味达美冰糖老抽酱油/1x12</td>
				    <td>1L味达美剁椒鱼头鲜豉油/1x6</td>
				  </tr>
					<tr>
					    <td>600G味达美压锅酱/1x6</td>
					    <td>2.2kg葱伴侣黄豆酱/1x6</td>
					    <td>5KG葱伴侣黄豆酱/桶装/新结构/1x2</td>
					</tr>
					<tr>
						  <td>2L味达美剁椒鱼头鲜豉油/1x6</td>
					    <td>2L味达美酸辣捞汁/1x6</td>
					    <td>2L味达美海鲜捞汁/1x6</td>
					</tr>
					<tr>
						  <td>6.18KG味达美尚品蚝油/1×2</td>
					    <td>2L味达美尚品生抽/1x6</td>
					    <td></td>
					</tr>
	    	</table>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;洞察前沿菜品趋势，创新菜品解决方案一览无遗。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有国内外知名旺店考察优先报名与专业烹饪书籍特价资格。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有味达美新品及客制化产品优先试用资格。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有高端定制厨师服、电子称、话费卡等精美大礼包。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有平台提供的其他活动参与资格。</p>
	    	<p class="smalltitle">未认证会员</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;参与以下欣和餐饮产品积分扫码活动，其中经销商、二批商及相关供货商等不得参与该活动，经查核提供虚假信息者即取消会员资格。</p>
	    	<table border="1" cellpadding="0" cellspacing="0" width="150px">
	    		<tr>
				    <td>2L味达美尚品生抽/1x6</td>
				  </tr>
				</table>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;洞察前沿菜品趋势，创新菜品解决方案一览无遗。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有国内外知名旺店考察优先报名与专业烹饪书籍特价资格。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有味达美新品及客制化产品优先试用资格。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有高端定制厨师服、电子称、话费卡等精美大礼包。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;享有平台提供的其他活动参与资格。</p>
	    </div>
	    
	    <span class="notice-title">积分的获取方式</span>
	    <div class="notice-box">
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;<span class="smalltitle">扫码积分：</span>通过扫描指定产品箱内积分卡片或者指定产品瓶身二维码，<span class="red" @click="backTop(1)">扫码规则点击查询</span>。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;<span class="smalltitle">每日签到：</span>每天登陆该平台【我的账户】，找到相关操作参与即可，每天签到可获得1积分，连续签到的第7天可获得一次抽奖机会，<span class="red">中奖率100%</span>。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;<span class="smalltitle">完善信息：</span>登陆该平台【我的账户】【求职简历】进入信息填写即可获得相应积分。</p>
	    	<p><span class="title-origin"></span>&nbsp;&nbsp;<span class="smalltitle">其他方式：</span>完成平台要求的指定活动任务即可赚取积分，规则以实际活动开展为准。</p>
	    </div>
	    
	    <span class="notice-title">积分的使用方式</span>
	    <div class="notice-box">
	    	<p>进入<span class="smalltitle">【我的账户】【积分商城】</span>，使用积分参与兑换相关礼品。</p>
	    	<p>进入<span class="smalltitle">【我的账户】【积分抽奖】</span>，使用积分参与抽奖活动获得礼品,综合中奖率60%。</p>
	    </div>
	    
	    <span class="notice-title">礼品说明</span>
	    <div class="notice-box">
	    	<p class="smalltitle">1、	实物类奖品:</p>
	    	<p>兑换成功后，请务必确认您的个人信息填写准备，如因自身原导致邮寄信息填写错误而无法送达，将由会员自身承担相应责任。</p>
	    	<p>兑换成功后，不予变更或取消。</p>
	    	<p>在积分商品送达时，请当场确认商品品类、规格、数量、包装等，若出现破损情况，请及时联系我们进行处理。</p>
	    	<p class="smalltitle">2、	充值类电子卡等：</p>
	    	<p>我们将按照您订单填写的联系方式以短信和订单详情的方式告知相关卡号卡密，请务必保证信息填写错误。</p>
	    	<p>兑换成功后，不予变更或取消。</p>
	    	<p>绝大部分情况下，充值的话费将在1分钟左右到账，但由于各地电讯运营商的充值接口可能会发生暂时性拥堵，或者遇到运营商系统维护，都有可能造成充值到账延迟。如果您对充值结果有异议，请先电话咨询当地充值运营商（移动、联通、电信服务商）确认解决。</p>
	    </div>
	    
	    <span class="notice-title">补充说明</span>
	    <div class="notice-box">
	    	<p>用户参与积分活动不得有涉及作弊、违法等行为，否则主办方有权在不事先通知的前提下取消会员资格，并保留追究用户法律责任的权利。</p>
	    	<p>凡参与积分活动的会员均被视为充分且无条件地接受并同意遵守本规则相关规定。</p>
	    	<p>欣和味达美餐饮服务微信平台保留对会员规则和积分规则进行更新或者修改的权利。</p>
	    </div>
    </div>
    
    <!--活动规则-->
    <div class="activityrule" v-else>
    	<p class="btitle">一、瓶码积分模式，针对全部会员</p>
	    <p class="rule-title">1.购买本产品，刮开涂层扫码</p>
	    <img src="../../static/credit/ruleproImg.png" class="rulebanner"/>
	    <p class="rule-title rule-titles">2.打开微信扫一扫，扫描卡片二维码，关注平台</p>
	    <img src="../../static/credit/ruleimg1.png" class="ruleimg1"/>
	    <p class="rule-title rule-titles">3.打开平台内菜单【我的】【扫一扫】</p>
	    <img src="../../static/credit/ruleimg02.png" class="ruleimg2"/>
	    <p class="rule-title rule-titles">4.积分产品说明</p>
	    <div class="rulebox" v-for="(item,index) in prolist1">
	    	<p class="rulebox-title">{{item.title}}</p>
	    	<div class="rule-z">
	    		<p><span class="red">{{item.pro}}</span>{{item.one}}</p>
	    		<p>{{item.two}}</p>
	    		<p>{{item.three}}</p>
	    	</div>
	    	<div class="rule-box">
	    		<div v-for="(item,index) in item.proarr">
	    			<img :src="item.img"/>
	    			<p class="rulebox-txt">{{item.text}}</p>
	    		</div>
	    	</div>
	    </div>
	    
    	<p class="btitle">二、箱码积分模式，针对已认证会员</p>
	    <img src="../../static/credit/rulebanners.png" class="rulebanner rulebanners"/>
	    <p class="rule-prompt">积分卡片示意图</p>
	    <p class="rule-title rule-titles">1.积分产品说明</p>
	    <div class="rulebox" v-for="(item,index) in prolist2">
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
	    
    	<p class="btitle">三、其他说明</p>
	    <p class="rule-other">1.该积分活动针对酒店终端用户开展，经销商、二批商等供货商不得参与，会员须知<span class="red" @click="backTop(0)">请点击此</span>查看。</p>
	    <p class="rule-other">2.积分兑好礼活动——2L味达美尚品生抽截止日期为2018年12月31日。</p>
	    <p class="rule-other rule-others">3.积分卡片有奖活动，每张卡片自其产品生产日期起两年内有效，逾期自动失效。</p>
    </div>
    
    </div>
  </div>
</template>

<script>
import { cookie } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
export default{
	components:{
		cookie
	},
	data(){
		return{
			userData:{},
			openId:'',
			timestamp:'',
      nonceStr:'',
      signature:'',
      url:'',//当前页面地址
      shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqvibLtjv92HOdZSu7ibVXiaC6JwSO20uKr43w4JXbn4sgdB7B74nBiafmolddyfZICbBVbGJZ6koYV2Yg/0?wx_fmt=jpeg',
			num:0,//默认会员须知
			prolist:[//扫码规则列表
			  {
				  title:'10积分/瓶',
				  pro:'注：',
				  one:'尚品生抽积分扫描有效截止时间：2018年12月31日。',
				  two:'2018年12月31日之前扫描该产品可正常获得积分并使用。',
				  three:'2018年12月31日之后活动失效，该积分产品无法继续扫描。',
				  proarr:[
				    {
							img:'../../static/credit/rule0.png',
							text:'2L味达美尚品生抽/1x6'
				    }
				  ]
			  },
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
			  },
			  {
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
			  },
			  {
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
			  },
			  {
					title:'100积分/箱',
					proarr:[
					  {
							img:'../../static/credit/rule7.png',
							text:'2L味达美剁椒鱼头鲜豉油/1x6'	
					  }
					]
			  },
			  {
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
		jump(link){//跳转到注册页
			this.$router.push(link);
		},
		tabClick(val){//tab 顶部切换
			this.num=val;
		},
		backTop(val){//回到顶部
			$(".membernotice").animate({scrollTop:0},300);
			this.num=val;
		},
		share(){
    	  var _this=this;
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
				            title:'会员须知',
				            link:apiUrl.getUrl+'/#/component/membernotice',
				            imgUrl:_this.shareicon,
				            success: function () { 
				              // 用户确认分享后执行的回调函数
				              _this.addECWXTranspondDetails(2);
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
										title:'会员须知', // 分享标题
										desc:'会员须知', // 分享描述
				            link:apiUrl.getUrl+'/#/component/membernotice',
										imgUrl:_this.shareicon,// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
//										  alert('分享给朋友成功');
										  _this.addECWXTranspondDetails(1);
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
    addECWXTranspondDetails(type){
    	var _this=this;
    	var params={
  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/membernotice",
  		  "Parameter":"",
  		  "OpenId":this.openId,
  		  "TranspondType":type
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addECWXTranspondDetails,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  		})
    }
	},
	computed:{
    prolist1:function(){
        return this.prolist.slice(0,1);
    },
    prolist2:function(){
        return this.prolist.slice(1);
    }
  },
	mounted(){
		//用户信息
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
	  
	  //分享
		var uri=window.location.href.split('#')[0];
	  this.url=encodeURIComponent(uri);
	  this.share();
	}
}
</script>

<style scoped>
	/*顶部*/
.member_top{line-height:40px;display: flex;justify-content: space-between;padding: 0px 20%;}
.member_top div{text-align: center; width: 40%;}
.member_tops{border-bottom: 1px solid #e83428;color:#e83428 ;}
.wrap{overflow: auto;}

/*会员须知*/
.notice-top{margin-top: 15px;}
.notice-title{display: inline-block;padding: 0px 20px 0px 10px;background: #e6222c;color: #fff;border-radius: 0 25px 25px 0;font-size: 14px;}
.notice-box{padding: 0px 10px 40px 10px;}
.membernotice p{font-size: 13px;color: #888888;margin-top: 20px;}
.smalltitle{color: #3E3E3E !important;}
.red{color: #e5212d;}
.title-origin{display: inline-block;width: 5px;height: 5px;border-radius: 50%;background: #e83632;position: relative;top: -1px;}
table{border:1px solid #ccc;color:#888;font-size: 12px;margin-top: 20px;}
table td{width: 32%;padding: 3px;}
.wrzbox{width: 40%;line-height: 25px;display: block;border: 1px solid #888;font-size: 12px;color: #888;margin-top: 15px;padding-left: 8px;}

/*活动规则*/
.btitle{font-size: 14px !important;color: #d50b17 !important;padding-left: 12px;letter-spacing: 1px;margin-top: 30px !important;}
.rule-title{font-size: 14px !important;color: #333333 !important;padding-left: 12px;margin-top: 16px;letter-spacing: 1px;}
.rule-titles{margin-top: 35px;}
.rulebanner{width:80%;margin-left: 10%;margin-top: 25px;}
.rulebanners{margin-top: 50px;}
.rule-prompt{font-size: 11px !important;text-align: center;color: #333333 !important;margin-top: 0px !important;}
.ruleimg1{width: 90%;margin-left: 5%;margin-top: 25px;}
.ruleimg2{width: 60%;margin-left: 20%;margin-top: 25px;}
.rule-box{width: 100%;display: flex;flex-wrap:wrap;}
.rule-box div{width: 30%;margin-left: 2.5%;}
.rule-box img{width: 100%;}
.rulebox-title{font-size: 12px !important;color: #d60d15 !important;text-align: center;margin-bottom: 20px;margin-top: 25px;}
.rulebox-txt{font-size: 10px !important;color: #333 !important;margin-top: 5px !important;}
.rule-other{padding:0 12px;font-size: 12px !important;color: #333 !important;margin-top: 10px !important;}
.rule-others{margin-bottom: 70px;}
.red{color: #d80c17;}
.rule-z p{font-size: 12px !important;color: #333 !important;padding-left:12px ;margin-top: 5px !important;text-indent: 24px;}
.rule-z p:nth-child(1){text-indent: 0px;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>