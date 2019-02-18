<template>
  <div class="starkitchen" style="height: 100%;">
  				<div class="page">
								<div class="content">
									   <!--星厨图片-->
								      <div class="tutorPhoto">
										  	<div class="tutorText">
										  		<p>{{chefStar.ChefStarName}}</p>
										  		<p>{{chefStar.ChefStarPosition}}</p>
										  		<p>{{chefStar.HotelName}}<span>{{chefStar.CityName}}</span></p>
										  	</div>
										  	<img v-bind:src="chefStar.PicUrl"/>
										  </div>
								      <!--星厨介绍-->
								      <div>
										     <div class="alike">
										     	 <img src="../../static/credit/tutor01.png"/>
										     	 <span>星厨介绍</span>
										     </div>
										     <div class="introduce ql-editor ql-blank"></div>	    
								      </div>
								      <!--代表菜品-->
										  <div>
										  	 <div class="alike">
										     	 <img src="../../static/credit/tutor02.png"/>
										     	 <span>代表菜品</span>
										     </div>
										     <grid :rows="2">
									         <grid-item v-for="(item,index) in chefDish" :key="index" :label="item.DishName" @on-item-click="onItemClick(item.DishChefStarId,item.DishType)">
									           <img slot="icon" :src="item.DishPicUrl">
									         </grid-item>
								         </grid>
										  </div>
										  <!--我要留言-->
										  <div>
										  	<div class="alike">
										     	 <img src="../../static/credit/tutor03.png"/>
										     	 <span>我要留言</span>
										    </div>
										    <ul class="wordUl">
										    	 <scroller lock-x scrollbar-y use-pullup :pullup-config="pullupConfig" ref="demo2"  @on-pullup-loading="load" class='scrollheight'>
										    	 	 <div class="box2">
											     	 	 <li v-for="(item,index) in speakdata" :key="index">
											     	 	 	 <div class="leftDiv"><img v-bind:src="item.HeadPicUrl"></div>
											     	 	 	 <div class="rightDiv">
											     	 	 	 	 <p>{{item.MemebName}}</p>
											     	 	 	 	 <p>{{item.Comment}}</p>
											     	 	 	 	 <p>{{item.CreateTime}}</p>
											     	 	 	 </div>
											     	 	 </li>
										     	 	</div>
									     	 	 </scroller>
								     	  </ul>
								     	  <div class="sendto"></div>
								     	</div>
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
import { Grid, GridItem,Scroller,cookie} from 'vux'
import apiUrl from "../apiUrls.js"
import wx from 'weixin-js-sdk'
var pageIndex=1;//传参页数
var pageSize=3;//传参个数
export default {
  components: {
    Grid, 
    GridItem,
    Scroller,
    cookie
  },
  data () {
    return{
    	timestamp:'',
    	nonceStr:'',
    	signature:'',
    	url:'',
    	openId:'',
    	userData:{},
    	chefStarId:'',//星厨Id
    	chefStar:{},
    	chefDish:[],
    	speakdata:"",//留言数据
    	flag1:true,//底栏显示
    	message:'',//留言内容
    	datetime:'',//留言时间
    	leng:500,//内容长度
    	count:'',//留言个数
    	pullupConfig: {
        content: '上拉加载更多',
        downContent: '松开进行加载',
        upContent: '上拉加载更多',
        loadingContent: '加载中...'
      }
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
  	this.chefStarId=this.$route.query.chefStarId;
  	var uri=window.location.href.split('#')[0];
		this.url=encodeURIComponent(uri);
  	this.getStarDetail();//星厨介绍信息
	  this.wordRequest();	 //获取留言  
  },
  methods:{
  	getStarDetail(){//星厨介绍信息
  		var _this=this;
  		var params={
  			"ChefStarId":this.chefStarId
  		}
		  this.$http({
		  	method:'POST',
		  	url:apiUrl.getStarDetail,
		  	data:params
		  }).then(function(response){		  	 		  	
		  	  var data=JSON.parse(response.data);
//		  	  console.log(data);
		  	  _this.chefStar=data.ChefStar;
		  	  _this.share();
		  	  _this.chefDish=data.chefDish;         
		  	  $(".introduce").html(_this.chefStar.Introduce);
		  	  //	导师介绍部分样式
		  	  $('.introduce img').css({//图片
    	        'width':'100%'
    	    })
		  	  $('.introduce p').css({//文字  	  	
						'margin-top': '10px'
		  	  })
		  })    
  	},
  	onItemClick(dishId,dishType){//跳转到星厨菜品详情
		  this.$router.push({path:'/component/starkitchendish',query:{dishId:dishId,dishType:dishType}})
  	},
  	wordRequest(){//获取留言
  		var _this =this;
  		var params='?chefStarId='+this.chefStarId+'&index='+1;
  		this.$http({
		  	method:'GET',
		  	url:apiUrl.getStarComment+params
		  }).then(function(response){	
		  	 var data=JSON.parse(response.data);
		  	 _this.count=data.Count;
		  	 _this.speakdata=data.data;
//		  	 console.log(data);	  	 
         //		  	  留言自适应高度
          _this.count > 3 ? $('.starkitchen .scrollheight').height(360) : $('.starkitchen .scrollheight').height(_this.count*120);
		  })  
  	},
  	load () {//上拉加载
	      setTimeout(() => {
//	      	pageIndex+=1;
          pageSize+=3;
					this.wordRequest();
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
		   self=this;
		   setTimeout(function(){
		   	 $('.sendsec textarea').focus();
		   },100);
		   $('.sendto').height(30);
		   $(document).click(function(ev){
		   	 self.flag1=true;  
		   	 $('.sendto').height(0);
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
//		var wordSize=$('.wordUl li').length;//当前留言长度
  		var _this=this;
  		var params={
  			"ChefStarId":this.chefStarId,
  			"OpenId":this.openId,
  			"MemberName":this.userData.Nickname,
  			"HeadPicUrl":this.userData.Headimgurl,
  			"Comment":this.message
  		}
  		if(this.message!=''){
	  		//新增留言接口
	  		this.$http({
	  			method:'POST',
	  			url:apiUrl.addStarComment,
	  			data:params
	  		}).then(function(response){
//	  			  pageSize=wordSize+1;
            _this.wordRequest();
			  		_this.message='';
			  		_this.leng=500;
			  		_this.flag1=true;
			  		$('.sendto').height(0);	  			
	  		})	 
  		}  		
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
			            title:'知味·达美星厨'+_this.chefStar.ChefStarName+'个人主页',
			            link:apiUrl.getUrl+'/#/component/starkitchen?chefStarId='+_this.chefStarId,
			            imgUrl:_this.chefStar.PicUrl,
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
									title: '知味·达美星厨'+_this.chefStar.ChefStarName+'个人主页', // 分享标题
									desc: '知味·达美星厨'+_this.chefStar.ChefStarName+'个人主页', // 分享描述
									link: apiUrl.getUrl+'/#/component/starkitchen?chefStarId='+_this.chefStarId, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
									imgUrl:_this.chefStar.PicUrl,// 分享图标
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
    	var arrUrl=window.location.href.split("?");
    	var params={
  		  "ECBrowseURL":arrUrl[0],
  		  "Parameter":arrUrl[1],
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
  }
}
</script>
<style scoped>
	@import '../../static/css/quill.bubble.css';
	@import '../../static/css/quill.core.css';
	@import '../../static/css/quill.snow.css';
	.tutorPhoto{overflow:hidden;width:100%;height: 375px;background: url('../../static/credit/tutorback.png') no-repeat center;background-size: 100% ;position: relative;}
	.tutorPhoto img{width: 60%;position: absolute;bottom: 0;right:0px;}
	.tutorText{margin-top: 66px;margin-left: 56px;}
	.tutorText p{width: 13px;float: left;font-size: 13px;line-height: 15px;margin-left: 8px;color: #3e3e3e;}
	.tutorText p:nth-child(1){margin-left: 0px;width: 23px;height:125px;font-size: 23px;line-height: 25px;padding-right: 8px;color: #58595b;border-right: 1px solid #59595b;}
	.tutorText p:nth-child(2){color: #e83423;}
	.tutorText span{margin-top: 10px;display: inline-block;}
	
  .alike{padding-left: 10px;margin-top: 20px;}
	.alike *{display: inline-block;vertical-align: middle;}
	.alike img{width: 20px;}
	.alike span{color: #e83426;font-size: 15px;font-weight: bold;}
	.wordUl{width: 100%;list-style: none;}
	.wordUl li{display: flex; padding-top: 30px;padding-bottom: 20px;}	
	.leftDiv{width: 18%;}
	.rightDiv{width: 82%;font-size: 12px;color: #282828;padding-right: 10px;}
	.rightDiv p:nth-child(1){font-weight: bold;}
	.rightDiv p:nth-child(2){word-wrap: break-word;}
	.rightDiv p:nth-child(3){text-align: right;color: #bebebe;	margin-top: 15px;}
	.leftDiv img{width: 33px;margin-left: 20px;}
	.sendto{height: 0px;}
	.send{width: 100%;background: #FFFFFF;}
	.sendfir{width: 100%;height: 50px;border-top: 1px solid #efefef;}
	.sendfir input{width:92%;height: 35px;border: none;background: #f6f6f6;margin-left: 4%;border-radius: 5px;text-indent: 1em;margin-top:8px;outline: none;}
	.sendfir img{width: 20px;margin-left: 5%;position: relative;top: 8px;}
	.sendsec{width: 90%;margin-left: 5%;text-align: right;}
	.txtArea{height:45px;background:#f6f6f6;position: relative;}
	.txtArea textarea{width: 100%;background:#f6f6f6;resize: none;border: none;outline: none;}
	.txtArea span{color: #c4c4c4;font-size: 12px;position: absolute;bottom: 0px;right: 10px; }
	.sendsec button{width: 13%;height: 25px;background: #7f7f7f;border-radius: 5px;	margin-top: 10px;	margin-bottom: 10px;color: white;border: none;font-size: 12px;outline: none;}
	
	
	.page {
			display: -webkit-box;
	    display: -webkit-flex;
	    display: -ms-flexbox;
	    display: flex;
			-webkit-box-orient: vertical;
	    -webkit-flex-direction: column;
	    -ms-flex-direction: column;
	    flex-direction: column;/* 该框从顶部列出其内容。 */
			height: 100%;
		}
/*滚动条不显示*/
		.content::-webkit-scrollbar {
			display:none
		}

		/*flex内容区*/
		.page .content {
			position: relative;
			-webkit-box-flex: 1;
	    -webkit-flex: 1;
	    -ms-flex: 1;
	    flex: 1;
			overflow: auto;
			-webkit-overflow-scrolling: touch;
		}

</style>
<style>
	#vux_view_box_body{background: #fff;}
	.starkitchen .weui-grid__icon {width: 100% !important;height: 100% !important;}	
	.starkitchen .weui-grid:before{border-right: none !important;}
	.starkitchen .weui-grid:after{	border-bottom: none !important;}
	.starkitchen .weui-grids:before{border-top: none !important;}		
	.starkitchen .ql-editor .ql-video.ql-align-center{margin: 0 0;width: 100%;}			
</style>
