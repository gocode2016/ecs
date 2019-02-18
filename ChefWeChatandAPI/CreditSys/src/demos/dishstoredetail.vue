<template>
  <div class="dishstoredetail">
    
    <div class="content">
    <!--轮播图-->
		<div class="swiper-container ">
				<div class="swiper-wrapper">
					<div class="swiper-slide" v-for="(item,index) in bannerList" :key="index">
      	    <iframe frameborder='0' allowtransparency="false" v-if=" item.videoUrl!=''&&item.videoUrl!=null " allowfullscreen='true' :src='item.videoUrl' width="100%" height="60%" style="position: absolute;top: 50%;transform: translateY(-50%);"></iframe>
      		  <!--<video :src='item.videoUrl' width="100%" controls="controls" v-if=" item.videoUrl!=''&&item.videoUrl!=null "  style="position: absolute;top: 50%;transform: translateY(-50%);">您的浏览器不支持 video 标签。</video>-->
						<img v-else :src="item.img"/>
					</div>
				</div>
				<!--分页-->
				<div class="swiper-pagination"></div>
		</div>
		
    <!--菜名-->
    <div class="info">
    	<p style="margin-top: 7px;"><span style="font-size: 15px;">{{dishInfo.CaiPinName}}</span><span style="font-size: 12px;color: #8F8F8F;float: right;">{{dishInfo.LlCount}}人看过</span></p>
    	<p>
    		<span style="font-size: 13px;color: #e83428;margin-left: -6px;">{{cookStyle}}</span>
    		<span style="float: right;" class="praisenum"><img :src="dishInfo.praiseImg" style="width: 20px;margin-right: 10px;" @click="praiseClick"><span style="font-size: 12px;color:#8F8F8F ;">{{dishInfo.DzCount}}</span></span>
    	</p>
    </div>
    
    <!--菜品推荐-->
    <div style="padding: 0 4%;" v-show="dishRecommendFlag">
    	<p class="plate_title"><img src="../../static/credit/detail_cpicon.png"><span>菜品推荐</span></p>
    	<div class="dish_content"></div>
			<p class="lookDetail lookDetails" @click="detailClick">查看详情</p>
    </div>
    
    <!--旺店介绍-->
    <div style="padding: 0 4%;" v-show="shopIntroFlag">
    	<p class="plate_title"><img src="../../static/credit/detail_wdicon.png"><span>旺店介绍</span></p>
    	<p class="shop_intro"></p>
    	<p class="gohere" @click="useMap">到这去</p>
    </div>
    
    <!--作者介绍-->
    <div style="padding: 0 4%;" v-show="authorIntroFlag">
    	<p class="plate_title"><img src="../../static/credit/book1.png"><span>作者介绍</span></p>
    	<p class="author_intro"></p>
    </div>
    
    <!--备料展示-->
    <div>
    	<p class="plate_title" style="padding: 0 4%;"><img src="../../static/credit/show.png"><span>备料展示</span></p>
    	<div v-for="(item,index) in dishlist" :key="index">
    		<p class="bl_type">{{item.type}}</p>
    		<div class="bl_content" v-for="(items,index) in item.Data" :key="index" :class="{styleA:item.Display==1,styleB:item.Display==2}">
    			<span style="color: #3E3E3E;" class="content_icon" :class="{content_a:item.type=='调料'&&items.isInfo=='1'}" @click="lookInfo(items.ProductId,item.type)">{{items.Name}}</span>
    			<span style="color: #6c6c6c;float: right;">{{items.Amount}}{{items.Unit}}</span>	
    			<!--<div :class="{seasonA:item.Display==1,seasonB:item.Display==2}" v-show="items.isInfo=='1'&&items.isShow=='1'">
    				<p v-for="aitem in items.Items" style="margin-top: 8px;">
    					<span>{{aitem.Name}}</span>
    					<span style="float: right;">{{aitem.Amount}}{{aitem.Unit}}</span>
    				</p>-->
    				<!--<p style="border-top: 1px solid #ccc;margin-top: 8px ;padding-top: 8px;">制作步骤</p>
    				<p v-for="i in 3" style="text-indent: 1em;margin-top: 3px;">1.黄豆压榨成水</p>-->
    			<!--</div>-->
    		</div>
    	</div>
    </div>
    
    
    <!--烹饪步骤-->
    <div style="padding: 0 4%;">
	    <div class="plate_title"><img src="../../static/credit/cook.png" /><span>烹饪步骤</span></div> 
	    <p class="cooktext" v-html="dishSteps"></p>	     
    </div>
    
    <!--更多菜品-->
    <!--<div style="padding: 0 10px;">
	    <div class="plate_title"><img src="../../static/credit/recom.png" /><span>更多菜品</span></div> 
	    <div class="dishWrap">
	    	<div class="dishBox" v-for="(item,index) in moredishlist" :key="index">
	    		<div><img :src="item.imgurl"></div>
	    		<p>balabala</p>
	    	</div>
	    </div>
    </div>-->
    
    <!--我要留言-->
    <div>
	    <div class="plate_title" style="padding: 0 4%;"><img src="../../static/credit/tutor03.png" /><span>我要留言</span></div> 
	      <ul class="wordUl">
		    	<!--<scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2"  @on-pullup-loading="load" class='scrollheight'>-->
		    	 	 <!--<div class="box2">-->
			     	 	  <li v-for="(item,index) in speakdata" :key="index">
			     	 	 	  <div class="leftDiv"><img v-bind:src="item.HeadImgUrl"></div>
			     	 	 	  <div class="rightDiv">
				     	 	 	 	<div style="padding-right: 10px;">
				     	 	 	 		<p>{{item.Nickname}}<span style="float: right;font-size: 12px;">{{item.HotelName}}</span></p>
			     	 	 	 	 		<p>{{item.LiuYan}}</p>
			     	 	 	 	 		<p>{{item.CreateDate}}</p>
				     	 	 	 	</div>
			     	 	 	 	  <!--<p style="margin-top:10px;padding-top:10px;border-top: 1px solid #ccc;color: #3E3E3E;font-size: 13px;" >
			     	 	 	 	  	<span style="color: #e83428;margin-right: 5px;">欣和餐饮服务:</span>
			     	 	 	 	  	<span style="margin-right: 10px;">回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容回复内容</span>
			     	 	 	 	  </p>-->
			     	 	 	  </div>
			     	 	  </li>
		     	 	<!--</div>-->
		 	 	  <!--</scroller>-->
		    </ul>
    </div>
    </div>
    
    <!--底部-->
		<div class="footer">
				<div class="send">
	     	 	<div class="sendfir" v-if="wordFlag">
		     	 	<!--<input type="text" placeholder="我有话要说..." @click='focus'/>-->
		     	 	<span class="speakDiv" @click='focus'>我有话要说...</span>
		     	 	<img v-bind:src="dishInfo.collectImg" @click="collectClick"/>
	     	 	</div>
	     	 	<div class="sendsec" v-else @click.stop>
	     	 	 	<div class="txtArea">
	     	 	 	 	<textarea @input='textChange()' v-model='message' maxlength='500'></textarea>
	     	 	 	  <span>{{leng}}</span>
	     	 	 	</div>     	 	 	
	     	 	 	<button @click="submit">发送</button>
	     	 	</div>
     		</div>
     	<!--收藏成功图片-->
     	<img src="../../static/credit/collect.png" class="colSuc" v-show="colShow"/>
	 	</div>
    
  </div>
</template>

<script>
import {  Group, Cell,CellFormPreview ,Scroller,cookie} from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
export default{
	components:{
		Group,
		Cell,
		CellFormPreview,
		Scroller,
		cookie
	},
	data(){
		return{
			timestamp:'',
		  nonceStr:'',
		  signature:'',
		  url:'',
			userData:{},
			openId:'',
			dishId:'',//菜品id
			dishInfo:{},//基本信息
			cookStyle:'',//菜系
			dishRecommend:{},//菜品推荐
			shopIntro:{},//旺店介绍
			authorIntro:{},//作者介绍
			dishRecommendFlag:false,
    	shopIntroFlag:false,
    	authorIntroFlag:false,
      mainMaterial:{},//主料
      access:{},//辅料
      season:[],//调料
      dishSteps:'',//步骤
			isShow:0,//查看详情状态
			wordFlag:true,//留言发送框
			message:'',//留言内容
			leng:'',//留言长度
			collectImg:'../../static/credit/love02.png',//收藏icon
			colShow:false,//收藏成功图片
			bannerList:[],//菜品轮播图
			dishlist:[],
			moredishlist:[
			  {
			  	imgurl:'../../static/credit/rule4.png',
			  	dishname:'balabala'
			  },
			  {
			  	imgurl:'../../static/credit/rule4.png',
			  	dishname:'balabala'
			  },
			  {
			  	imgurl:'../../static/credit/rule4.png',
			  	dishname:'balabala'
			  },
			  {
			  	imgurl:'../../static/credit/rule4.png',
			  	dishname:'balabala'
			  }
			],
			speakdata:[]
		}
	},
	methods:{
    getSpeackList(){//获取留言
      var self = this;
      var params = {
      	"page":"1",
				"pagesize":"10",
				"KeyType":"菜品",
				"KeyId":this.dishId
      }
      this.$http({
    		method:'post',
    		url:apiUrl.getLiuYanInfo,
    		data:params
    	}).then(function(res){
//  		console.log(JSON.parse(res.data));
//  		self.speakdata = JSON.parse(res.data);
    		self.speakdata = res.data;//测试
    		for(let i in self.speakdata){
    			var speak = self.speakdata[i];
    			speak.CreateDate = speak.CreateDate.substring(0,16);
    			speak.CreateDate = speak.CreateDate.replace('T',' ');
    			speak.CreateDate = speak.CreateDate.replace(/-/g,'/');
    		}
    		
    		//留言高度
    		if(self.speakdata.length < 3){
    			$('.wordUl').height(self.speakdata.length*123);
    		}else{
    			$('.wordUl').height(3*123);//留言高度
    		}
    		
    	})
    },
    addSpeackInfo(){//新增留言
      var self = this;
      var params = {
      	"OpenId":this.openId,
				"KeyType":"菜品",
				"KeyId":this.dishId,
				"ParentId":"0",
				"LiuYan":this.message
      }
      this.$http({
    		method:'post',
    		url:apiUrl.addLiuYanInfo,
    		data:params
    	}).then(function(res){
    		if(res.data == 'succ'){
    			//留言成功
	    		self.getSpeackList();//获取留言
					self.message='';
				  self.leng=500;
				  self.wordflag=true;
    		}
    	})
    },
    getBasicInfo(){//获取菜品基本信息
    	var self = this;
    	this.$http({
    		method:'get',
    		url:apiUrl.getCaiPinBasicInfo + '?caiPinId=' + this.dishId + '&openid=' + this.openId
    	}).then(function(res){
//  		console.log(JSON.parse(res.data)[0]);
//  		self.dishInfo = JSON.parse(res.data)[0];//基本信息
    		self.dishInfo = res.data[0];//基本信息 测试
    		
    		self.cookStyle = '';//菜系菜式食材
    		if(self.dishInfo.CaiXi != null){//菜系
    			self.dishInfo.CaiXi = self.dishInfo.CaiXi.split(',');
    			for(let i in self.dishInfo.CaiXi){
    				var cookStyle =  self.dishInfo.CaiXi[i];
    				self.cookStyle = self.cookStyle + '【' + cookStyle + '】';
    			}
    		}
    		
    		if(self.dishInfo.CaiShi != null){//菜式
    			self.dishInfo.CaiShi = self.dishInfo.CaiShi.split(',');
    			for(let i in self.dishInfo.CaiShi){
    			  var cookStyle =  self.dishInfo.CaiShi[i];
    			  self.cookStyle = self.cookStyle + '【' + cookStyle + '】';
    		  }
    		}
    		
    		if(self.dishInfo.ShiCai != null){//食材
    			self.dishInfo.ShiCai = self.dishInfo.ShiCai.split(',');
    			for(let i in self.dishInfo.ShiCai){
    				var cookStyle =  self.dishInfo.ShiCai[i];
    				self.cookStyle = self.cookStyle + '【' + cookStyle + '】';
    			}
    		}
    		
    		var bannerList = self.dishInfo.Images.split(',');//菜品图片
    		self.bannerList=[];//轮播图置空
    		for(var i=0;i<bannerList.length;i++){
						var banner=bannerList[i];
						var item={
              videoUrl:'',
							img:banner
						}
						self.bannerList.push(item);
				}
    		if(self.dishInfo.VideoUrl!=''&&self.dishInfo.VideoUrl!=null){//有视频
            var item={
              videoUrl:self.dishInfo.VideoUrl,
//            videoUrl:'https://s3.cn-north-1.amazonaws.com.cn/s3-010-shinho-ecshool-prd-bjs/mov/DJI_0050.MP4',
							img:''
						}
						self.bannerList.unshift(item);
    		}
    		console.log(self.bannerList);
    		
    		self.$nextTick(function() {//swiper
						var mySwiper = new Swiper('.swiper-container', {
							loop: true,
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
		    		
    		if(self.dishInfo.IsDianZan == 0){//未点赞
    			self.dishInfo.praiseImg = '../../static/credit/zanw.png';
    		}else if(self.dishInfo.IsDianZan == 1){//已点赞
    			self.dishInfo.praiseImg = '../../static/credit/zanr.png';
    		}
    		
    		if(self.dishInfo.IsCollect == 0){//未收藏
    			self.dishInfo.collectImg = '../../static/credit/love01.png';
    		}else if(self.dishInfo.IsCollect == 1){//已收藏
    			self.dishInfo.collectImg = '../../static/credit/love02.png';
    		}
    		
    	})
    },
    getRichInfo(){//获取菜品富文本信息
    	var self = this;
    	this.$http({
    		method:'get',
    		url:apiUrl.getCaiPinRichInfo + '?caiPinId=' + this.dishId
    	}).then(function(res){
//  		console.log(JSON.parse(res.data));
//  		var infoArr = JSON.parse(res.data);//富文本信息
    		var infoArr = res.data;//富文本信息 测试
    		
    		for(let i in infoArr){
    			var infoarr = infoArr[i];
    			if(infoarr.TypeId == 1){//菜品推荐
    				self.dishRecommend = infoarr;
    			}else if(infoarr.TypeId == 2){//旺店介绍
    				self.shopIntro = infoarr;
    				console.log(self.shopIntro);
    			}else if(infoarr.TypeId == 3){//作者介绍
    				self.authorIntro = infoarr;
    			}
    		}
    		
				if(JSON.stringify(self.dishRecommend) != "{}"){//菜品推荐无数据时不显示
				  self.dishRecommendFlag = true;
				}
				if(JSON.stringify(self.shopIntro) != "{}"){//旺店介绍无数据时不显示
				  self.shopIntroFlag = true;
				}
				if(JSON.stringify(self.authorIntro) != "{}"){//作者介绍无数据时不显示
				  self.authorIntroFlag = true;
				}
				
				//菜品推荐
				$('.dish_content').html(self.dishRecommend.Content);
				
				//旺店介绍
				$('.shop_intro').html(self.shopIntro.Content);
				
				//作者介绍
				$('.author_intro').html(self.authorIntro.Content);
				$('.author_intro p').css({
					'width':'50%',
					'float':'right',
					'margin-right':'18%',
					'font-size':'12px'
				})
				$('.author_intro p').eq(1).css({
					'font-size':'15px',
					'margin-top':'12px'
				})
				$('.author_intro p').eq(0).css({
					'width':'25%',
					'float':'left',
					'margin-right':'0'
				})
				$('.author_intro img').css({
					'border':'1px solid #e83428',
					'border-radius':'50%',
					'width':'100%',
					'height':'100%'
				})
				
				$('.dish_content img,.shop_intro img').css({
					'width':'100%'
				})
				
    	})
    },
    getMaterial(){//获取菜品调料 步骤
    	var self = this;
    	this.$http({
    		method:'get',
    		url:apiUrl.getCaiPinMaterial + '?caiPinId=' + this.dishId
    	}).then(function(res){
//  		var infoArr = JSON.parse(res.data);//调料 步骤信息
    		var infoArr = res.data;//调料 步骤信息 测试
    		
    		self.mainMaterial = infoArr.ZhuLiao;//主料
    		self.access = infoArr.FuLiao;//辅料
    		var season = infoArr.TiaoLiao;//调料
    		self.dishSteps = infoArr.Steps;//步骤
    		
    		for(let i in season){
    			var sea = season[i];
    			if(sea.ProductId == 0){//没有与调料相关的产品
    				sea.isInfo = '0';
    			}else{//有该产品
    				sea.isInfo = '1';
    			}
    		}
    		
    		self.mainMaterial.type = '主料';
    		self.dishlist.push(self.mainMaterial);
    		self.access.type = '辅料';
    		self.dishlist.push(self.access);
    		self.season.type = '调料';
    		self.season.Display = 1;
    		self.season.Data = season;
    		self.dishlist.push(self.season);
    		console.log(self.dishlist);
    		
    	})
    },
		detailClick(){//查看详情
			if(this.isShow==0){//查看详情
				$(".dish_content").css({
//	        opacity:1,
					height:"auto"
				})
				this.isShow=1;
				$('.lookDetail').html('隐藏详情');
				$('.lookDetail').removeClass('lookDetails');
				$('.lookDetail').addClass('hideDetails');
			}else if(this.isShow==1){//隐藏详情
				this.isShow=0;
				$(".dish_content").css({
//	        opacity:0.3,
					height:"50px"
				})
				$('.lookDetail').html('查看详情');
				$('.lookDetail').removeClass('hideDetails');
				$('.lookDetail').addClass('lookDetails');
			}
		},
//		lookInfo(items,index){//查看组合调料
//			var item=items;
//			var length = this.dishlist[0].Data.length + this.dishlist[1].Data.length + index
//			if(items.isInfo=='1'&&items.isShow=='0'){//显示组合调料
//				item.isShow='1';
//				this.dishlist[2].Data.splice(this.dishlist[2].Data.indexOf(items),1,item);
//				$('.content_icon').eq(length).removeClass('content_a');
//				$('.content_icon').eq(length).addClass('content_b');
//			}else if(items.isInfo=='1'&&items.isShow=='1'){//隐藏
//				item.isShow='0';
//				this.dishlist[2].Data.splice(this.dishlist[2].Data.indexOf(items),1,item);
//				$('.content_icon').eq(length).removeClass('content_b');
//				$('.content_icon').eq(length).addClass('content_a');
//			}
//			this.dishlist.splice(2,1,this.dishlist[2]);
//		},
		lookInfo(productId,type){//跳转到产品规格列表
			if(productId !=0 && type == '调料'){
				this.$router.push({path:'/component/dishproductlist',query:{productId:productId}});
			}
		},
		focus(){
      var _this=this;
			setTimeout(function(){
				_this.wordFlag = false;  //留言发送框
			},1);
			
		  setTimeout(function(){
		   	$('.sendsec textarea').focus();//获取焦点
		  },100);

		  $(document).click(function(){
		   	_this.wordFlag=true; 
		  })
		},
		textChange(){ 	
  		if(this.message.length>=1){//改变按钮颜色
  			$('.sendsec button').css("background","#e83426")
  		}else{
  			$('.sendsec button').css("background","#7f7f7f")
  		}
  		this.leng=500-this.message.length;
  	},
  	praiseClick(){//点赞
  		if(this.dishInfo.praiseImg == '../../static/credit/zanw.png' ){//未点赞
		    this.collectOrPraise('dianzan','1');//记录点赞
  		}else if(this.dishInfo.praiseImg == '../../static/credit/zanr.png' ){//点赞
		    this.collectOrPraise('dianzan','0');//取消点赞
  		}
  	},
		collectClick(){//点击收藏
			if(this.dishInfo.collectImg == '../../static/credit/love01.png' ){//未收藏
		    this.collectOrPraise('collect','1');//记录收藏
  		}else if(this.dishInfo.collectImg == '../../static/credit/love02.png' ){//收藏
		    this.collectOrPraise('collect','0');//取消收藏
  		}
		},
		collectOrPraise(RecordAction,ActionFlag){//记录收藏 点赞 浏览
				var self = this;
				var params = {
					"ModuleName":"菜品库",
					"OpenId":this.openId,
					"RecordAction":RecordAction,
					"RecordKeyType":"菜品",
					"RecordKeyId":this.dishId,//菜品id
					"ActionFlag":ActionFlag//1收藏 0取消收藏
				}
				this.$http({
					method:'post',
					url:apiUrl.addActionRecord,
					data:params
				}).then(function(res){
					console.log(res);
					if(res.data == 'succ'){
            self.getBasicInfo();//获取菜品基本信息
					}else{
//						alert('请求失败，请稍后再试');
					}
				})
		},
		submit(){//发送留言
			if(this.message != ''){
			  this.addSpeackInfo();//新增留言
			}
		},
		setConfig(){//配置config
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
					        'openLocation'//使用微信内置地图查看位置接口
					      ]
					    })
		  		}).catch(function(){
		  			_this.setConfig();
		  		})
		  	}).catch(function(){
		  		_this.setConfig();
		  	})
		},
		useMap(){//调用地图
			  var _this = this;
			  var lat = Number(this.shopIntro.HotelAtitude);
			  var lon = Number(this.shopIntro.HotelLongitude);
  			wx.ready(function(){
    			wx.openLocation({
						latitude:lat, // 纬度，浮点数，范围为90 ~ -90
						longitude:lon, // 经度，浮点数，范围为180 ~ -180。
						name: '', // 位置名
						address: '', // 地址详情说明
						scale: 16, // 地图缩放级别,整形值,范围从1~28。默认为最大
						infoUrl: '' // 在查看位置界面底部显示的超链接,可点击跳转
					});
		    })
  			wx.error(function(res){
//				alert('error:'+ JSON.stringify(res));
  			  _this.useMap();
  			})
	  }
	},
	mounted(){
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
		this.setConfig();//配置config
	  
    this.dishId = this.$route.query.dishId;
	  this.getBasicInfo();//获取菜品基本信息
	  this.getRichInfo();//获取菜品富文本信息
	  this.getMaterial();//获取菜品调料 步骤
	  this.getSpeackList();//获取留言
		this.collectOrPraise('scan','0');//浏览次数
		
		$('.swiper-wrapper').height($(window).width());
		$('.swiper-slide').height($(window).width());
    
	}
}
</script>

<style scoped>
/*轮播*/
.swiper-wrapper{width: 100%;}
/*.swiper-slide {background: rgba(0,0,0,0.5);}*/
.swiper-slide {background: black;}
.swiper-slide img{width: 100%;height: 100%;}
/*.pagination {position: relative; bottom: 30px; text-align: center;z-index: 1;height: 10px;}*/
/*菜品信息*/
.info{padding: 0 4%;} 
.praisenum *{vertical-align: middle;}
.praisenum{position: relative;top: -2px;}
/*菜品推荐*/
.plate_title *{vertical-align: middle;}
.plate_title{margin: 20px 0 10px 0;}
.plate_title img{width: 20px;margin-right: 10px;}
.plate_title span{font-size: 14px;color: #e83428;}
.dish_content{font-size: 13px;color: #3E3E3E;height: 50px;overflow: hidden;}
.lookDetail{text-align: center;font-size: 13px;color: #E83428;margin-top: 20px;}
.lookDetails:after {content: " ";display: inline-block;height: 9px;width: 9px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    position: relative;top: -1px;right: -9px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(90deg);}
.hideDetails:after {content: " ";display: inline-block;height: 9px;width: 9px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    position: relative;top: 3px;right: -9px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(-90deg);}
/*旺店介绍*/
.gohere{font-size: 13px;color: #e83428;}
.gohere:after{content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1px 1px 0 0;border-color: #e83428;border-style: solid;
    position: relative;top: 0px;right: -3px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
/*作者介绍*/
.author_intro{height: 85px;}
/*备料展示*/
.bl_type{line-height: 30px;font-size: 14px;background: #f1f1f1;color: #e83428;padding: 0 4%;margin: 5px 0;}
.bl_content{padding: 5px 4%;font-size: 13px;} 
.content_a:after{content: " ";display: inline-block;height: 7px;width:7px; border-width: 1px 1px 0 0;border-color: #3e3e3e;border-style: solid;
    position: relative;top: -1px;right: -3px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
.content_b:after{content: " ";display: inline-block;height: 7px;width:7px; border-width: 1px 1px 0 0;border-color: #3e3e3e;border-style: solid;
    position: relative;top: 1px;right: -8px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(-90deg);}
.styleA{display: inline-block;width:92%;}   /*单列*/
.styleB{display: inline-block;width: 42%;vertical-align: top;}   /*双列*/
.seasonA{width: 90%;margin-left: 5%;background: #f1f1f1;font-size: 12px;color: #3e3e3e;box-sizing: border-box;padding: 0px 2% 10px 5%;
          overflow: hidden;margin-top: 10px;}
.seasonB{width: 100%;background: #f1f1f1;font-size: 12px;color: #3e3e3e;box-sizing: border-box;padding: 0px 2% 6px 2%;
        overflow: hidden;margin-top: 10px;}    
/*烹饪步骤*/
.cooktext{font-size: 13px;color: #3e3e3e;margin-top: 15px;}
/*更多菜品*/
.dishWrap{width: 100%;display: flex;overflow-x: auto;}
.dishBox{margin-right: 8px;}
.dishBox div{width:105px;}
.dishBox div img{width:100%;}
.dishBox p{width:105px;text-align: center;font-size: 13px;color: #3E3E3E;}
.dishWrap::-webkit-scrollbar {display:none}/*隐藏滚动条*/
/*留言*/
.wordUl{width: 100%;list-style: none;height: 200px;overflow: scroll;}
.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;border-bottom: 1px solid #ccc;}
.leftDiv{width: 18%;}
.rightDiv{width: 82%;font-size: 12px;color: #3e3e3e;}
.rightDiv p:nth-child(2){word-wrap: break-word;}
.rightDiv p:nth-child(3){text-align: right;color: #888888;	margin-top: 15px;}
.rightDiv p:nth-child(4){padding-right: 0;}
.leftDiv img{width: 33px;margin-left: 20px;border-radius: 50%}	
.no-more{text-align:center;line-height: 60px;}
/*留言框*/
.send{width: 100%;background: #FFFFFF;}
.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
.sendfir .speakDiv{display:inline-block;color:#a6a6a6;width:80%;line-height: 35px;border: none;background: #f6f6f6;margin-left: 5%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
/*.sendfir img{width:6%;vertical-align: middle;margin-left: 4%;}*/
.sendfir img{width:6%;margin-left: 4%;position: relative;top: 3px;}
.sendsec{width: 90%;margin-left: 5%;text-align: right;}
.txtArea{height:45px;background:#f6f6f6;position: relative;}
.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}
.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
.colSuc{width: 50%;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;z-index: 10;}
.dishstoredetail {display: flex;flex-direction: column;height: 100%;}
.content::-webkit-scrollbar {display:none}/*滚动条不显示*/
.content {overflow: auto;}/*flex内容区*/
</style>
<style>
#vux_view_box_body{background: #fff;}
/*轮播图分页样式*/
/*.dishstoredetail .vux-slider > .vux-indicator > a > .vux-icon-dot.active, .vux-slider .vux-indicator-right > a > .vux-icon-dot.active{background-color:#e83428;}*/
/*.dishstoredetail .vux-slider > .vux-indicator > a > .vux-icon-dot, .vux-slider .vux-indicator-right > a > .vux-icon-dot{width: 8px; height: 8px;border-radius: 50%;}*/
/*轮播图分页样式*/
.swiper-pagination-bullet{background: #f6f6f6;opacity: 0.8;}
.swiper-pagination-bullet-active{background: #e83428;}
</style>