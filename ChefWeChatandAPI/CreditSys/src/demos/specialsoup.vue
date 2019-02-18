<template>
	<div class="specialsoup"  style="width: 100%;height: 100%;overflow: auto;" >
	    <!--浮动图片-->
	    <div><img :src="floatImg" class="floatImg"  @click="floatClick"/></div>
	    
	    <div @scroll="handleScroll($event)" style="height: 100%;overflow: auto;position: relative;" ref="special">
			<div class="special_top"><img src="../../static/credit/soup_banner.png" style="width: 100%;height: 100%;" /></div>
			<!--<div class="special_top"><img src="../../static/credit/soup_banner.png" style="width: 100%;" /></div>-->
			<!--轮播图-->
			<div style="height: 110px;"></div>
			<div class="swiper-container " :class="{container:flag == true,containers:flag == false}">
				<div class="swiper-wrapper">
					<div class="swiper-slide" v-for="(item,index) in dishList" :key="index">
						<img :src="item.imgUrl" />
					</div>
				</div>
				<!--分页-->
				<div class="swiper-pagination"></div>
				<!--左右按钮-->
				<div class="swiper-button-next"></div>
				<div class="swiper-button-prev"></div>
			</div>
			
			<div style="background: #e6e519;padding: 20px 0 60px 0;">
				<div class="dishname">{{cookInfo.dishName}}</div>
				<img :src="cookInfo.dishImg" style="width: 80%;margin-left:10%;margin-top: 20px;" />
				<!--创意心得-->
				<div class="xdBox">
					<p v-for="(item,index) in cookInfo.creativeMind" :key="index">{{item.text}}</p>
				</div>
				<!--备料-->
				<div class="blBox">
					<img src="../../static/credit/soup_bl.png" width="50%" style="margin-left: 3%;"/>
					<div class="pl_wrap">
					  	<div class="pl_box" v-for="(item,index) in cookInfo.material" :key="index">
							<span class="pl_type"><img :src="item.type"></span>
							<span class="pl_text" v-html="item.text"></span>
						</div>
				    </div>
				</div>
				<!--制作方法-->
				<div class="blBox">
					<img src="../../static/credit/soup_make.png" width="50%" style="margin-left: 3%;"/>
					<div class="make_wrap">
					  	<div class="pl_box" v-for="(item,index) in cookInfo.makestep" :key="index">
							<span class="pl_type makenum">{{index+1}}.</span>
							<span class="pl_text make_text">{{item.step}}</span>
						</div>
				    </div>
				</div>
			</div>
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
				timestamp:'',
			    nonceStr:'',
			    signature:'',
			    url:'',
			    shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqt2icMicU3HLu92a4cK1MDO0JV8nfsurmnDTicrN5kbicDrlRVQ3PogoeKUFTGBzeleDIrQnQAZRiaPz3g/0?wx_fmt=jpeg',
			    userData:{},
			    openId:'',
				flag: true,
				floatImg:'../../static/credit/soup_float.png',
				dishList: [
				    {
						imgUrl: '../../static/credit/soup_lb1.png'
					},
					{
						imgUrl: '../../static/credit/soup_lb2.png'
					},
					{
						imgUrl: '../../static/credit/soup_lb4.png'
					},
					{
						imgUrl: '../../static/credit/soup_lb5.png'
					},
					{
						imgUrl: '../../static/credit/soup_lb6.png'
					},
					{
						imgUrl: '../../static/credit/soup_lb3.png'
					}
				],
				cookInfo: {},
				cookbooklist: [
				    {
						"dishName": "酸汤海中鲜",
						"dishImg": "../../static/credit/soup_dish01.png",
						"creativeMind":[
						    {
						   	  text:'酸辣口味刺激味蕾，提升食材的附加值'
						    },
						    {
						   	  text:'使用分餐的器皿装入增加宴会的氛围'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "天鹅蛋肉160g，虾仁80g，贝丁80g"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"茭白200g，莴笋260g"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱45g，骨汤1300g，鸡粉4g，南瓜泥45g，食盐3g，文蛤精3g"
							}
						],
						"makestep": [
						    {
								step: "将海鲜及辅料分别焯水煮熟后装入器皿中备用；"
							},
							{
								step: "锅中加入辅料及调料调味后加入海鲜装入器皿中即可。"
							}
						]
					},
					{
						"dishName": "酸汤酱焗黄花鱼",
						"dishImg":"../../static/credit/soup_dish02.png",
						"creativeMind":[
						    {
						   	  text:'粤菜焗法，浓香厚味，鱼肉肥美，'
						    },
						    {
						   	  text:'保持食材本味；酸汤腌制，酸辣酱香。'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "黄花鱼2条（约300g/条）"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"大蒜仔260g，姜粒50g，干葱70g，鲜柠檬草茎8g，葱50g，姜片30g，香葱10g，青红尖椒末各15g"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱70g，葱伴侣黄豆酱70g，味达美尚品蚝油50g，鸡粉3g，白糖4g，香油15g，胡椒粉2g，花雕酒12g，藤椒油3g，葱姜油15g，白兰地酒30g"
							}
						],
						"makestep": [
						    {
								step: "将黄花鱼宰杀洗净从脊背片开，加入调料拌匀腌制后备用；"
							},
							{
								step: "取沙煲加入少许油，将蒜仔，香葱，姜粒，鲜柠檬草茎煸炒香，加入腌制的黄花鱼肉肉，盖上盖，中火焗11分钟，出锅时将白兰地酒到在锅盖上，点火增香。"
							}
						]
					},
					{
						"dishName": "酸汤三杂锅",
						"dishImg":"../../static/credit/soup_dish03.png",
						"creativeMind":[
						    {
						   	  text:'酸辣有嚼劲，汤浓色泽好，'
						    },
						    {
						   	  text:'开胃拒油腻'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "鸡杂片200g，熟猪肚条200g，熟猪大肠块200g"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"黄豆芽50g，姜片30g，大葱节30g，蒜瓣30g，海米20g，小米辣5g,青尖椒5g"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱80g，调料汁（味达美尚品生抽10g，味达美臻品料酒15g，鸡精8g，味精4g，白糖10g）食用油少许，骨汤1000g"
							}
						],
						"makestep": [
						    {
								step: "锅下油烧至六成热，加入姜片，大葱节，小米辣，青尖椒，蒜瓣炒香；"
							},
							{
								step: "将味达美酸汤酱放入骨汤中烧开,放入黄豆芽，海米，加入调料汁进行调味；"
							},
							{
								step: "放入三杂，煮至入味，装盘即可。"
							}
						]
					},
					{
						"dishName": "酸汤浸红虾",
						"dishImg": "../../static/credit/soup_dish04.png",
						"creativeMind":[
						    {
						   	  text:'红虾Q弹，味达美酸汤酱味道浓郁，'
						    },
						    {
						   	  text:'鸡汤味鲜，制作简单快速'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "阿根廷红虾8只"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"甜蜜红豆仁50g，大蒜适量"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱80g，浓鸡汤40g，葱油25g，白糖5g，白胡椒粉8g"
							}
						],
						"makestep": [
						    {
								step: "红虾去壳成虾仁，虾头备用，放入小葱、姜片、精盐、白胡椒粉腌制半个小时；"
							},
							{
								step: "锅内放入葱油，爆香蒜片，放入味达美酸汤酱煸炒出香味，放入鸡汤、白糖、白胡椒粉、熬制成酱汁。"
							}
						]
					},
					{
						"dishName": "酸汤海鲜汇",
						"dishImg": "../../static/credit/soup_dish05.png",
						"creativeMind":[
						    {
						   	  text:'成菜酸洌，微辣带鲜'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "鱿鱼花100g，北极贝100g，大虾仁100g，海参50g，菜心20g，金针菇30g，魔芋丝50g，酸菜30g"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"野山椒10g，泰椒10g"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱100g，鸡汁10g"
							}
						],
						"makestep": [
						    {
								step: "鱿鱼洗净打花刀，北极贝洗净，海参，虾仁，菜心，魔芋丝，金针菇一起焯水，捞出过凉；"
							},
							{
								step: "锅入鸡油烧热，下酸菜炒香，入酸汤酱、鸡汁、1000g水烧开。3下菜心，海参，魔芋丝，金针菇，鱿鱼烧40秒后放入北极贝，加野山椒泰椒出锅即可。"
							}
						]
					},
					{
						"dishName": "苏堤十里酸汤鸭",
						"dishImg": "../../static/credit/soup_dish06.png",
						"creativeMind":[
						    {
						   	  text:'汤汁酸鲜爽口，鸭肉味美可口'
						    },
						    {
						   	  text:'色泽金黄，酸辣味恰到好处'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soup_zl.png",
								text: "嫩鸭700g"
							},
							{
								type: "../../static/credit/soup_fl.png",
								text:"生姜片5片，菜籽油18g，猪油6g，高汤1000g"
							},
							{
								type: "../../static/credit/soup_tl.png",
								text:"味达美酸汤酱18g，味达美臻品料酒12g，绵白糖3g"
							}
						],
						"makestep": [
						    {
								step: "嫩鸭一只洗净切小块，冲水30分钟，冷菜下锅焯水，焯水后用清水洗净备用；"
							},
							{
								step: "净锅烧热，下菜籽油和猪油，炒香生姜片倒入鸭块大火炒香，烹臻品料酒，加味达美酸汤酱炒出香味，倒入烧开的高汤，没过鸭块大火开锅后转砂锅慢煨；"
							},
							{
								step: "汤汁收到过半，约煨35分钟即可。"
							}
						]
					}
				]
			}
		},
		methods: {
			handleScroll(e) {//滑动事件
				var roll = this.$refs.special.scrollTop;
				if(roll >= 155) {
					this.flag = false;
				} else {
					this.flag = true;
				}
			},
			floatClick(){//点击窗口浮动
				this.$router.push({path:'/component/productstoredetail',query:{ProductId:29,SpecificationId:45,ProductFirstId:6}})
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
								    title: '官宣！您的味达美酸汤酱电子菜谱到了！', // 分享标题
								    link: apiUrl.getUrl+'/?action=specialsoup', // 分享链接
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
										title: '官宣！您的味达美酸汤酱电子菜谱到了！', // 分享标题
										desc:  '鲜辣有度，酸爽当道', // 分享描述
										link:apiUrl.getUrl+'/?action=specialsoup', // 分享链接
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
			  		}).catch(function(){
//			  			_this.share();
			  		})
			  	}).catch(function(){
//			  		_this.share();
			  	})
	       },
	       addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
	      	    var _this=this;
		    	var params={
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/specialsoup",
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
		mounted() {
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
			
			this.$nextTick(function() {
				var _this = this;
				var mySwiper = new Swiper('.swiper-container', {
					loop: true,
					speed: 300,
					slidesPerView: 3,
					spaceBetween: 3, //间距
					centeredSlides: true,
					slideToClickedSlide: true, //点击的slide会居中
					pagination: {
						el: '.swiper-pagination',
						type: 'fraction',
					},
					navigation: {
						nextEl: '.swiper-button-next',
						prevEl: '.swiper-button-prev',
					},
					on: {
						slideChangeTransitionEnd: function(event) {
							if( this.realIndex== 4){
								$('.xdBox p').eq(0).css({
									'padding-top':'50px'
								})
							}
							var index = this.realIndex; //居中slide的下标值
							//改变菜谱
							_this.cookInfo = _this.cookbooklist[index];
						}
					},
				});
			})

		}
	}
</script>

<style scoped>
	.specialsoup {
		position: relative;
	}
	
	.floatImg{
		width: 12%;
		position: absolute;
		right: 4%;
		top: 278px;
		z-index: 10000;
	}
	
	.special_top {
		height: 155px;
	}
	/*轮播*/
	
	.container {
		position: absolute;
		top: 155px;
	}
	
	.containers {
		position: fixed;
		top: 0px;
	}
	/*滑动到顶部时固定*/
	
	.swiper-container {
		width: 100%;
		background: #f6fbfe;
		height: 110px;
	}
	
	.swiper-wrapper {
		width: 100%;
		height: 92px !important;
		margin-top:9px;
	}
	
	.swiper-slide {
		height: 92px !important;
		border-radius: 5px;
	}
	
	.swiper-slide img {
		width: 100%;
		height: 100%;
	}
	
	.swiper-button-next {
		background: url('../../static/credit/special_jts2.png') no-repeat center;
		background-color: rgba(0, 0, 0, 0.2);
		right: 0;
		width: 26px;
		height: 34px;
		border-radius: 5px 0 0 5px;
		background-size: 14px;
	}
	
	.swiper-button-prev {
		background: url('../../static/credit/special_jts1.png') no-repeat center;
		background-color: rgba(0, 0, 0, 0.2);
		left: 0;
		width: 26px;
		height: 34px;
		border-radius: 0 5px 5px 0;
		background-size: 14px;
	}
	
	.swiper-pagination {
		width: 33.3%;
		background: #f9ed1f;
		color: #522b0e;
		margin-left: 33.3%;
		line-height: 20px;
		position: relative;
		top: -19px;
	}
	
	.dishname{
		font-size: 18px;
		width: 46%;
		margin-left: 27%;
		height: 40px;
		line-height: 32px;
		color: #fde31a;
		text-align: center;
		background: url('../../static/credit/soup_dishback.png') no-repeat center;
		background-size: 100% 100%;
	}
	
	.xdBox{
		width: 80%;
		margin-left: 10%;
		background: url('../../static/credit/soup_txt.png') no-repeat center;
    	background-size: 100% 100%;
    	color: #341405;
    	text-align: center;
    	font-size: 14px;
    	margin-top: 10px;
    	height: 120px;
	}
	
	.xdBox p:nth-child(1){
		padding-top: 40px;
	}
	
	.blBox{
		margin-top: 20px;
	}
	
	.blBox .title{
		border-top: 1px #b0afad dashed;
		border-bottom: 1px #b0afad dashed;
		line-height: 30px;
		color: #43151f;
	}
	
	.blBox .title span{margin: 0 8% 0 4%;}
	
	.pl_wrap{
		background: url('../../static/credit/soup_matback.png') no-repeat center;
		background-size: 42%;
	}
	
	.pl_box{margin-top: 8px;}
	
	.pl_type{
		display: inline-block;
		vertical-align: top;
		padding: 0 1% 0 4%;
		width: 8%;
	}
	
	.pl_type img{
		width: 100%;
	}
	
	.makenum{
		width: 4%;
	}
	
	.pl_text{
		display: inline-block;
		width: 84%;
		height: 30px;
		font-size: 14px;
		color: #341302;
	}
	
	.make_text{
		width: 88%;
	}
	
</style>
<style>
	#vux_view_box_body {
		background: #fff;
	}
</style>