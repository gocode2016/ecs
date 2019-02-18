<template>
	<div class="specialsoy"  style="width: 100%;height: 100%;overflow: auto;" >
	    <!--浮动图片-->
	    <div><img :src="floatImg" class="floatImg"  @click="floatClick"/></div>
	    
	    <div @scroll="handleScroll($event)" style="height: 100%;overflow: auto;position: relative;" ref="special">
			<div class="special_top"><img src="../../static/credit/specialsoy_banner.jpg" style="width: 100%;height: 100%;" /></div>
			<!--<div class="special_top"><img src="../../static/credit/specialsoy_banner.jpg" style="width: 100%;" /></div>-->
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
			
			<div>
				<div class="dishname">{{cookInfo.dishName}}</div>
				<hr width="20%" height="2px" color="#4c1a23" style="margin: 0 auto;" />
				<img :src="cookInfo.dishImg" style="width: 100%;margin-top: 20px;" />
				<!--创意心得-->
				<div class="xdBox">
					<p v-for="(item,index) in cookInfo.creativeMind" :key="index">{{item.text}}</p>
				</div>
				<!--备料-->
				<div class="blBox">
					<p class="title"><span>备料</span>|  Preparation</p>
					<div class="pl_wrap">
					  	<div class="pl_box" v-for="(item,index) in cookInfo.material" :key="index">
							<span class="pl_type"><img :src="item.type"></span>
							<span class="pl_text" v-html="item.text"></span>
						</div>
				    </div>
				</div>
				<!--制作方法-->
				<div class="blBox">
					<p class="title"><span>制作方法</span>|  Making method</p>
					<div class="make_wrap">
					  	<div class="pl_box" v-for="(item,index) in cookInfo.makestep" :key="index">
							<span class="pl_type makenum">{{index+1}}.</span>
							<span class="pl_text make_text">{{item.step}}</span>
						</div>
				    </div>
				</div>
			   <img :src="cookInfo.productImg" class="botimg"/>
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
			    shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqutHkzAiaBeluQdIK35JTric9tTN0vibwRNZajriaSUvT8lEId8aNynZytLeuX7eJR2cSjjHpf6nxSibVg/0?wx_fmt=jpeg',
			    userData:{},
			    openId:'',
				flag: true,
				floatImg:'../../static/credit/soy_float.png',
				dishList: [
				    {
						imgUrl: '../../static/credit/specialsoy_dish5.png'
					},
					{
						imgUrl: '../../static/credit/specialsoy_dish01.jpg'
					},
					{
						imgUrl: '../../static/credit/specialsoy_dish2.png'
					},
					{
						imgUrl: '../../static/credit/specialsoy_dish3.png'
					},
					{
						imgUrl: '../../static/credit/specialsoy_dish4.png'
					},
					{
						imgUrl: '../../static/credit/specialsoy_dish6.png'
					}
				],
				cookInfo: {},
				cookbooklist: [
				    {
						"dishName": "野山蒜爆松板肉",
						"dishImg": "../../static/credit/soy_dish5.png",
						"productImg":"../../static/credit/soy_product2.png",
						"creativeMind":[
						    {
						   	  text:'野山蒜原先一直和海鲜搭配，成菜成本较高，'
						    },
						    {
						   	  text:'现在和肉类原料相结合既体现不一样的风味'
						    },
						    {
						   	  text:'也降低了成本。'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "松板肉250g"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"泡开野山蒜2两、青蒜粒5g"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"味达美尚品生抽20g、味达美尚品蚝油15g、味达美冰糖老抽5g、糖15g、黄油5g、水5g"
							}
						],
						"makestep": [
						    {
								step: "松板肉切条，入200度油锅炸50秒，捞出沥油；"
							},
							{
								step: "把泡好的野山蒜挤干水分入油锅炸香，捞出沥油；"
							},
							{
								step: "洗净锅，倒入调味料收浓后倒入炸好的松板肉和野山蒜，急火炒匀，收紧剩余料汁，出锅前撒入青蒜粒即可。"
							}
						]
					},
					{
						"dishName": "三月瓜爆爽肉",
						"dishImg": "../../static/credit/soy_dish01.png",
						"productImg":"../../static/credit/soy_product1.png",
						"creativeMind":[
						    {
						   	  text:'发挥食材本质特点'
						    },
						    {
						   	  text:'呈现多种口味'
						    },
						    {
						   	  text:'层次丰富。'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "猪颈肉150g"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"三月瓜80g、山药80g、小米辣10g、蒜瓣10g"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"味达美尚品生抽10g、味达美尚品蚝油10g、味达美味极鲜酱油5g、黑胡椒碎1g、生粉5g、盐10g、花生油100g"
							}
						],
						"makestep": [
						    {
								step: "三月瓜洗净后去掉里面的黄心，改刀成波浪块，山药去皮改刀成波浪块；"
							},
							{
								step: "猪颈肉洗净后改刀成0.5厘米x3厘米大小的条，加入盐1g，腌制好后上少许生粉；"
							},
							{
								step: "锅中加入水烧开后下入盐8g，下入切好的山药煮到五成熟时下入三月瓜一起煮至八成熟，捞起备用；"
							},
							{
								step: "锅中加入花生油，烧热后下入煮过的山药与三月瓜，快速煎至金黄色，倒出备用；"
							},
							{
								step: "用味达美尚品生抽10g、味达美尚品蚝油10g、味达美味极鲜5g、黑胡椒碎1g、盐1g，兑成味汁；"
							},
							{
								step: "锅中重新加油烧热后放入猪颈肉煎制，快要熟时放入小米辣与蒜瓣，炒至出味后加入提前煎制好的山药与三月瓜，再加入兑好的味汁，炒匀即可。"
							}
						]
					},
					{
						"dishName": "天府尚品爆雪龙牛",
						"dishImg": "../../static/credit/soy_dish2.png",
						"productImg":"../../static/credit/soy_product2.png",
						"creativeMind":[
						    {
						   	  text:'简单的一道家常菜'
						    },
						    {
						   	  text:'创意来源小炒肉'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "特选级牛肉300g"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"先青椒50g、红椒10g、涪陵榨菜1包、葱段5g、蒜碎5g"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"味达美尚品生抽30g、味达美尚品蚝油2g、味达美冰糖老抽1g、猪油适量、花生油适量、黄油3g、生粉2g"
							}
						],
						"makestep": [
						    {
								step: "将特选级牛肉改刀切成薄片，用味达美尚品生抽、生粉腌制2分钟，再用平底锅加黄油轻煎至七成熟；"
							},
							{
								step: "涪陵榨菜用水冲洗一下，入锅下猪油煸炒；"
							},
							{
								step: "青椒、红椒切成圈状，加入葱段、蒜碎，用猪油爆炒香，再倒入味达美尚品生抽、味达美尚品蚝油、味达美冰糖老抽，快速翻炒均匀，装盘即可。"
							}
						]
					},
					{
						"dishName": "新派孔府金桔红烧肉",
						"dishImg": "../../static/credit/soy_dish3.png",
						"productImg":"../../static/credit/soy_product3.png",
						"creativeMind":[
						    {
						   	  text:'红烧肉加入金桔，'
						    },
						    {
						   	  text:'搭配米饭，口感微甜不腻，'
						    },
						    {
						   	  text:'更易入口。'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "上等带皮五花肉500g"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"金桔70g、杏仁片5g、煮好的大米400g、香菜6g、香葱5g、薄荷叶1棵、姜适量"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"味达美尚品生抽60g、味达美味极鲜酱油20g、味达美冰糖老抽4g、红星二锅头20g、白糖40g、冰糖40g、十年古越龙山花雕酒1瓶"
							}
						],
						"makestep": [
						    {
								step: "将上等五花肉加葱姜，中火煮10分钟捞出过凉；"
							},
							{
								step: "将过凉的五花肉改刀成2.5厘米x2.5厘米的块，改好后用牙签在侧部反复穿插几次，保证煨制时更好的入味，排出多余油脂；"
							},
							{
								step: "把改好的五花肉放入砂锅，加入提前勾兑好的汁水、香菜、香葱，大火煮沸，转小火煨制45分钟，加入改刀好的金桔，大火收汁；"
							},
							{
								step:"把做好的红烧肉放入提前正好的米饭中装盘，最后加入杏仁片，薄荷叶点缀即可。"
							}
						]
					},
					{
						"dishName": "尚品香辣虾",
						"dishImg": "../../static/credit/soy_dish4.png",
						"productImg":"../../static/credit/soy_product2.png",
						"creativeMind":[
						    {
						   	  text:'川菜的香辣风味'
						    },
						    {
						   	  text:'结合粤菜白灼汁的技法熬制的汁水烹制菜品'
						    },
						    {
						   	  text:'口味新颖独特。'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "明虾400g；"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"①剁椒60g、红椒丁90g、蒜粒70g、青椒丁90g；②尖椒70g、姜片15g、胡萝卜70g、芹菜20g、香菜15g、圆葱60g、纯净水3斤"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"①味达美尚品生抽200g、味达美尚品蚝油90g、味达美冰糖老抽10g、味达美味极鲜150g、糖100g、鱼露50g、鸡粉5g、蔬菜水1000g（以上勾兑成大兑汁）；②葱姜油35g、藤椒油7g、油1200g；"
							}
						],
						"makestep": [
						    {
								step: "明虾从腹部改刀片开洗净备用"
							},
							{
								step: "锅中加入油烧到180℃，下入虾炸外焦里嫩；"
							},
							{
								step: "将辅料②熬成蔬菜水加入调料①勾兑成汁水备用；"
							},
							{
								step:"锅中加入葱姜油、蒜仔爆香后加剁椒炒香，加辅料①炒香烹入兑好的汁水35克，加入炸好的虾翻炒，淋上藤椒油即可；"
							}
						]
					},
					{
						"dishName": "荷兰豆开洋炒猪肚",
						"dishImg": "../../static/credit/soy_dish6.png",
						"productImg":"../../static/credit/soy_product4.png",
						"creativeMind":[
						    {
						   	  text:'荷兰豆和猪肚的荤素搭配'
						    },
						    {
						   	  text:'加入开洋另菜式'
						    },
						    {
						   	  text:'融入了别样的的海鲜风格'
						    }
						],
						"material": [
						    {
								type: "../../static/credit/soy_icon1.png",
								text: "猪肚250g"
							},
							{
								type: "../../static/credit/soy_icon2.png",
								text:"荷兰豆100g、姜片30g、开洋15g、红椒30g、葱段20g、蒜米3g"
							},
							{
								type: "../../static/credit/soy_icon3.png",
								text:"味达美尚品生抽10g、花雕酒15g、白糖3g"
							}
						],
						"makestep": [
						    {
								step: "将猪肚内部油脂剔除干净后，煮熟切条；"
							},
							{
								step: "荷兰豆、红椒切片，开洋泡发备用；"
							},
							{
								step: "炒香葱、姜、蒜后放入荷兰豆、开洋、猪肚条烹入调味料，急火快速炒匀即可出锅。"
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
				this.$router.push({path:'/component/productstoredetail',query:{ProductId:5,SpecificationId:12,ProductFirstId:1}})
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
								    title: '好的生抽懂得调和食材的本味', // 分享标题
								    link: apiUrl.getUrl+'/?action=specialsoy', // 分享链接
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
										title: '好的生抽懂得调和食材的本味', // 分享标题
										desc:  '味达美尚品生抽电子菜谱', // 分享描述
										link:apiUrl.getUrl+'/?action=specialsoy', // 分享链接
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
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/specialsoy",
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
							if(this.realIndex == 3 || this.realIndex == 5){
								$('.botimg').css({
									width:'30%',
									margin:'40px 35% 20px 35%'
								})
							}else{
								$('.botimg').css({
									width:'46%',
									margin:'40px 27% 20px 27%'
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
	.specialsoy {
		position: relative;
	}
	
	.floatImg{
		width: 15%;
		position: absolute;
		right: 0;
		top: 262px;
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
		background: #964240;
		color: #fff;
		margin-left: 33.3%;
		line-height: 20px;
		position: relative;
		top: -19px;
	}
	
	.dishname{
		font-size: 18px;
		width: 56%;
		margin-left: 22%;
		height: 30px;
		margin-top: 20px;
		color: #41161f;
		text-align: center;
		background-size: 100% 100%;
	}
	
	.xdBox{
		width: 86%;
		margin-left: 7%;
		background: url('../../static/credit/soy_txt.jpg') no-repeat center;
    	background-size: 100% 100%;
    	color: #341405;
    	text-align: center;
    	font-size: 14px;
    	margin-top: 40px;
    	height: 150px;
	}
	
	.xdBox p:nth-child(1){
		padding-top: 50px;
	}
	
	.blBox{
		margin-top: 40px;
	}
	
	.blBox .title{
		border-top: 1px #b0afad dashed;
		border-bottom: 1px #b0afad dashed;
		line-height: 30px;
		color: #43151f;
	}
	
	.blBox .title span{margin: 0 8% 0 4%;}
	
	.pl_wrap{
		background: url('../../static/credit/soy_back1.jpg') no-repeat center;
		background-size: 100%;
	}
	
	.make_wrap{
		background: url('../../static/credit/soy_back2.png') no-repeat center;
		background-size: 100%;
	}
	
	.pl_box{margin-top: 20px;}
	
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
	
	.botimg{
		width: 46%;
		margin: 40px 27% 20px 27%;
	}
	
</style>
<style>
	#vux_view_box_body {
		background: #fff;
	}
</style>