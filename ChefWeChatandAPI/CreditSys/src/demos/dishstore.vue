<template>
  <div class="dishstore">
  	
    <!--搜索-->
    <div class="searchDiv">
    	<input type="text" placeholder="搜索菜名\食材" @input="searchChange" v-model="searchValue" />
    	<img src="../../static/credit/search.png" @click="searchClick"/>
    	<div class="keyStyle" v-show="searchFlag">
	    	<p v-for="(item,index) in keyList" @click="keyClick(item.CaiPinName,item.CaiPinId)">{{item.CaiPinName}}</p>
	    </div>
    </div>
    <div style="display: inline-block;width:18%;position: relative; right: -6%;">
    	<span style="color: #3E3E3E;font-size: 13px;">分类</span>
    	<img src="../../static/credit/dishstore_menu.png" @click="jump('/component/dishcookbook')" style="width: 36%;vertical-align: middle; "/>
    </div>
    
    <!--轮播图-->
		<div class="swiper-container">
				<div class="swiper-wrapper">
					<div class="swiper-slide" v-for="(item,index) in bannerList" :key="index">
						<img :src="item.PicUrl" @click="jump(item.Link)"/>
					</div>
				</div>
				<!--分页-->
				<div class="swiper-pagination"></div>
		</div>
    
    <div style="margin-top: 40px;">
	    <!--图标按钮-->
	    <grid :rows="4">
	      <grid-item v-for="(item, index) in iconBtnList" :key="index" :link="item.url" :label="item.title">
	        <img slot="icon" :src="item.img">
	      </grid-item>
	    </grid>
	    
	    <!--上榜新菜-->
	    <div class="sbxcDiv">
	    	<p class="dish_title"><span>上榜新菜</span></p>
		    <div class="dishWrap">
		    	<div class="dishBox" v-for="(item,index) in sbxcList" :key="index">
		    		<div><img :src="item.Images" @click="lookDish(item.CaiPinId)"></div>
		    		<p @click="lookDish(item.CaiPinId)">{{item.CaiPinName}}</p>
		    		<p><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,'sbxc',index)"><span>{{item.LlCount}}人看过</span></p>
		    	</div>
				</div>
      	<iframe frameborder='0' allowtransparency="false" allowfullscreen='true' :src='videoUrl' width="94%" height="100%" style="margin:10px 3% 0 3%;"  id="myIframe" @load="hideImg"></iframe>
      	<!--<div style="width: 94%;margin: 0 3%;">
      		<video :src='videoUrl' width="100%" controls="controls">您的浏览器不支持 video 标签。</video>
      	</div>-->
	    </div>
	    
	    <!--当月人气-->
	    <div class="dyrqDiv">
				<p class="dish_title dish_titles" @click="jump('/component/dishmonthpop')"><span>当月人气</span></p>
	    	<div class="dyrq_box" v-for="(item,index) in dyrqList" :key="index" style="margin-bottom: 10px;">
	    		<div style="position: relative;">
	    			<img :src="item.Images" style="width: 100%;" @click="lookDish(item.CaiPinId)"/>
	    			<div class="decstyle">{{index+1}}</div>
	    		</div>
	    		<div>
	    			<p style="margin-top:7px;" @click="lookDish(item.CaiPinId)">{{item.CaiPinName}}</p>
	    			<p class="intro_a"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,'dyrq',index)"><span>{{item.LlCount}}人看过</span></p>
	    			<p class="intro_b">上榜理由</p>
	    			<p class="intro_c" v-html="item.Sbly"></p>
	    		</div>
	    	</div>
	    </div>
	    
	    <!--旺店热卖-->
	    <div class="wdrmDiv">
	    	<p class="dish_title dish_titles"  @click="jump('/component/dishhotshop')"><span>旺店热卖</span></p>
	    	<div class="wdrm_wrap">
	    		<div class="wdrm_box" v-for="(item,index) in wdrmList" :key="index">
			    		<div><img :src="item.Images" style="width: 100%;" @click="lookDish(item.CaiPinId)"/></div>
			    		<p class="wdrm_a">{{item.city}}</p>
			    		<p class="wdrm_b" @click="lookDish(item.CaiPinId)">{{item.CaiPinName}}</p>
			    		<p class="wdrm_c"><img :src="item.collectImg" @click="collectClick(item.CaiPinId,item.collectImg,'wdrm',index)"><span>{{item.LlCount}}人看过</span></p>
	    		</div>
	    	</div>
	    </div>
	    
	    <!--产品菜谱-->
	    <div class="cpcpDiv">
	    	<p class="dish_title dish_titles" @click="jump('/component/dishmenu')"><span>产品菜谱</span></p>
	    	<div class="cpcp_wrap">
	    		<div class="cpcp_box" v-for="(item,index) in cpList" :key="index">
	    			<img :src="item.img" @click="jump(item.url)"/>
	    		</div>
	    	</div>
	    </div>
  	</div>
    
    <!--备案-->
    <div style="position: relative;margin-top: 20px;margin-bottom: 80px;" >
    	<span class="record" @click="recordClick">鲁ICP备10013060号</span>
    </div>
    
    <!--遮罩指引层-->
    <div class="mask" v-show="masknum<=3" @click="maskClick">
    	<div class="cpDiv" v-show="masknum==1">
    		<!--全部菜谱指引-->
    		<img src="../../static/credit/dishstore_ts1.png" />
    		<img src="../../static/credit/dishstore_txt1.png" />
    	</div>
    	<div class="flDiv" v-show="masknum==2">
    		<!--分类指引-->
    		<img src="../../static/credit/dishstore_ts2.png" />
    		<img src="../../static/credit/dishstore_txt2.png" />
    	</div>
    	<div class="vedioDiv" v-show="masknum==3">
    		<!--视频学菜指引-->
    		<img src="../../static/credit/dishstore_ts3.png" />
    		<img src="../../static/credit/dishstore_txt3.png" />
    	</div>
    	<!--<div class="scDiv">
    		<!--视频学菜指引-->
    		<!--<img src="../../static/credit/dishstore_ts4.png" />-->
    		<!--<img src="../../static/credit/dishstore_txt4.png" />-->
    	<!--</div>-->
    </div>
      
  </div>
</template>

<script>
	import {Grid, GridItem,Cell,cookie } from 'vux'
	import apiUrl from '../apiUrls.js'
	
	export default{
		components:{
			Grid, 
			GridItem,
			Cell,
			cookie
		},
		data(){
			return{
				masknum:1,
				userData:{},
			  openId:'',
				searchFlag:false,//搜索框下拉
				searchValue:'',
				searchcpId:'',
				keyList:[],//搜索下拉框值
				bannerIndex:0,
				sbxcList:[],//上榜新菜
				dyrqList:[],//当月人气
				wdrmList:[],//旺店热卖
				cpList:[//产品菜谱
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
					}
				],
				bannerList:[],
				iconBtnList:[
					{
					  url: '/component/dishcookbook',
					  img: '../../static/credit/dish_icon1.png',
					  title: '全部菜谱'
					},
					{
					  url: '/component/dishstarkitchen',
					  img: '../../static/credit/dish_icon2.png',
					  title: '星厨星菜'
					},
					{
					  url: '/component/dishvideolearn',
					  img: '../../static/credit/dish_icon3.png',
					  title: '视频学菜'
					},
					{
					  url: 'https://mp.weixin.qq.com/mp/homepage?__biz=MjM5MDY0NDQzOQ==&hid=1&sn=8689486d1e23845c19e3093410c3c554',
					  img: '../../static/credit/dish_icon4.png',
					  title: '千滋百味'
					}
				],
    		videoUrl:'https://v.qq.com/txp/iframe/player.html?vid=e0815za8jvw'//视频地址
//  		videoUrl:'https://v.qq.com/txp/iframe/player.html?vid=m05441d4gcj'//视频地址
//  		videoUrl:'https://s3.cn-north-1.amazonaws.com.cn/s3-010-shinho-ecshool-prd-bjs/mov/DJI_0050.MP4'//视频地址
			}
		},
		methods:{
			recordClick(){//点击备案
				window.location.href = 'http://www.miitbeian.gov.cn/state/outPortal/loginPortal.action;jsessionid=yZGh3fHt8OZyxjnAk5z89RzZPqOh3mGi-rolRbIEDyLR4-2KQcQp!-736616813'
			},
			maskClick(){//点击遮罩层
				this.masknum++;
				if(this.masknum>=4){
					$('.dishstore').css({
				  	'overflow':'scroll'
			    })
				}
			},
			searchChange(){//获取搜索框下拉值
				this.searchcpId = '';//菜品id设置为空
				if(this.searchValue !='' ){
					var _this = this;
					var params = {
						"CpName": this.searchValue,
    				"BkName": ""
					}
					this.$http({
						method:'post',
						url:apiUrl.getCaiPinNameListByParms,
						data:params
					}).then(function(res){
//						var data = JSON.parse(res.data);
						var data = res.data;//测试
						console.log(data);
						_this.keyList = data;
						_this.searchFlag = true;
					})
				}else{
					this.searchFlag = false;
				}
			},
			keyClick(cpName,cpId){//关键字赋值到搜索框
				this.searchValue = cpName;//菜品名
				this.searchcpId = cpId;//菜品id
			  this.searchFlag = false;//隐藏下拉框
			},
			searchClick(){//点击搜索图标
				if(this.searchValue == ''){
					alert('请输入搜索内容');
				}else{
				  this.$router.push({path:'/component/dishsearchlist',query:{searchValue:this.searchValue}});
				}
			},
	    getBanner(){//获取轮播图
    	  	var self = this;
    	  	var params={
  		      "BannerType":3
		  		}
		  		this.$http({
		  			method:'post',
		  			url:apiUrl.getBanner,
		  			data:params
		  		}).then(function(response){
		  			var obj = response.data;
						self.bannerList = JSON.parse(obj);
						console.log(self.bannerList);
						
						self.$nextTick(function() {//swiper
								var mySwiper = new Swiper('.swiper-container', {
									loop: true,
									autoplay : true,
									effect: 'coverflow',
						      grabCursor: true,
						      centeredSlides: true,
						      slidesPerView: 'auto',
						      coverflowEffect: {
						        rotate: 10,
						        stretch: 1,
						        depth: 100,
						        modifier: 1,
						        slideShadows : true,
						      },
									pagination: {
										el: '.swiper-pagination',
		//              type: 'fraction',
									},
									on: {
										slideChangeTransitionEnd: function(event) {
											for (var i = 0; i < $(".swiper-pagination-bullet").length; i++) {
												$(".swiper-pagination-bullet").eq(i).removeClass("swiper-pagination-bullet-active");
											}
											$(".swiper-pagination-bullet").eq(this.realIndex).addClass("swiper-pagination-bullet-active");
											
										}
									},
						    })
					  })
						
		  		})
    	},
			getDishList(){//获取各版块菜品信息
				var self = this;
				this.$http({
		  		method:'get',
		  		url:apiUrl.getCaiPinNameList + '?openid=' + this.openId
		  	}).then(function(res){
//					console.log(JSON.parse(res.data));
//					console.log(res.data);
					
//					var cpList = JSON.parse(res.data);
					var cpList = res.data;//ces
          for(let i in cpList){
          	var cplist = cpList[i];
          	if(cplist.Images != null){
					  	cplist.Images = cplist.Images.split(',')[0];
          	}
					  
					  if(cplist.IsCollect == 0){//未收藏
					  	cplist.collectImg = '../../static/credit/love01.png'
					  }else if(cplist.IsCollect == 1){//已收藏
					  	cplist.collectImg = '../../static/credit/love02.png'
					  }
					  
          	if(cplist.BanKuaiName == '上榜新菜'){
          		self.sbxcList.push(cplist);
          	}else if(cplist.BanKuaiName == '旺店热卖'){
          		self.wdrmList.push(cplist);
          	}
//        	else if(cplist.BanKuaiName == '当月人气'){
//        		self.dyrqList.push(cplist);
//        	}
          }
          
	      	self.wdrmList = self.wdrmList.slice(0,4);//旺店热卖 显示4个
//        self.dyrqList = self.dyrqList.slice(0,2);//当月人气 显示2个
	      	for(let i in self.wdrmList){//根据经纬度获取地区
	      		var wdrmInfo = self.wdrmList[i];
	      		if(wdrmInfo.HotelAtitude != '' && wdrmInfo.HotelAtitude != null){
	      			self.getLocation(wdrmInfo.HotelAtitude,wdrmInfo.HotelLongitude,i);
	      		}
	      	}
		  	})
			},
			getDyrqList(){//获取当月人气列表
        var self = this;
				this.$http({
		  		method:'get',
		  		url:apiUrl.getDyrqList + '?openid=' + this.openId
		  	}).then(function(res){
//					console.log(JSON.parse(res.data));
//					var data = JSON.parse(res.data);
					var data = res.data;//ces
					for(let i in data){
          	var datalist = data[i];
          	if(datalist.Images != null){
					  	datalist.Images = datalist.Images.split(',')[0];
          	}
					  if(datalist.IsCollect == 0){//未收藏
					  	datalist.collectImg = '../../static/credit/love01.png'
					  }else if(datalist.IsCollect == 1){//已收藏
					  	datalist.collectImg = '../../static/credit/love02.png'
					  }
          	self.dyrqList.push(datalist);
          }
          self.dyrqList = self.dyrqList.slice(0,2);//当月人气 显示2个
//					console.log(self.dyrqList);
		  	})
			},
			getLocation(latitude,longitude,index){//根据经纬度获取省市区名称
	    	var self = this;
	      $.ajax({
	        url: 'http://apis.map.qq.com/ws/geocoder/v1/?location='+latitude+','+longitude+'&key=4MFBZ-LJMC3-NKG3O-YM3LT-OKQRV-PMBJA&output=jsonp',
	        type: 'get',
	        dataType: 'jsonp',
	        processData: false, 
	        data: {},
	        success: function (data) {
	          var city=data.result.ad_info.city.slice(0,-1);//市  去掉'市'
//	          console.log(city);
	          self.wdrmList[index].city = city;
	          self.wdrmList.splice(index,1,self.wdrmList[index])
	        }
	      })
	    },
			collectClick(dishId,collectImg,arr,index){//点击收藏
				var self = this;
				var ActionFlag = '';
				if(collectImg == '../../static/credit/love01.png'){//未收藏
					ActionFlag = '1';//收藏
				}else if(collectImg == '../../static/credit/love02.png'){//收藏
					ActionFlag = '0';//取消收藏
				}
				
				var params = {
					"ModuleName":"菜品库",
					"OpenId":this.openId,
					"RecordAction":"collect",
					"RecordKeyType":"菜品",
					"RecordKeyId":dishId,//菜品id
					"ActionFlag":ActionFlag//1收藏 0取消收藏
				}
				this.$http({
					method:'post',
					url:apiUrl.addActionRecord,
					data:params
				}).then(function(res){
					if(res.data == 'succ'){
						if(arr == 'sbxc'){//上榜新菜
							if(collectImg == '../../static/credit/love01.png'){//未收藏
								self.sbxcList[index].collectImg = '../../static/credit/love02.png';
						  }else if(collectImg == '../../static/credit/love02.png'){//收藏
								self.sbxcList[index].collectImg = '../../static/credit/love01.png';
						  }
						}else if(arr == 'dyrq'){//当月人气
							if(collectImg == '../../static/credit/love01.png'){//未收藏
								self.dyrqList[index].collectImg = '../../static/credit/love02.png';
						  }else if(collectImg == '../../static/credit/love02.png'){//收藏
								self.dyrqList[index].collectImg = '../../static/credit/love01.png';
						  }
						}else if(arr == 'wdrm'){//旺店热卖
							if(collectImg == '../../static/credit/love01.png'){//未收藏
								self.wdrmList[index].collectImg = '../../static/credit/love02.png';
						  }else if(collectImg == '../../static/credit/love02.png'){//收藏
								self.wdrmList[index].collectImg = '../../static/credit/love01.png';
						  }
						}
					}else{
						alert('收藏失败，请稍后再试');
					}
				})
			},
			jump(link){//页面跳转 列表页
				if(link.indexOf('component')>0){
					this.$router.push(link);
				}else{
					window.location.href = link;
				}
			},
			lookDish(dishId){//菜品详情页
				this.$router.push({path:'/component/dishstoredetail',query:{dishId:dishId}});
			},
			hideImg(){
//				console.log(event.target.contentWindow.document);
			}
		},
		mounted(){
			//		判断是否显示指引蒙版
	    var storage=window.localStorage;
	    var dishlooknum1=storage.getItem("dishlooknum1");//获取storage
	    if(dishlooknum1==null){//没有记录
	    	//第一次显示指引层
	    	this.masknum = 1;
	    	storage.setItem("dishlooknum1",'yes');//设置
	    	$('.dishstore').css({
				  'overflow':'hidden'
			  })
	    }else{
	    	//不显示指引层
	    	this.masknum = 4;
	    }
			
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
	    this.getBanner();//获取轮播图
      this.getDishList();//获取各版块菜品信息
      this.getDyrqList();//获取当月人气列表

//    	console.log($("#myIframe")[0].contentWindow)
//    	console.log(document.getElementById('myIframe').contentWindow.document.getElementById("video_container"))
      
      
		}
	}
</script>

<style scoped>
.mask{width: 100%;height: 100%;background: rgba(0,0,0,0.7);position: fixed;top: 0;z-index: 100;}
/*全部菜谱*/
.cpDiv img:nth-child(1){width: 24%;position: absolute;top: 305px;left: 2%;}
.cpDiv img:nth-child(2){width: 50%;position: absolute;top: 70%;left: 25%;}
/*分类*/
.flDiv img:nth-child(1){width: 24%;position: absolute;top: 5px;right: 1%;}
.flDiv img:nth-child(2){width: 50%;position: absolute;top: 25%;left: 25%;}
/*视频学菜*/
.vedioDiv img:nth-child(1){width: 28%;position: absolute;top: 310px;left: 52%;}
.vedioDiv img:nth-child(2){width: 34%;position: absolute;top: 70%;left: 33%;}
/*.scDiv img:nth-child(1){width: 28%;position: absolute;top: 310px;left: 52%;}*/
/*.scDiv img:nth-child(2){width: 34%;position: absolute;top: 70%;left: 33%;}*/
	/*轮播*/
.swiper-wrapper{width: 95%;height: 200px;}
.swiper-slide {background: rgba(0,0,0,0.5);}
.swiper-slide img{width: 100%;height: 100%;border-radius: 5px;}

.dishstore{height: 100%;overflow: scroll;}
.searchDiv{position:relative;width: 70%;height: 40px; background: #f4f4f4;margin-left: 20px 3%; margin: 10px 0 30px 3%;border-radius: 5px;display: inline-block;}
.searchDiv input,.searchDiv img{display: inline-block;vertical-align: middle;}
.searchDiv img{width: 8%;}
.searchDiv input{width: 87%;height: 100%;border: none;background:#f4f4f4;text-indent: 6%;border-radius: 5px;letter-spacing: 1px;outline: none;font-size: 15px;}
.keyStyle{width:100%;background:#f1f1f1;opacity: 0.9;border-radius: 0 0 5px 5px;position: absolute;
         top: 40px; left: 0px;z-index: 100;}
.keyStyle p{border-top: 1px solid #ccc;font-size: 13px;color: #3E3E3E;line-height: 40px;text-indent: 1em;}
.sbxcDiv,.dyrqDiv,.wdrmDiv,.cpcpDiv{border-top: 5px solid #F1F1F1;margin-top: 20px;}
.dish_title{text-align: center;margin: 14px 0;}
.dish_title span{display: inline-block;border-bottom: 2px solid #e83428;}
.dish_titles:after {content: " ";display: inline-block;height: 10px;width: 10px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    position: relative;top: 7px;right: 3%;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);float: right;}
/*上榜新菜*/
.dishWrap{width: 100%;display: flex;overflow-x: auto;margin-top: 10px;}
.dishBox{margin-left: 8px;}
.dishBox div{width:156px;}
.dishBox div img{width:100%;}
.dishBox p:nth-child(2){color: #3E3E3E;font-size: 13px;}
.dishBox p:nth-child(3) *{display: inline-block;vertical-align: middle;}
.dishBox p:nth-child(3){color: #8c8c8c;font-size: 11px;margin-top: 10px;margin-bottom: 15px;}
.dishBox p:nth-child(3) img{width: 15px;margin-right: 10px;}
.dishWrap::-webkit-scrollbar {display:none}/*隐藏滚动条*/
/*当月人气*/
.dyrq_box{display: flex;padding: 0 3%;}
.dyrq_box div{flex: 1;}
.dyrq_box div:nth-child(2){margin-left: 10px;}
.intro_a *{display: inline-block;vertical-align: middle;}
.intro_a img{width: 15px;margin-right: 10px;}
.intro_a span{color: #8b8b8b;font-size: 12px;}
.intro_b{margin: 10px 0 5px 0;color: #3E3E3E;font-size: 14px;}
.intro_c{color: #8c8c8c;font-size: 12px;/*height: 40px;*/overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp: 3;-webkit-box-orient: vertical;}
.decstyle{width:26px;height:30px;background:url('../../static/credit/dish_dec.png') no-repeat center ;background-size: 100% 100%;position: absolute;top: 0;left: 10px;
          color:#fff;text-align: center;}
/*旺店热卖*/
.wdrm_wrap{display: flex;flex-wrap: wrap;}
.wdrm_box{width: 47%;margin-left: 2%;margin-bottom: 10px;}
.wdrm_box div{width: 100%;}
.wdrm_a{font-size: 14px;color: #e83428;}
.wdrm_b{font-size: 14px;color: #3e3e3e;}
.wdrm_c *{display: inline-block;vertical-align: middle;}
.wdrm_c img{width: 15px;margin-right: 10px;}
.wdrm_c span{font-size: 12px;color: #8c8c8c;}
/*产品菜谱*/
.cpcp_wrap{display: flex;flex-wrap: wrap;}
.cpcp_box{width: 47%;margin-left:2%;margin-bottom: 10px;}
.cpcp_box img{width: 100%;}
/*备案*/
.record{padding:10px 16px;background: #f6f6f6;color: #818181;font-size: 12px;text-align: center;border-radius: 10px;letter-spacing: 1px;
position: absolute;left: 50%;transform: translateX(-50%);}
</style>
<style>
#vux_view_box_body{background: #fff;}
.dishstore .weui-grid__icon{width: 50%;height: 50%;}
.dishstore .weui-grid{padding:0;}
.dishstore .weui-grid:before { border-right: none; }
.dishstore .weui-grid:after { border-bottom: none; }
.dishstore .weui-grids:before{border-top: none;}
.dishstore .swiper-pagination-bullet-active{background: #e83428!important;}
.dishstore .weui-grid__icon + .weui-grid__label{margin-top: 10px;}
/*.dishstore .txp_player_inframe .txp_player{background: #fff!important;}*/
/*.dishstore .txp_video_container video{background: #fff!important;}*/
</style>
