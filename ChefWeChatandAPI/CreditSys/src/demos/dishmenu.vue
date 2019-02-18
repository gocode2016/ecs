<template>
	<div class="dishmenu">
		<div v-for="(item,index) in datalist" :key="index" @click="jump(item.url)" class="menuDiv">
			<img :src="item.img"/>
		</div>
	</div>
</template>

<script>
	import { cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
	import wx from 'weixin-js-sdk'
	export default {
		components: {
			cookie
		},
		data() {
			return {
				timestamp: '',
				nonceStr: '',
				signature: '',
				url: '',
				shareicon: '',
				userData: {},
				openId: '',
				datalist: [
				    {
					  url: '/component/specialoyster',
					  img: '../../static/credit/dish_cp1.jpg'
					},
					{
					  url: '/component/specialsoup',
					  img: '../../static/credit/dish_cp2.jpg'
					},
					{
					  url: '/component/specialsoy',
					  img: '../../static/credit/dish_cp3.jpg'
					},
					{
					  url: 'http://i.eqxiu.com/s/r9XPnzyh',
					  img: '../../static/credit/dish_cp4.jpg'
					},
					{
					  url: 'https://mp.weixin.qq.com/s/7Ps9enYT5Pk9gVNgLE35SQ',
					  img: '../../static/credit/dish_cp5.jpg'
					},
					{
					  url: '/component/special',
					  img: '../../static/credit/dish_cp6.jpg'
					}
				]
			}
		},
		methods: {
			getCaiPinList(){//获取菜品列表
				var self = this;
				var params = {
			    "page": 1,
			    "pagesize": 100,
			    "OrderBy": 'UpdateDate',
			    "SecondId": '',
			    "BanKuaiName": '产品菜谱',
			    "CaiPinName": '',
			    "OpenId":this.openId
				}
				this.$http({
					method:'post',
					url:apiUrl.getCaiPinList,
					data:params
				}).then(function(res){
					console.log(JSON.parse(res.data));
					var dataArr = JSON.parse(res.data).data;
					for(let i in dataArr){
						var data = dataArr[i];
						if(data.Images != null){
					  		data.Images = data.Images.split(',')[0];
          	            }
					}
					self.datalist = dataArr;
				})
			},
			jump(link) {
				if(link.indexOf('component')>0){
					this.$router.push(link);
				}else{
					window.location.href = link;
				}
			},
			share() { //分享到朋友圈
				var _this = this;
				this.$http({
					method: 'GET',
					url: apiUrl.getTimestampAndNonceStr
				}).then(function(response) {
					var data = response.data;
					var dataArr = data.split('|');
					_this.timestamp = parseInt(dataArr[0]); //时间戳
					_this.nonceStr = dataArr[1]; //随机字符串

					var params = '?noncestr=' + _this.nonceStr + '&timestamp=' + _this.timestamp + '&url=' + _this.url;
					_this.$http({
						method: 'GET',
						url: apiUrl.getJsapiTicketSignature + params
					}).then(function(response) {
						_this.signature = response.data;

						wx.config({
							debug: false,
							appId: apiUrl.appId,
							timestamp: _this.timestamp,
							nonceStr: _this.nonceStr,
							signature: _this.signature,
							jsApiList: [
								'onMenuShareTimeline', //分享到朋友圈
								'onMenuShareAppMessage' //分享给朋友
							]
						})

						wx.ready(function() {
							wx.onMenuShareTimeline({
								title: '菜谱', // 分享标题
								link: apiUrl.getUrl + '/#/component/menu', // 分享链接
								imgUrl: _this.shareicon, // 分享图标
								success: function() {
									// 用户确认分享后执行的回调函数
									_this.addECWXTranspondDetails(2);
								},
								cancel: function() {
									//										alert('取消');
									// 用户取消分享后执行的回调函数
								}
							});
							wx.onMenuShareAppMessage({
								title: '菜谱', // 分享标题
								desc: '菜谱', // 分享描述
								link: apiUrl.getUrl + '/#/component/menu', // 分享链接
								imgUrl: _this.shareicon, // 分享图标
								success: function() {
									// 用户确认分享后执行的回调函数
									//									  alert('分享成功');
									_this.addECWXTranspondDetails(1);
								},
								cancel: function() {
									//										alert('取消');
									// 用户取消分享后执行的回调函数
								},
								fail: function(res) {
									//						        alert(res.errMsg);
								}
							});
						})

					})
				})
			},
			addECWXTranspondDetails(type) { //转发页面记录  1转发给朋友  2转发到朋友圈
				var _this = this;
				var params = {
					"ECBrowseURL": apiUrl.getUrl + "/#/component/menu",
					"Parameter": "",
					"OpenId": this.openId,
					"TranspondType": type
				}
				this.$http({
					method: 'post',
					url: apiUrl.addECWXTranspondDetails,
					data: params
				}).then(function(response) {
					//			console.log(response.data);
				})
			}
		},
		mounted() {
			//用户信息
			this.userData = cookie.get('WeiXinUser', function(val) {
				var a = val.split("&");
				var obj = {};
				for(var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
			})
			this.openId = this.userData.Openid;
//			this.getCaiPinList();//获取菜品列表

			//分享
			//		    var uri=location.href.split('#')[0];
			//			this.url=encodeURIComponent(uri);
			//			this.share();
		}
	}
</script>

<style scoped>
	.menuDiv {
		width: 100%;
		/*height: 162px;*/
		margin-top: 8px;
	}
	
	.menuDiv img {
		width: 100%;
		/*height: 100%;*/
	}
</style>
<style>
	#vux_view_box_body {
		background: #fff;
	}
</style>