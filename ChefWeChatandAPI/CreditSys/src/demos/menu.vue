<template>
  <div class="menu">
     <div v-for="(item,index) in datalist" :key="index" @click="jump(item.link)" class="menuDiv">
     	 <img :src="item.img"/>
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
				timestamp:'',
		    nonceStr:'',
		    signature:'',
		    url:'',
		    shareicon:'',
		    userData:{},
		    openId:'',
				datalist:[
				  {
						img:'../../static/credit/menu_hy.jpg',
						link:'http://jifenweixin.shinho.net.cn/#/component/specialoyster'
					},
				  {
						img:'../../static/credit/menu_stj.jpg',
						link:'http://jifenweixin.shinho.net.cn/#/component/specialsoup'
					},
				  {
						img:'../../static/credit/menu_dzx.jpg',
						link:'https://mp.weixin.qq.com/s/20_bBtSxPO0paAXpObkXSw'
					},
				  {
						img:'../../static/credit/menu_ync.jpg',
						link:'https://mp.weixin.qq.com/s/-5ThH4s56rhUVdvDVk25TQ'
					},
				  {
						img:'../../static/credit/menu_hxf.jpg',
						link:'https://mp.weixin.qq.com/s/bKPMUzl3vmOJm1WKitZPzQ'
					},
				  {
						img:'../../static/credit/menu_rm.jpg',
						link:'https://mp.weixin.qq.com/s/1uwkCkhlsfrWtS0mOIrriQ'
					},
				  {
						img:'../../static/credit/menu_sc.jpg',
						link:'http://jifenweixin.shinho.net.cn/#/component/specialsoy'
					},
				  {
						img:'../../static/credit/menu_img7.png',
						link:'http://jifenweixin.shinho.net.cn/#/component/special'
					},
					{
						img:'../../static/credit/menu_img11.png',
						link:'http://u6879651.viewer.maka.im/k/09NLKVP8?from=singlemessage'
					},
				  {
						img:'../../static/credit/menu_img01.png',
						link:'http://mp.weixin.qq.com/s/7Ps9enYT5Pk9gVNgLE35SQ'
					},
					{
						img:'../../static/credit/menu_img2.png',
						link:'http://mp.weixin.qq.com/s/UvqeutBJi6xAkx-RYnKYGQ'
					},
					{
						img:'../../static/credit/menu_img3.png',
						link:'http://mp.weixin.qq.com/s/Y72soRqRcS7ni1I77kxziw'
					},
					{
						img:'../../static/credit/menu_img4.png',
						link:'http://u476561.viewer.maka.im/k/PQG6BMRK'
					},
					{
						img:'../../static/credit/menu_img5.png',
						link:'http://i.eqxiu.com/s/r9XPnzyh'
					},
					{
						img:'../../static/credit/menu_img6.png',
						link:'https://mp.weixin.qq.com/s?__biz=MjM5MTAyMzgwNA==&mid=201499990&idx=1&sn=f878cf89a94ab450887d2997161725ad#rd'
					},
					{
						img:'../../static/credit/menu_img8.png',
						link:'https://mp.weixin.qq.com/s?__biz=MjM5MTAyMzgwNA==&mid=200636265&idx=1&sn=cfdd3333e1dbb631e256e41f631910d3#rd'
					},
					{
						img:'../../static/credit/menu_img9.png',
						link:'https://mp.weixin.qq.com/s?__biz=MjM5MTAyMzgwNA==&mid=200619433&idx=1&sn=f17bf242f45022e558f41c63f4571e10#rd'
					},
					{
						img:'../../static/credit/menu_img10.png',
						link:'https://mp.weixin.qq.com/s?__biz=MjM5MDY0NDQzOQ==&mid=2649882258&idx=1&sn=4c0e1fab3b85e7b00a5961d47fe6308e&chksm=be44ff2a8933763c1cf55ce079d127ef5534d828a5abd187671febcbfad64063355b2f62fcda#rd'
					}
				]
			}
		},
		methods:{
			jump(link){
				window.location.href=link;
			},
			share(){//分享到朋友圈
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
								    title: '菜谱', // 分享标题
								    link: apiUrl.getUrl+'/#/component/menu', // 分享链接
										imgUrl: _this.shareicon, // 分享图标
										success: function () { 
										// 用户确认分享后执行的回调函数
										  _this.addECWXTranspondDetails(2);
										},
										cancel: function () {
	//										alert('取消');
										// 用户取消分享后执行的回调函数
										}
								});
			          wx.onMenuShareAppMessage({
										title: '菜谱', // 分享标题
										desc:  '菜谱', // 分享描述
										link:apiUrl.getUrl+'/#/component/menu', // 分享链接
										imgUrl: _this.shareicon,// 分享图标
										success: function () {
										// 用户确认分享后执行的回调函数
	//									  alert('分享成功');
										  _this.addECWXTranspondDetails(1);
										},
										cancel: function () {
	//										alert('取消');
										// 用户取消分享后执行的回调函数
										},
										fail:function(res){
	//						        alert(res.errMsg);
							      }
								});
	    			  })
			  			
			  		})
			  	})
	     },
	     addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
	      	var _this=this;
		    	var params={
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/menu",
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
	    var uri=location.href.split('#')[0];
			this.url=encodeURIComponent(uri);
			this.share();
		}
	}
</script>

<style scoped>
.menuDiv{width: 100%;height:162px;margin-top: 8px;}
.menuDiv img{width: 100%;height: 100%;}
</style>
<style>
#vux_view_box_body{background: #fff;}
</style>