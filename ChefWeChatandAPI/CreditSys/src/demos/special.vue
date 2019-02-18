<template>
	<div class="special"  style="width: 100%;height: 100%;overflow: auto;" >
	    <!--浮动图片-->
	    <div><img :src="floatImg" class="floatImg"  @click="floatClick"/></div>
	    
	    <div @scroll="handleScroll($event)" style="height: 100%;overflow: auto;position: relative;" ref="special">
			<div class="special_top"><img src="../../static/credit/special_banners.jpg" style="width: 100%;height: 100%;" /></div>
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
				<img :src="cookInfo.dishImg" style="width: 100%;" />
				<p class="title"><img src="../../static/credit/special_xd.png"></p>
				<div class="xd_text" >
					<p v-for="(item,index) in cookInfo.creativeMind">{{item.text}}</p>
				</div>
				<div class="introDiv">
					<div style="text-align: center;font-size: 14px;line-height: 25px;">
						<p>出品时间</p>
						<p style="margin-top: 10px;"><span style="font-size: 30px;">{{cookInfo.time}}</span>分钟</p>
					</div>
					<div style="text-indent: 1em;font-size: 13px;">
						<p style="margin-top: 3px;">原料成本 {{cookInfo.cost}}元</p>
						<p >建议售价 {{cookInfo.price}}元</p>
						<p style="font-size: 18px;">毛利率≈{{cookInfo.rate}}%</p>
					</div>
				</div>
				
				<div class="makeDiv">
				    <p class="title title1"><img src="../../static/credit/special_pl.png"></p>
				    <div class="pl_wrap">
					  	<div class="pl_box" v-for="(item,index) in cookInfo.material" :key="index">
							<span class="pl_type">{{item.type}}:</span>
							<span class="pl_text" v-html="item.text"></span>
						</div>
				    </div>
				    <p class="title title2"><img src="../../static/credit/special_make.png"></p>
				    <div class="makestepDiv">
				    	<p style="margin: 0 0 5px 7%;" v-for="(item,index) in cookInfo.makestep" :key="index" class="makestep">
							<span>{{index+1}}.</span>
							<span>{{item.step}}</span>
						</p>
						<img :src="cookInfo.productImg" style="width: 38%;margin-left: 31%;margin-top: 4px;" />
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
			    shareicon:'https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqszC98EbKqIWWECWYERho02Xm185ySb32Dovj1oGRNyibkHoxHRibr4GtoRF0ibYw0qbFx5Fxrb1JJJQ/0?wx_fmt=jpeg',
			    userData:{},
			    openId:'',
				flag: true,
				floatImg:'../../static/credit/floatImg1.png',
				dishList: [
					{
						imgUrl: '../../static/credit/special_dish1.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish2.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish3.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish4.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish5.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish6.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish7.jpg'
					},
					{
						imgUrl: '../../static/credit/special_dish8.jpg'
					}
				],
				cookInfo: {},
				cookbooklist: [
					{
						"dishName": "捞爽脆藕蛤肉",
						"dishImg": "../../static/credit/special_dish01.png",
						"productImg":"../../static/credit/special_cp2.png",
						"time":'10',
						"cost":'12',
						"price":'26',
						"rate":'53',
						"creativeMind":[
						    {
						   	  text:'充分运用味达美海鲜捞汁冰镇更清爽的特点'
						    },
						    {
						   	  text:'结合嫩脆的藕片'
						    },
						    {
						   	  text:'使菜品呈现清凉爽脆的效果。'
						    }
						],
						"material": [{
								type: "主料",
								text: "脆藕320g、蛤肉60g"
							},
							{
								type: "辅料",
								text: "<span style='color:#0e7dce'>黄飞红麻辣花生</span>50g、朝天椒圈3g、香葱末5g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美海鲜捞汁</span>120g、<span style='color:#0e7dce'>味达美清香米醋</span>28g、纯净水60g、白糖3g、红油28g、藤椒油6g"
							}
						],
						"makestep": [{
								step: "将嫩藕去皮洗净，切薄片在清水中洗净;"
							},
							{
								step: "藕片开水中烫熟，过凉水加清香米醋和冰纯净水浸泡;"
							},
							{
								step: "将藕片和蛤肉装盘，将冰镇的味达美海鲜捞汁淋上，撒花生碎、香葱等即可。"
							}
						]
					},
					{
						"dishName": "秋葵鲜虾仁",
						"dishImg": "../../static/credit/special_dish02.png",
						"productImg":"../../static/credit/special_cp3.png",
						"time":'8',
						"cost":'20',
						"price":'39',
						"rate":'48',
						"creativeMind":[
						    {
						   	  text:'秋葵的嫩与虾仁的鲜结合'
						    },
						    {
						   	  text:'口感爽滑鲜嫩'
						    },
						    {
						   	  text:'配以辣根，微辣更开胃。'
						    }
						],
						"material": [{
								type: "主料",
								text: "虾仁200g"
							},
							{
								type: "辅料",
								text: "秋葵200g、青红杭椒20g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美海鲜捞汁</span>200g、辣根5g、白糖30g、矿泉水100g"
							}
						],
						"makestep": [{
								step: "秋葵切丁拉油，青红杭椒切圈拉油，分别飞水过凉、虾仁飞水;"
							},
							{
								step: "味达美海鲜捞汁加辣根白糖、矿泉水调匀;"
							},
							{
								step: "秋葵垫底摆上虾仁，撒青红杭椒圈，淋上捞汁即可。"
							}
						]
					},
					{
						"dishName": "香菌捞汁鲜虾",
						"dishImg": "../../static/credit/special_dish03.png",
						"productImg":"../../static/credit/special_cp4.png",
						"time":'5',
						"cost":'15',
						"price":'38',
						"rate":'60',
						"creativeMind":[
						    {
						   	  text:'芥末和味达美海鲜捞汁的充分结合'
						    },
						    {
						   	  text:'最大程度的保护了香菇的香味和虾的蛋白质'
						    }
						],
						"material": [{
								type: "主料",
								text: "鲜香菇200g、基围虾100g"
							},
							{
								type: "辅料",
								text: "青椒20g、豇豆100g、小米辣10g、大蒜10g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美海鲜捞汁</span>50g、<span style='color:#0e7dce'>味达美臻品蚝油</span>15g、<span style='color:#0e7dce'>味达美清香米醋</span>20g、芥末10g、葱油30g"
							}
						],
						"makestep": [{
								step: "先将鲜香菇切花，飞水备用，鲜虾去壳切花，腌好，过油，飞水备用;青椒、豇豆过油，飞水备用；"
							},
							{
								step: "再用味达美海鲜捞汁调入味达美清香米醋、味达美臻品蚝油、小米辣、蔬菜水、芥末拌匀后淋在拌好的食材，淋上葱油即可。"
							}
						]
					},
					{
						"dishName": "捞汁有机观音菜",
						"dishImg": "../../static/credit/special_dish04.png",
						"productImg":"../../static/credit/special_cp5.png",
						"time":'5',
						"cost":'10',
						"price":'20',
						"rate":'50',
						"creativeMind":[
						    {
						   	  text:'观音菜含有铁元素，有补血的功效'
						    },
						    {
						   	  text:'加入捞汁和蜂蜜'
						    },
						    {
						   	  text:'既酸甜可口，又营养保健。'
						    }
						],
						"material": [{
								type: "主料",
								text: "有机观音菜400g"
							},
							{
								type: "辅料",
								text: "大蒜子20个、鲜泰椒8个、干辣椒15g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美海鲜捞汁</span>130g、<span style='color:#0e7dce'>味达美清香米醋</span>80g、<span style='color:#0e7dce'>味达美冰糖老抽</span>10g、蜂蜜100g、白糖30g"
							}
						],
						"makestep": [{
								step: "有机观音菜洗净待用；"
							},
							{
								step: "调料混合均匀放入拍蒜、泰椒段、干椒段；"
							},
							{
								step: "将调匀的调味料放入观音菜内即可。"
							}
						]
					},
					{
						"dishName": "捞汁黄瓜拌螺片",
						"dishImg": "../../static/credit/special_dish05.jpg",
						"productImg":"../../static/credit/special_cp6.png",
						"time":'8',
						"cost":'26',
						"price":'48',
						"rate":'44',
						"creativeMind":[
						    {
						   	  text:'青瓜切薄片冰镇后蘸食更脆爽'
						    },
						    {
						   	  text:'加入螺片使整道菜肴味道更富层次感'
						    }
						],
						"material": [{
								type: "主料",
								text: "青瓜300g、海螺500g"
							},
							{
								type: "辅料",
								text: "青椒20g、豇豆100g、小米辣10g、大蒜10g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美海鲜捞汁</span>100g、<span style='color:#0e7dce'>味达美臻品蚝油</span>50g、<span style='color:#0e7dce'>味达美清香米醋</span>50g、青芥辣7g、蜂蜜50g"
							}
						],
						"makestep": [{
								step: "青瓜洗净片成片加入冰水浸泡；"
							},
							{
								step: "海螺取肉片成片焯水过凉备用；"
							},
							{
								step: "将主辅料装盘蘸食即可。"
							}
						]
					},
					{
						"dishName": "酸辣牛蒡丝",
						"dishImg": "../../static/credit/special_dish06.png",
						"productImg":"../../static/credit/special_cp7.png",
						"time":'15',
						"cost":'7.7',
						"price":'18',
						"rate":'57',
						"creativeMind":[
						    {
						   	  text:'牛蒡针对糖尿病、高血压、高血脂、'
						    },
						    {
						   	  text:'抗癌等疾病的食疗作用，成本低'
						    },
						    {
						   	  text:'与味达美酸辣捞汁搭配后口感爽脆'
						    }
						],
						"material": [{
								type: "主料",
								text: "加工好的牛蒡丝150g"
							},
							{
								type: "辅料",
								text: "胡萝卜30g、有机黄瓜50g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美酸辣捞汁</span>80g、<span style='color:#0e7dce'>味达美柠檬酱油沙拉汁</span>80g、藤椒油5g"
							}
						],
						"makestep": [{
								step: "将牛蒡洗净去皮切成2毫米的丝入凉水中浸泡后捞出，放入0度的冰箱保存；"
							},
							{
								step: "将辅料切成2毫米的丝，胡萝卜焯水过凉备用；"
							},
							{
								step: "将主辅料拌匀装盘后浇上勾兑好的味达美酸辣捞汁即可。"
							}
						]
					},
					{
						"dishName": "酸辣捞汁牛肉粒",
						"dishImg": "../../static/credit/special_dish07.png",
						"productImg":"../../static/credit/special_cp7.png",
						"time":'25',
						"cost":'28',
						"price":'58',
						"rate":'52',
						"creativeMind":[
						    {
						   	  text:'香气浓郁的牛肉加上味达美酸辣捞汁'
						    },
						    {
						   	  text:'让菜肴的口味更有层次感'
						    }
						],
						"material": [{
								type: "主料",
								text: "熟牛肉粒400g"
							},
							{
								type: "辅料",
								text: "野山椒30g、水萝卜丁50g、青豆粒10g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美酸辣捞汁</span>80g、<span style='color:#0e7dce'>味达美柠檬酱油沙拉汁</span>40g、香油10g"
							}
						],
						"makestep": [{
								step: "牛肉酱熟后切丁备用；"
							},
							{
								step: "味达美酸辣捞汁加野山椒、樱桃水萝卜、青豆粒，烧香香油调成汁；"
							},
							{
								step: "把酱牛肉放在杯中，浇上配料和步骤2中调配好的酸辣海鲜汁（汁不能太多，是牛肉的一半即可），用绿植点缀。"
							}
						]
					},
					{
						"dishName": "酸辣捞麻糊",
						"dishImg": "../../static/credit/special_dish08.png",
						"productImg":"../../static/credit/special_cp9.png",
						"time":'5',
						"cost":'12',
						"price":'28',
						"rate":'57',
						"creativeMind":[
						    {
						   	  text:'麻糊是味美价廉的高蛋白，高纤维素的半流质食品'
						    },
						    {
						   	  text:'造型清爽'
						    },
						    {
						   	  text:'加入味达美酸辣捞汁，色味俱全。'
						    }
						],
						"material": [{
								type: "主料",
								text: "麻糊一盒"
							},
							{
								type: "辅料",
								text: "小开洋2g、小干贝2g、蛤蜊干2g、葱姜蒜各1.5g"
							},
							{
								type: "调料",
								text: "<span style='color:#0e7dce'>味达美酸辣捞汁</span>120g、花椒油1g"
							}
						],
						"makestep": [{
								step: "麻糊用刀切厚片；"
							},
							{
								step: "小开洋、小干贝、蛤蜊干用净水浸泡，切成末；"
							},
							{
								step: "将切好的小开洋和小干贝、蛤蜊干摆盘,撒在麻糊上面，加入葱姜蒜和香菜；"
							},
							{
								step: "将味达美酸辣捞汁混合花椒油淋在麻糊上即可。"
							}
						]
					}
				]
			}
		},
		methods: {
			handleScroll(e) {//滑动事件
				//			console.log(this.$refs.special.scrollTop);
				var roll = this.$refs.special.scrollTop;
				if(roll >= 170) {
					this.flag = false;
				} else {
					this.flag = true;
				}
			},
			floatClick(){//点击窗口浮动
				if(this.floatImg == '../../static/credit/floatImg1.png'){//海鲜捞汁
					this.$router.push({path:'/component/productstoredetail',query:{ProductId:12,SpecificationId:24,ProductFirstId:6}})
				}else if(this.floatImg == '../../static/credit/floatImg2.png'){//酸辣捞汁
					this.$router.push({path:'/component/productstoredetail',query:{ProductId:4,SpecificationId:11,ProductFirstId:6}})
				}
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
								    title: '极简捞拌 鲜爽味蕾', // 分享标题
								    link: apiUrl.getUrl+'/#/component/special', // 分享链接
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
										title: '极简捞拌 鲜爽味蕾', // 分享标题
										desc:  '味达美捞汁系列菜谱', // 分享描述
										link:apiUrl.getUrl+'/#/component/special', // 分享链接
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
		  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/special",
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
						type: 'fraction'
					},
					navigation: {
						nextEl: '.swiper-button-next',
						prevEl: '.swiper-button-prev',
					},
					on: {
						slideChangeTransitionEnd: function(event) {
//							console.log(this.realIndex);
							if(this.realIndex <= 4){
								_this.floatImg = '../../static/credit/floatImg1.png';
							}else{
								_this.floatImg = '../../static/credit/floatImg2.png';
							}
							var index = this.realIndex; //居中slide的下标值
							//改变菜谱
							_this.cookInfo = _this.cookbooklist[index];
						}
					},
				});
				//给每个页码绑定跳转的事件 
				//			  $('.swiper-button-prev').click(function(){
				//			  	var index = mySwiper.activeIndex-2;
				//			  	if(index-3<0){
				//			  		index = 9 -(3 -index);
				//			  	}
				////			  	console.log(index);
				//			  	mySwiper.slideTo(index-3, 1000, false);
				//			  })
//						    $('.btn1').click(function(){
//							  	mySwiper.slideTo(i, 1000, false);
//							})

			})

		}
	}
</script>

<style scoped>
	.special {
		position: relative;
	}
	
	.floatImg{
		width: 22%;
		position: absolute;
		right: 0;
		top: 52%;
		z-index: 10000;
	}
	
	.special_top {
		height: 170px;
	}
	/*轮播*/
	
	.container {
		position: absolute;
		top: 170px;
	}
	
	.containers {
		position: fixed;
		top: 0px;
	}
	/*滑动到顶部时固定*/
	
	.swiper-container {
		width: 100%;
		background: #e9f3fc;
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
		background: #69bef3;
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
		height: 34px;
		line-height: 29px;
		background: url('../../static/credit/special_name.png') no-repeat center;
		margin-top: 20px;
		color: #fff;
		text-align: center;
		background-size: 100% 100%;
	}
	
	.title{
		text-align: center;
	}
	
	.title img{
		height: 25px;
    }
    
    .xd_text p{
    	text-align: center;
    	font-size: 14px;
    	color: #383838;
    }
    
    .introDiv{
    	display: flex;
    	height: 74px;
    	margin-left: 20%;
    	color: #fff;
    	margin-top: 20px;
    }
    
    .introDiv div:nth-child(1){
    	width: 24%;
    	height: 100%;
    	background: url('../../static/credit/special_bj1.png') no-repeat center;
    	background-size: 100% 100%;
    }
    
    .introDiv div:nth-child(2){
    	width: 50%;
    	height: 100%;
    	margin-left: 2%;
    	background: url('../../static/credit/special_bj2.png') no-repeat center;
    	background-size: 100% 100%;
    }
	
	.makeDiv{
		position: relative;
		margin-top: 10px;
		width: 86%;
		margin-left: 7%;
		height: 500px;
		background: url('../../static/credit/special_bk.png') no-repeat center;
    	background-size: 100% 100%;
	}
	
	.title1 img{
		height: 25px;
		margin-top: 26px;
	}
	
	.pl_box{margin-bottom: 5px;}
	
	.pl_type{
		display: inline-block;
		vertical-align: top;
		width: 20%;
		line-height: 30px;
		font-size: 16px;
		color: #fff;
		text-align: center;
		background: url('../../static/credit/special_t.png') no-repeat center;
    	background-size: 100% 100%;
	}
	
	.pl_text{
		margin-left: 2%;
		display: inline-block;
		width: 74%;
		height: 30px;
		font-size: 15px;
		color: #383838;
	}
	
	.title2{
		position: absolute;
    	top: 240px;
    	width: 100%;
    	text-align: center;
	}
	
	.title2 img{
		height: 25px;
	}
	
	.makestepDiv{
		margin-bottom: 20px;
		width: 100%;
		position: absolute;
		top:300px;
	}
	
	.makestep{
		font-size: 15px;
		color: #383838;
	}
	
	.makestep span:nth-child(1){
		display: inline-block;
		width: 7%;
		vertical-align: top;
	}
	
	.makestep span:nth-child(2){
		display: inline-block;
		width: 90%;
	}
	
</style>
<style>
	#vux_view_box_body {
		background: #fff;
	}
</style>