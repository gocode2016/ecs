<template>
  <div class="productstoredetail">
  	
    <div class="page">
	    <div class="content">
		    <!--产品图-->
		    <img :src="specification.SpecificationPicUrl" style="width: 100%;"/>
		    <div style="padding: 0 10px;">
		    	<!--产品信息-->
		    	<p><span class="productName">{{productKuInfo.ProductName}}</span><span class="productLook">{{specification.VisitCount}}人看过</span></p>
		    	<p style="margin-top: 5px;">
		    		<span class="productType">品牌：{{productKuInfo.BrandName}}</span>
		    		<span class="typemore" @click="moreClick"></span>
		    		<span class="praiseShow"><img :src="praiseImg" @click="praiseClick"><span>{{specification.PraiseCount}}</span></span>
		    	</p>
		    	<p style="position: relative;top: -5px;"><span class="productType">规格：{{specification.Amount}}{{specification.Unit}}</span></p>
		    	<!--产品介绍-->
		    	<p class="proTitle proTitles"><img src="../../static/credit/product_js.png"><span>产品介绍</span></p>
		    	<p class="proSmallTitle">产品特点</p>
		    	<p class="proFeature protext ql-editor ql-blank"></p>
		    	<p class="proSmallTitle">产品价值</p>
		    	<p class="proValue protext ql-editor ql-blank"></p>
		    	<div class="productIntr">
			    	<p class="proSmallTitle">烹饪技法</p>
			    	<p class="cookSkill protext ql-editor ql-blank"></p>
			    	<p class="proSmallTitle">基本信息</p>
			    	<p class="proBasicInfo protext ql-editor ql-blank"></p>
		    	</div>
		    	<!--查看详情-->
		    	<p class="lookDetail" @click="detailClick">查看详情</p>
		    	<!--销售区域-->
		    	<p class="proTitle proTitles"><img src="../../static/credit/product_area.png"><span>销售区域</span></p>
		    	<div class="proSale">
			    	<span v-for="(item,index) in areaList">{{item.ProvinceName}}</span>
		    	</div>
		    	<!--研发故事-->
		    	<div v-show="storyFlag">
			    	<p class="proTitle"><img src="../../static/credit/book1.png"><span>研发故事</span></p>
			    	<!--大师介绍-->
			    	<div class="proMaster" v-for="(item,index) in masterList" :key="index">
			    		<div><img :src="item.MasterHeaderPicUrl"></div>
			    		<div>
			    			<p>{{item.MasterName}}</p>
			    			<p style="color: #e52e26;font-size: 12px;">{{item.MasterStation}}</p>
			    			<p style="font-size: 12px;">{{item.MasterHotal}}</p>
			    		</div>
			    	</div>
			    	<!--故事-->
			    	<p class="rdStory ql-editor ql-blank"></p>
		    	</div>
		    </div>
		    
		    <!--应用菜品-->
		    <p class="proTitle" style="padding-left: 10px;"><img src="../../static/credit/product_cp.png"><span>应用菜品</span></p>
		    <div class="dishWrap">
		    	<div class="dishBox" v-for="(item,index) in dishInfo" @click="dishClick(item.DishProductId)">
		    		<div><img :src="item.DishPicUrl"></div>
		    		<p>{{item.DishName}}</p>
		    	</div>
		    </div>
		    <!--相关产品-->
		    <p class="proTitle" style="padding-left: 10px;"><img src="../../static/credit/product_xgcp.png"><span>相关产品</span></p>
		    <div class="dishWrap">
		    	<div class="dishBox" v-for="(item,index) in productInfo" @click="productClick(item.ProductId,item.SpecificationId,item.ProductFirstId)">
		    		<div><img :src="item.SpecificationPicUrl"></div>
		    		<p>{{item.Amount}}{{item.Unit}}{{item.ProductName}}</p>
		    	</div>
		    </div>
		    <!--我要留言-->
		    <p class="proTitle" style="padding-left: 10px;"><img src="../../static/credit/tutor03.png"><span>我要留言</span></p>
		    <ul class="wordUl">
		    	<scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2"  @on-pullup-loading="load" class='scrollheight'>
		    	 	 <div class="box2">
			     	 	 <li v-for="(item,index) in speakdata" :key="index">
			     	 	 	 <div class="leftDiv"><img v-bind:src="item.HeadPicUrl"></div>
			     	 	 	 <div class="rightDiv">
			     	 	 	 	 <p>{{item.MemebName}}<span style="float: right;font-size: 12px;">{{item.HotelName}}</span></p>
			     	 	 	 	 <p>{{item.Comment}}</p>
			     	 	 	 	 <p>{{item.CreateTime}}</p>
			     	 	 	 </div>
			     	 	 </li>
		     	 	</div>
		 	 	  </scroller>
		    </ul>
	    </div>
	    <!--底部-->
			<div class="footer">
				<div class="send">
	     	 	 <div v-if='flag1' class="sendfir">
		     	 	 <input type="text" placeholder="我有话要说..." @focus='focus'/>
	     	 	 </div>
	     	 	 <div v-else class="sendsec" @click.stop>
	     	 	 	 <div class="txtArea">
	     	 	 	 	 <textarea @input='textChange()' v-model='message' maxlength='500'></textarea>
	     	 	 	   <span>{{leng}}</span>
	     	 	 	 </div>     	 	 	
	     	 	 	 <button @click="submit">发送</button>
	     	 	 </div>
	     	</div>
		 	</div>
    </div>
    
  </div>
</template>

<script>
import { Scroller,cookie } from 'vux'
import apiUrl from '../apiUrls.js'
import wx from 'weixin-js-sdk'
export default{
	components:{
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
			userId:'',
			userType:'',
			openId:'',
			nickName:'',
      productId:'',//接口参数
      specificationId:'',//接口参数
      productFirstId:'',//接口参数
      specification:{},//产品规格信息
      productKuInfo:{},//产品库信息
      praiseImg:'',//点赞图片
      areaList:[],//销售区域
      masterList:[],//研发大师
      storyFlag:true,//研发故事板块
      dishInfo:[],//应用菜品
      productInfo:[],//相关产品
      pageIndex:1,//留言页数
			speakdata:[],//留言数据
			count:'',//留言总长度
			pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
				loadingContent: '加载中...'
	    },
	    flag1:true,//底栏显示
      message:'',//留言内容
      leng:500,//内容长度
      isShow:0,//详情是否显示全部
		}
	},
	methods:{
		moreClick(){//点击品牌 更多
			if(this.productKuInfo.BrandName=='葱伴侣'){
				window.location.href="http://congbanlv.2dc.shinho.net.cn/?from=singlemessage";
			}else if(this.productKuInfo.BrandName=='味达美'){
				window.location.href="http://weidamei.2dc.shinho.net.cn/?from=singlemessage";
			}else if(this.productKuInfo.BrandName=='六月鲜'){
				window.location.href="http://liuyuexian.2dc.shinho.net.cn/?from=singlemessage";
			}else if(this.productKuInfo.BrandName=='黄飞红'){
				window.location.href="http://huangfeihong.2dc.shinho.net.cn";
			}
		},
		getProductKuDetail(){//获取产品详情
			var _this=this;
			var params={
  		  "ProductId":this.productId,
  		  "SpecificationId":this.specificationId,
  		  "OpenId":this.openId,
  		  "ProductFirstId":this.productFirstId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getProductKuDetail,
  			data:params
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
			  _this.specification=data.Specification;//产品规格信息
			  _this.productKuInfo=data.ProductKuInfo;//产品信息
			  if(data.IsPaise=="f"){//点赞状态
			  	_this.praiseImg="../../static/credit/zanw.png";
			  }else{
			  	_this.praiseImg="../../static/credit/zanr.png";
			  }
			  $('.proFeature').html(_this.productKuInfo.ProductFeature);//产品特点
			  $('.proValue').html(_this.productKuInfo.ProductValue);//产品价值
			  $('.cookSkill').html(_this.productKuInfo.CookingSkill);//烹饪技法
			  $('.proBasicInfo').html(_this.productKuInfo.ProductBasicInfo);//基本信息
			  $('.proFeature img,.proValue img,.cookSkill img,.proBasicInfo img').css({
			  	'width':'100%'
			  })
			  _this.areaList=data.Area;//销售区域
			  _this.masterList=data.RDMaster;//研发大师
			  var masterlist=(JSON.parse(_this.masterList)).length;//转化研发大师数组格式 
        _this.dishInfo=data.DishInfo;//应用菜品
			  _this.productInfo=data.ProductInfo;//相关产品
			  
			  if(data.RDInfo==null&&masterlist==0){//研发大师和研发故事都为空 隐藏此版块
			  	_this.storyFlag=false;
			  }else if(data.RDInfo!=null){
			    $('.rdStory').html(data.RDInfo.RDStory) ;//研发故事
			    $('.rdStory img').css('width','100%');
			  }
			  
  		})
		},
		praiseClick(){//点赞
			if(this.praiseImg=='../../static/credit/zanw.png'){
				this.addSpecificationPraiseLog();
			}
		},
		addSpecificationPraiseLog(){//增加规格点赞
			var _this=this;
			var params={
  		  "SpecificationId":this.specificationId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addSpecificationPraiseLog,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.specification.PraiseCount=data.PraiseCount;
  			_this.praiseImg="../../static/credit/zanr.png";
  		})
		},
		addSpecificationVisitLog(){//增加规格浏览量
			var params={
  		  "SpecificationId":this.specificationId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addSpecificationVisitLog,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  		})
		},
		getSpecificationComment(){//获取规格留言
			var _this=this;
			var params={
  		  "SpecificationId":this.specificationId,
  		  "Index":this.pageIndex
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getSpecificationComment,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
//			console.log(data);
  			_this.speakdata=_this.speakdata.concat(data.data);
  			_this.count=data.Count;
  			_this.speakdata.length<=3 ? $('.scrollheight').height(_this.speakdata.length*122) : $('.scrollheight').height(3*122);
  		})
		},
		addSpecificationComment(){//增加留言
			var _this=this;
			var params={
  		  "SpecificationId":this.specificationId,
  		  "OpenId":this.openId,
  		  "MemberName":this.nickName,
  		  "HeadPicUrl":this.userData.Headimgurl,
  		  "Comment":this.message
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addSpecificationComment,
  			data:params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			_this.pageIndex=1;
  			_this.speakdata=[];
        _this.getSpecificationComment();
	  		_this.message='';
	  		_this.leng=500;
	  		_this.flag1=true;
	  		$('.scrollheight').animate({scrollTop:0},300);
  		})
		},
		productClick(ProductId,SpecificationId,ProductFirstId){//点击相关产品
			this.productId=ProductId;
			this.specificationId=SpecificationId;
			this.productFirstId=ProductFirstId;
			this.pageIndex=1;
			this.speakdata=[];
      this.storyFlag=true;		//研发故事显示	
      this.isShow=0;
			$(".productIntr").css({
        opacity:0.3,
				height:"100px"
			})
			$('.lookDetail').html('查看详情');
			$('.lookDetail').removeClass('lookDetails');
			this.getProductKuDetail();//获取产品详情
			this.getSpecificationComment();//获取规格留言
			$('.content').animate({scrollTop:0},1);
		},
		dishClick(dishId){//应用菜品
			this.$router.push({path:'/component/productdishdetail',query:{dishId:dishId}})
		},
		detailClick(){//点击查看详情
			if(this.isShow==0){//查看详情
				$(".productIntr").css({
	        opacity:1,
					height:"auto"
				})
				this.isShow=1;
				$('.lookDetail').html('隐藏详情');
				$('.lookDetail').addClass('lookDetails');
			}else if(this.isShow==1){//隐藏详情
				this.isShow=0;
				$(".productIntr").css({
	        opacity:0.3,
					height:"100px"
				})
				$('.lookDetail').html('查看详情');
				$('.lookDetail').removeClass('lookDetails');
			}
		},
		load () {//上拉加载
	      setTimeout(() => {
	      	this.pageIndex+=1;
					this.getSpecificationComment();
	        setTimeout(() => {
	          this.$refs.demo2.donePullup()
	        }, 100)
	        if (this.speakdata.length ==this.count) { // unload plugin
	          setTimeout(() => {
	            this.$refs.demo2.disablePullup()
	          }, 100)
	        }
	      }, 500)
	  },
	  focus(){
		   this.flag1=false;  
		   var _this=this;
		   setTimeout(function(){
		   	 $('.sendsec textarea').focus();
		   },100);
		   $(document).click(function(){
		   	 _this.flag1=true;  
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
  	submit(){//发送按钮
  		if(this.message!=''){  			 		
	  		//新增菜品留言
	  		this.addSpecificationComment();
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
							    title: _this.specification.Amount+_this.specification.Unit+_this.productKuInfo.ProductName+'产品应用说明', // 分享标题
							    link: apiUrl.getUrl+'/#/component/productstoredetail?ProductId='+_this.productId+'&SpecificationId='+_this.specificationId+'&ProductFirstId='+_this.productFirstId, // 分享链接
									imgUrl: _this.specification.SpecificationPicUrl, // 分享图标
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
									title:  _this.specification.Amount+_this.specification.Unit+_this.productKuInfo.ProductName+'产品应用说明', // 分享标题
									desc:  _this.specification.Amount+_this.specification.Unit+_this.productKuInfo.ProductName+'产品应用说明', // 分享描述
									link:apiUrl.getUrl+'/#/component/productstoredetail?ProductId='+_this.productId+'&SpecificationId='+_this.specificationId+'&ProductFirstId='+_this.productFirstId, // 分享链接
									imgUrl:_this.specification.SpecificationPicUrl,// 分享图标
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
	  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/productstoredetail",
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
//		用户信息
		this.userData=cookie.get('WeiXinUser',function(val){
		  var a = val.split("&");
				var obj = {};
				for (var i = 0; i < a.length; i++) {
					var b = a[i].split("=");
					obj[b[0]] = b[1];
				}
				return obj;
	  }) 
	  this.userId=parseInt(this.userData.UserId);
	  this.userType=parseInt(this.userData.UserType);
	  this.openId=this.userData.Openid;
	  this.nickName=decodeURI(this.userData.Nickname);
//	  获取数据
	  this.productId=this.$route.query.ProductId;
	  this.specificationId=this.$route.query.SpecificationId;
	  this.productFirstId=this.$route.query.ProductFirstId;
		this.getProductKuDetail();//获取产品详情
		this.addSpecificationVisitLog();//增加规格浏览量
		this.getSpecificationComment();//获取规格留言
		//分享
	  var uri=location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
		this.share();
	}
}
</script>

<style scoped>
@import '../../static/css/quill.bubble.css';
@import '../../static/css/quill.core.css';
@import '../../static/css/quill.snow.css';
.productstoredetail{background: #fff;height: 100%;}
.productName{font-size: 14px;color: #3e3e3e;}
.productLook{font-size: 11px;color: #8c8c8c;line-height: 26px;float: right;}
.productType{font-size: 12px;color: #8c8c8c;}
.typemore:after{content: " ";display: inline-block;height: 7px;width: 7px; border-width: 1.5px 1.5px 0 0;border-color: #E83428;border-style: solid;
            position: relative;top: -0.5px;right: -2px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(0deg);}
.praiseShow{float: right;}
.praiseShow *,.proTitle *{display: inline-block;vertical-align: middle;}
.praiseShow img,.proTitle img{width: 20px;}
.proTitles img{width: 17px;}
.praiseShow span{font-size: 12px;color: #8c8c8c;margin-left: 8px;position: relative;top: 2px;}
.proTitle{margin-top: 10px;}
.proTitle span{font-size: 14px;color: #e92f23;margin-left: 8px;}
.proSmallTitle{font-size: 14px;color: #3e3e3e;margin-top: 20px;margin-bottom: 10px;}
.proSale span{font-size: 13px;color: #3E3E3E;margin-top: 4px;margin-right: 5px;}
.proMaster{width: 100%;display: flex;margin-top: 10px;margin-bottom: 15px;}
.proMaster div:nth-child(1){width: 100px;height: 100px;border-radius: 50%;overflow: hidden;}
.proMaster div:nth-child(1) img{width: 100%;height: 100%;}
.proMaster div:nth-child(2){width: 60%;margin-left: 5%;padding-top: 15px;color: #3E3E3E;}
.dishWrap{width: 100%;display: flex;overflow-x: auto;margin-top: 10px;}
.dishBox{margin-left: 8px;}
.dishBox:nth-child(1){margin-left: 10px;}
.dishBox div{width:105px;}
.dishBox div img{width:100%;}
.dishBox p{width:105px;text-align: center;font-size: 13px;color: #3E3E3E;}
.dishWrap::-webkit-scrollbar {display:none}/*隐藏滚动条*/
.wordUl{width: 100%;list-style: none;}
.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;}
.leftDiv{width: 18%;}
.rightDiv{width: 82%;font-size: 12px;color: #3e3e3e;padding-right: 10px;}
.rightDiv p:nth-child(2){word-wrap: break-word;}
.rightDiv p:nth-child(3){text-align: right;color: #888888;	margin-top: 15px;}
.leftDiv img{width: 33px;margin-left: 20px;border-radius: 50%}	
.send{width: 100%;background: #FFFFFF;}
.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
.sendfir input{width:90%;height: 35px;border: none;background: #f6f6f6;margin-left: 5%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
.sendsec{width: 90%;margin-left: 5%;text-align: right;}
.txtArea{height:45px;background:#f6f6f6;position: relative;}
.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}
.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
.protext{font-size: 13px;}
/*查看详情*/
.productIntr{height: 100px;overflow-y: hidden;opacity:0.3;}
/*background-image: linear-gradient(to bottom, #000 ,#fff);-webkit-background-clip: text; -webkit-text-fill-color: transparent;*/
            
.lookDetail{text-align: center;font-size: 14px;color: #E83428;margin-top: 20px;}
.lookDetail:after {content: " ";display: inline-block;height: 10px;width: 10px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    position: relative;top: -3px;right: -9px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(90deg);}
.lookDetails:after {content: " ";display: inline-block;height: 10px;width: 10px; border-width: 1.5px 1.5px 0 0;border-color: #C8C8CD;border-style: solid;
    position: relative;top: 3px;right: -9px;transform: matrix(0.71, 0.71, -0.71, 0.71, 0, 0) rotate(-90deg);}
/*底部固定*/ 
.page {display: -webkit-box;display: -webkit-flex;display: -ms-flexbox;display: flex;-webkit-flex-direction: column;-ms-flex-direction: column;
			flex-direction: column;height: 100%;}
.content::-webkit-scrollbar {display:none;}/*滚动条不显示*/
.content {overflow: auto;}/*flex内容区自适应*/

		
</style>
<style>
  
</style>