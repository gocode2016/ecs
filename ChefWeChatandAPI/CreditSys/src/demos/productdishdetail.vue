<template>
  <div class="productdishdetail" style="height: 100%;"> 	
  	<div class="page">
			<div class="content">
				<!--头部-->
			    <img :src='dishInfo.DishPicUrl' class="dishImg">
			    <p class="look">{{dishInfo.VisitCount}}人看过</p>
			    <div class="dishname">{{dishInfo.DishName}}</div>
			    <p class="good">
			     	<img :src='praiseImg' @click="praiseClick"/>
			     	<span>{{dishInfo.PrasieCount}}</span>
			    </p>
				<!--菜品故事-->
			    <div class="show">
				    <div class="alike">
				     	<img src="../../static/credit/book1.png" />
				     	<span>菜品故事</span>
				    </div>
				    <p class="storytext ql-editor ql-blank"></p>
			    </div>
			    <!--备料展示-->
			    <div class="alike show show1">
			     	<img src="../../static/credit/show.png" />
			     	<span>备料展示</span>
			    </div>    
			    <group v-for="(item,index) in dishlist" :key="index" gutter=0>
				     <cell :title='item.type' class='cellTitle'></cell>
				     <cell-form-preview :list="item.list"></cell-form-preview>
			    </group> 
			    <!--烹饪步骤-->
			    <div class="show">
				    <div class="alike">
				     	<img src="../../static/credit/cook.png" />
				     	<span>烹饪步骤</span>
				    </div> 
				    <p class="cooktext" v-for="(item,index) in dishStep" :key="index">{{item.StepId}}、{{item.StepName}}</p>	     
			    </div>
		        <!--我要留言-->
			    <div class="alike show">
			     	<img src="../../static/credit/tutor03.png" />
			     	<span>我要留言</span>
			    </div> 
		      <ul class="wordUl">
			    	<scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2" @on-pullup-loading="load" class='scrollheight'>
			    	 	<div class="box2">
				     	 	<li v-for="(item,index) in speakdata" :key="index">
				     	 	 	<div class="leftDiv"><img v-bind:src="item.HeadPicUrl"></div>
				     	 	 	<div class="rightDiv">
				     	 	 	 	<p>{{item.MemberName}}</p>
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
			     	 	<img :src="collectImg" @click="collectClick"/>
		     	 	</div>
		     	 	<div v-else class="sendsec" @click.stop>
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
  </div>
</template>

<script>
import { CellFormPreview,Cell,Scroller,Group,cookie} from 'vux'
import apiUrl from "../apiUrls.js"
import wx from 'weixin-js-sdk'

export default {
  components: {
    CellFormPreview,
    Cell,
    Scroller,
    Group,
    cookie
  },
  data () {
    return{ 
    	userData:{},
    	openId:'',
    	dishId:'',
    	praiseImg:'',//点赞图标
    	dishInfo:{},//菜品信息
    	dishMateria:[],//备料展示
    	dishStep:[],//烹饪步骤
    	dishlist:[],//备料展示
    	list1:[],
      list2:[],
      list3:[],
      pageIndex:1,//留言页数
    	speakdata:[],//留言数据
    	count:'',//留言个数
    	flag1:true,//底栏显示
    	message:'',//留言内容
    	leng:500,//内容长度
    	pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      },
      nickName:'',
    	collectImg:'',//收藏图标
    	colShow:false,//收藏成功显示
    	timestamp:'',
    	nonceStr:'',
    	signature:'',
    	url:'',
    	chefName:'',
    	starDishId:'',//菜品Id
    	dishType:'',//菜品类型 创新菜1 本地菜2
    }
  },
  methods:{
    getDishProductWX(){//获取菜品详情
    	var _this=this;
    	var params={
  		   "DishId":this.dishId,
  		   "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getDishProductWX,
  			data:params
  		}).then(function(response){
//			  console.log(JSON.parse(response.data));
			  var data=JSON.parse(response.data);
			  _this.dishInfo=data.DishProduct;//菜品信息
			  $('.storytext').html(_this.dishInfo.DishStory);//菜品故事
			  $('.storytext img').css({
			  	'width':'100%'
			  })
			  data.IsPrasie=="f" ? _this.praiseImg="../../static/credit/zanw.png" : _this.praiseImg="../../static/credit/zanr.png";//点赞图标
			  data.IsCollect=="f" ? _this.collectImg="../../static/credit/love01.png" : _this.collectImg="../../static/credit/love02.png";//收藏图标
			  _this.dishMateria=data.DishMaterial;//备料展示
			  var dishProductRF = data.DishProductRF;//推荐调料
			  for(var i=0;i<_this.dishMateria.length;i++){
	     	  var dishMater=_this.dishMateria[i];
	     	  if(dishMater.MaterialType==1){
	          var item1={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list1.push(item1);
	     	  }else if(dishMater.MaterialType==2){
	          var item2={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list2.push(item2);
	     	  }else if(dishMater.MaterialType==3){
	          var item3={
	          		label:dishMater.Material,
	              value:dishMater.Unit+'g'
	          }
	          _this.list3.push(item3);
	     	  }            	  
	      }
        for(var i=0;i<dishProductRF.length;i++){
        	var dishProduct=dishProductRF[i];
        	var item={
        		label:dishProduct.ProductName,
	          value:dishProduct.Unit+'g'
        	}
        	_this.list3.unshift(item);
        }
	      _this.dishlist=[{
		      	type:"主料",
		      	list:_this.list1
		      },{
		      	type:"辅料",
		      	list:_this.list2
		      },{
		      	type:"调料",
		      	list:_this.list3
			  }]
        _this.dishStep=data.DishStep;//步骤
        
  		})
    },
    praiseClick(){//点赞
    	if(this.praiseImg=="../../static/credit/zanw.png"){
    		this.addDishProductPraise();//菜品点赞
    	}
    },
    addDishProductPraise(){//菜品点赞
    	var _this=this;
    	var params={
  		  "DishId":this.dishId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addDishProductPraise,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
  			_this.praiseImg="../../static/credit/zanr.png";
			  _this.dishInfo.PrasieCount=data.PrasieCount;
  		})
    },
    addDishVisit(){//菜品浏览次数
    	var params={
  		  "DishId":this.dishId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addDishVisit,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  		})
    },
    getDishProductComment(){//获取菜品留言
    	var _this=this;
    	var params={
  		  "DishId":this.dishId,
  		  "Index":this.pageIndex
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.getDishProductComment,
  			data:params
  		}).then(function(response){
//			console.log(JSON.parse(response.data));
  			var data=JSON.parse(response.data);
  			_this.speakdata=_this.speakdata.concat(data.data);
  			_this.count=data.Count;
  			_this.speakdata.length<=3 ? $('.scrollheight').height(_this.speakdata.length*122) : $('.scrollheight').height(3*122);
  		})
    },
    addDishProductComment(){//新增留言
    	var _this=this;
    	var params={
  		  "DishId":this.dishId,
  		  "OpenId":this.openId,
  		  "HeadPicUrl":this.userData.Headimgurl,
  		  "MemberName":this.nickName,
  		  "Comment":this.message
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addDishProductComment,
  			data:params
  		}).then(function(response){
//			console.log(response.data);
  			_this.pageIndex=1;
  			_this.speakdata=[];
        _this.getDishProductComment();//获取菜品留言
	  		_this.message='';
	  		_this.leng=500;
	  		_this.flag1=true;
	  		$('.scrollheight').animate({scrollTop:0},300);
  		})
    },
    collectClick(){//点击收藏
    	var _this=this;
    	var params={
  		  "DishId":this.dishId,
  		  "OpenId":this.openId
  		}
  		this.$http({
  			method:'post',
  			url:apiUrl.addDishProductCollect,
  			data:params
  		}).then(function(response){
  			var data=JSON.parse(response.data);
			  if(data.IsCollect=="t"){
			  	_this.collectImg="../../static/credit/love02.png";
			  	_this.colShow=true;
	  			setTimeout(function(){
	  			  _this.colShow=false;
	  			},1000)
			  }else{
			  	_this.collectImg="../../static/credit/love01.png";
			  }
  		})
    },
    load () {//上拉加载
	      setTimeout(() => {
	      	this.pageIndex+=1;
	      	this.getDishProductComment();//获取菜品留言
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
	  		this.addDishProductComment();//新增留言
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
						    title: _this.dishInfo.DishName, // 分享标题
						    link: apiUrl.getUrl+'/#/component/productdishdetail?dishId='+_this.dishId, // 分享链接
								imgUrl: _this.dishInfo.DishPicUrl, // 分享图标
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
								title:  _this.dishInfo.DishName, // 分享标题
								desc:  _this.dishInfo.DishName, // 分享描述
								link:apiUrl.getUrl+'/#/component/productdishdetail?dishId='+_this.dishId, // 分享链接
								imgUrl:_this.dishInfo.DishPicUrl,// 分享图标
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
  		  "ECBrowseURL":apiUrl.getUrl+"/#/component/productdishdetail",
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
	  this.nickName=decodeURI(this.userData.Nickname);
		
		this.dishId=this.$route.query.dishId;
		this.getDishProductWX();//获取菜品详情
		this.addDishVisit();//菜品浏览次数
		this.getDishProductComment();//获取菜品留言
		
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
	.productdishdetail{background: #ffffff;}
	.dishImg{width: 100%;}
	.look{color: #8c8c8c;text-align: right;padding-right: 10px;font-size: 12px;}
	.dishname{width: 50%;height:35px;margin-left: 25%;background: url('../../static/credit/back.png');background-size: 100% 100%;color:white;line-height: 35px;text-indent: 1em;font-size: 18px;}
	.good{text-align: right;padding-right: 10px;font-size: 12px;}
	.good *{display: inline-block;vertical-align: middle;}
	.good img{width: 20px;}
	.good span{color: #8c8c8c;position: relative;top: 2px;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
  .show{margin-top: 20px;padding-left: 10px;padding-right: 10px;}
  .show1{margin-bottom: 20px;}
  .storytext{font-size: 13px;}
	.cellTitle{background: #f9f9f9 !important;color: #e83426 !important;}
	.cooktext{font-size: 13px;color: #3e3e3e;margin-top: 15px;}
	.wordUl{width: 100%;list-style: none;}
	.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;}
	.leftDiv{width: 18%;}
	.rightDiv{width: 82%;font-size: 12px;color: #282828;padding-right: 10px;}
	.rightDiv span{float: right;font-weight:normal;}
	.rightDiv p:nth-child(1){font-weight: bold;}
	.rightDiv p:nth-child(2){word-wrap: break-word;}
	.rightDiv p:nth-child(3){text-align: right;color: #bebebe;margin-top: 15px;}
	.leftDiv img{width: 33px;margin-left: 20px;}
	.send{width: 100%;background: #FFFFFF;}
	.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
	.sendfir input{width:80%;height: 35px;border: none;background: #f6f6f6;margin-left: 4%;border-radius: 5px;text-indent: 1em;margin-top: 8px;outline: none;}
	.sendfir img{width: 20px;margin-left: 4%;position: relative;top: 6px;}
	.sendsec{width: 90%;margin-left: 5%;text-align: right;}
	.txtArea{height:45px;background:#f6f6f6;position: relative;}
	.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
	.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px;}	
	.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;margin-top: 10px;margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
	.colSuc{width: 50%;	position:absolute;top: 0;left: 0;right:0;bottom: 0;margin: auto;}
	
	.page {/*flex最外框*/
	  display: flex;
	  flex-direction: column;/* 该框从顶部列出其内容。 */
		height: 100%;
	}
	.content::-webkit-scrollbar {/*滚动条不显示*/
		display:none
	}
	.content {/*flex内容区*/
		overflow: auto;
	}
</style>
<style>
	#vux_view_box_body{background: #fff;}
	.productdishdetail .weui-cell:before{border-top: none !important;}
	.productdishdetail .weui-cells:after{border-bottom: none !important;}
	.productdishdetail .weui-form-preview__label {min-width: 0 !important;color: #3e3e3e !important;font-size: 13px;}
  .productdishdetail .weui-form-preview__value {color: #6c6c6c;font-size: 13px;}
</style>
