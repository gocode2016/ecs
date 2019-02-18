<template>
  <div class="home">
  	<swiper auto :list="bannerList" v-model="bannerIndex" dots-position='center' :show-desc-mask=false height="194px"></swiper>
    
    <!--公告-->
    <div class="notice" v-show="noticeShow">
	    <cell>
	    	<img slot="icon" width="20" style="display:block;margin-right:5px;" :src="noticeImg">
	      <marquee>
	        <marquee-item v-for="(item,index) in noticeList" :key="index" class="align-center">用户:{{item.MemberName}} &nbsp; &nbsp;报名成功 &nbsp; &nbsp;{{item.AreaName}}</marquee-item>
	      </marquee>
	    </cell>
    </div>
    
    <!--图标按钮-->
    <!--<grid :rows="5">
      <grid-item v-for="(item, index) in iconBtnList" :key="index" :link="item.url" :label="item.title">
        <img slot="icon" :src="item.img">
      </grid-item>
    </grid>-->

    <!-- 导师介绍 -->
    <div class="tutorintro">
			<group>
				<cell style="height: 44px; background: url('../../static/credit/dsjs_img_title.png') no-repeat 10px center; background-size: 40%;"></cell>
			</group>
    </div>
    <div class="video-box">
      <iframe frameborder='0' allowtransparency="false" allowfullscreen='true' :src='videoUrl' width="100%" height="175px"></iframe>
    </div>

		<group style="margin-top: -20px;">
			<cell link="/component/tutorlist" value="更多"></cell>
		</group>

	  <!-- 导师列表 -->
    <div class="scroller-box">
	    <div class="modify-scroller">
	      <div class="ds-box">
	        <div class="ds-item" v-for="item in tutorList" v-on:click="showTutorDetail(item.url,item.TutorId)">
	        	<div class="tutorurl" v-bind:style="{background:'url('+item.HeadPicUrl+') no-repeat center 5px #fafafa',backgroundSize:'120px 158px'}"></div>
	        	<span>{{ item.TutorName }}</span>
	        </div>
	      </div>
	    </div>
	    <div class="mask-scroller"></div>
    </div>

	  <!-- 参赛菜品 -->
		<!--<group>
			<cell link="/component/competitiondishlist" value="更多" style="background: url('../../static/credit/cscp_img_title.png') no-repeat 10px center; background-size: 40%;"></cell>
		</group>
	
		<div style="color: #e93524; margin-top: 15px; margin-bottom: 5px; text-indent: 15px;">最新入选</div>

	  <div class="scroller-box">
	    <div class="modify-scroller">
	      <div class="cp-rx-box">
	        <div class="cp-rx-item" v-for="(item,index) in cpRxList" :key="index" v-on:click="showDishDetail(item.url,item.DishChefId,item.DishType,item.ChefId,item.IsSelected)">
	        	<div style="height: 150px;overflow: hidden;"><img :src="item.DishPicUrl" width="100%" height="100%"></div>
	        	<p>{{ item.DishName }}</p>
	        	<span class="memspan">{{ item.MemberName }}</span><span>{{item.HotelName}}</span>
	        	<div><img :src="item.collectImg" @click.stop="collectClick(item.IsCollect,item.DishChefId,item.DishType,index,'rx')" class="collectImg">{{ item.VisitCount }}人看过</div>
	        </div>
	      </div>
  	    </div>
	    <div class="mask-scroller"></div>
    </div>
  
	  <div style="color: #e93524; margin-top: 15px; margin-bottom: 5px; text-indent: 15px;">人气抢手</div>

		<div class="scroller-box">
	    <div class="modify-scroller">
	      <div class="cp-rq-box">
	        <div class="cp-rq-item" v-for="(item,index) in cpRqList" :key='index' v-on:click="showDishDetail(item.url,item.DishChefId,item.DishType,item.ChefId,item.IsSelected)">
	        	<div style="height: 150px;overflow: hidden;"><img :src="item.DishPicUrl" width="100%" height="100%"></div>
	        	<p>{{ item.DishName }}</p>
	        	<span class="memspan">{{ item.MemberName }}</span><span>{{item.HotelName}}</span>
	        	<div>
	        		<img :src="item.collectImg" @click.stop="collectClick(item.IsCollect,item.DishChefId,item.DishType,index,'rq')" class="collectImg">{{ item.VisitCount }}人看过
	        	</div>
	        </div>
	      </div>
	    </div>
	    <div class="mask-scroller"></div>
	  </div>-->

		<!-- 培训交流 -->
		<group>
			<cell link="/component/trainlist" value="更多" style="background: url('../../static/credit/pxjl_img_title.png') no-repeat 10px center; background-size: 40%;"></cell>
		</group>
		<swiper auto :list="pxjlList" :show-dots=false style="margin-top: 10px; text-align: center;"></swiper>
	
		<!-- EC新品 -->
		<group>
			<cell  style="background: url('../../static/credit/ecxp_img_title.png') no-repeat 10px center; background-size: 40%;" class="ecxpcell"></cell>
		</group>
		<!--<div class="ecxp-box">
			<div class="ecxp-item" v-for="item in ecxpList">
				<div class="left">
					<p>{{item.name}}</p>
					<p>{{item.title}}</p>
				</div>
				<div class="right">
					<img :src="item.img">
				</div>
			</div>
		</div>-->
		<div class="ecxpDiv">
			<div v-for="(item,index) in ecxpList" :key="index">
				<img :src="item.url" @click="ecxpClick(item.link)"/>
			</div>
		</div>

	  <!--星厨家 HeadPicUrl ChefStarName ChefStarPosition-->
		<group>
			<cell link="/component/starkitchenlist" value="更多" style="background: url('../../static/credit/xcj_img_title.png') no-repeat 10px center; background-size: 40%;"></cell>
		</group>
		<div class="xcj-box">
			<div class="xcj-item" v-for="(item,index) in xcjList" :key='index' @click=showStarDetail(item.ChefStarId)>
				<div class="left">
					<img :src="item.HeadPicUrl">
				</div>
				<div class="right">
					<p>星厨{{item.ChefStarName}}</p>
					<p>{{item.ChefStarPosition}}</p>
				</div>
			</div>
		</div>

    <!--备案-->
    <div style="position: relative;margin-top: 20px;margin-bottom: 80px;" >
    	<span class="record"  @click="recordClick">鲁ICP备10013060号</span>
    </div>

	  <!--收藏成功图片-->
	  <img src="../../static/credit/collect.png" class="colSuc" v-show="colShow"/>
    
  </div>
</template>

<script>
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
import { Scroller, Swiper, Grid, GridItem, Marquee, MarqueeItem, Cell, Group, AjaxPlugin,cookie } from 'vux'
export default {
  components: {
  	Scroller,
  	Swiper,
  	Grid,
  	GridItem,
  	Marquee,
  	MarqueeItem,
    Cell,
    Group,
    AjaxPlugin ,
    cookie
  },
  data () {
    return {
    	timestamp:'',
    	nonceStr:'',
    	signature:'',
    	url:'',
    	videoUrl:'https://v.qq.com/iframe/player.html?vid=j03936tqd06',//视频地址
    	colShow:false,//收藏成功
    	ChefActivity: '', // EC活动信息，ChefActivityId, ChefName
    	userData:{},
    	openId: '',
    	bannerIndex: 0,
      bannerList:[],
			iconBtnList: [{
				  url: 'javascript:',
				  img: '../static/credit/bm_btn.png',
				  title: '报名'
				}, {
				  url: 'javascript:',
				  img: '../static/credit/xb_btn.png',
				  title: '选拔'
				}, {
				  url: 'javascript:',
				  img: '../static/credit/tp_btn.png',
				  title: '投票'
				}, {
				  url: 'javascript:',
				  img: '../static/credit/gs_btn.png',
				  title: '公示'
				}, {
				  url: 'javascript:',
				  img: '../static/credit/wt_btn.png',
				  title: '舞台'
				}],
	    noticeImg: '../static/credit/notice.png',
	    noticeShow: false,
	    noticeList: [], // 公告
	    tutorList:[],
		  cpRxList: [],
      cpRqList:[],
		  pxjlList: [{
			  url: '/component/trainlist',
			  img: '../../static/credit/pxjl2.jpg',
			  title: '味达美新品品鉴交流会-上海站'
			}],
//		  ecxpList: [{
//			  url: 'javascript:',
//			  img: '../static/credit/xp1.png',
//			  name: '红烧焖酱',
//			  title: '河南当地最地道火爆的红焖羊肉来了',
//			}, {
//			  url: 'javascript:',
//			  img: '../static/credit/xp2.png',
//			  name: '红烧焖酱',
//			  title: '河南当地最地道火爆的红焖羊肉来了',
//			}, {
//			  url: 'javascript:',
//			  img: '../static/credit/xp3.png',
//			  name: '红烧焖酱',
//			  title: '河南当地最地道火爆的红焖羊肉来了',
//			},{
//			  url: 'javascript:',
//			  img: '../static/credit/xp4.png',
//			  name: '红烧焖酱',
//			  title: '河南当地最地道火爆的红焖羊肉来了',
//			}],
			ecxpList:[{
				url:'../../static/credit/ecxp1.png',
				link:'/component/productstoredetail?ProductId=29&SpecificationId=45&ProductFirstId=6'
			},{
				url:'../../static/credit/ecxp2.png',
				link:''
			},{
				url:'../../static/credit/ecxp3.png',
				link:''
			},{
				url:'../../static/credit/ecxp5.png',
				link:'/component/productstoredetail?ProductId=10&SpecificationId=22&ProductFirstId=6'
			}],
			xcjList:[]
    }
  },
  methods: {
  	recordClick(){//点击备案
			window.location.href = 'http://www.miitbeian.gov.cn/state/outPortal/loginPortal.action;jsessionid=yZGh3fHt8OZyxjnAk5z89RzZPqOh3mGi-rolRbIEDyLR4-2KQcQp!-736616813'
		},
    showNotice (id) {
      // this.$router.push("/component/tutor");
    },
    showTutorDetail(link,tutorId) {
     // console.log(id);
      this.$router.push({path:link,query:{tutorId:tutorId}});
    },
    showStarDetail(ChefStarId) {
      this.$router.push({path:'/component/starkitchen',query:{chefStarId:ChefStarId}})
    },
    showDishDetail(link,dishId,dishType,chefId,isVoteDish){
    	this.$router.push({path:link,query:{dishId:dishId,dishType:dishType,chefId:chefId,isVoteDish:isVoteDish}});
    },
    ecxpClick(link){//点击ec新品
    	this.$router.push(link);
    },
    collectClick(IsCollect,dishId,dishType,index,content){
    	var _this=this;
			var params={
				"DishId":dishId,
				"OpenId":this.openId,
				"DishType":dishType
			}
			if(IsCollect=="false"){//未收藏			
				this.$http({ 	//收藏
					method:'POST',
					url:apiUrl.addCollect,
					data:params
				})
				.then(function(response){
						if(response.data=="OK"){			
						 	if(content=='rx'){
							 	_this.cpRxList[index].collectImg='../../static/credit/love02.png';
							 	_this.cpRxList[index].IsCollect='true';				 	 						 	 
							 	_this.cpRxList.splice(index,1,_this.cpRxList[index]);
						 	}else{
						 		_this.cpRqList[index].collectImg='../../static/credit/love02.png';
						 	  _this.cpRqList[index].IsCollect='true';				 	 						 	 
						 	  _this.cpRqList.splice(index,1,_this.cpRqList[index]);
						 	}						 	 
				     //收藏成功样式
				       _this.colShow=true;
					     setTimeout(function(){
					     	_this.colShow=false;
					     },1000)               
				    } 				
				})
			}else{//已收藏				
				this.$http({ 	//取消收藏
					method:'POST',
					url:apiUrl.delCollect,
					data:params
				}).then(function(response){ 	
						if(response.data=="OK"){		
						 	 if(content=='rx'){
							 	 _this.cpRxList[index].collectImg='../../static/credit/love01.png';
							 	 _this.cpRxList[index].IsCollect='false';						 
							 	 _this.cpRxList.splice(index,1,_this.cpRxList[index]);
						 	 }else{
						 	 	 _this.cpRqList[index].collectImg='../../static/credit/love01.png';
						 	   _this.cpRqList[index].IsCollect='false';						 
						 	   _this.cpRqList.splice(index,1,_this.cpRqList[index]);
						 	 }						 	 
				    } 				
				})
			}			
    },
    getBanner() {
      var _this = this;
		  this.$http({
	 	  	method: 'GET',
	 	  	url: apiUrl.getBanner
	 	  }).then(function(respose){
				var obj = respose.data;
				var data = JSON.parse(obj);
				_this.ChefActivity = data.ChefActivity;	
				var Banner = data.Banner;	
				for(var i=0;i<Banner.length;i++){
					var bannerlist=Banner[i];
					var item={
						url:bannerlist.Link,
						img:bannerlist.PicUrl
					}
					_this.bannerList.push(item);
				}
				_this.share();
	 	  })
    },
    getNotice() {
      var _this = this;
		  this.$http({
	 	  	method: 'GET',
	 	  	url: apiUrl.getNotice
	 	  }).then(function(respose){
				var obj = respose.data;
				var data = JSON.parse(obj);
				_this.noticeList = data;
				data.length > 0 ? _this.noticeShow = true : '';
//				console.log(data);
//				console.log(_this.noticeList);
	 	  })
    },
    getTutor() {
      var _this = this;
		  this.$http({
	 	  	method: 'GET',
	 	  	url: apiUrl.getTutor
	 	  }).then(function(respose){
				var obj = respose.data;
				var data = JSON.parse(obj);
//				 console.log(data);
				_this.tutorList = data;			
				for(var i=0;i<_this.tutorList.length;i++){
					var tutorItem=_this.tutorList[i];
					tutorItem.url='/component/tutor'
				}
	 	  })
    },
    getDishList() {
      var _this = this;
		  this.$http({
	 	  	method: 'GET',
	 	  	url: apiUrl.getDishList + '?openId='+this.openId
	 	  }).then(function(respose){
				var obj = respose.data;
				var data = JSON.parse(obj);
				_this.xcjList=data.newChefStar;//星厨家
				_this.cpRxList = data.newSelectSqlJson; // 最新入选		
				for(var i=0;i<_this.cpRxList.length;i++){
					var rxitem=_this.cpRxList[i];
          //详情页需要参数 DishChefId DishType openId ChefId
					rxitem.IsCollect=='false' ? rxitem.collectImg='../../static/credit/love01.png' : rxitem.collectImg='../../static/credit/love02.png';
					rxitem.url='/component/competitiondish';
//					console.log($('.rxImg').eq(i).width());
//					console.log($('.rxImg').eq(i).height());
				}
				_this.cpRqList = data.newTopVisitCountJson; // 人气抢手
				for(var i=0;i<_this.cpRqList.length;i++){
					var rqitem=_this.cpRqList[i];
				  //详情页需要参数 DishChefId DishType openId ChefId
					rqitem.IsCollect=='false' ? rqitem.collectImg='../../static/credit/love01.png' : rqitem.collectImg='../../static/credit/love02.png';
					rqitem.url='/component/competitiondish';
				}
	 	  })
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
			            title:'知味·达美活动首页',
			            link:apiUrl.getUrl+'/#/',
			            imgUrl:"https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTtUTdXVWn7MvEo8e4FBxjHJoHibVtzGG0DrKt4ia3SLVrXcRt6XrUogmw/0?wx_fmt=jpeg",
			            success: function () { 
			              // 用户确认分享后执行的回调函数
//				              alert('分享到朋友圈成功');
			              _this.addECWXTranspondDetails(2);
			            },
			            cancel: function () { 
			              // 用户取消分享后执行的回调函数
//				              alert('你没有分享到朋友圈');
			            },
			            fail:function(res){
//			            	alert(res.errMsg);
			            }
	            });
	            wx.onMenuShareAppMessage({
									title: '知味·达美活动首页', // 分享标题
									desc: '知味·达美活动首页', // 分享描述
									link:apiUrl.getUrl+'/#/', // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
									imgUrl:"https://mmbiz.qpic.cn/mmbiz_jpg/uuwJXDpEBqtIndjMF7wbHqDy3CVVvrmTtUTdXVWn7MvEo8e4FBxjHJoHibVtzGG0DrKt4ia3SLVrXcRt6XrUogmw/0?wx_fmt=jpeg",// 分享图标
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
    addECWXTranspondDetails(type){//转发页面记录  1转发给朋友  2转发到朋友圈
    	var params={
  		  "ECBrowseURL":apiUrl.getUrl+"/#/",
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
  created(){
  	var locationHref=window.location.href;
		if(locationHref.indexOf('action')!=-1){//有action
			var paramsValue=locationHref.split('?')[1];
			var actionValue=paramsValue.split('=')[1];
			if(actionValue.indexOf('registeredmembers')!=-1){//会员注册来源追溯
        var str=actionValue.substring(17);
        if(str == ''){
			  	window.location.href = apiUrl.getUrl+"/#/component/registeredmembers";
        }else{
			    window.location.href = apiUrl.getUrl+"/#/component/registeredmembers?remark="+str;
        }
			}else if(actionValue.indexOf('recommend')!=-1 && actionValue.length==9){//推荐同行注册
				var str1=locationHref.split('?')[2].split('=')[1];
				var str2=locationHref.split('?')[3].split('=')[1];
				var str3=locationHref.split('?')[4].split('=')[1].slice(0,1);
				window.location.href = apiUrl.getUrl+"/#/component/recommend?userId="+str1+"&img="+str2+"&userType="+str3;
			}else if(actionValue.indexOf('signsuccess')!=-1){//有|  有activityId 签到成功页面
			  var str=actionValue.substring(11);
			  window.location.href =apiUrl.getUrl+"/#/component/signsuccess?activityId="+str;
			}else if(actionValue.indexOf('registeredrecommend')!=-1){//推荐同行注册页面
			  var str=actionValue.substring(19);
			  window.location.href =apiUrl.getUrl+"/#/component/registeredrecommend?RecommendId="+str;
			}else if(actionValue.indexOf('scanhome')!=-1){//扫码红包首页
			  window.location.href =apiUrl.getUrl+"/#/component/scanhome";
			}else if(actionValue.indexOf('specialsoy')!=-1){//生抽专题页
			  window.location.href =apiUrl.getUrl+"/#/component/specialsoy";
			}else if(actionValue.indexOf('specialsoup')!=-1){//酸汤酱专题页
			  window.location.href =apiUrl.getUrl+"/#/component/specialsoup";
			}else if(actionValue.indexOf('thanksdraw')!=-1){//感恩节抽奖 
			  window.location.href =apiUrl.getUrl+"/#/component/thanksdraw";
			}else if(actionValue.indexOf('tasteshare')!=-1){//分享链接
			  window.location.href =apiUrl.getUrl+"/#/component/tasteshare";
			}else if(actionValue.indexOf('shopdetail')!=-1){//申领试用 商品详情
//      var val = paramsValue.split('=')[2];
//			  window.location.href =apiUrl.getUrl+"/#/component/shopdetail?ProductId=1055&ProductType=1&source=" + val;
 
			  var arr = locationHref.split('&');
			  var str = arr[1].split('=')[1];
			  var str1 = arr[2].split('=')[1];
			  window.location.href =apiUrl.getUrl+"/#/component/shopdetail?ProductId="+str+"&ProductType="+str1;
			  
			}else if(actionValue.indexOf('productdishdetail')!=-1){//产品库菜品详情
        var val = paramsValue.split('=')[2];
			  window.location.href =apiUrl.getUrl+"/#/component/productdishdetail?dishId=" + val;
			}else if(actionValue.indexOf('productstoredetail')!=-1){//产品详情
        var val = paramsValue.split('=')[2];
        if(val == '5' || val == '5#/'){//尚品生抽
			  	window.location.href =apiUrl.getUrl+"/#/component/productstoredetail?ProductId=5&SpecificationId=12&ProductFirstId=1";
        }else if(val == '7' || val == '7#/'){//尚品耗油
			  	window.location.href =apiUrl.getUrl+"/#/component/productstoredetail?ProductId=7&SpecificationId=18&ProductFirstId=2";
        }else if(val == '17' || val == '17#/'){//冰糖老抽
			  	window.location.href =apiUrl.getUrl+"/#/component/productstoredetail?ProductId=17&SpecificationId=31&ProductFirstId=1";
        }
			}else if(actionValue.indexOf('dishstoredetail')!=-1){//菜品库详情
        var val = paramsValue.split('=')[2];
			  window.location.href = apiUrl.getUrl+"/#/component/dishstoredetail?dishId="+val;
			}else if(actionValue.indexOf('dishspecial')!=-1){//菜品库专题页
//				var str=actionValue.substring(11);
//			  window.location.href = apiUrl.getUrl+"/#/component/dishspecial?specialId="+str;
			  window.location.href = apiUrl.getUrl+"/#/component/dishspecial";
			}else{
				if(actionValue.indexOf('shop')!=-1){//商城
			  	window.location.href =apiUrl.getUrl+"/#/component/shop";
				}else{
			    window.location.href =apiUrl.getUrl+"/#/component/"+actionValue;
				}
			}
		}else{
			$('title').html('知味·达美活动首页');
		}
  },
  mounted () {
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
	  var uri=window.location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
	    	
     // 获取轮播banner图
     this.getBanner();
     // 获取公告信息
     this.getNotice();
     // 获取导师介绍
     this.getTutor();
     // 参赛菜品
     // 最新入选 人气抢手  星·厨家
     this.getDishList();
     // 培训交流
  }
}
</script>

<style scoped>
	body {
	    font-family: Helvetica, sans-serif;
	    background-color: #ffffff;
	}
	/*导师介绍*/
	.ds-box { width: 715px; height: 120px; position: relative; overflow: hidden;font-size: 13px;color: #3E3E3E; }
	.ds-item { width: 85px; height: 120px; display:inline-block; margin-left: 15px; float: left; text-align: center; }	
	.ds-item:last-child { margin-right: 15px; }
	.tutorurl{height: 85px;border-radius: 50%;border: 1px solid #f0f0f0;overflow: hidden;box-sizing: border-box;}
	.tutorurl img{width: 100%;}
	/*参赛菜品*/
	.cp-rx-box, .cp-rq-box { width: 825px; position: relative; overflow: hidden;}
	.cp-rx-item, .cp-rq-item {width: 150px;display: inline-block; margin-left: 10px;}
	.cp-rx-item:last-child, .cp-rq-item:last-child { margin-right: 15px; }
	.cp-rx-item p, 
	.cp-rq-item p{font-size: 14px;}
	.cp-rx-item div, 
	.cp-rq-item div {font-size: 12px; line-height: 20px; color: #8f8f8f; }
	.collectImg{width: 16px; margin-top: -3px; margin-right: 15px; vertical-align: middle;}
	.cp-rx-item span, 
	.cp-rq-item span {font-size: 12px; color: #8f8f8f;}
	.cp-rx-item .memspan,.cp-rq-item .memspan{margin-right: 12px;}
	/*培训交流*/
	
	/*EC新品*/
	.ecxp-box { overflow: hidden; margin-top: 10px;}
	.ecxp-item { width: 47%;height: 124px; float: left; position: relative; margin-left: 2%; margin-bottom: 2%; background: #f9f9f9; overflow: hidden; }
	.ecxp-item .left {height: 124px; float: left; width: 70%; }
	.ecxp-item .left p:nth-child(1){font-size: 13px;color: #E83428;margin-top: 16px;margin-bottom: 13px;margin-left: 8px;}
	.ecxp-item .left p:nth-child(2){font-size: 11px;color: #3e3e3e;margin-left: 8px;}
	.ecxp-item .right {height: 124px; position: absolute; right: 8px; top: 0px; overflow: hidden; }
	.ecxp-item .right img { height: 124px; }
	.ecxpDiv{display: flex;flex-wrap: wrap;margin-top: 10px;}
	.ecxpDiv div{width: 47%;margin-left: 2%;}
	.ecxpDiv div img{width: 100%;}
	
	/*星厨·家*/
	.xcj-box {}
	.xcj-item { overflow: hidden;height: 125px;border-bottom: 1px solid #f0f0f0; }
	.xcj-item div { width: 47%; height: 100%; float: left; margin-left: 2%;margin-top: 10px;}
	.xcj-item .left img { width: 100%;}
	.xcj-item .right p:first-child {font-size: 14px;color: #E83428;margin-top: 20px;margin-bottom: 10px;}
	.xcj-item .right p:last-child {font-size: 13px;color: #3E3E3E;}
	/*收藏图片*/
	.colSuc{width: 50%;position: absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
  /*备案*/
	.record{padding:10px 16px;background: #f6f6f6;color: #818181;font-size: 12px;text-align: center;border-radius: 10px;letter-spacing: 1px;
					position: absolute;left: 50%;transform: translateX(-50%);}
</style>
<style>
	 #vux_view_box_body{background: #fff;}
	.home .weui-grid:before { border-right: none; }
	.notice { background: #f9f9f9; }
	.notice .weui-cells  { background: none !important; font-size: 0.9em; }
	.notice .weui-cells:before,
	.notice .weui-cells:after{border: none;}
	.notice .weui-cell { padding: 10px 15px; }
	.notice .weui-cell .weui-cell__hd img { width: 20px; margin-right: 5px !important; }
	.notice .weui-cell .vux-cell-bd { display: none !important; }
	.notice .align-center{font-size:12px;color:#3E3E3E;} 
	.notice .vux-cell-primary{flex:0 !important;}
	.home .weui-grid__icon { width: 35px !important; height: 35px !important; }
	.home .weui-grid:after { border-bottom: none; }
	
	.video-box { width: 90%; margin: 0 auto;}
	.video-box img { width: 100%; }
	.home .weui-cells { background: none;}
	.home .weui-cells:before { border-top: 0px !important; }
	.weui-cells:after { border-bottom: 0px !important; }
	.home .weui-cell__ft{color: #adadad;font-size: 12px;}
	.home .weui-cell_access .weui-cell__ft:after{border-width: 1px 1px 0 0 !important;}
	.vux-slider > .vux-swiper > .vux-swiper-item > a > .vux-swiper-desc{background: rgba(0,0,0,0.4);height: 1.8em !important;padding: 0px !important;font-size: 20px !important;}
	.home .weui-cell{margin-top: 20px;}
	.home .weui-cells{margin-top:0 !important;}
	.notice .weui-cell{margin-top: 0 !important;}
	.tutorintro .weui-cell{margin-top: 0 !important;}
    /*ec新品*/
    .ecxpcell {height: 16.8px;}
	/*重构滚动条，左右滑动滚动条 start*/
	.scroller-box { position: relative; }
	.scroller-box .mask-scroller { width: 100%;position: absolute; left: 0; bottom: 0; background: #fff; }
	.scroller-box .modify-scroller { overflow: auto; }
	/*以下可忽略*/
	.scroller-box .modify-scroller { overflow-x: visible; }
	.scroller-box .modify-scroller::-webkit-scrollbar { display: none; }
	.scroller-box .modify-scroller:horizontal { display: none; }
	/*end*/
</style>
